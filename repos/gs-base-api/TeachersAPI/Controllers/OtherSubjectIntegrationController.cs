using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Infrastructure;
using TeachersAPI;
using GSPlanning.Domain;
using GSPlanning.Services.Factories;
using GSPlanning.Infrastructure;
using MySqlConnector;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/planning-free-content/other-subject-integration")]
    public class OtherSubjectIntegrationController : ControllerBase
    {
        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetOtherSubjectIntegrations(int teacherPlanId)
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
                    var service = TeacherPlanOtherSubjectIntegrationServiceFactory.CreateService(dbConn);
                    var athenasSubjects = AthenasHandler.GetAthenasSubjects(responseSession.LangCode).ToDictionary(x => x.Id);
                    var teacherPlanOtherSubjectIntegrationByTeacherPlanId = await service.GetTeacherPlanOtherSubjectIntegrationByTeacherPlanId(teacherPlanId);

                    foreach (var integration in teacherPlanOtherSubjectIntegrationByTeacherPlanId)
                    {
                        foreach (var subject in integration.TeacherPlanOtherSubjectInegrationSubjects)
                        {
                            subject.SubjectName = athenasSubjects.TryGetValue(subject.SubjectId.ToString(), out var athenasSubject)
                                ? athenasSubject.Name
                                : string.Empty;
                        }
                    }

                    return Ok(teacherPlanOtherSubjectIntegrationByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return Ok(new List<TeacherPlanOtherSubjectIntegration>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOtherSubjectIntegration([FromBody] TeacherPlanOtherSubjectIntegration teacherPlanOtherSubjectIntegration)
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
                    var teacherPlanOtherSubjectIntegrationService = TeacherPlanOtherSubjectIntegrationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanOtherSubjectIntegration.ModifiedBy = userId;
                    teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanOtherSubjectIntegrationService.CreateTeacherPlanOtherSubjectIntegration(teacherPlanOtherSubjectIntegration);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOtherSubjectIntegration([FromBody] TeacherPlanOtherSubjectIntegration teacherPlanOtherSubjectIntegration)
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
                    var teacherPlanOtherSubjectIntegrationService = TeacherPlanOtherSubjectIntegrationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanOtherSubjectIntegration.ModifiedBy = userId;
                    teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectInegrationSubjects.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanOtherSubjectIntegration.TeacherPlanOtherSubjectIntegrationDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanOtherSubjectIntegrationService.UpdateTeacherPlanOtherSubjectIntegration(teacherPlanOtherSubjectIntegration);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveOtherSubjectIntegration(int teacherPlanId)
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
                    var teacherPlanOtherSubjectIntegrationService = TeacherPlanOtherSubjectIntegrationServiceFactory.CreateService(dbConn);
                    await teacherPlanOtherSubjectIntegrationService.RemoveTeacherPlanOtherSubjectIntegration(teacherPlanId);
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
