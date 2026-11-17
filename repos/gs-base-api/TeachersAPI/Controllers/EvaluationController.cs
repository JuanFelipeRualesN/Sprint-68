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
    /// To handle the evaluation maintenance.
    /// </summary>
    [ApiController]
    [Route("teachersapi/planning-free-content/evaluation")]
    public class EvaluationController : ControllerBase
    {
        [HttpGet("types")]
        public async Task<IActionResult> GetEvaluationTypes()
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
                    var service = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    var evaluationTypes = await service.GetEvaluationTypes();
                    return Ok(evaluationTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateEvaluationTypes([FromBody] EvaluationType evaluationType)
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
                    var service = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    evaluationType.ModifiedBy = userId;
                    var newEvaluationType = await service.CreateEvaluationType(evaluationType);
                    return Created(string.Empty, newEvaluationType);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetEvaluations(int teacherPlanId)
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
                    var service = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    var teacherPlanEvaluationByTeacherPlanId = await service.GetTeacherPlanAEvaluationByTeacherPlanId(teacherPlanId);
                    return Ok(teacherPlanEvaluationByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvaluation([FromBody] TeacherPlanEvaluation teacherPlanEvaluation)
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
                    var teacherPlanEvaluationService = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanEvaluation.ModifiedBy = userId;
                    teacherPlanEvaluation.TeacherPlanEvaluationTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanEvaluation.TeacherPlanEvaluationsDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanEvaluationService.CreateTeacherPlanEvaluation(teacherPlanEvaluation);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvaluation([FromBody] TeacherPlanEvaluation teacherPlanEvaluation)
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
                    var teacherPlanEvaluationService = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanEvaluation.ModifiedBy = userId;
                    teacherPlanEvaluation.TeacherPlanEvaluationTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanEvaluation.TeacherPlanEvaluationsDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanEvaluationService.UpdateTeacherPlanEvaluation(teacherPlanEvaluation);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveEvaluation(int teacherPlanId)
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
                    var teacherPlanEvaluationService = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    await teacherPlanEvaluationService.RemoveTeacherPlanEvaluation(teacherPlanId);
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
