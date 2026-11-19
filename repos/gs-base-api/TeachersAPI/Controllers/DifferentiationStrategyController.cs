using Microsoft.AspNetCore.Mvc;
using GSPlanning.Services.Factories;
using MySqlConnector;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using GSPlanning.Models;

namespace TeachersAPI.Controllers
{

    /// <summary>
    /// To handle the accommodation maintenance. 
    /// </summary>
    [ApiController]
    [Route("teachersapi/planning-free-content/differentiation_strategy")]
    public class DifferentiationStrategyController : ControllerBase
    {

        /// <summary>
        /// To get the accomodation types.
        /// </summary>
        /// <returns>Returns a list of types.</returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetDifferentiationStrategyTypes()
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
                    var service = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var differentiationStrategiesTypes = await service.GetDifferentiationStrategyTypeTranslations(responseSession.LangCode);
                    return Ok(differentiationStrategiesTypes);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateDifferentiationStrategyTypes([FromBody] DifferentiationStrategyType differentiationStrategyType)
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
                    var service = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    differentiationStrategyType.ModifiedBy = userId;
                    var newDifferentiationStrategyType = await service.CreateDifferentiationStrategyType(differentiationStrategyType);
                    return Created(string.Empty, newDifferentiationStrategyType);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetDifferentiationStrategyCategories()
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
                    var service = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var differentiationStrategyCategories = await service.GetDifferentiationStrategyCategoryTranslations(responseSession.LangCode);
                    return Ok(differentiationStrategyCategories);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetDifferentiationStrategiesByTeacherPlanId(int teacherPlanId)
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
                    var service = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var teacherPlanDifferentiationStrategyByTeacherPlanId = await service.GetTeacherPlanDifferentiationStrategiesByTeacherPlanId(teacherPlanId, responseSession.LangCode);
                    return Ok(teacherPlanDifferentiationStrategyByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacherPlanDifferentiationStrategies([FromBody] TeacherPlanDifferentiationStrategy teacherPlanDifferentiationStrategy)
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
                    var teacherPlanDifferentiationStrategyService = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanDifferentiationStrategy.ModifiedBy = userId;
                    teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanDifferentiationStrategyService.CreateTeacherPlanDifferentiationStrategy(teacherPlanDifferentiationStrategy);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacherPlanDifferentiationStrategies([FromBody] TeacherPlanDifferentiationStrategy teacherPlanDifferentiationStrategy)
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
                    var teacherPlanDifferentiationStrategyService = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanDifferentiationStrategy.ModifiedBy = userId;
                    teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanDifferentiationStrategy.TeacherPlanDifferentiationStrategyDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanDifferentiationStrategyService.UpdateTeacherPlanDifferentiationStrategy(teacherPlanDifferentiationStrategy);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveTeacherPlanDifferentiationStrategies(int teacherPlanId)
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
                    var teacherPlanDifferentiationStrategyService = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    await teacherPlanDifferentiationStrategyService.RemoveTeacherPlanDifferentiationStrategy(teacherPlanId);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private MySqlConnection CreateMySqlConnection()
        {
            var connStr = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"];
            MySqlConnection dbConn = new DBConnection(connStr).GetConnection();
            return dbConn;
        }

    }
}
