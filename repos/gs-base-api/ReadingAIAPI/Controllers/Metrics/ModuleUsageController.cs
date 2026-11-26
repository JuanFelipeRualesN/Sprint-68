using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement;

namespace ReadingAPI.Controllers.Metrics
{
    [Route("readingapi/metrics/module")]
    [ApiController]
    public class ModuleUsageController : ControllerBase
    {
        private readonly ModuleUsageService _moduleUsageService;

        public ModuleUsageController(ModuleUsageService moduleUsageService)
        {
            _moduleUsageService = moduleUsageService;
        }

        /// <summary>
        /// Returns module usage scoped to the user's role:
        /// - admin: with no filter, a region-only summary, with RegionId/SchoolId, return that region with its schools/School Info.
        /// - admin-ore: their own region with its schools.
        /// - director: their school's teachers, each with their groups and each group's usage percentage.
        /// - teacher: their connected students, each flagged as having used the module or not.
        /// </summary>
        /// <param name="request">The user request, with optional filters used by admins to narrow by region/school</param>
        /// <param name="fromPage">From page index</param>
        /// <param name="limit">Page size for schools-groups</param>
        [HttpPost("usage")]
        public async Task<IActionResult> GetModuleUsage([FromBody] UserSessionMetricsRequestDto request, [FromQuery] int fromPage = 0, [FromQuery] int limit = 10)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ReadingMetricsRead");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                {
                    return Unauthorized();
                }

                var usage = await _moduleUsageService.GetModuleUsage(
                    request ?? new UserSessionMetricsRequestDto(),
                    session.UserId, session.IsStaff, session.IsDirector, session.OreId, fromPage, limit);

                return Ok(usage);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error ocurred");
            }
        }
    }
}
