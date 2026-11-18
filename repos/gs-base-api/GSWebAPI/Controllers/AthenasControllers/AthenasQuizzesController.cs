using Microsoft.AspNetCore.Mvc;
using GSWebAPI.GSWebAPI.Services.Athenas;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;

namespace GSWebAPI.Controllers.AthenasControllers
{
    [ApiController]
    [Route("athenasapi")]
    public class AthenasQuizzesController : ControllerBase
    {

        /// <summary>
        /// To get a quiz.
        /// </summary>
        /// <param name="quizId">The quiz’s id.</param>
        /// <param name="randomResponses">true to randomize responses, false to not.
        /// querystring ?response=0 return null in Correct & Value
        ///             ?response=1 return return real response 
        ///</param>
        /// <returns>Returns the quiz’s details.</returns>
        [Route("quizzes/quiz/{quizId}/{randomResponses}")]
        [HttpGet]
        public async Task<IActionResult> ReadQuiz(string quizId, bool randomResponses = false, string response = "1")
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var getQuizByQuizId = new AthenasQuizzesService();
                AthenasResponseLessonModel quizResponseModel = await getQuizByQuizId.GetQuizByQuizId(quizId , randomResponses);
                return Ok(quizResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
