using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Services.Factories;
using MySqlConnector;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Infrastructure;
using TeachersAPI.TeachersAPI.Infrastructure;


namespace TeachersAPI.Controllers
{
    /// <summary>
    /// To handle the accommodation maintenance. 
    /// </summary>
    [ApiController]
    [Route("teachersapi/planning-free-content/accommodation")]
    public class AccommodationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccommodationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAccommodationTypes()
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
                    var service = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    var accommodationTypes = await service.GetAccommodationTypes();
                    return Ok(accommodationTypes);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateAccommodationTypes([FromBody] AccommodationType accommodationType)
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
                    var service = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    accommodationType.ModifiedBy = userId;
                    var newAccommodationType = await service.CreateAccommodationType(accommodationType);
                    return Created(string.Empty, newAccommodationType);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetAccommodations(int teacherPlanId)
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
                    var service = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    var teacherPlanAccommodationByTeacherPlanId = await service.GetTeacherPlanAccommodationsByTeacherPlanId(teacherPlanId);
                    return Ok(teacherPlanAccommodationByTeacherPlanId);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccommodation([FromBody] TeacherPlanAccommodation teacherPlanAccommodation)
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
                    var teacherPlanAccommodationService = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanAccommodation.ModifiedBy = userId;
                    teacherPlanAccommodation.TeacherPlanAccommodationTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanAccommodation.TeacherPlanAccommodationDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanAccommodationService.CreateTeacherPlanAccommodation(teacherPlanAccommodation);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccommodation([FromBody] TeacherPlanAccommodation teacherPlanAccommodation)
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
                    var teacherPlanAccommodationService = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanAccommodation.ModifiedBy = userId;
                    teacherPlanAccommodation.TeacherPlanAccommodationTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanAccommodation.TeacherPlanAccommodationDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanAccommodationService.UpdateTeacherPlanAccommodation(teacherPlanAccommodation);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveAccommodation(int teacherPlanId)
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
                    var teacherPlanAccommodationService = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    await teacherPlanAccommodationService.RemoveTeacherPlanAccommodation(teacherPlanId);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private MySqlConnection CreateMySqlConnection()
        {
            var connStr = _configuration.GetConnectionString("TeacherWebAPIConnectionString");
            MySqlConnection dbConn = new DBConnection(connStr).GetConnection();
            return dbConn;
        }
    }
}
