using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Filters;
using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Models.PromptMonitoring.Request;
using GSAIServicesAPI.Models.Quotas;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Services.DataAssistant;
using GSAIServicesAPI.Services.PromptMonitoring;
using GSAIServicesAPI.Services.Quotas;
using GSAIServicesAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Data assistant of the usage insights dashboard: it answers questions about the platform usage with the usage insights queries as tools
    /// of the model. The session is validated with the platform session service; the data scope of the person is applied by the data analysis module
    /// (the token is forwarded), never by this controller and never by the model.
    /// </summary>
    [Route("aiservicesapi/data-assistant")]
    public class DataAssistantController : BaseController
    {
        // Same permission as the Power BI dashboard (embed-powerbi-info): the people who see the dashboard are the ones who can ask about it.
        private const string Module = "embed-powerbi-info";
        private const string DownloadsRoute = "/aiservicesapi/data-assistant/downloads/";
        private const string ChartsRoute = "/aiservicesapi/data-assistant/charts/";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = null,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        private readonly IConfiguration _configuration;
        private readonly DataAssistantService _assistant;
        private readonly IDataAssistantDownloadRegistry _downloads;
        private readonly IDataAnalysisFeignAPI _dataAnalysis;
        private readonly QuotaService _quotaService;
        private readonly IPromptMonitoringService _promptMonitoringService;
        private readonly IRoleNameResolver _roleNameResolver;
        private readonly ILogger<DataAssistantController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantController"/> class.
        /// </summary>
        /// <param name="config">The application configuration.</param>
        /// <param name="assistant">The data assistant.</param>
        /// <param name="downloads">The download registry.</param>
        /// <param name="dataAnalysis">The data analysis module.</param>
        /// <param name="quotaService">The AI quota.</param>
        /// <param name="promptMonitoringService">The prompt monitoring.</param>
        /// <param name="roleNameResolver">Maps a role id to its name.</param>
        /// <param name="logger">The logger.</param>
        public DataAssistantController(IConfiguration config, DataAssistantService assistant, IDataAssistantDownloadRegistry downloads, IDataAnalysisFeignAPI dataAnalysis,
            QuotaService quotaService, IPromptMonitoringService promptMonitoringService, IRoleNameResolver roleNameResolver, ILogger<DataAssistantController> logger)
            : base(config)
        {
            _configuration = config;
            _assistant = assistant;
            _downloads = downloads;
            _dataAnalysis = dataAnalysis;
            _quotaService = quotaService;
            _promptMonitoringService = promptMonitoringService;
            _roleNameResolver = roleNameResolver;
            _logger = logger;
        }

        /// <summary>
        /// Answers a question about the platform usage.
        /// </summary>
        /// <remarks>
        /// One question can make up to six calls to the model, so the quota reserves an estimate (instructions, tools, history and question plus the
        /// assumed output of every step), settles the sum of the tokens reported by all the calls, and releases the reservation when the question fails.
        /// </remarks>
        /// <param name="request">The question, the conversation to continue, the dashboard context and the panel language.</param>
        /// <returns>
        /// 200 with the answer; 200 with ErrorCode content_blocked when the content filter of Azure blocked the message; 400 for an empty question;
        /// 401 without a valid session; 403 for a role without access; 429 when the quota is spent; 502 when the model is not available;
        /// 503 when the AI is switched off or the model denied access.
        /// </returns>
        [HttpPost("ask")]
        [RequiresAiEnabled]
        public async Task<IActionResult> Ask([FromBody] DataAssistantAskRequest request)
        {
            var session = await GetAuthenticatedSessionAsync(Module);
            if (session is null)
            {
                return Failure(StatusCodes.Status401Unauthorized, "unauthorized", "Unauthorized");
            }

            if (!IsRoleAllowed(session.RoleId))
            {
                return Failure(StatusCodes.Status403Forbidden, "forbidden", "DataAssistantForbidden");
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return Failure(StatusCodes.Status400BadRequest, "empty_question", "DataAssistantEmptyQuestion");
            }

            var estimatedTokens = _assistant.EstimateTokens(request, session.UserId);
            var reservation = await _quotaService.ReserveAsync(session.UserId, session.RoleId, session.Token, QuotaResourceType.TextTokens, estimatedTokens, Module);
            if (!reservation.Allowed)
            {
                return new ObjectResult(new
                {
                    error = GetLocalizedMessage(reservation.DenialMessageKey ?? "QuotaExceeded"),
                    ErrorCode = "quota_exceeded",
                    remaining = reservation.RemainingAmount,
                    resetsAt = reservation.PeriodEnd,
                })
                { StatusCode = StatusCodes.Status429TooManyRequests };
            }

            var language = GetRequestLanguage(request.Language);
            try
            {
                var result = await _assistant.AskAsync(request, new DataAssistantCaller(session.UserId, session.Token, language), HttpContext.RequestAborted);

                // The real consumption is the sum of every model call. If the service reported none, the estimate of what was sent and answered is charged.
                var actualTokens = result.TotalTokens > 0 ? result.TotalTokens : estimatedTokens - (TokenEstimator.AssumedOutputTokens * DataAssistantService.MaxSteps)
                    + TokenEstimator.FromText(result.Response.Answer);
                await _quotaService.CommitAsync(reservation, actualTokens);

                await CaptureForMonitoringAsync(session, request.Question!, result.Response.Answer);

                return Json(StatusCodes.Status200OK, result.Response);
            }
            catch (ModelInvocationException exception) when (exception.IsContentFiltered)
            {
                await _quotaService.ReleaseAsync(reservation);
                return Json(StatusCodes.Status200OK, new DataAssistantAskResponse(GetLocalizedMessage("DataAssistantContentBlocked"), request.ConversationId ?? string.Empty,
                    new List<JsonNode>(), new List<string>(), new List<string>(), new List<JsonNode>(), "content_blocked"));
            }
            catch (ModelInvocationException exception) when (exception.IsAccessDenied)
            {
                _logger.LogWarning("The data assistant model refused the credentials (status {Status}).", exception.StatusCode);
                await _quotaService.ReleaseAsync(reservation);
                return Failure(StatusCodes.Status503ServiceUnavailable, "model_access_denied", "DataAssistantModelAccessDenied");
            }
            catch (Exception exception) when (exception is ModelInvocationException or HttpRequestException or TaskCanceledException or TimeoutException)
            {
                _logger.LogWarning(exception, "The data assistant model is not available.");
                await _quotaService.ReleaseAsync(reservation);
                return Failure(StatusCodes.Status502BadGateway, "model_unavailable", "DataAssistantModelUnavailable");
            }
            catch
            {
                await _quotaService.ReleaseAsync(reservation);
                throw;
            }
        }

        /// <summary>
        /// Leaves an audit record of who asked what about institutional data. A failure of the monitoring store never takes down an answer
        /// the person already got (the quota was already settled), so it is logged without the prompt and swallowed.
        /// </summary>
        private async Task CaptureForMonitoringAsync(FeignAPIs.Models.ValidationResponseModel session, string question, string answer)
        {
            try
            {
                await _promptMonitoringService.CaptureAsync(new PromptMonitoringCaptureRequestModel
                {
                    UserId = session.UserId,
                    Role = _roleNameResolver.Resolve(session.RoleId),
                    Prompt = question,
                    Response = answer,
                    Subject = DataAssistantService.MonitoringSubject,
                    ClassificationJson = null,
                    IpAddress = IPAddressDetector.GetClientIpAddress(HttpContext),
                });
            }
            catch (Exception exception)
            {
                _logger.LogWarning("The prompt monitoring could not store a data assistant prompt: {ExceptionType}", exception.GetType().Name);
            }
        }

        /// <summary>
        /// Downloads the Excel file of a query of an answer. The file is rebuilt with the token of whoever asks (so their scope applies again);
        /// the API keeps only the query, never the data.
        /// </summary>
        /// <param name="id">The id of the download, from the answer.</param>
        /// <returns>The .xlsx file; 404 download_expired when the id does not exist, expired or belongs to another person.</returns>
        [HttpGet("downloads/{id}")]
        public async Task<IActionResult> GetDownload(string id)
        {
            var (session, failure) = await AuthorizeAsync();
            if (session is null)
            {
                return failure!;
            }

            if (_downloads.Find(id, session.UserId) is not { } download)
            {
                return Failure(StatusCodes.Status404NotFound, "download_expired", "DataAssistantDownloadExpired");
            }

            DataAnalysisApiResponse response;
            try
            {
                response = await _dataAnalysis.ExportUsageInsightAsync(download.QueryName, "xlsx", download.QueryString, session.Token, RequestHandler.GetLanguage(HttpContext) ?? "en",
                    HttpContext.RequestAborted);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or InvalidOperationException)
            {
                _logger.LogWarning("The data analysis module did not answer a download: {ExceptionType}", exception.GetType().Name);
                return Failure(StatusCodes.Status502BadGateway, "data_unavailable", "DataAssistantModelUnavailable");
            }

            _logger.LogInformation("data assistant download. query={Query} status={Status} bytes={Bytes}", download.QueryName, response.StatusCode, response.Content.Length);
            return response.IsSuccess
                ? File(response.Content, response.ContentType ?? "application/octet-stream", response.FileName)
                : Relay(response);
        }

        /// <summary>
        /// Gets the expanded chart of a query of an answer ("expand" window of the panel). Same id and same owner as the download.
        /// </summary>
        /// <param name="id">The id of the download, from the answer.</param>
        /// <returns>
        /// The chart specification; options all, order, includeZeros and limit of the query string go over the ones of the chat;
        /// 404 download_expired when the id does not exist, expired or belongs to another person.
        /// </returns>
        [HttpGet("charts/{id}")]
        public async Task<IActionResult> GetChart(string id)
        {
            var (session, failure) = await AuthorizeAsync();
            if (session is null)
            {
                return failure!;
            }

            if (_downloads.Find(id, session.UserId) is not { } download)
            {
                return Failure(StatusCodes.Status404NotFound, "download_expired", "DataAssistantDownloadExpired");
            }

            DataAnalysisApiResponse response;
            try
            {
                response = await _dataAnalysis.ExportUsageInsightAsync(download.QueryName, "chart", DataAssistantService.BuildExpandedChartQuery(download.QueryString, Request.Query),
                    session.Token, RequestHandler.GetLanguage(HttpContext) ?? "en", HttpContext.RequestAborted);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or InvalidOperationException)
            {
                _logger.LogWarning("The data analysis module did not answer a chart: {ExceptionType}", exception.GetType().Name);
                return Failure(StatusCodes.Status502BadGateway, "data_unavailable", "DataAssistantModelUnavailable");
            }

            var body = response.GetText();
            if (response.IsSuccess && JsonNode.Parse(body) is JsonObject specification)
            {
                specification["Id"] = id;
                specification["Url"] = ChartsRoute + id;
                body = specification.ToJsonString(JsonOptions);
            }

            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = response.StatusCode };
        }

        // ── Helpers ─────────────────────────────────────────────────────────────────────

        private async Task<(FeignAPIs.Models.ValidationResponseModel? Session, IActionResult? Failure)> AuthorizeAsync()
        {
            var session = await GetAuthenticatedSessionAsync(Module);
            if (session is null)
            {
                return (null, Failure(StatusCodes.Status401Unauthorized, "unauthorized", "Unauthorized"));
            }

            return IsRoleAllowed(session.RoleId)
                ? (session, null)
                : (null, Failure(StatusCodes.Status403Forbidden, "forbidden", "DataAssistantForbidden"));
        }

        /// <summary>
        /// Checks the role of the session against AppSettings:data_assistant_allowed_role_ids. The panel only shows to the administrators;
        /// the backend also keeps the directors (their scope is a single school).
        /// </summary>
        private bool IsRoleAllowed(string? roleId)
        {
            var allowed = (_configuration["AppSettings:data_assistant_allowed_role_ids"] ?? "2,8,6")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return !string.IsNullOrWhiteSpace(roleId) && allowed.Contains(roleId.Trim());
        }

        /// <summary>
        /// Gets the language of the caller ("es" or "en"). The language of the panel that comes in the body wins when it is present;
        /// otherwise the Accept-Language header decides.
        /// </summary>
        private string GetRequestLanguage(string? panelLanguage = null)
        {
            var language = string.IsNullOrWhiteSpace(panelLanguage) ? RequestHandler.GetLanguage(HttpContext) : panelLanguage;
            return string.Equals(language?.Split(',', ';', '-')[0].Trim(), "es", StringComparison.OrdinalIgnoreCase) ? "es" : "en";
        }

        private IActionResult Failure(int statusCode, string errorCode, string messageKey) =>
            Json(statusCode, new { error = GetLocalizedMessage(messageKey), ErrorCode = errorCode });

        private ContentResult Json(int statusCode, object value) =>
            new() { Content = JsonSerializer.Serialize(value, value.GetType(), JsonOptions), ContentType = "application/json", StatusCode = statusCode };

        /// <summary>
        /// Passes on the answer of the data analysis module (status and body) when it is not a file, so the caller sees why it failed.
        /// </summary>
        private static ContentResult Relay(DataAnalysisApiResponse response) =>
            new() { Content = response.GetText(), ContentType = response.ContentType ?? "application/json", StatusCode = response.StatusCode };
    }
}
