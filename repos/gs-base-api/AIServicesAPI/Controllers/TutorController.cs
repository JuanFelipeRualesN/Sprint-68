using GSAIServicesAPI.Filters;
using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.DTOs;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Services.PromptMonitoring;
using GSAIServicesAPI.Models.Quotas;
using GSAIServicesAPI.Services.Quotas;
using GSAIServicesAPI.Services.Tutor;
using GSAIServicesAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// AI tutor endpoint. Picks the engine based on the lesson's grade and streams the answer back as Server-Sent Events.
    /// Grades configured as RAG grades -> RAG engine, which only answers from indexed educational material.
    /// Any other grade -> generative engine (Azure OpenAI).
    /// An unknown / non-numeric grade (course codes) defaults to RAG for now.
    /// The set of RAG grades lives in ai_tutor_settings table and can be edited by an admin through /api/tutor/settings endpoint
    /// Both engines emit the same SSE shape (<c>{"delta":...}</c> / <c>[DONE]</c>)
    /// </summary>
    [Route("aiservicesapi/tutor")]
    public class TutorController : BaseController
    {
        private readonly GenerativeTutorEngine _generativeTutorEngine;
        private readonly RagTutorEngine _ragTutorEngine;
        private readonly TutorSettingsService _tutorSettingsService;
        private readonly AthenasLessonClient _athenasLessonClient;
        private readonly SubjectLanguageProvider _subjectLanguageProvider;
        private readonly IPromptMonitoringService _promptMonitoringService;
        private readonly IRoleNameResolver _roleNameResolver;
        private readonly QuotaService _quotaService;
        private readonly IConfiguration _config;

        public TutorController(
            GenerativeTutorEngine generative,
            RagTutorEngine rag,
            TutorSettingsService settings,
            AthenasLessonClient athenas,
            SubjectLanguageProvider subjectLanguageProvider,
            IPromptMonitoringService promptMonitoringService,
            IRoleNameResolver roleNameResolver,
            QuotaService quotaService,
            IConfiguration config) : base(config)
        {
            _generativeTutorEngine = generative;
            _ragTutorEngine = rag;
            _tutorSettingsService = settings;
            _athenasLessonClient = athenas;
            _subjectLanguageProvider = subjectLanguageProvider;
            _promptMonitoringService = promptMonitoringService;
            _roleNameResolver = roleNameResolver;
            _config = config;
            _quotaService = quotaService;
        }

        /// <summary>
        /// Answers a tutor question, streaming the response as SSE.
        /// </summary>
        [HttpPost]
        [RequiresAiEnabled]
        public async Task Post([FromBody] GenerateRequest request)
        {
            var responseSession = await GetAuthenticatedSessionAsync("AITutor");

            if (responseSession == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                await Response.WriteAsJsonAsync(new { error = GetLocalizedMessage("Unauthorized") });
                return;
            }

            if (request.Messages == null || request.Messages.Count == 0)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsJsonAsync(new { error = GetLocalizedMessage("MissingModelOrPrompt") });
                return;
            }

            // Resolve the lesson's grade and language server-side from Athenas using the lesson id.
            // If the grade cannot be resolved, IsRagGradeAsync treats it as unknown and routes to RAG.
            var lessonMetadata = await ResolveLessonMetadataAsync(request.LessonContext, responseSession.Token, HttpContext.RequestAborted);
            var grade = lessonMetadata?.Grade;

            // The answer's language follows the lesson, so an English lesson is
            // never answered in Spanish. The subject is what carries the language: Athenas stores
            // it per subject (crs_subjects.lang_code)
            var subjectCode = lessonMetadata?.SubjectCode ?? request.LessonContext?.SubjectCode;

            var lessonLanguage = await _subjectLanguageProvider.GetLanguageAsync(subjectCode);

            // Fallback for when the subject list could not be fetched: the code's own suffix
            if (lessonLanguage == LessonLanguage.Unknown)
            {
                lessonLanguage = LessonLanguageResolver.FromSubjectCode(subjectCode);
            }

            // Hold quota before opening the stream
            var estimatedTokens = TokenEstimator.FromTexts(request.Messages.Select(m => m.Content)) + TokenEstimator.AssumedOutputTokens;

            var reservation = await _quotaService.ReserveAsync(
                responseSession.UserId,
                responseSession.RoleId,
                responseSession.Token,
                QuotaResourceType.TextTokens,
                estimatedTokens,
                "AITutor");

            // Normalize the numeric role id from auth into its name, so the audit record stores a name
            // and the classification gate reads a name instead of a magic number.
            var role = _roleNameResolver.Resolve(responseSession.RoleId);

            // Only a student's prompt is classified. Driven by the session role, not the client, so a
            // client cannot turn classification on or off. A non-student runs exactly as before.
            var isStudent = string.Equals(
                role, _config["AppSettings:student_role_id"], StringComparison.OrdinalIgnoreCase);

            var streamRequest = new TutorStreamRequest
            {
                Messages = request.Messages,
                LessonContext = request.LessonContext,
                Session = responseSession,
                ConversationId = request.ConversationId,
                IsStudent = isStudent,
                LessonLanguage = lessonLanguage
            };

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

            // The engine owns the streaming and returns what the turn consumed. Declared here so the
            // finally can still audit whatever streamed before a failure.
            var result = new TutorStreamResult();

            try
            {
                Response.ContentType = "text/event-stream";
                Response.Headers.CacheControl = "no-cache, no-transform";
                Response.Headers.Connection = "keep-alive";
                await Response.Body.FlushAsync(HttpContext.RequestAborted);

                var isRag = await _tutorSettingsService.IsRagGradeAsync(grade);
                ITutorEngine engine = isRag ? _ragTutorEngine : _generativeTutorEngine;

                result = await engine.StreamAsync(streamRequest, Response, HttpContext.RequestAborted);
            }
            catch
            {
                await _quotaService.ReleaseAsync(reservation);
                throw;
            }
            finally
            {
                // The audit copy is the text the engine already streamed, so building it costs no extra
                // tokens. Neither engine classifies inline, so the classification is left to
                // CaptureGenerateExchangeAsync, which runs the dedicated classifier for a student.
                // The write runs after the student already has the answer, so it adds no perceived
                // latency, and runs on failure too, so a partial exchange is still on record.
                var capture = new TutorCaptureData(result.StreamedText, ClassificationJson: null);

                await _promptMonitoringService.CaptureGenerateExchangeAsync(
                    responseSession.UserId,
                    role,
                    request,
                    isTutorFlow: true,
                    capture.Answer,
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    capture.ClassificationJson);
            }

            await SettleQuotaAsync(reservation, result, estimatedTokens);
        }

        /// <summary>
        /// Resolves the lesson's grade, language and subject from Athenas via lesson id.
        /// Returns null when there is no lesson id or Athenas cannot resolve it
        /// </summary>
        private async Task<LessonMetadata?> ResolveLessonMetadataAsync(LessonContextDto? lessonContext, string token, CancellationToken cancellationToken)
        {
            var lessonId = lessonContext?.LessonId;
            if (string.IsNullOrWhiteSpace(lessonId))
            {
                return null;
            }

            return await _athenasLessonClient.GetLessonMetadataAsync(lessonId, token, cancellationToken);
        }

        /// <summary>
        /// Charges the user for what the turn actually cost and returns the unused part
        /// of the reservation.
        /// </summary>
        /// <br></br>
        /// The provider's own count is used when the engine could obtain it: the generative
        /// engine reads it from Azure and the RAG orchestrator reports it as total_tokens.
        /// If a turn ever streams output without a reported count, it is billed from an
        /// estimate of the streamed text.
        /// Only a turn that produced no output at all releases the whole reservation.
        private async Task SettleQuotaAsync(QuotaReservation reservation, TutorStreamResult result, long estimatedTokens)
        {
            if (result.ReportedTokens > 0)
            {
                await _quotaService.CommitAsync(reservation, result.ReportedTokens);
                return;
            }

            if (result.StreamedText.Length > 0)
            {
                // Bill what the engine actually sent, not the full client history: both engines trim it, so the
                // untrimmed estimate would overcount
                var promptTokens = result.PromptTokensSent > 0
                    ? result.PromptTokensSent
                    : estimatedTokens - TokenEstimator.AssumedOutputTokens;
                await _quotaService.CommitAsync(reservation, promptTokens + TokenEstimator.FromText(result.StreamedText));
                return;
            }

            await _quotaService.ReleaseAsync(reservation);
        }
    }
}
