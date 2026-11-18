using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Metrics;

namespace ReadingAPI.Controllers.Metrics
{
    [Route("readingapi/metrics/student")]
    [ApiController]
    public class StudentMetricsController : ControllerBase
    {
        private readonly StudentMetricsService _studentMetricsService;

        public StudentMetricsController(StudentMetricsService studentMetricsService)
        {
            _studentMetricsService = studentMetricsService;
        }

        /// <summary>
        /// Returns all-time metrics for a student.
        /// Current streak, average usage time, total completed stories, and average slides per session.
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetStudentSummary()
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingSummaryRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId) || !int.TryParse(session.UserId, out int studentId))
                {
                    return Unauthorized();
                }

                var summary = await _studentMetricsService.GetStudentSummary(studentId);

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
