using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Metrics.ProgressAndDomain;

namespace ReadingAPI.Controllers.Metrics
{
    [Route("readingapi/metrics/performance")]
    [ApiController]
    public class PerformanceMetricsController : ControllerBase
    {
        private readonly PerformanceMetricsService _performanceMetricsService;

        public PerformanceMetricsController(PerformanceMetricsService performanceMetricsService)
        {
            _performanceMetricsService = performanceMetricsService;
        }

        /// <summary>
        /// Returns performance scores grouped by period and split into the english and spanish
        /// story variants, scoped to the requesting user's role.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">Date range to filter the results, defaults to the current calendar week</param>
        [HttpPost]
        public async Task<IActionResult> GetPerformanceSummary([FromBody] UserSessionMetricsRequestDto request, [FromQuery] MetricsPeriod period = MetricsPeriod.Week)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var summary = await _performanceMetricsService.GetPerformanceSummary(
                    request, period, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Returns quiz completion rates per reading level (k–5)
        /// scoped to the requesting user's role.
        /// </summary>
        /// <param name="request">The user request</param>
        [HttpPost("units")]
        public async Task<IActionResult> GetUnitsCompletion([FromBody] UserSessionMetricsRequestDto request)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var units = await _performanceMetricsService.GetUnitsCompletion(
                    request, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(units);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Returns the reading lessons evaluations taken by the students the requesting user
        /// is responsible for.
        /// Only teachers and client accounts (homeschooling) are allowed to read this table.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="fromPage">Zero based page index</param>
        /// <param name="limit">Results per page</param>
        /// <param name="period">Date range to filter the results, defaults to the current calendar week</param>
        [HttpPost("lessons/results")]
        public async Task<IActionResult> GetReadingLessonsResults([FromBody] ReadingLessonsResultsRequestDto request, [FromQuery] int fromPage = 0, [FromQuery] int limit = 10, [FromQuery] MetricsPeriod period = MetricsPeriod.Week)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (!IsTeacherOrHomeschooling(session.RoleId))
                    return StatusCode(StatusCodes.Status401Unauthorized, new { Message = "This report is only available for teachers and homeschooling accounts." });

                var results = await _performanceMetricsService.GetReadingLessonsResults(request, period, fromPage, limit, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Returns the reading lessons evaluations already grouped by date, used by the
        /// reading comprehension chart.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">Date range to filter the results, defaults to the current calendar week</param>
        [HttpPost("lessons/results/series")]
        public async Task<IActionResult> GetReadingLessonsSeries([FromBody] UserSessionMetricsRequestDto request, [FromQuery] MetricsPeriod period = MetricsPeriod.Week)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var series = await _performanceMetricsService.GetReadingLessonsSeries(request, period, session.UserId, session.IsStaff, session.IsDirector, session.OreId);

                return Ok(series);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." });
            }
        }

        private static bool IsTeacherOrHomeschooling(string? roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                return false;

            return roleId == AppConfiguration.Configuration["AppSettings:ur_role_teacher"]
                || roleId == AppConfiguration.Configuration["AppSettings:ur_role_client"];
        }
    }
}
