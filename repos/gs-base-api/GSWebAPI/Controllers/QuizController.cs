using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSWebAPI.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.UserQuizzes;
using Microsoft.AspNetCore.Mvc;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle quiz details.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class QuizController : ControllerBase
    {
        /// <summary>
        /// To insert the students quiz’s results. It also returns the result’s details.
        /// </summary>
        /// <param name="generalRequestModel">Request details to insert.</param>
        /// <param name="subscriptionType">The subscription's type (H for Homeschooling, S for School).</param>
        /// <param name="customPlanQuizEvaluator">Applies the submitted quiz to the student's active custom plans (practice, exam or pre/post test and lesson completion); does nothing for students without one and never affects the saved result.</param>
        /// <param name="learningGap">1 if learning Gap, else 0</param>
        /// <returns>Returns a transaction with results details.</returns>
        [HttpPost]
        [Route("quiz/student/submit/")]
        public async Task<IActionResult> SetStudentQuizResponse(UserResponseRequestModel generalRequestModel, string subscriptionType,
            [FromServices] ICustomPlanQuizEvaluator customPlanQuizEvaluator, int learningGap = 0)
        {
            var generalResponseModel = new GameQuizResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(subscriptionType))
                {
                    return BadRequest(
                       new GeneralResponseModel()
                       {
                           Messages = new List<string>()
                           {
                               "subscription type param required",
                           },
                       });
                }

                /* Token validation in Users API */
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "QuizUserResponseCreate");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                /* End of Token Validation in Users API */
                var messageManager = new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], responseSession.LangCode);

                // Continue with the operation
                // First, we call Athenas, and we get the Quiz
                var modelObject = AthenasDetailsHandler.GetQuizById(generalRequestModel.UserResponseModelList[0].QuizId, responseSession.Token);

                var userQuizManager = new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                     AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                     responseSession.LangCode);
                var userResponseModelListToInsert = new List<GSWebAPI.Domain.Models.UserQuiz.UserResponseModel>();

                // Convert the responses
                for (int i = 0; i < generalRequestModel.UserResponseModelList.Count; i++)
                {
                    var userResponseModelToInsert = new GSWebAPI.Domain.Models.UserQuiz.UserResponseModel
                    {
                        QuestionId = generalRequestModel.UserResponseModelList[i].QuestionId,
                        ResponseId = generalRequestModel.UserResponseModelList[i].ResponseId,
                        Correct = generalRequestModel.UserResponseModelList[i].Correct,
                    };
                    userResponseModelListToInsert.Add(userResponseModelToInsert);
                }

                var response = userQuizManager.SubmitQuizResponse(
                    subscriptionType, 
                    learningGap, modelObject.QuizRequestModel.QuizModel.Id,
                    responseSession.Token, responseSession.UserId,
                    userResponseModelListToInsert, responseSession.LangCode,
                    modelObject.QuizRequestModel.SubjectCode,
                    modelObject.QuizRequestModel.LevelCode);

                // Students with an active custom plan: update the plan's practice/exam/test and the lesson. Regular students
                // have no plan, so nothing changes for them. It never affects the quiz result already saved above.
                var submission = CustomPlanQuizSubmission.From(
                    responseSession.UserId,
                    modelObject.QuizRequestModel.QuizModel.LessonId,
                    modelObject.QuizRequestModel.QuizModel.Id,
                    modelObject.QuizRequestModel.QuizModel.IsExam,
                    response?.UserQuizResultModel);
                if (submission != null)
                {
                    await customPlanQuizEvaluator.EvaluateAsync(submission, HttpContext.RequestAborted);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return error about invalid json
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Verifies if a quiz was completed before by the student.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <returns>Returns a HTTP Response with a bool.</returns>
        [HttpGet]
        [Route("quiz/worked/{quizId}")]
        public async Task<IActionResult> WasQuizWorkedBefore(string quizId)
        {
            return Ok(
                new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                     AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], null).GetUserQuizResults(
                    new UserQuizResultModel()
                    {
                        QuizId = quizId,
                    }).Count > 0);
        }
    }
}
