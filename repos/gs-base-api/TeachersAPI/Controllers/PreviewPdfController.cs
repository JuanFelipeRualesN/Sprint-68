using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.CustomClasses;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.Domain.Models.Generals.Responses;
using GSPlanning.Services.Factories;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.TeachersAPI.Infrastructure;
using TeachersAPI.TeachersAPI.Services.PlanningPreview;
using TeachersAPI.Domain.Models.Teachers.Preview;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/planning/{teacherPlanId}/preview/pdf")]
    public class PreviewPdfController : ControllerBase
    {
        [HttpGet("generate")]
        public async Task<IActionResult> GeneratePdf(string teacherPlanId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var teacherPlanningDetailResponseModel = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode)
                    .GetTeacherPlanFullDetails(teacherPlanId, responseSession.UserId, RequestHandler.GetToken(HttpContext));
                if (teacherPlanningDetailResponseModel.TeacherPlanModel != null)
                {
                    teacherPlanningDetailResponseModel.TeacherPlanModel.TeacherFullName = responseSession.FullName;
                }
                var teacherPlanLessonFullDetailModels = (List<TeacherPlanLessonFullDetailModel>)PlanningReaderManager.GetPlanningLessonsWithQuizzes(teacherPlanId)["QuizzesItems"];
                string[] lessonIds = teacherPlanningDetailResponseModel.LessonAttachedModelList.Select(x => x.ItemId).ToArray();
                var lessonResponseModels = AthenasHandler.GetLessonsDetail(RequestHandler.GetToken(HttpContext), lessonIds);
                var forumsResponseModel = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode)
                    .ReadTeacherForums(token: responseSession.Token, ipAddress: responseSession.IpAddress, teacherPlanId: teacherPlanId, fromPage: 0, limiter: 200);
                var teacherPlanningPreviewPdfDataBuilder = TeacherPlanningPreviewPdfDataBuilder.Builder
                    .AddLessonModels(lessonResponseModels)
                    .AddTeacherPlanningDetail(teacherPlanningDetailResponseModel)
                    .AddTeacherPlanLessonFullDetail(teacherPlanLessonFullDetailModels)
                    .AddForums(forumsResponseModel.ForumModelList);
                using (var dbConn = CreateMySqlConnection())
                {
                    var planId = int.Parse(teacherPlanId);
                    var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                    var teacherPlanAccommodationService = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                    var teacherPlanEvaluationService = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                    var teacherPlanInitiativeProjectInnovationService = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                    var teacherPlanMaterialService = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                    var teacherPlanOtherSubjectIntegrationService = TeacherPlanOtherSubjectIntegrationServiceFactory.CreateService(dbConn);
                    var teacherPlanPraxisReflectionService = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                    var teacherPlanStrategyService = TeacherPlanStratergyServiceFactory.CreateService(dbConn);
                    teacherPlanningPreviewPdfDataBuilder.AddCustomLessons(await teacherPlanCustomLessonService.GetTeacherPlanCustomLessonsByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddAccommodations(await teacherPlanAccommodationService.GetTeacherPlanAccommodationsByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddEvaluations(await teacherPlanEvaluationService.GetTeacherPlanAEvaluationByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddInitiativeProjectInnovations(await teacherPlanInitiativeProjectInnovationService.GetTeacherPlanInitiativeProjectInnovationByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddMaterials(await teacherPlanMaterialService.GetTeacherPlanAMaterialByTeacherPlanId(planId));
                    var teacherPlanOtherSubjectIntegrations = await teacherPlanOtherSubjectIntegrationService.GetTeacherPlanOtherSubjectIntegrationByTeacherPlanId(planId);
                    if (teacherPlanOtherSubjectIntegrations.Any())
                    {
                        var athenasSubjects = AthenasHandler.GetAthenasSubjects(responseSession.LangCode).ToDictionary(x => x.Id);
                        teacherPlanOtherSubjectIntegrations.ToList().ForEach(x => x.TeacherPlanOtherSubjectInegrationSubjects.ToList().ForEach(y => y.SubjectName = athenasSubjects[y.SubjectId.ToString()].Name));
                    }
                    teacherPlanningPreviewPdfDataBuilder.AddOtherSubjectIntegrations(teacherPlanOtherSubjectIntegrations);
                    teacherPlanningPreviewPdfDataBuilder.AddPraxisReflections(await teacherPlanPraxisReflectionService.GetTeacherPlanPraxisReflectionsByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddStratergies(await teacherPlanStrategyService.GetTeacherPlanStrategyByTeacherPlanId(planId));
                }
                var teacherPlanningPreviewPdfData = teacherPlanningPreviewPdfDataBuilder.Build();
                var response = new { PreviewBase64 = PDFPreviewGenerator.GeneratePreviewPdf(teacherPlanningPreviewPdfData) };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private static MySqlConnection CreateMySqlConnection()
        {
            MySqlConnection dbConn = new DBConnection(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"]).GetConnection();
            return dbConn;
        }
    }
}
