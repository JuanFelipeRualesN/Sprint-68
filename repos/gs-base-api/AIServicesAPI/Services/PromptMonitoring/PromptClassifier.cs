using System.ClientModel;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace GSAIServicesAPI.Services.PromptMonitoring
{
    /// <inheritdoc />
    public class PromptClassifier : IPromptClassifier
    {
        private readonly AzureOpenAIClient _client;
        private readonly ILogger<PromptClassifier> _logger;
        private readonly string _model;

        /// <summary>
        /// Upper bound on the classification call, so a stalled model can never hold the request open
        /// after the answer was already delivered.
        /// </summary>
        private static readonly TimeSpan ClassifyTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Verdict recorded when Azure's content filter blocks the prompt outright. A blocked prompt is
        /// itself a strong signal, so it is flagged rather than lost. Shaped to match
        /// <c>PromptMonitoringClassificationModel</c> so it deserializes like a real tool call.
        /// </summary>
        private const string ContentFilterVerdictJson =
            "{\"isFlagged\":true,\"alertCategories\":[\"content_filter\"]," +
            "\"alertReason\":\"El proveedor de IA bloqueó el mensaje por su filtro de contenido de seguridad.\"}";

        public PromptClassifier(
            AzureOpenAIClient client,
            IConfiguration config,
            ILogger<PromptClassifier> logger)
        {
            _client = client;
            _logger = logger;
            _model = config["AppSettings:azure_openai_deployment"] ?? "gpt-4o-mini";
        }

        /// <inheritdoc />
        public async Task<string?> ClassifyAsync(string? prompt, CancellationToken cancellationToken = default)
        {
            // Nothing was asked, so there is nothing to classify.
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return null;
            }

            try
            {
                ChatClient chatClient = _client.GetChatClient(_model);

                // Only the student's message: this call reviews the prompt, not the conversation or the
                // answer, so it stays as small and cheap as possible.
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(PromptClassificationTool.InstructionText),
                    new UserChatMessage(prompt)
                };

                var options = new ChatCompletionOptions
                {
                    // Force the tool. This call exists only to get the verdict, so there is no answer
                    // for the model to weigh against calling it.
                    ToolChoice = ChatToolChoice.CreateFunctionChoice(PromptClassificationTool.FunctionName)
                };
                options.Tools.Add(PromptClassificationTool.Definition);

                // Its own bounded token, linked to the caller's: the student already has the answer, so a
                // stalled classification must not hold anything open beyond the timeout.
                using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeout.CancelAfter(ClassifyTimeout);

                var completion = (await chatClient.CompleteChatAsync(messages, options, timeout.Token)).Value;

                var call = completion.ToolCalls
                    .FirstOrDefault(toolCall => toolCall.FunctionName == PromptClassificationTool.FunctionName);

                return call?.FunctionArguments?.ToString();
            }
            catch (ClientResultException ex) when (IsContentFilterBlock(ex))
            {
                // Azure refused the prompt at its content filter, so the model never got to classify it.
                // The block itself is the alert: record it as flagged instead of losing the prompt.
                _logger.LogWarning(ex, "Prompt blocked by the content filter; recording it as flagged.");
                return ContentFilterVerdictJson;
            }
            catch (Exception ex)
            {
                // Best-effort: a failed classification leaves the prompt unclassified, never breaks the flow.
                _logger.LogError(ex, "Prompt classification failed.");
                return null;
            }
        }

        /// <summary>
        /// True when the failure is Azure's content filter rejecting the request (HTTP 400 with a
        /// <c>content_filter</c> code), as opposed to a transient or configuration error.
        /// </summary>
        private static bool IsContentFilterBlock(ClientResultException ex) =>
            ex.Status == 400
            && ex.Message.Contains("content_filter", StringComparison.OrdinalIgnoreCase);
    }
}
