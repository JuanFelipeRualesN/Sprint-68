using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.FeignAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/grades")]
    public class GradeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetGrades()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var athenasGrades = await AthenasHandler.GetAthenasGradesAsync();
                return Ok(athenasGrades);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}