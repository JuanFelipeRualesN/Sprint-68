using Microsoft.AspNetCore.Mvc;
using System;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// To handle the time zones that will be used when assigning an exam, that should have a specific date and time, including timezone. 
    /// </summary>
    [ApiController]
    [Route("teachersapi/timezone")]
    public class TimeZoneController : ControllerBase
    {
        /// <summary>
        /// To get the timezones list.
        /// </summary>
        /// <returns>Returns the list of timezones.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTimeZones()
        {
            try
            {
                // Check access
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                } 
                // Return error about invalid request
                return Ok(TimeZoneReader.GetTimeZones());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
