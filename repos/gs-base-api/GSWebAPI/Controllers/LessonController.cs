using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.Lessons;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.UserQuizzes;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle details about the lessons from Genial API to Athenas API.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class LessonController : ControllerBase
    {
        /// <summary>
        /// To get the lesson general details
        /// </summary>
        /// <param name="lessonId">The id of the lesson to call.</param>
        /// <returns>Returns the lesson details.</returns>
        [HttpGet]
        [Route("lessons/{lessonId}/")]
        public async Task<IActionResult> GetLessonGeneralDetails(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var modelObject = AthenasDetailsHandler.GetLessonById(lessonId);
                return Ok(modelObject.LessonModifierRequestModel.LessonModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// To get the lesson's concept.
        /// </summary>
        /// <param name="lessonId">Id of the lesson.</param>
        /// <returns>Returns the lesson concept.</returns>
        [HttpGet]
        [Route("lessons/{lessonId}/concept")]
        public async Task<IActionResult> GetLessonConcept(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var modelObject = AthenasDetailsHandler.GetLessonById(lessonId);
                return Ok(new LessonConceptResponseModel
                {
                    ClassicFormat = modelObject.LessonModifierRequestModel.ClassicFormat,
                    Concept = modelObject.LessonModifierRequestModel.LessonDetailModel.Concept,
                    LessonLangCode = modelObject.LessonLangCode,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the lesson definitions' list.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson's definitions list. </returns>
        [HttpGet]
        [Route("lessons/{lessonId}/definitions")]
        public async Task<IActionResult> GetLessonDefinitions(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var modelObject = AthenasDetailsHandler.GetLessonById(lessonId);
                return Ok(new LessonDefinitionResponseModel
                {
                    IsDefinitionALink = modelObject.IsDefinitionALink,
                    LessonDefinitionModelList = modelObject.LessonModifierRequestModel.LessonDefinitionModelList,
                    LessonLangCode = modelObject.LessonLangCode,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the lesson examples' list.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson's examples list. </returns>
        [HttpGet]
        [Route("lessons/{lessonId}/examples")]
        public async Task<IActionResult> GetLessonExamples(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var modelObject = AthenasDetailsHandler.GetLessonById(lessonId);
                return Ok(new LessonExampleRsponseModel
                {
                    IsExampleALink = modelObject.IsExampleALink,
                    LessonExampleModelList = modelObject.LessonModifierRequestModel.LessonExampleModelList,
                    LessonLangCode = modelObject.LessonLangCode,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the lesson practices' list.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson's practices list. </returns>
        [HttpGet]
        [Route("lessons/{lessonId}/practices")]
        public async Task<IActionResult> GetLessonPractices(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonQuizzesResponseModelJSON = AthenasDetailsHandler.GetLessonQuizzes(lessonId);
                var quizzes = lessonQuizzesResponseModelJSON.Quizzes.FindAll(x => x.IsExam == "0");
                if (quizzes.Any())
                {
                    quizzes = quizzes.OrderBy(x => x.LevelOfKnowledge).ThenBy(x => x.Title).ToList();
                }
                return Ok(quizzes);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the lesson exams.
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("lessons/{lessonId}/exams")]
        public async Task<IActionResult> GetLessonExams(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonQuizzesResponseModelJSON = AthenasDetailsHandler.GetLessonQuizzes(lessonId);
                List<QuizModel> quizModelsToEvaluate = lessonQuizzesResponseModelJSON.Quizzes.FindAll(x => x.IsExam != "0").ToList();
                List<UserQuizResultModel> userQuizResults = UserResults.GetUserQuizResults(responseSession.UserId, lessonId);

                var isUserHomeschool = SubscriptionHandler.CheckIStudentIsHomeschool(responseSession.Token);

                // Also get the max rewards
                List<string> quizIdsToFindRewards = new List<string>();
                foreach (UserQuizResultModel userQuizResult in userQuizResults)
                {
                    quizIdsToFindRewards.Add(userQuizResult.QuizId);
                }
                List<UserRewardMaxPricePerQuizModel> userRewardMaxPricePerQuizModelsList = UserResults.GetMaxRewardsByQuiz(responseSession.UserId, quizIdsToFindRewards);

                // Seggregate the exams
                List<QuizModel> quizModelsExams = lessonQuizzesResponseModelJSON.Quizzes.FindAll(x => x.IsExam != "1").ToList();

                // Now prepare the response.
                List<QuizResponseModel> quizResponseModelList = new List<QuizResponseModel>();
                foreach (QuizModel quizModel in quizModelsToEvaluate)
                {
                    QuizResponseModel quizResponseModel = new QuizResponseModel()
                    {
                        Title = quizModel.Title,
                        Id = quizModel.Id,
                        LevelOfKnowledge = quizModel.LevelOfKnowledge,
                        IsExam = quizModel.IsExam,
                    };
                    if (isUserHomeschool)
                    {
                        quizResponseModel.DateAvailable = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        quizResponseModel.AvailableForUser = true;
                        quizResponseModel.DueDateTimezone = TimeZoneInfo.Local.Id;
                        quizResponseModel.Duration = 1440;
                        quizResponseModel.DateTimeUTCNow = DateTime.UtcNow;
                        quizResponseModel.DateTimeConvertedUTCFrom = DateTime.UtcNow;
                        quizResponseModel.DateTimeConvertedUTCTo = DateTime.UtcNow;
                    }
                    List<UserQuizResultModel> quizResults = userQuizResults.FindAll(x => x.QuizId == quizModel.Id);

                    // If the quiz id is repeated, take the highest totalpercent per quiz. 
                    if (quizResults != null && quizResults.Count > 0)
                    {
                        quizResults = quizResults
                         .GroupBy(x => x.QuizId)
                         .Select(g =>
                             g.OrderByDescending(x =>
                             {
                                 double percent;
                                 return double.TryParse(x.TotalPercent, out percent) ? percent : 0;
                             }).First()
                         ).ToList();
                    }
                    if (quizModel.IsExam == "2")
                    {
                        quizResponseModel.AvailableForUser = quizResults == null || quizResults.Count == 0;
                    }
                    else if (quizModel.IsExam == "1") // If it is a regular exam.
                    {
                        // We chack if there are no results before and if the pre test has being made.
                        if ((quizResults == null || quizResults.Count == 0) && userQuizResults.FindAll(x => x.IsExam == "2") != null)
                        {
                            quizResponseModel.AvailableForUser = true;
                        }
                        // If homeschool, then yes. 
                        if (isUserHomeschool)
                        {
                            quizResponseModel.AvailableForUser = true;
                        }
                    }
                    else if (quizModel.IsExam == "3") // If it is a post exam
                    {
                        bool allowPostTest = true;
                        if (quizResults != null && quizResults.Count > 0)
                        {
                            allowPostTest = false;
                        }
                        else
                        {
                            // Check if all the exams were completed.
                            // Check if the quizzess of Athenas exists on the results
                            foreach (QuizModel quizModelToCheck in quizModelsExams)
                            {
                                if (userQuizResults.FindAll(x => x.QuizId == quizModelToCheck.Id) == null)
                                {
                                    allowPostTest = false;
                                    break;
                                }
                            }
                        }
                        quizResponseModel.AvailableForUser = allowPostTest;
                    }
                    if (quizResults != null && quizResults.Count > 0)
                    {
                        // Get last quiz
                        UserQuizResultModel quizResult = quizResults.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).First();
                        double score = double.Parse(quizResult.TotalPercent);
                        quizResponseModel.HasUserWorkIn = true;
                        quizResponseModel.Passed = score >= 70 ? true : false;
                        quizResponseModel.TotalScore = score;
                        quizResponseModel.FinishedByUser = true;
                        quizResponseModel.GotDiamonds = userRewardMaxPricePerQuizModelsList.Find(x => x.QuizId == quizModel.Id) != null;
                    }
                    quizResponseModelList.Add(quizResponseModel);
                }
                if (quizResponseModelList.Count > 0)
                {
                    quizResponseModelList = quizResponseModelList.OrderBy(x => x.IsExam).ThenBy(x => x.LevelOfKnowledge).ThenBy(x => x.Title).ToList();
                }
                return Ok(quizResponseModelList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
