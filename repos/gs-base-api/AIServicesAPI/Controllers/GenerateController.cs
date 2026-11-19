using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using GSAIServicesAPI.Filters;
using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.Quotas;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Services.PromptMonitoring;
using GSAIServicesAPI.Services.Quotas;
using GSAIServicesAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using OpenAI.Responses;
using System.Text;
using System.Text.Json;

#pragma warning disable OPENAI001 // Responses API types are marked experimental in the OpenAI .NET SDK

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Handles streaming AI text generation requests via a Microsoft Foundry agent
    /// that has a knowledge base attached.
    /// All endpoints require a valid session token scoped to the authenticated user.
    /// </summary>
    /// <remarks>
    /// Responses are streamed using the <c>text/event-stream</c> (SSE) protocol.
    /// <br/><br/>
    /// The agent is chosen server-side from configuration (<c>Foundry:AgentName</c>).
    /// <c>GenerateRequest.Model</c> sent by the frontend is ignored: the model, the instructions
    /// and the knowledge base all live in the agent definition in Foundry.
    /// <br/><br/>
    /// Token consumption is metered against the caller's quota. The reservation includes an
    /// allowance for knowledge-base retrieval, because the chunks the agent retrieves are billed
    /// as input tokens even though they never appear in the request.
    /// </remarks>
    [Route("aiservicesapi/[controller]")]
    public class GenerateController : BaseController
    {
        private readonly AIProjectClient _projectClient;
        private readonly IPromptMonitoringService _promptMonitoringService;
        private readonly IRoleNameResolver _roleNameResolver;
        private readonly IConfiguration _config;
        private readonly QuotaService _quotaService;
        private readonly string _agentName;
        private readonly long _retrievalTokenAllowance;

        /// <summary>
        /// The module name forwarded to the external auth service on every session validation call.
        /// </summary>
        private const string Module = "AIGenerate";

        public GenerateController(
            AIProjectClient projectClient,
            IPromptMonitoringService promptMonitoringService,
            IRoleNameResolver roleNameResolver,
            QuotaService quotaService,
            IConfiguration config) : base(config)
        {
            _projectClient = projectClient;
            _promptMonitoringService = promptMonitoringService;
            _roleNameResolver = roleNameResolver;
            _quotaService = quotaService;
            _config = config;

            _agentName = config["Foundry:AgentName"]
                ?? throw new InvalidOperationException("Missing configuration value 'Foundry:AgentName'.");

            // Extra input tokens to hold for the chunks the knowledge base injects.
            // Tune it after a few real requests by comparing the estimate with the reported usage.
            _retrievalTokenAllowance = config.GetValue<long?>("Foundry:RetrievalTokenAllowance") ?? 4000;
        }

        /// <summary>
        /// Streams the agent's response token by token using Server-Sent Events (SSE).
        /// </summary>
        /// <remarks>
        /// <b>SSE frame format (unchanged for the frontend):</b>
        /// <code>
        /// data: {"delta": "token text"}\n\n   ← one per token
        /// data: [DONE]\n\n                     ← end of stream
        /// data: {"error": "message"}\n\n       ← on failure
        /// </code>
        /// </remarks>
        [HttpPost]
        [RequiresAiEnabled]
        public async Task Post([FromBody] GenerateRequest request, [FromQuery] bool? isFromTeacherApp = false)
        {
            var session = await GetAuthenticatedSessionAsync(Module);
            if (session == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                await Response.WriteAsJsonAsync(new { error = GetLocalizedMessage("Unauthorized") });
                return;
            }

            var isStudent = isFromTeacherApp.HasValue && isFromTeacherApp != true;

            // The model check is gone (the agent decides the model); a teacher still needs a prompt.
            if (!isStudent && string.IsNullOrWhiteSpace(request.User))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsJsonAsync(new { error = GetLocalizedMessage("MissingModelOrPrompt") });
                return;
            }

            var messages = BuildMessages(request, isStudent);

            var estimatedTokens = EstimatePromptTokens(messages)
                + _retrievalTokenAllowance
                + TokenEstimator.AssumedOutputTokens;

            var reservation = await _quotaService.ReserveAsync(
                session.UserId,
                session.RoleId,
                session.Token,
                QuotaResourceType.TextTokens,
                estimatedTokens,
                Module);

            if (!reservation.Allowed)
            {
                Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await Response.WriteAsJsonAsync(new
                {
                    error = GetLocalizedMessage(reservation.DenialMessageKey ?? "QuotaExceeded"),
                    remaining = reservation.RemainingAmount,
                    resetsAt = reservation.PeriodEnd
                });
                return;
            }

            Response.ContentType = "text/event-stream";
            Response.Headers.CacheControl = "no-cache, no-transform";
            Response.Headers.Connection = "keep-alive";
            await Response.Body.FlushAsync();

            var role = _roleNameResolver.Resolve(session.RoleId);

            var answerBuilder = new StringBuilder();
            var cancellationToken = HttpContext.RequestAborted;
            long reportedTokens = 0;

            try
            {
                // Responses client bound to the Foundry agent by name. The agent's model,
                // instructions and knowledge base apply automatically: the agent decides when
                // to search the knowledge base, so there's nothing to wire up here.
                ProjectResponsesClient responsesClient = _projectClient.ProjectOpenAIClient
                    .GetProjectResponsesClientForAgent(defaultAgent: _agentName);

                var inputItems = ToResponseItems(messages);

                await foreach (StreamingResponseUpdate update in
                    responsesClient.CreateResponseStreamingAsync(inputItems, cancellationToken: cancellationToken))
                {
                    switch (update)
                    {
                        // Only the final answer text is forwarded. Knowledge-base search calls and their
                        // results arrive as other update types and are skipped on purpose.
                        case StreamingResponseOutputTextDeltaUpdate textDelta when !string.IsNullOrEmpty(textDelta.Delta):
                            answerBuilder.Append(textDelta.Delta);
                            var outgoingJson = JsonSerializer.Serialize(new { delta = textDelta.Delta });
                            await Response.WriteAsync($"data: {outgoingJson}\n\n", cancellationToken);
                            await Response.Body.FlushAsync(cancellationToken);
                            break;

                        // Usage covers the whole run, including the retrieved chunks.
                        case StreamingResponseCompletedUpdate completed when completed.Response?.Usage is not null:
                            reportedTokens = completed.Response.Usage.TotalTokenCount;
                            break;

                        case StreamingResponseFailedUpdate failed:
                            throw new InvalidOperationException(failed.Response?.Error?.Message ?? "Agent run failed.");

                        case StreamingResponseErrorUpdate error:
                            throw new InvalidOperationException(error.Message ?? "Agent stream error.");
                    }
                }

                // A run that ends with no text usually means the knowledge tool is waiting for an
                // approval (MCP tools default to "require approval"). Set it to "never" in the agent.
                if (answerBuilder.Length == 0)
                    throw new InvalidOperationException(
                        "The agent returned no answer. Check that its knowledge tool does not require approval.");

                await Response.WriteAsync("data: [DONE]\n\n");
                await Response.Body.FlushAsync();
            }
            catch (OperationCanceledException)
            {
                // Client disconnected
            }
            catch (Exception ex)
            {
                var errJson = JsonSerializer.Serialize(new { error = ex.Message });
                await Response.WriteAsync($"data: {errJson}\n\n");
                await Response.Body.FlushAsync();
            }
            finally
            {
                var answer = answerBuilder.ToString();

                await _promptMonitoringService.CaptureGenerateExchangeAsync(
                    session.UserId,
                    role,
                    request,
                    isStudent,
                    answer,
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    classificationJson: null);

                await SettleQuotaAsync(reservation, reportedTokens, answer, messages);
            }
        }

        /// <summary>
        /// Charges the caller for what the run actually cost and returns the unused
        /// part of the reservation.
        /// </summary>
        private async Task SettleQuotaAsync(QuotaReservation reservation, long reportedTokens, string streamedText, List<ChatMessage> messages)
        {
            if (reportedTokens > 0)
            {
                await _quotaService.CommitAsync(reservation, reportedTokens);
                return;
            }

            if (streamedText.Length > 0)
            {
                // No usage arrived (stream broke). Retrieval was very likely billed, so count the allowance too.
                var estimated = EstimatePromptTokens(messages)
                    + _retrievalTokenAllowance
                    + TokenEstimator.FromText(streamedText);
                await _quotaService.CommitAsync(reservation, estimated);
                return;
            }

            await _quotaService.ReleaseAsync(reservation);
        }

        private static long EstimatePromptTokens(List<ChatMessage> messages)
        {
            return TokenEstimator.FromTexts(messages.SelectMany(m => m.Content).Select(p => p.Text));
        }

        /// <summary>
        /// Converts the chat messages into Responses API input items.
        /// System prompts become developer messages, which are added on top of the agent's
        /// own instructions in Foundry; they don't replace them.
        /// </summary>
        private static List<ResponseItem> ToResponseItems(List<ChatMessage> messages)
        {
            var items = new List<ResponseItem>();
            foreach (var message in messages)
            {
                var text = string.Concat(message.Content.Select(p => p.Text));
                if (string.IsNullOrWhiteSpace(text)) continue;

                items.Add(message switch
                {
                    SystemChatMessage => ResponseItem.CreateDeveloperMessageItem(text),
                    AssistantChatMessage => ResponseItem.CreateAssistantMessageItem(text),
                    _ => ResponseItem.CreateUserMessageItem(text)
                });
            }
            return items;
        }

        /// <summary>
        /// Assembles the input sent to the agent.
        /// The agent's instructions in Foundry are the main system prompt now, so the app only adds
        /// what the agent can't know on its own: the student's lesson context and the conversation.
        /// </summary>
        private static List<ChatMessage> BuildMessages(GenerateRequest request, bool isStudent)
        {
            var messages = new List<ChatMessage>();

            if (isStudent)
            {
                // Lesson context still comes from the app; the agent combines it with its knowledge base.
                // IMPORTANT: if TutorPromptBuilder tells the model to answer ONLY from the lesson context,
                // it will stop the agent from using the knowledge base. Reword that rule, or move the
                // tutor rules into the agent's instructions and send only the lesson context here.
                messages.Add(TutorPromptBuilder.Build(request.LessonContext));

                foreach (var message in request.Messages ?? [])
                {
                    if (string.IsNullOrWhiteSpace(message.Content)) continue;
                    if (string.Equals(message.Role, "assistant", StringComparison.OrdinalIgnoreCase))
                        messages.Add(new AssistantChatMessage(message.Content));
                    else
                        messages.Add(new UserChatMessage(message.Content));
                }
            }
            else
            {
                // The old "You are a helpful assistant." default is gone: it would sit on top of the
                // agent's instructions and dilute them. A system prompt is only sent if the client supplied one.
                if (!string.IsNullOrWhiteSpace(request.System))
                    messages.Add(new SystemChatMessage(request.System));

                messages.Add(new UserChatMessage(request.User));
            }

            return messages;
        }
    }
}