using Microsoft.AspNetCore.Mvc;
using System;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.FeignAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/subjects")]
    public class SubjectController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var athenasSubjects = AthenasHandler.GetAthenasSubjects(responseSession.LangCode);
                return Ok(athenasSubjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}