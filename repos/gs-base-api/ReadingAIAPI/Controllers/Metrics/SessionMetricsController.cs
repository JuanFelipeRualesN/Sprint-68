using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement;

namespace ReadingAPI.Controllers.Metrics
{
    [Route("readingapi/metrics/sessions")]
    [ApiController]
    public class SessionMetricsController : ControllerBase
    {
        private readonly SessionMetricsService _sessionMetricsService;

        public SessionMetricsController(SessionMetricsService sessionMetricsService)
        {
            _sessionMetricsService = sessionMetricsService;
        }

        /// <summary>
        /// Returns a single aggregated object for the current period
        /// Intended for summary indicators that don't require chart data.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">Date range to filter the results, defaults to the current calendar week</param>
        [HttpPost("totals")]
        public async Task<IActionResult> GetSessionTotals([FromBody] UserSessionMetricsRequestDto request, [FromQuery] MetricsPeriod period = MetricsPeriod.Week)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                {
                    return Unauthorized();
                }

                var totals = await _sessionMetricsService.GetSessionTotals(
                    request, period, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(totals);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Returns session metrics scoped to the requesting user's role, intended for chart data.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">Date range to filter the results, defaults to the current calendar week</param>
        [HttpPost]
        public async Task<IActionResult> GetSessionSummary([FromBody] UserSessionMetricsRequestDto request, [FromQuery] MetricsPeriod period = MetricsPeriod.Week)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                {
                    return Unauthorized();
                }

                var summary = await _sessionMetricsService.GetSessionSummary(
                    request, period, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(summary);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}
