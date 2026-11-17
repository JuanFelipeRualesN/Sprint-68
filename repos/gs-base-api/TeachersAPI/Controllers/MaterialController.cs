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
    [Route("teachersapi/planning-free-content/material")]
    public class MaterialController : ControllerBase
    {
        [HttpGet("types")]
        public async Task<IActionResult> GetMaterialTypes()
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
                    var service = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    var materialTypes = await service.GetMaterialTypes();
                    return Ok(materialTypes);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateMaterialTypes([FromBody] MaterialType materialType)
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
                    var service = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    materialType.ModifiedBy = userId;
                    var newMaterialType = await service.CreateEvaluationType(materialType);
                    return Created(string.Empty, newMaterialType);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{teacherPlanId}")]
        public async Task<IActionResult> GetMaterials(int teacherPlanId)
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
                    var service = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    var teacherPlanMaterialByTeacherPlanId = await service.GetTeacherPlanAMaterialByTeacherPlanId(teacherPlanId);
                    return Ok(teacherPlanMaterialByTeacherPlanId);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromBody] TeacherPlanMaterial teacherPlanMaterial)
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
                    var teacherPlanMaterialService = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanMaterial.ModifiedBy = userId;
                    teacherPlanMaterial.TeacherPlanMaterialTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanMaterial.TeacherPlanMaterialDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanMaterialService.CreateTeacherPlanMaterial(teacherPlanMaterial);
                    return Created(string.Empty, new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMaterial([FromBody] TeacherPlanMaterial teacherPlanMaterial)
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
                    var teacherPlanMaterialService = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    var userId = int.Parse(responseSession.UserId);
                    teacherPlanMaterial.ModifiedBy = userId;
                    teacherPlanMaterial.TeacherPlanMaterialTypes.ToList().ForEach(x => x.ModifiedBy = userId);
                    teacherPlanMaterial.TeacherPlanMaterialDates.ToList().ForEach(x => x.ModifiedBy = userId);
                    await teacherPlanMaterialService.UpdateTeacherPlanMaterial(teacherPlanMaterial);
                    return Ok(new { IsValid = true });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teacherPlanId}")]
        public async Task<IActionResult> RemoveMaterial(int teacherPlanId)
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
                    var teacherPlanMaterialService = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    await teacherPlanMaterialService.RemoveTeacherPlanMaterial(teacherPlanId);
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
