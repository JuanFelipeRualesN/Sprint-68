using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Infrastructure;
using GSPlanning.Domain;
using GSPlanning.Services.Factories;
using GSPlanning.Infrastructure;
using MySqlConnector;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/planning-free-content/initiative-project-innovation")]
    public class InitiativeProjectInnovationController : ControllerBase
    {
        [HttpGet("types")]
        public async Task<IActionResult> GetInitiativeProjectInnovationTypes()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                using (var dbConn = CreateMySqlConnection())
                {
                    var service = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    var initiativeProjectInnovationTypes = await service.GetInitiativeProjectInnovationTypeTranslations(responseSession.LangCode);
                    return Ok(initiativeProjectInnovationTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateInitiativeProjectInnovationTypes([FromBody] InitiativeProjectInnovationType initiativeProjectInnovationType)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                using (var dbConn = CreateMySqlConnection())
                {
                    var service = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    initiativeProjectInnovationType.ModifiedBy = userId;
                    var newInitiativeProjectInnovationTypes = await service.CreateInitiativeProjectInnovationType(initiativeProjectInnovationType);
                    return Created(string.Empty, newInitiativeProjectInnovationTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetInitiativeProjectInnovations(int teacherPlanId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                using (var dbConn = CreateMySqlConnection())
                {
                    var service = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    var teacherPlanInitiativeProjectInnovationByTeacherPlanId = await service.GetTeacherPlanInitiativeProjectInnovationByTeacherPlanId(teacherPlanId, responseSession.LangCode);
                    return Ok(teacherPlanInitiativeProjectInnovationByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInitiativeProjectInnovation([FromBody] TeacherPlanInitiativeProjectInnovation teacherPlanInitiativeProjectInnovation)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                using (var dbConn = CreateMySqlConnection())
                {
                    var teacherPlanInitiativeProjectInnovationService = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanInitiativeProjectInnovation.ModifiedBy = userId;
                    teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanInitiativeProjectInnovationService.CreateTeacherPlanInitiativeProjectInnovation(teacherPlanInitiativeProjectInnovation);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInitiativeProjectInnovation([FromBody] TeacherPlanInitiativeProjectInnovation teacherPlanInitiativeProjectInnovation)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                using (var dbConn = CreateMySqlConnection())
                {
                    var teacherPlanInitiativeProjectInnovationService = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanInitiativeProjectInnovation.ModifiedBy = userId;
                    teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanInitiativeProjectInnovation.TeacherPlanInitiativeProjectInnovationDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanInitiativeProjectInnovationService.UpdateTeacherPlanInitiativeProjectInnovation(teacherPlanInitiativeProjectInnovation);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveInitiativeProjectInnovation(int teacherPlanId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                using (var dbConn = CreateMySqlConnection())
                {
                    var teacherPlanInitiativeProjectInnovationService = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    await teacherPlanInitiativeProjectInnovationService.RemoveTeacherPlanInitiativeProjectInnovation(teacherPlanId);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private static MySqlConnection CreateMySqlConnection()
        {
            MySqlConnection dbConn = new DBConnection(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"]).GetConnection();
            return dbConn;
        }
    }
}
