using Microsoft.AspNetCore.Mvc;
using System;
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
    [Route("teachersapi/planning-free-content/strategy")]
    public class StrategyController : ControllerBase
    {
        [HttpGet("types")]
        public async Task<IActionResult> GetStrategyTypes()
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
                    var service = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    var strategyTypes = await service.GetStrategyTypeTranslations(responseSession.LangCode);
                    return Ok(strategyTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("types")]
        public async Task<IActionResult> CreateStrategyTypes([FromBody] StrategyType strategyType)
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
                    var service = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    strategyType.ModifiedBy = userId;
                    var newStrategyType = await service.CreateStrategyType(strategyType);
                    return Created(string.Empty, newStrategyType);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetStrategyCategories()
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
                    var service = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    var strategyCategories = await service.GetStrategyCategoryTranslations(responseSession.LangCode);
                    return Ok(strategyCategories);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetStrategy(int teacherPlanId)
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
                    var service = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    var teacherPlanStrategyByTeacherPlanId = await service.GetTeacherPlanStrategyByTeacherPlanId(teacherPlanId, responseSession.LangCode);
                    return Ok(teacherPlanStrategyByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateStrategy([FromBody] TeacherPlanStrategy teacherPlanStrategy)
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
                    var teacherPlanStrategyService = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanStrategy.ModifiedBy = userId;
                    teacherPlanStrategy.TeacherPlanStrategyTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanStrategy.TeacherPlanStrategyDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanStrategyService.CreateTeacherPlanStrategy(teacherPlanStrategy);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStrategy([FromBody] TeacherPlanStrategy teacherPlanStrategy)
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
                    var teacherPlanStrategyService = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanStrategy.ModifiedBy = userId;
                    teacherPlanStrategy.TeacherPlanStrategyTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanStrategy.TeacherPlanStrategyDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanStrategyService.UpdateTeacherPlanStrategy(teacherPlanStrategy);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveStrategy(int teacherPlanId)
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
                    var teacherPlanStrategyService = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    await teacherPlanStrategyService.RemoveTeacherPlanStrategy(teacherPlanId);
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
