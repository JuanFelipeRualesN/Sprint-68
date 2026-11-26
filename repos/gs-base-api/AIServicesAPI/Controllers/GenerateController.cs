using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.Filters;
using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.DTOs.Plannings;
using GSAIServicesAPI.Models.Quotas;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Services.Foundry;
using GSAIServicesAPI.Services.Plannings;
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
        private readonly RagProjectClient _ragProjectClient;
        private readonly PlanningsService _planningsService;
        private readonly AthenasLessonsService _athenasLessonsService;
        private readonly ILogger<GenerateController> _logger;

        /// <summary>
        /// The module name forwarded to the external auth service on every session validation call.
        /// </summary>
        private const string Module = "AIGenerate";

        /// <summary>
        /// The output limit of a full plan
        /// </summary>
        private const int PlanMaxOutputTokens = 12000;

        private static readonly JsonSerializerOptions CandidatesJsonOptions = new() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        /// <summary>
        /// The longest topic a teacher can write for a plan
        /// </summary>
        private const int PlanTopicMaxLength = 500;

        /// <summary>
        /// The model deployment in the RAG Foundry project that generates the plans, when the configuration has none
        /// </summary>
        private const string DefaultPlanningDeploymentName = "plannings-gpt-6-luna";

        /// <summary>
        /// The model deployment in the RAG Foundry project that generates the plans
        /// </summary>
        private string PlanningDeploymentName => _config["AppSettings:AiPlanningGenerationDeploymentName"] is { Length: > 0 } name ? name : DefaultPlanningDeploymentName;

        /// <param name="projectClient">The Foundry project of the agent that answers the general requests.</param>
        /// <param name="ragProjectClient">The RAG Foundry project, whose model deployment generates the plans.</param>
        /// <param name="planningsService">Reads the unit plannings the plan generation is built from.</param>
        /// <param name="athenasLessonsService">Finds the Athenas lessons to recommend in a plan.</param>
        /// <param name="logger">The logger.</param>
        public GenerateController(
            AIProjectClient projectClient,
            RagProjectClient ragProjectClient,
            IPromptMonitoringService promptMonitoringService,
            IRoleNameResolver roleNameResolver,
            QuotaService quotaService,
            PlanningsService planningsService,
            AthenasLessonsService athenasLessonsService,
            ILogger<GenerateController> logger,
            IConfiguration config) : base(config)
        {
            _projectClient = projectClient;
            _ragProjectClient = ragProjectClient;
            _planningsService = planningsService;
            _athenasLessonsService = athenasLessonsService;
            _logger = logger;
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
        /// Streams a full planning, built from the unit's plannings in the RAG.
        /// </summary>
        /// <param name="request">The teacher's choices: unit, week, options and the plan's days and lessons.</param>
        [HttpPost("plan")]
        [RequiresAiEnabled]
        public async Task GeneratePlan([FromBody] PlanGenerationRequest request)
        {
            var session = await GetAuthenticatedSessionAsync(Module);
            if (session == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                await Response.WriteAsJsonAsync(new { error = GetLocalizedMessage("Unauthorized") });
                return;
            }

            var hasUnit = !string.IsNullOrWhiteSpace(request.UnitId);
            if (string.IsNullOrWhiteSpace(request.SubjectCode) || string.IsNullOrWhiteSpace(request.LevelCode)
                || (!hasUnit && string.IsNullOrWhiteSpace(request.Topic)))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsJsonAsync(new { error = GetLocalizedMessage("MissingModelOrPrompt") });
                return;
            }

            if (request.Topic?.Length > PlanTopicMaxLength)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsJsonAsync(new { error = $"The topic can have at most {PlanTopicMaxLength} characters." });
                return;
            }

            var language = RequestHandler.GetLanguage(HttpContext);
            UnitPlanning? planning = null;

            if (hasUnit)
            {
                try
                {
                    planning = await _planningsService.GetUnitPlanningAsync(
                        request.SubjectCode, request.LevelCode, request.UnitId!, request.Week, language, HttpContext.RequestAborted);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error reading the plannings of unit {UnitId}.", request.UnitId);
                    Response.StatusCode = StatusCodes.Status502BadGateway;
                    await Response.WriteAsJsonAsync(new { error = "The unit plannings could not be read." });
                    return;
                }

                if (planning is null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsJsonAsync(new { error = "The unit or the week has no plannings." });
                    return;
                }
            }

            var athenasLessons = await FindAthenasLessonsAsync(request, planning);
            var (system, user) = PlanPromptBuilder.Build(request, planning, athenasLessons);
            var deploymentName = PlanningDeploymentName;

            // The prompt sets the content's language from the subject
            var generateRequest = new GenerateRequest(deploymentName, system, user, null, null, null, null);

            // The candidates go first, so the client only accepts lessons that really exist in Athenas.
            var candidatesEvent = JsonSerializer.Serialize(new
            {
                athenasLessons = athenasLessons.Select(lesson => new { id = lesson.Id, title = lesson.Title, description = lesson.Description })
            }, CandidatesJsonOptions);

            // JSON mode keeps the model from returning a plan that cannot be parsed
            await StreamCompletionAsync(session, generateRequest, BuildMessages(generateRequest, isStudent: false), isStudent: false, candidatesEvent,
                options =>
                {
                    options.TextOptions = new ResponseTextOptions { TextFormat = ResponseTextFormat.CreateJsonObjectFormat() };
                    options.MaxOutputTokenCount = PlanMaxOutputTokens;
                });
        }

        /// <summary>
        /// To find the Athenas lessons related to the plan. The plan can still be generated without them,
        /// so a failure reading them from Athenas is logged and leaves no candidates.
        /// </summary>
        private async Task<List<AthenasLessonCandidate>> FindAthenasLessonsAsync(PlanGenerationRequest request, UnitPlanning? planning)
        {
            var topicTexts = new List<string?> { request.Topic };
            if (planning is not null)
            {
                topicTexts.AddRange(planning.Preview.Weeks.Select(week => week.Theme));
                topicTexts.AddRange(planning.Days.Select(day => day.LessonTitle));
                topicTexts.AddRange(planning.Preview.Weeks.SelectMany(week => week.Objectives));
            }

            try
            {
                return await _athenasLessonsService.FindAsync(request.SubjectCode, request.LevelCode, request.UnitId, RequestHandler.GetLanguage(HttpContext), topicTexts, HttpContext.RequestAborted);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error searching the Athenas lessons of {SubjectCode} {LevelCode}.", request.SubjectCode, request.LevelCode);
                return [];
            }
        }

        /// <summary>
        /// Reserves the quota, streams the completion as SSE, and then audits the exchange and settles the quota.
        /// The completion comes from a model deployment of the RAG Foundry project.
        /// </summary>
        /// <param name="firstEvent">A JSON event to send before the completion, if any.</param>
        /// <param name="configureOptions">Changes to the completion options, if any.</param>
        private async Task StreamCompletionAsync(ValidationResponseModel session, GenerateRequest request, List<ChatMessage> messages, bool isStudent,
            string? firstEvent = null, Action<CreateResponseOptions>? configureOptions = null)
        {
            // Hold the prompt plus an assumed answer length.
            // Reserving before the call is what stops an out-of-quota user from costing money
            var estimatedTokens = EstimatePromptTokens(messages) + TokenEstimator.AssumedOutputTokens;

            var reservation = await _quotaService.ReserveAsync(
                session.UserId,
                session.RoleId,
                session.Token,
                QuotaResourceType.TextTokens,
                estimatedTokens,
                Module);

            if (!reservation.Allowed)
            {
                // Rejected before any SSE header is written
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

            if (firstEvent is not null)
            {
                await Response.WriteAsync($"data: {firstEvent}\n\n");
                await Response.Body.FlushAsync();
            }

            // Normalize the numeric role id from auth into its name, so the audit record stores a name
            // and the classifier's student gate reads a name instead of a magic number.
            var role = _roleNameResolver.Resolve(session.RoleId);

            // Collects the answer while it streams, so the audit record keeps what the user saw.
            var answerBuilder = new StringBuilder();

            var cancellationToken = HttpContext.RequestAborted;
            long reportedTokens = 0;
            var streamedText = new StringBuilder();

            try
            {
                // The model deployment in the RAG Foundry project
                ProjectResponsesClient responsesClient = _ragProjectClient.GetResponsesClientForModel(request.Model);

                // The response is not kept in the project: each one is generated on its own
                var options = new CreateResponseOptions { StoredOutputEnabled = false };
                foreach (var item in ToResponseItems(messages))
                    options.InputItems.Add(item);
                configureOptions?.Invoke(options);

                await foreach (StreamingResponseUpdate update in
                    responsesClient.CreateResponseStreamingAsync(options, cancellationToken))
                {
                    switch (update)
                    {
                        case StreamingResponseOutputTextDeltaUpdate textDelta when !string.IsNullOrEmpty(textDelta.Delta):
                            answerBuilder.Append(textDelta.Delta);
                            streamedText.Append(textDelta.Delta);
                            var outgoingJson = JsonSerializer.Serialize(new { delta = textDelta.Delta });
                            await Response.WriteAsync($"data: {outgoingJson}\n\n", cancellationToken);
                            await Response.Body.FlushAsync(cancellationToken);
                            break;

                        // Usage arrives with the completed response and covers the whole run
                        case StreamingResponseCompletedUpdate completed when completed.Response?.Usage is not null:
                            reportedTokens = completed.Response.Usage.TotalTokenCount;
                            break;

                        // A plan cut by the output limit still arrives; the client repairs or reports it as truncated
                        case StreamingResponseIncompleteUpdate incomplete when incomplete.Response?.Usage is not null:
                            reportedTokens = incomplete.Response.Usage.TotalTokenCount;
                            break;

                        case StreamingResponseFailedUpdate failed:
                            throw new InvalidOperationException(failed.Response?.Error?.Message ?? "The plan generation failed.");

                        case StreamingResponseErrorUpdate error:
                            throw new InvalidOperationException(error.Message ?? "The plan generation stream failed.");
                    }
                }

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
                // Runs after the user already has the answer, so it adds no perceived latency, and
                // runs on failure too, so a partial exchange is still on record. The service resolves
                // the prompt text, bounds the write with its own timeout, and never throws.
                // classificationJson is null on purpose: the capture runs the dedicated classifier for a
                // student when none is supplied here.
                await _promptMonitoringService.CaptureGenerateExchangeAsync(
                    session.UserId,
                    role,
                    request,
                    isStudent,
                    answerBuilder.ToString(),
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    classificationJson: null);

                await SettleQuotaAsync(reservation, reportedTokens, streamedText.ToString(), messages);
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
