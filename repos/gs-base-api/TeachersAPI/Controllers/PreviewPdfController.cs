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
using GSPlanning.Services.Generals;

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
                    var teacherPlanDifferentiationStrategyService = TeacherPlanDifferentiationStrategyServiceFactory.CreateService(dbConn);
                    var teacherPlanObservationService = TeacherPlanObservationServiceFactory.CreateService(dbConn);
                    var teacherPlanCommonExperienceService = TeacherPlanCommonExperienceServiceFactory.CreateService(dbConn);
                    var teacherPlanActiveResponseService = TeacherPlanActiveResponseServiceFactory.CreateService(dbConn);
                    teacherPlanningPreviewPdfDataBuilder.AddCustomLessons(await teacherPlanCustomLessonService.GetTeacherPlanCustomLessonsByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddAccommodations(await teacherPlanAccommodationService.GetTeacherPlanAccommodationsByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddEvaluations(await teacherPlanEvaluationService.GetTeacherPlanAEvaluationByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddInitiativeProjectInnovations(await teacherPlanInitiativeProjectInnovationService.GetTeacherPlanInitiativeProjectInnovationByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddMaterials(await teacherPlanMaterialService.GetTeacherPlanAMaterialByTeacherPlanId(planId, responseSession.LangCode));
                    var teacherPlanOtherSubjectIntegrations = await teacherPlanOtherSubjectIntegrationService.GetTeacherPlanOtherSubjectIntegrationByTeacherPlanId(planId);
                    if (teacherPlanOtherSubjectIntegrations.Any())
                    {
                        var athenasSubjects = AthenasHandler.GetAthenasSubjects(responseSession.LangCode).ToDictionary(x => x.Id);
                        teacherPlanOtherSubjectIntegrations.ToList().ForEach(x => x.TeacherPlanOtherSubjectInegrationSubjects.ToList().ForEach(y => y.SubjectName = athenasSubjects[y.SubjectId.ToString()].Name));
                    }
                    teacherPlanningPreviewPdfDataBuilder.AddOtherSubjectIntegrations(teacherPlanOtherSubjectIntegrations);
                    teacherPlanningPreviewPdfDataBuilder.AddPraxisReflections(await teacherPlanPraxisReflectionService.GetTeacherPlanPraxisReflectionsByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddStratergies(await teacherPlanStrategyService.GetTeacherPlanStrategyByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddDifferentiationStrategies(await teacherPlanDifferentiationStrategyService.GetTeacherPlanDifferentiationStrategiesByTeacherPlanId(planId, responseSession.LangCode));
                    teacherPlanningPreviewPdfDataBuilder.AddObservations(await teacherPlanObservationService.GetTeacherPlanObservationByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddCommonExperiences(await teacherPlanCommonExperienceService.GetTeacherPlanCommonExperienceByTeacherPlanId(planId));
                    teacherPlanningPreviewPdfDataBuilder.AddActiveResponses(await teacherPlanActiveResponseService.GetTeacherPlanActiveResponsesByTeacherPlanId(planId, responseSession.LangCode));
                }
                var teacherPlanningPreviewPdfData = teacherPlanningPreviewPdfDataBuilder.Build(responseSession.LangCode);
                var response = new { PreviewBase64 = PDFPreviewGenerator.GeneratePreviewPdf(teacherPlanningPreviewPdfData, responseSession.LangCode) };
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
