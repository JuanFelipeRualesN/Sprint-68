using Microsoft.AspNetCore.Mvc;
using GSPlanning.Services.Factories;
using MySqlConnector;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using GSPlanning.Models;
using MySqlConnector;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// To handle the accommodation maintenance. 
    /// </summary>
    [ApiController]
    [Route("teachersapi/planning-free-content/active-response")]
    public class ActiveResponseController : ControllerBase
    {

        /// <summary>
        /// To get the accomodation types.
        /// </summary>
        /// <returns>Returns a list of types.</returns>
        [HttpGet("types")] 
        public async Task<IActionResult> GetActiveResponseTypes()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                //if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                //{
                //    return Unauthorized();
                //}

                using (var dbConn = CreateMySqlConnection())
                {
                    var service = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    var activeResponsesTypes = await service.GetActiveResponseTypeTranslations(responseSession.LangCode);
                    return Ok(activeResponsesTypes);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateActiveResponseTypes([FromBody] ActiveResponseType activeResponseType)
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
                    var service = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    activeResponseType.ModifiedBy = userId;
                    var newActiveResponseType = await service.CreateActiveResponseType(activeResponseType);
                    return Created(string.Empty, newActiveResponseType);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("includes")]
        public async Task<IActionResult> GetTeacherPlanActiveResponseIncludes()
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
                    var service = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    var activeResponseIncludes = await service.GetTeacherPlanActiveResponseIncludes();
                    return Ok(activeResponseIncludes);
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
                    var service = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    var teacherPlanActiveResponseByTeacherPlanId = await service.GetTeacherPlanActiveResponsesByTeacherPlanId(teacherPlanId, responseSession.LangCode);
                    return Ok(teacherPlanActiveResponseByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacherPlanDifferentiationStrategies([FromBody] TeacherPlanActiveResponse teacherPlanActiveResponse)
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
                    var teacherPlanActiveResponseService = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanActiveResponse.ModifiedBy = userId;
                    teacherPlanActiveResponse.TeacherPlanActiveResponseTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanActiveResponse.TeacherPlanActiveResponseDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanActiveResponseService.CreateTeacherPlanActiveResponse(teacherPlanActiveResponse);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacherPlanDifferentiationStrategies([FromBody] TeacherPlanActiveResponse teacherPlanActiveResponse)
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
                    var teacherPlanActiveResponseService = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanActiveResponse.ModifiedBy = userId;
                    teacherPlanActiveResponse.TeacherPlanActiveResponseTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanActiveResponse.TeacherPlanActiveResponseDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanActiveResponseService.UpdateTeacherPlanActiveResponse(teacherPlanActiveResponse);
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
                    var teacherPlanActiveResponseService = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    await teacherPlanActiveResponseService.RemoveTeacherPlanActiveResponse(teacherPlanId);
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
