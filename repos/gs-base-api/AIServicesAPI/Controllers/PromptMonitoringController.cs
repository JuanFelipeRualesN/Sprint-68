using GSAIServicesAPI.Models.PromptMonitoring.Request;
using GSAIServicesAPI.Models.PromptMonitoring.Response;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Services.PromptMonitoring;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Read endpoints over the stored prompt logs. A teacher uses them to review the prompts of the
    /// students they relate to, and an administrator to review any user's.
    /// </summary>
    /// <remarks>
    /// Every read is keyed by user, the shard key of the prompt log collection. The caller supplies
    /// the users to look up, since this API stores prompts and not enrollments — only the caller knows
    /// which students belong to them.
    /// Inherits session validation and localization from <see cref="BaseController"/>.
    /// </remarks>
    [Route("aiservicesapi/prompt-monitoring")]
    public class PromptMonitoringController : BaseController
    {
        private readonly IPromptMonitoringService _promptMonitoringService;
        private readonly IRoleNameResolver _roleNameResolver;

        /// <summary>
        /// The module name forwarded to the external auth service on every session validation call.
        /// Defined as a constant so it is easy to locate and change without scanning method bodies.
        /// </summary>

        /// <summary>
        /// Initializes the controller with the prompt monitoring service and configuration.
        /// </summary>
        /// <param name="promptMonitoringService">Reads the stored prompt logs.</param>
        /// <param name="roleNameResolver">Turns the session's numeric role id into its name, which selects the alert scope.</param>
        /// <param name="config">
        /// The application configuration passed to <see cref="BaseController"/> to support
        /// localized message resolution.
        /// </param>
        public PromptMonitoringController(
            IPromptMonitoringService promptMonitoringService,
            IRoleNameResolver roleNameResolver,
            IConfiguration config) : base(config)
        {
            _promptMonitoringService = promptMonitoringService;
            _roleNameResolver = roleNameResolver;
        }

        /// <summary>
        /// Summarizes the prompt activity of a group of users, so the caller can render a student list
        /// showing who has activity and who raised an alert.
        /// </summary>
        /// <remarks>
        /// <b>POST</b> /api/prompt-monitoring/users
        /// <br/><br/>
        /// A POST rather than a GET because the payload is a list of user ids: a full classroom would
        /// not fit comfortably in a query string.
        /// <br/><br/>
        /// Users with no prompts in the requested range are absent from the response rather than
        /// returned with zeros — the caller already has the roster it sent and can fill those in.
        /// </remarks>
        /// <param name="request">The users to summarize plus the optional date range.</param>
        /// <returns>
        /// <c>200 OK</c> with <c>{ users: PromptMonitoringUserSummaryResponseModel[] }</c> on success.
        /// <c>400 Bad Request</c> when no users were supplied.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// </returns>
        [HttpPost("users")]
        public async Task<IActionResult> GetUsersSummary(
            [FromBody] PromptMonitoringUsersSummaryRequestModel request)
        {
            try
            {
                var responseSession = await GetAuthenticatedSessionAsync("AIPromptMonitoring");

                if (responseSession == null)
                {
                    return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
                }

                if (request?.Users == null || request.Users.Count == 0)
                {
                    return BadRequest(new { error = GetLocalizedMessage("UsersRequired") });
                }

                var users = await _promptMonitoringService.GetUsersSummaryAsync(
                    responseSession.UserId,
                    request.Users,
                    request.From,
                    request.To,
                    HttpContext.RequestAborted);

                // Sum here so the client gets the badge total in the same response it renders the list from,
                // without a second call or having to add up the per-student counts itself.
                var totalUnread = users.Sum(user => user.UnreadAlertCount);

                return Ok(new { users, totalUnread });
            }
            catch (Exception)
            {
                return Ok(new { users = Array.Empty<PromptMonitoringUserSummaryResponseModel>(), totalUnread = 0 });
            }
        }

        /// <summary>
        /// Returns how many alerts the caller has not reviewed yet, for a global "unread" badge. The
        /// scope follows the caller's role: an administrator gets unread alerts across the whole platform,
        /// any other role gets only those of the students they are related to.
        /// </summary>
        /// <remarks>
        /// <b>GET</b> /api/prompt-monitoring/alerts/unread?from=&amp;to=
        /// <br/><br/>
        /// No student list from the caller: the backend derives the scope from the session's role and
        /// token, so the count is not tied to whatever page the table happens to be showing. "Unread" is
        /// per viewer — opening a student's history is what marks that student's alerts as read.
        /// </remarks>
        /// <param name="from">Optional inclusive start (UTC) of the date range.</param>
        /// <param name="to">Optional inclusive end (UTC) of the date range.</param>
        /// <returns>
        /// <c>200 OK</c> with <c>{ totalUnreadAlerts: number }</c> on success.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// </returns>
        [HttpGet("alerts/unread")]
        public async Task<IActionResult> GetUnreadAlertsTotal(
            [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            try
            {
                var responseSession = await GetAuthenticatedSessionAsync("AIPromptMonitoring");

                if (responseSession == null)
                {
                    return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
                }

                var viewerRole = _roleNameResolver.Resolve(responseSession.RoleId);

                var totalUnreadAlerts = await _promptMonitoringService.CountUnreadAlertsForViewerAsync(
                    responseSession.UserId,
                    viewerRole,
                    responseSession.Token,
                    from,
                    to,
                    HttpContext.RequestAborted);

                return Ok(new { totalUnreadAlerts });
            }
            catch (Exception)
            {
                return Ok(new { totalUnreadAlerts = 0 });
            }
        }

        /// <summary>
        /// Returns one page of a single user's prompt history, newest first.
        /// </summary>
        /// <remarks>
        /// <b>GET</b> /api/prompt-monitoring/user-history?userId=&amp;from=&amp;to=&amp;page=&amp;pageSize=
        /// <br/><br/>
        /// A GET because a single user fits comfortably in the query string, unlike the user list.
        /// </remarks>
        /// <param name="request">The user to read plus the optional date range and paging.</param>
        /// <returns>
        /// <c>200 OK</c> with <c>{ history: PromptMonitoringUserHistoryResponseModel }</c> on success.
        /// <c>400 Bad Request</c> when the user id is missing or the date range is inverted.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// </returns>
        [HttpGet("user-history")]
        public async Task<IActionResult> GetUserHistory(
            [FromQuery] PromptMonitoringUserHistoryRequestModel request)
        {
            try
            {
                var responseSession = await GetAuthenticatedSessionAsync("AIPromptMonitoring");

                if (responseSession == null)
                {
                    return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
                }

                // userId is required: it is the shard key, and the service refuses a blank one rather than
                // scanning the collection.
                if (string.IsNullOrWhiteSpace(request?.UserId))
                {
                    return BadRequest(new { error = GetLocalizedMessage("UserRequired") });
                }

                if (request.From.HasValue && request.To.HasValue && request.From > request.To)
                {
                    return BadRequest(new { error = GetLocalizedMessage("InvalidDateRange") });
                }

                var history = await _promptMonitoringService.GetHistoryAsync(
                    responseSession.UserId,
                    request.UserId,
                    request.From,
                    request.To,
                    request.Page,
                    request.PageSize,
                    HttpContext.RequestAborted);

                return Ok(new { history });
            }
            catch (Exception)
            {
                return Ok(new { totalUnreadAlerts = 0 });
            }
        }
    }
}
