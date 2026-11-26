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
    /// <summary>
    /// To handle the custom lesson maintenance.
    /// </summary>
    [ApiController]
    [Route("teachersapi/planning-free-content/custom-lesson")]
    public class CustomLessonController : ControllerBase
    {
        [HttpGet("criterias")]
        public async Task<IActionResult> GetCustomLessonEducationalCriteria()
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
                    var service = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    var educationalCriterias = await service.GetEducationCriteriaTranslations(responseSession.LangCode);
                    return Ok(educationalCriterias);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("criterias")]
        public async Task<IActionResult> CreateEducationalCriteria([FromBody] EducationalCriteria educationalCriteria)
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
                    var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    educationalCriteria.ModifiedBy = userId;
                    var criteria = await teacherPlanCustomLessonService.CreateEducationCriteria(educationalCriteria);
                    return Created(string.Empty, criteria);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("criterias/{teacherPlanCustomLessonId}/{educationalCriteriaId}")]
        public async Task<IActionResult> RemoveEducationalCriteria(int teacherPlanCustomLessonId, int educationalCriteriaId)
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
                    var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    await teacherPlanCustomLessonService.RemoveEducationCriteria(teacherPlanCustomLessonId, educationalCriteriaId);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetCustomLessons(int teacherPlanId)
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
                    var service = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    var teacherPlanCustomLessonByTeacherPlanId = await service.GetTeacherPlanCustomLessonsByTeacherPlanId(teacherPlanId, responseSession.LangCode);
                    return Ok(teacherPlanCustomLessonByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomLesson([FromBody] TeacherPlanCustomLesson teacherPlanCustomLesson)
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
                    var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanCustomLesson.ModifiedBy = userId;
                    teacherPlanCustomLesson.TeacherPlanCustomLessonEducationalCriterias.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanCustomLesson.TeacherPlanCustomLessonDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanCustomLessonService.CreateTeacherPlanCustomLesson(teacherPlanCustomLesson);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomLesson([FromBody] TeacherPlanCustomLesson teacherPlanCustomLesson)
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
                    var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanCustomLesson.ModifiedBy = userId;
                    teacherPlanCustomLesson.TeacherPlanCustomLessonEducationalCriterias.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanCustomLesson.TeacherPlanCustomLessonDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanCustomLessonService.UpdateTeacherPlanCustomLesson(teacherPlanCustomLesson);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveCustomLesson(int teacherPlanId)
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
                    var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    await teacherPlanCustomLessonService.RemoveTeacherPlanCustomLesson(teacherPlanId);
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
            // You may need to refactor this for ASP.NET Core DI
            MySqlConnection dbConn = new DBConnection(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"]).GetConnection();
            return dbConn;
        }
    }
}
