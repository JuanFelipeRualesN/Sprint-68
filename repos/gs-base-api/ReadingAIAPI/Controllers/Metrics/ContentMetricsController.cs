using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Metrics.ReadingAndContent;

namespace ReadingAPI.Controllers.Metrics
{
    [Route("readingapi/metrics/content")]
    [ApiController]
    public class ContentMetricsController : ControllerBase
    {
        private readonly ContentMetricsService _contentMetricsService;

        public ContentMetricsController(ContentMetricsService contentMetricsService)
        {
            _contentMetricsService = contentMetricsService;
        }

        /// <summary>
        /// Returns reading counts per category and average story completion time for the selected period,
        /// split into the english and spanish story variants and scoped to the requesting user's role
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">Date range to filter the results, defaults to the current calendar week</param>
        [HttpPost]
        public async Task<IActionResult> GetContentSummary([FromBody] UserSessionMetricsRequestDto request, [FromQuery] MetricsPeriod period = MetricsPeriod.Week)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var summary = await _contentMetricsService.GetContentSummary(
                    request, period, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(summary);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}
