using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Domain.DTOs.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement;

namespace ReadingAPI.Controllers
{
    /// <summary>
    /// User session controller for handling usage metrics.
    /// </summary>
    [Route("readingapi/tracking")]
    [ApiController]
    public class SessionTrackerController : ControllerBase
    {
        private readonly SessionTrackerService _sessionTrackerService;

        public SessionTrackerController(SessionTrackerService sessionTrackerService)
        {
            _sessionTrackerService = sessionTrackerService;
        }

        /// <summary>
        /// Records a session event.
        /// </summary>
        /// <param name="request">The request containing the Session ID</param>
        [HttpPost("events")]
        public async Task<IActionResult> RecordEvent([FromBody] SessionEventRequestDto request)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumRead");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var userId = responseSession.UserId;

            // Record the session event
            var status = await _sessionTrackerService.RecordSessionEvent(
                request.SessionId,
                userId,
                request.EventType
            );

            // If the Session ID does not belong to the user (Unauthorized access)
            if (status == SessionStatus.Unauthorized)
            {
                return Unauthorized(); 
            }

            // If the provided Session ID does not exist in the DB returns Not Found
            if (status == SessionStatus.NotFound)
            {
                return NotFound();
            }

            // If the session already has an "end" event
            if (status == SessionStatus.Closed)
            {
                return BadRequest(new { Message = "Session is already closed." });
            }

            // If the client to insert an invalid event
            if (status == SessionStatus.InvalidEventType)
            {
                return BadRequest(new { Message = "Invalid event type." });
            }

            if (status == SessionStatus.Error)
            {
                return StatusCode(500, new { Message = "Internal server error." });
            }

            return Ok(new { Message = "Event recorded" });
        }

        /// <summary>
        /// Initiates a new user session for usage metric tracking.
        /// </summary>
        /// <param name="request">The request containing the device type for the session.</param>
        /// <returns>The Session ID</returns>
        [HttpPost("start-session")]
        public async Task<IActionResult> StartUserSession([FromBody] UserSessionRawEntity request)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ForumRead");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var userId = responseSession.UserId;

            int isSchoolHours = _sessionTrackerService.CalculateIsSchoolHours();

            // Insert the new raw session and return the Session ID
            var newSessionId = await _sessionTrackerService.InsertUserSessionRaw(
                userId,
                request.DeviceType,
                isSchoolHours
            );

            if (newSessionId > 0)
            {
                return Ok(new { SessionId = newSessionId });
            }

            return StatusCode(500, new { Message = "Internal error creating the new metrics session record." });
        }
    }
}
