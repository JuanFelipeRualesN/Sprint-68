using System.Net.Http.Headers;
using System.Text;
using GSWebAPI.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.UserQuizzes;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;


namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle the reports.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class ReportsController : ControllerBase
    {
        /// <summary>
        /// Homeschooling endpoint. To get the overall average by product and profile id.
        /// (*Homeschooling. For schools, it is calculated on Data Analysis and Teachers API).
        /// </summary>
        /// <param name="profileId">The student's user id.</param>
        /// <param name="levelCode">Optional: The level code. Example: K, 1, 2, 3, etc.</param>
        /// <param name="dateFrom">Optional: The date from. </param>
        /// <param name="dateTo">Optional: The date to. </param>
        /// <returns>Returns the profile’s results details.</returns>
        [HttpGet]
        [Route("reports/profile/{profileId}/average/")]
        public async Task<IActionResult> GetReportProfileAverages(string profileId, string levelCode = null, string dateFrom = null, string dateTo = null)
        {
            try
            {
                var generalResponseModel = new ProfileReportAverageModel();

                /* Token validation in Users API */
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileAverageByLessons");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Verify if user is related
                // Check if user can access to the student details.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext, responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                var userQuizManager = new UserQuizService(
                                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                                                                             responseSession.LangCode);
                var averages = userQuizManager.CalculateStudentGeneralAverages(profileId, levelCode, dateFrom, dateTo);

                return Ok(averages);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Homeschooling endpoint. To get the average per lessons.
        /// (*Homeschooling. For schools, it is calculated on Data Analysis and Teachers API).
        /// </summary>
        /// <param name="profileId">The student's user id.</param>
        /// <param name="subjectCode">Optional: The subject's code.</param>
        /// <param name="levelCode">Optional: The level code. Example: K, 1, 2, 3, etc.</param>
        /// <param name="dateFrom">Optional: The date from. </param>
        /// <param name="dateTo">Optional: The date to. </param>
        /// <returns>Returns the profile’s results details.</returns>
        [HttpGet]
        [Route("reports/profile/{profileId}/average/lessons")]
        public async Task<IActionResult> GetReportProfileLessonAverages(string profileId, string subjectCode = null, string levelCode = null, string dateFrom = null, string dateTo = null)
        {
            try
            {
                var generalResponseModel = new ProfileReportAverageModel();

                /* Token validation in Users API */
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileAverageByLessons");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Verify if user is related
                // Check if user can access to the student details.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext, responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                var userQuizManager = new UserQuizService(
                                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                                                                             responseSession.LangCode);
                var averages = userQuizManager.GetAveragePerLesson(profileId, subjectCode, levelCode, dateFrom, dateTo);

                return Ok(averages);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// For Homeschooling: To get the lesson details (the exams and results).
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <param name="lessonId">The lesson id.</param>
        /// <returns>Returns the result details.</returns>
        [HttpGet]
        [Route("reports/profile/{profileId}/average/lessons/{lessonId}")]
        public async Task<IActionResult> GetReportResultPerLessonDetails(string profileId, string lessonId)
        {
            try
            {
                var generalResponseModel = new ProfileReportAverageModel();

                /* Token validation in Users API */
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileAverageByLessons");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Verify if user is related
                // Check if user can access to the student details.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext, responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                var userQuizManager = new UserQuizService(
                                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                                                                             responseSession.LangCode);
                var results = userQuizManager.GetProfileLessonDetails(profileId, lessonId);

                return Ok(results);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// To get the overall average by product and profile id.
        /// (*Homeschooling. For schools, it is calculated on Data Analysis and Teachers API).
        /// </summary>
        /// <param name="profileId">The student's user id.</param>
        /// <param name="subjectCode">The subject code. Example: MAT-SP, MAT-EN, SCI-EN, etc.</param>
        /// <param name="levelCode">The level code. Example: K, 1, 2, 3, etc.</param>
        /// <returns>Returns the profile’s results details.</returns>
        [HttpGet]
        [Route("reports/profile/average/overall/{profileId}/{subjectCode}/{levelCode}")]
        public async Task<IActionResult> GetReportProfileAverages(string profileId, string subjectCode, string levelCode)
        {
            try
            {
                ProfileReportAverageModel generalResponseModel = new ProfileReportAverageModel();

                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileAverageByLessons");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Verify if user is related
                // Check if user can access to the student details.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext, responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                // End if user is related.
                UserQuizService userQuizManager = new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);
                generalResponseModel.AverageWithLessonResultDetails = userQuizManager.CalculateAverageWithLessonAveragesDetails(
                    profileId,
                    levelCode,
                    subjectCode);

                // Call the lessons
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    AthenasJSONPostModel athenasJSONPostModel = new AthenasJSONPostModel
                    {
                        LevelCode = levelCode,
                        LangCode = responseSession.LangCode,
                        IsForOutsideSource = true,
                        SubjectCode = subjectCode,
                        Token = responseSession.Token,
                    };

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(athenasJSONPostModel), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:athenas_api_lessons_read"], stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        responseString = responseString.Replace("\\\"", "\"");
                        responseString = responseString.Replace("\\\\\\", "\\");
                        responseString = responseString.Replace("\\\\", "\\");
                        responseString = responseString.Replace("\\\"", "\\\\'");
                        responseString = responseString.Trim('"');

                        AthenasJSONLessonsDefailsResponseModel modelObject = JsonConvert.DeserializeObject<AthenasJSONLessonsDefailsResponseModel>(responseString, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                        // Now connect the data
                        generalResponseModel.LessonAverageCompleteInfoResponseList = new List<LessonsAverageCompleteInfoResponseModel>();

                        for (int i = 0; i < modelObject.LessonRequestModel.Count; i++)
                        {
                            LessonsAverageCompleteInfoResponseModel lessonsAverageCompleteInfo = new LessonsAverageCompleteInfoResponseModel
                            {
                                LessonId = modelObject.LessonRequestModel[i].Id,
                                LessonNo = modelObject.LessonRequestModel[i].LessonNo,
                                LessonTitle = modelObject.LessonRequestModel[i].LessonTitle,
                            };

                            if (generalResponseModel.AverageWithLessonResultDetails != null && generalResponseModel.AverageWithLessonResultDetails.LessonAverageList != null && generalResponseModel.AverageWithLessonResultDetails.LessonAverageList.Count > 0)
                            {
                                LessonAverageResponseModel lessonsAverageCompleteInfoResponseModel = generalResponseModel.AverageWithLessonResultDetails.LessonAverageList.Find(x => x.LessonId == modelObject.LessonRequestModel[i].Id);

                                // Find the average
                                if (lessonsAverageCompleteInfoResponseModel != null)
                                {
                                    lessonsAverageCompleteInfo.Average = lessonsAverageCompleteInfoResponseModel.LessonAverage;
                                }
                            }

                            generalResponseModel.LessonAverageCompleteInfoResponseList.Add(lessonsAverageCompleteInfo);
                        }
                    }
                }

                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the average of a lesson for a student (highest score according to tests).
        /// (*Homeschooling. For schools, it is calculated on Data Analysis and Teachers API).
        /// </summary>
        /// <param name="profileId">The profile's id (student user's id).</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns a response with the report's details.</returns>
        [HttpGet]
        [Route("reports/profile/average/lesson/{profileId}/{lessonId}")]
        public async Task<IActionResult> GetProfileAverageLesson(string profileId, string lessonId)
        {
            try
            {
                // Check if JSON is not null
                if (string.IsNullOrWhiteSpace(profileId) || string.IsNullOrWhiteSpace(lessonId))
                {
                    return BadRequest();
                }

                /* End of general values to process the request */
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileAverageByLessons");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Verify if user is related
                // Check if user can access to the student details.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext,responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                // End if user is related.

                // First, we need to find the product subscription code and level code
                UserQuizService userQuizManager = new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);

                return Ok(userQuizManager.CalculateUserLessonAverage(profileId, lessonId).ToString());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the quiz tries with all details.
        /// (*Homeschooling. For schools, it is calculated on Data Analysis and Teachers API).
        /// </summary>
        /// <param name="profileId">The profile's id (user student's id).</param>
        /// <param name="quizId">The quiz's id (from Athenas API).</param>
        /// <returns>Returns the profile’s results tries details.</returns>
        [HttpGet]
        [Route("reports/profile/quiz/tries/{profileId}/{quizId}")]
        public async Task<IActionResult> GetProfileQuizTriesDetails(string profileId, string quizId)
        {
            try
            {
                // Check if JSON is not null
                if (string.IsNullOrWhiteSpace(profileId) || string.IsNullOrWhiteSpace(quizId))
                {
                    return BadRequest();
                }

                /* End of general values to process the request */
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext,
                    "ProfileQuizTriesRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Check if user can access to the student details.
                // Verify if users are related.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext,responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                // End of related
                UserQuizService userQuizManager = new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);

                return Ok(userQuizManager.GetUserQuizResultAndReturnByTry(profileId, quizId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To get the profiles activities for the main account, so the main account can see what the profiles has done.
        /// </summary>
        /// <returns>Returns the profiles' activities details.</returns>
        [HttpGet]
        [Route("reports/profile/activities")]
        public async Task<IActionResult> GetProfileActivities()
        {
            try
            {
                List<ProfilesActivity> profilesActivities = new List<ProfilesActivity>();
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfilesActivity");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation
                List<UserResponse> profilesSharedDetails = GetUserDetails.GetMainAccountUserProfilesSharedDetails(responseSession.Token, responseSession.IpAddress, 20);

                if (profilesSharedDetails == null || profilesSharedDetails.Count == 0)
                {
                    return Ok(profilesActivities);
                }

                // Now get the profiles activities
                List<UserQuizResultModel> userQuizResultToSearch = new List<UserQuizResultModel>();
                for (int i = 0; i < profilesSharedDetails.Count; i++)
                {
                    userQuizResultToSearch.Add(new UserQuizResultModel()
                    {
                        UserId = profilesSharedDetails[i].UserId,
                    });
                }

                List<UserQuizResultModel> userQuizResultResult = new List<UserQuizResultModel>();
                UserQuizService userQuizManager = new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);
                userQuizResultResult = userQuizManager.GetUsersQuizResults(userQuizResultToSearch);

                if (userQuizResultResult.Count == 0)
                {
                    return Ok(profilesActivities);
                }

                // Get only the last 5 activities
                userQuizResultResult = userQuizResultResult.OrderByDescending(x => x.CreatedAt).ToList();
                int activitiesQuantity = 5;
                if (userQuizResultResult.Count < activitiesQuantity)
                {
                    activitiesQuantity = userQuizResultResult.Count;
                }

                // Now get the user info of those 5 activities
                for (int i = 0; i < activitiesQuantity; i++)
                {
                    ProfilesActivity profilesActivity = new ProfilesActivity()
                    {
                        Level = userQuizResultResult[i].Level,
                        Subject = userQuizResultResult[i].Subject,
                        IsExam = userQuizResultResult[i].IsExam == "0" ? false : true,
                        TotalPercent = userQuizResultResult[i].TotalPercent,
                        CreatedDate = userQuizResultResult[i].CreatedAt,
                    };

                    UserResponse userSharedDetail = profilesSharedDetails.Find(x => x.UserId == userQuizResultResult[i].UserId);

                    if (userSharedDetail != null)
                    {
                        profilesActivity.StudentName = userSharedDetail?.GivenName + " " + userSharedDetail?.Surname;
                    }

                    profilesActivities.Add(profilesActivity);
                }

                return Ok(profilesActivities);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the Genial Lesson’s quizzes results by profile and userQuizId.
        /// </summary>
        /// <param name="profileId">The student user’s id.</param>
        /// <param name="userQuizResultId">The user quiz result’s id.</param>
        /// <param name="pdfRequired">If PDF required (results)</param>
        /// <returns>Returns the profile’s results details..</returns>
        [HttpGet]
        [Route("reports/profile/quiz/results/{profileId}/{userQuizResultId}")]
        public async Task<IActionResult> GetProfileQuizResultDetails(string profileId, string userQuizResultId, bool pdfRequired = true)
        {
            try
            {
                var generalResponseModel = new ProfileReportsResponseModel();
                /* Token validation in Users API */
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "ProfileQuizResultDetailsRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Check if user can access to the student details.
                // Verify if users are related.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext, responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                // End of related
                var userQuizManager = new UserQuizService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);
                var userQuizResponseModel = userQuizManager.GetUserQuizResponse(userQuizResultId, AppConfiguration.Configuration["AppSettings:web_app_url"]);

                var modelObject = AthenasDetailsHandler.GetQuizById(userQuizResponseModel.UserQuizResult.QuizId, responseSession.Token);

                // Get the lesson title
                string lessonTitle = null;
                if (!string.IsNullOrWhiteSpace(userQuizResponseModel.UserQuizResult.LessonId))
                {
                    var lessonDetails = AthenasDetailsHandler.GetLessonModels(
                        new List<string> { userQuizResponseModel.UserQuizResult.LessonId });

                    lessonTitle = lessonDetails?.LessonModelList
                        ?.FirstOrDefault(x => x.Id == userQuizResponseModel.UserQuizResult.LessonId)
                        ?.LessonTitle;
                }

                // Now connect the data
                generalResponseModel.UserQuizFullDetails = new QuizUserResultsCompletedInfoResponseModel
                {
                    QuizId = userQuizResponseModel.UserQuizResult.QuizId,
                    QuizTitle = modelObject.QuizRequestModel.QuizModel.Title,
                    Instructions = modelObject.QuizRequestModel.QuizModel.Instruction,
                    ExtraInfo = modelObject.QuizRequestModel?.QuizExtraInfoModel?.ExtraInfo,
                    UserQuizId = userQuizResponseModel.UserQuizResult.Id,
                    QuestionQuizList = new List<QuestionQuiz>(),
                    TotalPercent = userQuizResponseModel.UserQuizResult.TotalPercent,
                    DoK = userQuizResponseModel.UserQuizResult.Dok,
                    TotalCorrect = userQuizResponseModel.UserQuizResult.TotalCorrect,
                    TryNo = userQuizResponseModel.UserQuizResult.TryNo,
                    CreatedAt = userQuizResponseModel.UserQuizResult.CreatedAt,
                    LessonTitle = lessonTitle,
                    Subject = userQuizResponseModel.UserQuizResult.Subject,
                    Grade = userQuizResponseModel.UserQuizResult.Level,
                };

                // Resolve subject code
                var athenasHandler = new AthenasDetailsHandler();
                var subjects = athenasHandler.GetAthenasSubjects(responseSession.LangCode);
                var subjectMatch = subjects?.FirstOrDefault(s => string.Equals(s.Code, userQuizResponseModel.UserQuizResult.Subject, StringComparison.OrdinalIgnoreCase));
                if (subjectMatch != null)
                {
                    generalResponseModel.UserQuizFullDetails.Subject = subjectMatch.Name;
                }

                // Index all the quiz responses by id. In pairing quizzes the student can pick
                // a response that belongs to another question, so we search across the whole quiz.
                var quizResponsesById = new Dictionary<string, string>();
                foreach (var questionRequestModel in modelObject.QuizRequestModel.QuestionRequestModelList)
                {
                    if (questionRequestModel.QuizQuestionResponseModelList == null)
                    {
                        continue;
                    }

                    foreach (var quizQuestionResponseModel in questionRequestModel.QuizQuestionResponseModelList)
                    {
                        if (!string.IsNullOrWhiteSpace(quizQuestionResponseModel.Id))
                        {
                            quizResponsesById.TryAdd(quizQuestionResponseModel.Id, quizQuestionResponseModel.Response);
                        }
                    }
                }

                for (int i = 0; i < modelObject.QuizRequestModel.QuestionRequestModelList.Count; i++)
                {
                    var questionQuizToAdd = new QuestionQuiz()
                    {
                        QuestionId = modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id,
                        Question = modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Desc,
                        QuestionResponses = new List<QuestionResponse>(),
                    };

                    var userQuizQuestionsResponsesToFind
                        = userQuizResponseModel.UserResponsesList.FindAll(x => x.QuestionId == modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id);

                    // Now attach the responses
                    for (int j = 0; j < userQuizQuestionsResponsesToFind.Count; j++)
                    {
                        // Find the response text on athenas object, keep the response even if it is not found
                        string? responseText = null;
                        if (!string.IsNullOrWhiteSpace(userQuizQuestionsResponsesToFind[j].ResponseId))
                        {
                            quizResponsesById.TryGetValue(userQuizQuestionsResponsesToFind[j].ResponseId, out responseText);
                        }

                        var questionResponse = new QuestionResponse
                        {
                            ResponseId = userQuizQuestionsResponsesToFind[j].ResponseId,
                            Response = responseText ?? string.Empty,
                            ResponseUserId = userQuizQuestionsResponsesToFind[j].Id,
                            Correct = userQuizQuestionsResponsesToFind[j].Correct == "1",
                        };
                        questionQuizToAdd.QuestionResponses.Add(questionResponse);
                    }

                    // Add every question of the quiz, answered correctly or not
                    generalResponseModel.UserQuizFullDetails.QuestionQuizList.Add(questionQuizToAdd);
                }

                List<UserResponse> userSharedDetails = GetUserDetails.GetSharedDetails(responseSession.Token, new List<string>() { userQuizResponseModel.UserQuizResult.UserId }, responseSession.IpAddress);
                if (userSharedDetails != null && userSharedDetails.Count > 0)
                {
                    generalResponseModel.StudentName = userSharedDetails[0]?.Surname + ", " + userSharedDetails[0]?.GivenName;
                }
                else
                {
                    generalResponseModel.StudentName = string.Empty;
                }

                try
                {
                    if (pdfRequired)
                    {
                        generalResponseModel.PDFLocation = await PDFResultGenerator.GeneratePDF(generalResponseModel, AppConfiguration.Configuration["AppSettings:generate_pdf_in_cloud_storage"] == "true");
                    }
                }
                catch (Exception ex)
                {
                    // Continue and return at least the results without the PDF. 
                    Console.WriteLine(ex);
                    Console.Error.WriteLine($"Exception: {ex}");
                }
                return Ok(generalResponseModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error results GSWeb", ex.ToString());
                Console.Error.WriteLine($"Exception: {ex}");
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To get the pre and post results, for Homeschooling or Schools that does not apply the curricular plan system.
        /// </summary>
        /// <param name="profileId">The profile's id.</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Return the pre/post results.</returns>
        [HttpGet]
        [Route("reports/profile/pre_post/results/{profileId}/{lessonId}")]
        public async Task<IActionResult> GetProfilePrePostResultDetails(string profileId, string lessonId)
        {
            try
            {
                ProfileReportsResponseModel generalResponseModel = new ProfileReportsResponseModel();
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "ProfileQuizResultDetailsRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // End of validation

                // Check if user can access to the student details.
                // Verify if users are related.
                bool related = new GetUserDetails().CanUserAManageUserB(HttpContext, responseSession.UserId, profileId, responseSession.Token);

                // If it is the same user, just allow it.
                if (responseSession.UserId == profileId)
                {
                    related = true;
                }

                if (!related)
                {
                    return Unauthorized();
                }

                // End of related
                // Now get the pre-posts
                return Ok(UserResults.GetUserPrePostResults(profileId, lessonId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}