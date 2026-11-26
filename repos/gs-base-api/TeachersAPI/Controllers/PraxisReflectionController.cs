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
    [Route("teachersapi/planning-free-content/praxis-reflection")]
    public class PraxisReflectionController : ControllerBase
    {
        [HttpGet("types")]
        public async Task<IActionResult> GetPraxisReflectionTypes()
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
                    var service = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                    var praxisReflectionTypes = await service.GetPraxisReflectionTypeTranslations(responseSession.LangCode);
                    return Ok(praxisReflectionTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetPraxisReflections(int teacherPlanId)
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
                    var service = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                    var teacherPlanPraxisReflectionByTeacherPlanId = await service.GetTeacherPlanPraxisReflectionsByTeacherPlanId(teacherPlanId, responseSession.LangCode);
                    return Ok(teacherPlanPraxisReflectionByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePraxisReflection([FromBody] TeacherPlanPraxisReflection teacherPlanPraxisReflection)
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
                    var teacherPlanPraxisReflectionService = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanPraxisReflection.ModifiedBy = userId;
                    teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanPraxisReflectionService.CreateTeacherPlanPraxisReflection(teacherPlanPraxisReflection);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePraxisReflection([FromBody] TeacherPlanPraxisReflection teacherPlanPraxisReflection)
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
                    var teacherPlanPraxisReflectionService = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanPraxisReflection.ModifiedBy = userId;
                    teacherPlanPraxisReflection.TeacherPlanPraxisReflectionCompletedReasons.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanPraxisReflection.TeacherPlanPraxisReflectionDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanPraxisReflectionService.UpdateTeacherPlanPraxisReflection(teacherPlanPraxisReflection);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemovePraxisReflection(int teacherPlanId)
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
                    var teacherPlanPraxisReflectionService = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                    await teacherPlanPraxisReflectionService.RemoveTeacherPlanPraxisReflection(teacherPlanId);
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
