using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/pre_post")]
    public class PrePostController : ControllerBase
    {
        [HttpPost("{lessonId}/submit")]
        public async Task<IActionResult> InsertStudentGSWebResultsPrePost(string lessonId, [FromBody] PlanPrePostUserResultModel planPrePostUserResultModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AuthorizationQuizCode");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var planLessonIds = TeacherPlans.GetPlanLessonIdsAccordingToLessonAndStudents(lessonId, responseSession.UserId);
                if (planLessonIds == null || planLessonIds.Count == 0)
                {
                    return BadRequest();
                }
                var planLessonPrePostToInsert = new List<PlanPrePostUserResultModel>();
                var planLessonIdsStringList = new List<string>();
                foreach (var planLessons in planLessonIds)
                {
                    planLessonPrePostToInsert.Add(new PlanPrePostUserResultModel
                    {
                        PlanLessonId = planLessons.Id,
                        Score = planPrePostUserResultModel.Score,
                        PrePost = planPrePostUserResultModel.PrePost,
                        QuizId = planPrePostUserResultModel.QuizId,
                        UserId = responseSession.UserId,
                        UsquiUserResultId = planPrePostUserResultModel.UsquiUserResultId,
                        TotalCorrect = planPrePostUserResultModel.TotalCorrect,
                        TotalQuestions = planPrePostUserResultModel.TotalQuestions,
                    });
                    planLessonIdsStringList.Add(planLessons.Id);
                }
                if (!TeacherTryEvaluator.CheckIfUserHasPrePostTryAvailable(planLessonIdsStringList, responseSession.UserId, planPrePostUserResultModel.QuizId))
                {
                    return BadRequest();
                }
                var planningModifierManager = new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                planningModifierManager.AddPlanLessonPrePostResultModels(planLessonPrePostToInsert, responseSession.UserId);
                foreach (var planLessons in planLessonIds)
                {
                    var teacherPlanLessonQuizzes = new TeacherPlanLessonQuizDAO().Read(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], new Domain.Models.Teachers.TeacherPlanLessonQuizModel { PlanLessonId = planLessons.Id }, false, false);
                    planningModifierManager.CreateStudentPlanLessonIfNotExists(responseSession.UserId, planLessons.Id, responseSession.UserId);
                    planningModifierManager.UpdateLessonCompletionStatus(planLessons, responseSession.UserId, teacherPlanLessonQuizzes.Select(q => q.QuizId).ToList(), responseSession.UserId);
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("pre/completed/{lessonId}")]
        public async Task<IActionResult> GetLessonsPrePostTriesForStudent(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonQuizzesResponseModelJSON = AthenasHandler.GetLessonQuizzes(lessonId);
                var quizModelsToEvaluate = lessonQuizzesResponseModelJSON.Quizzes.FindAll(x => x.IsExam == "2");
                if (quizModelsToEvaluate == null || quizModelsToEvaluate.Count == 0)
                {
                    return Ok(new Domain.Models.Generals.Responses.QuizTriesLeftModel());
                }
                return Ok(TeacherTryEvaluator.GetUserLessonPrePostTriesAvailable(lessonId, responseSession.UserId, quizModelsToEvaluate[0].Id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("tries/{lessonId}/{quizId}")]
        public async Task<IActionResult> GetLessonsPrePostTriesForStudent(string lessonId, string quizId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                return Ok(TeacherTryEvaluator.GetUserLessonPrePostTriesAvailable(lessonId, responseSession.UserId, quizId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("results/{planLessonId}/{studentId}")]
        public async Task<IActionResult> GetPlanLessondPrePost(string planLessonId, string studentId)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            if (responseSession.UserId != studentId)
            {
                if (!GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token))
                {
                    return Unauthorized();
                }
            }
            return Ok(UserResults.GetPlanPrePostResults(planLessonId, studentId));
        }
    }
}