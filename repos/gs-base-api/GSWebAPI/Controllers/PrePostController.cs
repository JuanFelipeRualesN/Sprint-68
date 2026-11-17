using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.UserQuizzes;
using Microsoft.AspNetCore.Mvc;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle pre & posts related requests.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class PrePostController : ControllerBase
    {
        /// <summary>
        /// For STUDENTS:To check if the pre was completed.
        /// </summary>
        /// <param name="lessonId">The lesson's id to check.</param>
        /// <returns>Returns the lesson quiz tries.</returns>
        [HttpGet]
        [Route("pre_post/pre/completed/{lessonId}/")]
        public async Task<IActionResult> GetLessonsPrePostTriesForStudent(string lessonId)
        {
            try
            {
                // Check Token, and if user can be connected with the user or not.
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "QuizUserResponseCreate");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get Athenas quizzes and find the pre
                // Get the quizzess
                LessonQuizzesResponseModel lessonQuizzesResponseModelJSON = AthenasDetailsHandler.GetLessonQuizzes(lessonId);

                // Identify the pre
                List<QuizModel> quizModelsToEvaluate = lessonQuizzesResponseModelJSON.Quizzes.FindAll(x => x.IsExam == "2");

                // Now check if its avaiable
                if (quizModelsToEvaluate == null || quizModelsToEvaluate.Count == 0)
                {
                    return Ok(new QuizTriesLeftModel());
                }

                return Ok(QuizTriesCheckManager.CanMakePrePost(responseSession.UserId, quizModelsToEvaluate[0].Id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
