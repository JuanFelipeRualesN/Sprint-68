using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Services.Generals;
using UsersAPI.UsersAPI.Services.Metrics.UsageAndEngagement;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle the user metrics. 
    /// </summary>
    [ApiController]
    [Route("usersapi/metrics")]
    public class UsersMetricsController : ControllerBase
    {
        /// <summary>
        /// To retrieve the usage and engagement user metrics
        /// </summary>
        [HttpGet("usage-and-engagement/{userId}")]
        public async Task<IActionResult> GetUserSessionsWeekly(int userId, [FromQuery] int? weekOffset = 0)
        {
            try
            {
                string token = RequestHandler.GetToken(HttpContext);
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Ok();
                }

                var (startDate, endDate, weekNumberString, weekNumberInt) = WeeklySessionsService.GetWeekDateRange(weekOffset ?? 0);

                var summary = await WeeklySessionsService.GetUserWeeklySessions(
                    userId,
                    startDate,
                    endDate,
                    weekNumberInt);

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
