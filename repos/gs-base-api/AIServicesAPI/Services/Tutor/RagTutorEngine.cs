using System.Text;
using System.Text.Json;
using GSAIServicesAPI.Services.Quotas;
using Microsoft.AspNetCore.Http;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// RAG tutor engine. Forwards the question to the Azure RAG orchestrator and re-emits its SSE stream in the unified tutor shape.
    /// In this tutor section we only need the streamed text.
    /// </summary>
    public class RagTutorEngine : ITutorEngine
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private readonly ILogger<RagTutorEngine> _logger;

        public RagTutorEngine(ILogger<RagTutorEngine> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Forwards the latest question to the RAG orchestrator and re-emits its
        /// stream in the unified SSE shape
        /// </summary>
        public async Task<TutorStreamResult> StreamAsync(TutorStreamRequest request, HttpResponse response, CancellationToken cancellationToken)
        {
            // Accumulates the answer as it is re-emitted, for the audit record and the quota settlement.
            var result = new TutorStreamResult();

            // Answer the latest question. The conversation_id lets
            // the orchestrator keep the thread's context across turns.
            var question = request.Messages.LastOrDefault(message =>
                string.Equals(message.Role, "user", StringComparison.OrdinalIgnoreCase))?.Content
                ?? request.Messages.LastOrDefault()?.Content
                ?? string.Empty;

            // Only the question is forwarded, so a broken stream is billed from that rather
            // than the full client history the caller estimated up-front.
            result.PromptTokensSent = TokenEstimator.FromText(question);

            var orcEndpoint = AppConfiguration.Configuration["AppSettings:azure_orc_endpoint"];

            if (string.IsNullOrWhiteSpace(orcEndpoint))
            {
                await SseWriter.WriteErrorAsync(response, "RAG orchestrator is not configured.", cancellationToken);
                return result;
            }

            var payload = JsonSerializer.Serialize(new
            {
                conversation_id = request.ConversationId,
                question,
                module = "tutor",
                profile = "students",
                lesson_id = request.LessonContext?.LessonId ?? "",
                lesson_lang = request.LessonLanguage.ToIsoCode() ?? "",
                client_principal_id = request.Session.UserId,
                client_principal_name = request.Session.FullName ?? "",
                is_search_included = true
            });

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, orcEndpoint)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Add("Token", request.Session.Token);

            var orcKey = AppConfiguration.Configuration["AppSettings:AzureOrcKey"];
            if (!string.IsNullOrWhiteSpace(orcKey))
            {
                requestMessage.Headers.Add("X-API-KEY", orcKey);
            }

            HttpResponseMessage upstreamResponse;
            try
            {
                upstreamResponse = await HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to RAG orchestrator");
                await SseWriter.WriteErrorAsync(response, $"Error connecting with RAG service: {ex.Message}", cancellationToken);
                return result;
            }

            using (upstreamResponse)
            {
                if (!upstreamResponse.IsSuccessStatusCode)
                {
                    var errBody = await upstreamResponse.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("RAG orchestrator returned {StatusCode}. Body: {ErrorBody}", (int)upstreamResponse.StatusCode, errBody);
                    await SseWriter.WriteErrorAsync(response, $"RAG service returned {(int)upstreamResponse.StatusCode}.", cancellationToken);
                    return result;
                }

                await using var upstream = await upstreamResponse.Content.ReadAsStreamAsync(cancellationToken);

                await TranslateStreamAsync(upstream, response, result, cancellationToken);
            }

            // The answer came from the external orchestrator, so nothing was classified inline here.
            // The capture step classifies a student's prompt with its own standalone call.
            return result;
        }

        /// <summary>
        /// Reads the orchestrator's SSE stream frame by frame and re-emits each one
        /// in the unified tutor format. Frames are separated by a blank line; only
        /// <c>token</c> frames produce output and the <c>done</c> frame ends the
        /// stream. Consumption seen along the way accumulates into <paramref name="result"/>.
        /// </summary>
        private async Task TranslateStreamAsync(Stream upstream, HttpResponse response, TutorStreamResult result, CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(upstream, Encoding.UTF8);
            var buffer = new StringBuilder();
            var conversationIdSent = new bool[1];

            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var line = await reader.ReadLineAsync(cancellationToken);
                    if (line is null) break;

                    // A blank line terminates an SSE frame.
                    if (line.Length == 0)
                    {
                        if (buffer.Length > 0)
                        {
                            var emittedDone = await EmitFrameAsync(buffer.ToString(), response, conversationIdSent, result, cancellationToken);
                            buffer.Clear();
                            if (emittedDone) return;
                        }
                        continue;
                    }

                    // Ignore SSE comments (e.g. ": keep-alive").
                    if (line[0] == ':') continue;

                    // Format used in the orchestrator's SSE stream.
                    if (line.StartsWith("data:", StringComparison.Ordinal))
                    {
                        if (buffer.Length > 0) buffer.Append('\n');
                        buffer.Append(line.Substring(5).Trim());
                    }
                }

                // Flush any trailing frame that was not terminated by a blank line.
                if (buffer.Length > 0)
                {
                    var emittedDone = await EmitFrameAsync(buffer.ToString(), response, conversationIdSent, result, cancellationToken);
                    if (emittedDone) return;
                }

                // Orchestrator closed without an explicit done frame.
                await SseWriter.WriteDoneAsync(response, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Client disconnected
            }
            catch (Exception ex)
            {
                // The orchestrator connection dropped mid-stream. Keep whatever was already
                // streamed into result so the caller settles from it instead of
                // refunding tokens the provider already produced
                _logger.LogError(ex, "RAG orchestrator stream failed mid-way");
                await SseWriter.WriteErrorAsync(response, "RAG service connection was interrupted.", cancellationToken);
            }
        }

        /// <summary>
        /// Translates one orchestrator data payload into the unified Tutor format.
        /// </summary>
        private static async Task<bool> EmitFrameAsync(string payload, HttpResponse response, bool[] conversationIdSent, TutorStreamResult result, CancellationToken cancellationToken)
        {
            JsonElement root;

            try
            {
                using var doc = JsonDocument.Parse(payload);
                root = doc.RootElement.Clone();
            }
            catch (JsonException)
            {
                // ignore.
                return false;
            }

            if (root.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            // The orchestrator returns conversation_id on the "done" frame, but we read it from any frame defensively and forward
            // it once, so the client can send it back on the next request.
            if (!conversationIdSent[0]
                && root.TryGetProperty("conversation_id", out var convProp)
                && convProp.ValueKind == JsonValueKind.String)
            {
                var conversationId = convProp.GetString();
                if (!string.IsNullOrEmpty(conversationId))
                {
                    await SseWriter.WriteConversationIdAsync(response, conversationId, cancellationToken);
                    conversationIdSent[0] = true;
                }
            }

            // The orchestrator reports the request's consumption on its "done" frame.
            // When it is absent the caller falls back to estimating from the streamed text.
            if (root.TryGetProperty("total_tokens", out var totalTokensProp)
                && totalTokensProp.TryGetInt64(out var totalTokens)
                && totalTokens > 0)
            {
                result.ReportedTokens = totalTokens;
            }

            if (!root.TryGetProperty("type", out var typeProp))
            {
                return false;
            }

            var type = typeProp.GetString();

            switch (type)
            {
                case "token":
                    if (root.TryGetProperty("value", out var valueProp))
                    {
                        var text = valueProp.GetString();

                        if (!string.IsNullOrEmpty(text))
                        {
                            result.AppendStreamedText(text);
                            await SseWriter.WriteDeltaAsync(response, text, cancellationToken);
                        }
                    }
                    return false;

                case "done":
                    await SseWriter.WriteDoneAsync(response, cancellationToken);
                    return true;

                case "error":
                    var message = root.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : "RAG service error.";
                    await SseWriter.WriteErrorAsync(response, message ?? "RAG service error.", cancellationToken);
                    return true;

                default:
                    return false;
            }
        }
    }
}