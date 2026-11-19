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
using GSPlanning.Models;
using GSPlanning.Services.Generals;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/planning-free-content/common-experiences")]
    public class CommonExperiencesController: ControllerBase
    {
        [HttpGet("types")]
        public async Task<IActionResult> GetTeacherPlanCommonExperienceTypes()
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
                    var service = TeacherPlanCommonExperienceServiceFactory.CreateService(dbConn);
                    var teacherPlanCommonExperienceTypes = await service.GetTeacherPlanCommonExperienceTypes();
                    return Ok(teacherPlanCommonExperienceTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetTeacherPlanCommonExperience(int teacherPlanId)
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
                    var service = TeacherPlanCommonExperienceServiceFactory.CreateService(dbConn);
                    var teacherPlanCommonExperienceByTeacherPlanId = await service.GetTeacherPlanCommonExperienceByTeacherPlanId(teacherPlanId);
                    return Ok(teacherPlanCommonExperienceByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommonExperience([FromBody] TeacherPlanCommonExperience teacherPlanCommonExperience)
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
                    var teacherPlanCommonExperienceService = TeacherPlanCommonExperienceServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanCommonExperience.ModifiedBy = userId;
                    teacherPlanCommonExperience.TeacherPlanCommonExperienceDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanCommonExperienceService.CreateTeacherPlanCommonExperience(teacherPlanCommonExperience);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacherPlanCommonExperience([FromBody] TeacherPlanCommonExperience teacherPlanCommonExperience)
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
                    var teacherPlanCommonExperienceService = TeacherPlanCommonExperienceServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanCommonExperience.ModifiedBy = userId;
                    teacherPlanCommonExperience.TeacherPlanCommonExperienceDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanCommonExperienceService.UpdateTeacherPlanCommonExperience(teacherPlanCommonExperience);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveTeacherPlanCommonExperience(int teacherPlanId)
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
                    var teacherPlanCommonExperienceService = TeacherPlanCommonExperienceServiceFactory.CreateService(dbConn);
                    await teacherPlanCommonExperienceService.RemoveTeacherPlanCommonExperience(teacherPlanId);
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
