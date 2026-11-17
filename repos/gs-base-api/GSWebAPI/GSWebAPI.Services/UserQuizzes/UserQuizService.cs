using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.UserQuizzes;
using GSWebAPI.GSWebAPI.Services.UserQuizzes.CustomClasses;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.LessonsOldVeersions;
using GSWebAPI.GSWebAPI.Domain.Models.LessonsOldVersions;

namespace GSWebAPI.GSWebAPI.Services.UserQuizzes
{
    /// <summary>
    /// To handle the user quiz results.
    /// </summary>
    public class UserQuizService
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;
        private readonly string _gsWebDbConnection;
        private readonly string _gsWebDbConnectionReader;
        private readonly string _gwpMessageCodeCannotSaveExam;
        private readonly string[] _gwpDpkToEvaluate;

        public enum QuizType
        {
            Practice = 0,
            Exam = 1,
            PreTest = 2,
            PostTest = 3
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserQuizService"/> class.
        /// </summary>
        /// <param name="connectionString">The writer connection string.</param>
        /// <param name="connectionStringRead">The reader connction string.</param>
        /// <param name="langCode">The language code.</param>
        public UserQuizService(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
            _gsWebDbConnection = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"];
            _gsWebDbConnectionReader = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"];
            _gwpMessageCodeCannotSaveExam = AppConfiguration.Configuration["AppSettings:gwp_message_code_cannot_save_exam"];
            _gwpDpkToEvaluate = AppConfiguration.Configuration["AppSettings:gwp_dpk_to_evaluate"].Split(',');
        }

        /// <summary>
        /// To handle the student general averages. This is used to calculate the averages of the user by subject and level code.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="levelCode">Optional: The level code if any.</param>
        /// <param name="fromDate">Optional: From date.</param>
        /// <param name="toDate">Optional: To date.</param>
        /// <returns>Returns the user averages. </returns>
        /// <exception cref="ArgumentException"></exception>
        public List<UserLessonAverageModel> CalculateStudentGeneralAverages(string userId, string levelCode = null, string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var userResults = UserResults.GetUserLessonAverages(userId, null, null, levelCode);

           if(userResults == null || !userResults.Any())
            {
                return new List<UserLessonAverageModel>();
            }

            if(!string.IsNullOrWhiteSpace(fromDate))
            {
                // Convert to DateTime for filtering
                DateTime? from = string.IsNullOrWhiteSpace(fromDate) ? (DateTime?)null : DateTime.Parse(fromDate);

                if (string.IsNullOrWhiteSpace(toDate))
                {
                    // Set as today
                    toDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                
                DateTime? to = string.IsNullOrWhiteSpace(toDate) ? (DateTime?)null : DateTime.Parse(toDate);
                // Filter by date range if provided
                if (from.HasValue || to.HasValue)
                {
                    userResults = userResults.Where(x =>
                                       (!from.HasValue || DateTime.Parse(x.CreatedAt) >= from.Value) &&
                                                          (!to.HasValue || DateTime.Parse(x.CreatedAt) <= to.Value)).ToList();
                }
            }

            var userLessonAverages = userResults.GroupBy(x => new { x.SubjectCode })
                                          .Select(g => new UserLessonAverageModel
                                          {
                                              UserId = userId,
                                              SubjectCode = g.Key.SubjectCode,
                                              Average = AverageCalculator.CalculateAverage(g.Select(x => decimal.Parse(x.Average)).ToArray(), true).ToString(),
                                          }).ToList();

            // Now, calculate the overall average for each subject
            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                userLessonAverages = userResults.GroupBy(x => new { x.SubjectCode, x.LevelCode })
                                                    .Select(g => new UserLessonAverageModel
                                                    {
                                                        UserId = userId,
                                                        SubjectCode = g.Key.SubjectCode,
                                                        LevelCode = g.Key.LevelCode,
                                                        Average = AverageCalculator.CalculateAverage(g.Select(x => decimal.Parse(x.Average)).ToArray(), true).ToString(),
                                                    }).ToList();
            }

            // Call Athenas Subjects
            var subjects = new AthenasDetailsHandler().GetAthenasSubjects(null);
            // Change the subject code to the subject name
            foreach (var userLessonAverage in userLessonAverages)
            {
                var subject = subjects.FirstOrDefault(x => x.Code == userLessonAverage.SubjectCode);
                if (subject != null)
                {
                    userLessonAverage.SubjectName = subject.Name;
                }
            }

            return userLessonAverages;
        }

        /// <summary>
        /// To handle the student general averages. This is used to calculate the averages of the user by subject and level code.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="subjectCode">Optional: The subject code</param>
        /// <param name="levelCode">Optional: The level code if any.</param>
        /// <param name="fromDate">Optional: From date.</param>
        /// <param name="toDate">Optional: To date.</param>
        /// <returns>Returns the user averages per lessons. </returns>
        /// <exception cref="ArgumentException"></exception>
        public List<UserLessonAverageModel> GetAveragePerLesson(string userId, string subjectCode = null, string levelCode = null, string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var userResults = UserResults.GetUserLessonAverages(userId, null, subjectCode, levelCode);

            if (userResults == null || !userResults.Any())
            {
                return new List<UserLessonAverageModel>();
            }

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                // Convert to DateTime for filtering
                DateTime? from = string.IsNullOrWhiteSpace(fromDate) ? (DateTime?)null : DateTime.Parse(fromDate);

                if (string.IsNullOrWhiteSpace(toDate))
                {
                    // Set as today
                    toDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                DateTime? to = string.IsNullOrWhiteSpace(toDate) ? (DateTime?)null : DateTime.Parse(toDate);
                // Filter by date range if provided
                if (from.HasValue || to.HasValue)
                {
                    userResults = userResults.Where(x =>
                                       (!from.HasValue || DateTime.Parse(x.CreatedAt) >= from.Value) &&
                                                          (!to.HasValue || DateTime.Parse(x.CreatedAt) <= to.Value)).ToList();
                }
            }

            // Get the id of the lessons.
            var lessonIds = userResults.Select(x => x.LessonId).Distinct().ToList();

            // Get the lesson titles from Athenas
            if (lessonIds == null || !lessonIds.Any())
            {
                return new List<UserLessonAverageModel>();
            }

            var lessonModelTitles = AthenasDetailsHandler.GetLessonModels(lessonIds);
            var lessonsOldList = new LessonsOldVersionDAO().Read(_connectionStringRead,
                new LessonsOldVersionModel(), false, false);

            // Relate the lesson titles with the user results
            foreach(var userResult in userResults)
            {
                var lessonTitle = lessonModelTitles.LessonModelList.FirstOrDefault(x => x.Id == userResult.LessonId)?.LessonTitle;
                if (lessonTitle != null)
                {
                    userResult.LessonTitle = lessonTitle;
                    userResult.CanSeeResults = true;
                }
                else
                {
                    // Since the results cannot be seen, we send it as false.
                    userResult.CanSeeResults = false;
                    // Check if exists in the old lessons
                    var oldLesson = lessonsOldList.FirstOrDefault(x => x.Id == userResult.LessonId);
                    if (oldLesson != null)
                    {
                        userResult.LessonTitle = oldLesson.LessonTitle;  
                    }
                    else
                    {
                        userResult.LessonTitle = "Not found"; // Fallback if no title found
                    }
                }
            }

            // Order by updated at date (the recent first)
            userResults = userResults.OrderByDescending(x => DateTime.Parse(x.UpdatedAt)).ToList();

            return userResults;
        }

        /// <summary>
        /// To get the quiz details by lesson.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="lessonId">The lesson id.</param>
        /// <returns>Return the list of the results.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<UserQuizResultModel> GetProfileLessonDetails(string userId, string lessonId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(lessonId))
            {
                throw new ArgumentException("User ID and Lesson ID cannot be null or empty.");
            }

            var userQuizResultModelToFind = new UserQuizResultModel
            {
                UserId = userId,
                LessonId = lessonId
            };

            var results = GetUserQuizResults(userQuizResultModelToFind);

            var quizDetails = AthenasDetailsHandler.GetLessonQuizzes(lessonId);

            // Set the quiz title.

            foreach (var result in results)
            {
                var quiz = quizDetails.Quizzes.FirstOrDefault(q => q.Id == result.QuizId);
                if (quiz != null)
                {
                    result.QuizTitle = quiz.Title;
                    result.Status = "C"; // Completed
                }
            }

            // Now for each quiz id, get the one with the highest total percent.
            var highestScores = results
                .GroupBy(r => r.QuizId)
                .Select(g => g.OrderByDescending(r => double.Parse(r.TotalPercent)).FirstOrDefault())
                .ToList();


            // Add the quizzes that the user has not done yet.
            var allQuizzes = quizDetails.Quizzes.Where(q => q.IsExam != ((int)QuizType.Practice).ToString()).ToList();
            foreach (var quiz in allQuizzes)
            {
                if (!highestScores.Any(h => h.QuizId == quiz.Id))
                {
                    highestScores.Add(new UserQuizResultModel
                    {
                        QuizId = quiz.Id,
                        QuizTitle = quiz.Title,
                        IsExam = quiz.IsExam,
                        TotalPercent = "0",
                        TotalCorrect = "0",
                        LessonId = lessonId,
                        Status = "P" // Pending
                    });
                }
            }

            // Now, order by the following: 
            // Pre (is_exam = 2) first, 
            // Post (is_exam = 3), second, 
            // Then is_exam = 1 by title. 
            highestScores = highestScores
                .OrderBy(r => r.IsExam == ((int)QuizType.PreTest).ToString() ? 0 :
                                             r.IsExam == ((int)QuizType.PostTest).ToString() ? 1 : 2)
                .ThenBy(r => r.QuizTitle)
                .ToList();

            return highestScores;

        }

        /// <summary>
        /// To submit the quiz response from the user.
        /// </summary>
        /// <param name="subscriptionType">The subscription type can be "H" for Homeschooling or "S" for School.</param>
        /// <param name="learningGap">1 if learning gap, 0 if no.</param>
        /// <param name="subjectCode">Subject code of the quiz.</param>
        /// <param name="levelCode">Level code of the quiz.</param>
        /// <param name="langCode">Language code for the messages.</param>
        /// <param name="quizId">Quiz ID to submit.</param>
        /// <param name="userId">Id of the user submitting the quiz.</param>
        /// <param name="userResponseModels">List of user responses to the quiz.</param>
        /// <param name="userToken">Token of the user for authentication.</param>
        /// <returns>Returns the response for the game quiz.</returns>
        public GSWebAPI.Domain.Models.General.Responses.GameQuizResponseModel SubmitQuizResponse(string subscriptionType, 
            int learningGap, 
            string quizId,
            string userToken,
            string userId,
            List<UserResponseModel> userResponseModels, string langCode, 
            string subjectCode, string levelCode)
        {
            var generalResponseModel = new GameQuizResponseModel();

            try
            {
                // --- Input Validation Guard Clauses ---
                if (string.IsNullOrWhiteSpace(subscriptionType))
                {
                    generalResponseModel.UserQuizResultModel = new UserQuizResultModel
                    {
                        Valid = false,
                        Messages = new List<string> { "Subscription type parameter is required." }
                    };
                }

                var messageManager = new MessageManager(_gsWebDbConnectionReader, langCode);

                // --- Retrieve Quiz Details ---
                var quizDetails = AthenasDetailsHandler.GetQuizById(quizId, userToken);
                if (quizDetails == null || quizDetails.QuizRequestModel?.QuizModel == null)
                {
                    generalResponseModel.UserQuizResultModel = new UserQuizResultModel
                    {
                        Valid = false,
                        Messages = new List<string> { "Quiz not found. " }
                    };
                }

                // --- Prepare User Quiz Result ---
                var userQuizResultToInsert = new UserQuizResultModel
                {
                    UserId = userId,
                    IsExam = quizDetails.QuizRequestModel.QuizModel.IsExam,
                    Dok = quizDetails.QuizRequestModel.QuizModel.LevelOfKnowledge,
                    LessonId = quizDetails.QuizRequestModel.QuizModel.LessonId,
                    Subject = quizDetails.QuizRequestModel.SubjectCode,
                    Level = quizDetails.QuizRequestModel.LevelCode,
                    QuizId = quizDetails.QuizRequestModel.QuizModel.Id,
                };

                // --- Convert User Responses ---
                var userResponseListToInsert = userResponseModels
                    .Select(response => new GSWebAPI.Domain.Models.UserQuiz.UserResponseModel
                    {
                        QuestionId = response.QuestionId,
                        ResponseId = response.ResponseId,
                        Correct = response.Correct,
                    }).ToList();

                // --- Quiz Submission Rules based on Subscription and Quiz Type ---
                if (learningGap == 0) // Special rules for learning gap, otherwise apply normal quiz rules
                {
                    var quizType = (QuizType)int.Parse(userQuizResultToInsert.IsExam);

                    // Homeschooling exams.
                    if (quizType == QuizType.Exam && subscriptionType == "H")
                    {
                        var quizTryModel = QuizTriesCheckManager.GetQuizTry(userId, userQuizResultToInsert.QuizId);
                        if (quizTryModel.TriesLeft == 0)
                        {
                            generalResponseModel.UserQuizResultModel = new UserQuizResultModel
                            {
                                Valid = false,
                                Messages = new List<string> { messageManager.GetMessageDetails(_gwpMessageCodeCannotSaveExam) }
                            };
                            return generalResponseModel;
                        }
                    }
                    // Not homeschooling exams 
                    else if (quizType == QuizType.Exam && subscriptionType != "H")
                    {
                        if (!TeachersApiHandler.CheckIfExamCanbeSubmitted(userToken, userQuizResultToInsert.QuizId))
                        {
                            generalResponseModel.UserQuizResultModel = new UserQuizResultModel
                            {
                                Valid = false,
                                Messages = new List<string> { messageManager.GetMessageDetails(_gwpMessageCodeCannotSaveExam) }
                            };
                            return generalResponseModel;
                        }
                    }
                    else if ((quizType == QuizType.PreTest || quizType == QuizType.PostTest) && subscriptionType == "H")
                    {
                        if (QuizTriesCheckManager.CanMakePrePost(userId, userQuizResultToInsert.QuizId).TriesLeft == 0)
                        {
                            generalResponseModel.UserQuizResultModel = new UserQuizResultModel
                            {
                                Valid = false,
                                Messages = new List<string> { messageManager.GetMessageDetails(_gwpMessageCodeCannotSaveExam) }
                            };
                            return generalResponseModel;
                        }
                    }
                }

                // --- Insert User Quiz and Responses ---
                UserQuizResultModel responseUserQuiz =
                    InsertUserQuizAndResponsesFullProccess(
                        quizDetails.QuizRequestModel.QuestionRequestModelList,
                        userQuizResultToInsert,
                        userResponseListToInsert,
                        userQuizResultToInsert.IsExam != ((int)QuizType.Practice).ToString(),
                        decimal.Parse(quizDetails.QuizRequestModel.QuizModel.CustomValue));

                // --- Generate User Rewards ---
                var gameUserRewardsManager = new GameUserRewardsService(
                    _gsWebDbConnection,
                    _gsWebDbConnectionReader,
                    langCode);

                generalResponseModel.UserRewardModel = new GSWebAPI.Domain.Models.GameUserReward.UserRewardModel(); // Initialize if needed
                double totalPointsStudent = double.Parse(responseUserQuiz.TotalPercent);
                generalResponseModel.UserRewardModel = gameUserRewardsManager.GenerateReward(responseUserQuiz, totalPointsStudent, responseUserQuiz.TotalPercent); 

                // --- Integrate with Teachers API for Non-Homeschooling Subscriptions ---
                if (subscriptionType != "H")
                {
                    var quizType = (QuizType)int.Parse(userQuizResultToInsert.IsExam);

                    if (quizType == QuizType.PreTest || quizType == QuizType.PostTest)
                    {
                        TeachersApiHandler.InsertPrePostResults(userToken, userQuizResultToInsert.LessonId, new PlanLessonPrePostUserResult()
                        {
                            QuizId = userQuizResultToInsert.QuizId,
                            Score = totalPointsStudent.ToString(),
                            UsquiUserResultId = responseUserQuiz.Id,
                            PrePost = (quizType == QuizType.PreTest) ? "0" : "1",
                            TotalCorrect = responseUserQuiz.TotalCorrect,
                            TotalQuestions = responseUserQuiz.TotalQuestions,
                        });
                    }
                    else if (quizType == QuizType.Exam)
                    {
                        TeachersApiHandler.InsertExamResults(userToken, userQuizResultToInsert.LessonId, new PlanLessonQuizUserResult()
                        {
                            QuizId = userQuizResultToInsert.QuizId,
                            Score = totalPointsStudent.ToString(),
                            UsquiUserResultId = responseUserQuiz.Id,
                        });
                    }
                }

                // --- Handle Learning Gap (if applicable) ---
                if (userQuizResultToInsert.IsExam != ((int)QuizType.Practice).ToString() && learningGap == 1)
                {
                    try
                    {
                        TeachersApiHandler.InsertLearningGap(userToken, new LearningGapQuizSubmitRequestModel()
                        {
                            StudentId = userId,
                            LessonId = userQuizResultToInsert.LessonId,
                            QuizValue = quizDetails.QuizRequestModel.QuizModel.CustomValue,
                            QuizId = userQuizResultToInsert.QuizId,
                            UsquiQuizResultId = responseUserQuiz.Id,
                            Score = totalPointsStudent.ToString(),
                            LearningSheetIds = userQuizResultToInsert.LearningSheetIds
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the exception but continue, as per original logic
                        // Consider more robust logging
                        Console.WriteLine($"Error inserting learning gap: {ex.Message}");
                    }
                }

                // --- Update User Lesson Average and Completion (used for exams and for client view)
                if (userQuizResultToInsert.IsExam == ((int)QuizType.Exam).ToString() || userQuizResultToInsert.IsExam == ((int)QuizType.PreTest).ToString() || userQuizResultToInsert.IsExam == ((int)QuizType.PostTest).ToString())
                {
                    try
                    {
                        // This is only for lesson and averages for Genial Skills Web API (it is not related 
                        // to the Teachers API)
                        UpdateUserLessonAverageAndCompletion(userId, userQuizResultToInsert.LessonId, subjectCode, levelCode);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception but return a bad request, as per original logic
                        // Ignore. This is not required anymore. 
                    }
                }

                generalResponseModel.UserQuizResultModel = responseUserQuiz;
                generalResponseModel.UserQuizResultModel.Valid = true;
                return generalResponseModel;
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                Console.WriteLine($"Error in SubmitQuizResponse: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Returns the results of a quiz in order of tries.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="quizId">The quiz ID.</param>
        /// <returns>Returns the user quiz in order of tries.</returns>
        public List<UserQuizResultModel> GetUserQuizResultAndReturnByTry(string userId, string quizId)
        {
            UserQuizResultModel userQuizResultToFind = new UserQuizResultModel
            {
                UserId = userId,
                QuizId = quizId,
            };
            List<UserQuizResultModel> userQuizResultListTry = new List<UserQuizResultModel>();
            userQuizResultListTry = GetUserQuizResults(userQuizResultToFind);
            return userQuizResultListTry.OrderBy(x => int.Parse(x.TryNo)).ToList();
        }

        /// <summary>
        /// Return a list of the user quiz results (the quizzes that already been made by the user).
        /// </summary>
        /// <param name="userQuizResultModel">The user quiz result to call.</param>
        /// <returns>Returns the user quiz results details.</returns>
        public List<UserQuizResultModel> GetUserQuizResults(UserQuizResultModel userQuizResultModel)
        {
            return new UserQuizResultDAO().Read(_connectionStringRead, userQuizResultModel, false, true);
        }

        /// <summary>
        /// Generic to work with user quiz results models.
        /// </summary>
        /// <param name="userQuizResultModels">To get the user quiz results list.</param>
        /// <returns>Returns the user quiz results list.</returns>
        public List<UserQuizResultModel> GetUsersQuizResults(List<UserQuizResultModel> userQuizResultModels)
        {
            return new UserQuizResultDAO().Read(_connectionStringRead, userQuizResultModels);
        }

        /// <summary>
        /// Inserts the User Quiz Result and return the object with all details.
        /// </summary>
        /// <param name="userQuizResultModelToInsert">The User Quiz Result Model to Insert.</param>
        /// <returns>Returns the User Quiz Result Model with all details of the object inserted.</returns>
        public UserQuizResultModel InsertUserQuizResult(UserQuizResultModel userQuizResultModelToInsert)
        {
            return new UserQuizResultDAO().Create(_connectionString, userQuizResultModelToInsert)[0];
        }

        /// <summary>
        /// Insert a list of user response questions.
        /// </summary>
        /// <param name="userResponseQuestionModelList">The list of the user response question details.</param>
        public void InsertUserResponseQuestionsList(List<UserResponseModel> userResponseQuestionModelList)
        {
            new UserResponseDAO().Create(_connectionString, userResponseQuestionModelList);
        }

        /// <summary>
        /// To calculate the user average.
        /// </summary>
        /// <param name="userQuizResultModel">The user quiz result to calculate.</param>
        /// <returns>Returns the average calculated.</returns>
        private decimal CalculateAverageWithRule(UserQuizResultModel userQuizResultModel)
        {
            List<UserQuizResultModel> userQuizResultModelList = new List<UserQuizResultModel>();
            UserQuizResultDAO userQuizResultDAO = new UserQuizResultDAO();
            userQuizResultModelList = userQuizResultDAO.Read(_connectionStringRead, userQuizResultModel, false, false);

            decimal average = 0;

            // If there is nothing, then, just finish the process and return 0
            if (userQuizResultModelList.Count == 0)
            {
                return average;
            }

            // If there is more, then group by quiz id, and calculate the highest scores
            var groupedQuizList = userQuizResultModelList.GroupBy(u => u.QuizId).Select(grp => grp.ToList()).ToList();

            // Now, for each quiz, we calculate the highest score 
            List<UserQuizResultModel> userQuizResultModelHIghestScore = new List<UserQuizResultModel>();
            var scoreList = new List<decimal>(); // The high score list 
            for (int i = 0; i < groupedQuizList.Count; i++)
            {
                List<UserQuizResultModel> listOfQuizzesToSort = new List<UserQuizResultModel>();
                listOfQuizzesToSort = userQuizResultModelList.FindAll(x => x.QuizId == groupedQuizList[i][0].QuizId).OrderByDescending(x => decimal.Parse(x.TotalPercent)).ToList(); // Find and sort by highest score

                scoreList.Add(decimal.Parse(listOfQuizzesToSort[0].TotalPercent));
            }
            return AverageCalculator.CalculateAverage(scoreList.ToArray(), true);
        }

        /// <summary>
        /// Returns the average of each lesson.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="levelCode">The level code.</param>
        /// <param name="subjectCode">The subject code.</param>
        /// <returns>Returns the calculated average of the lessons that the user has work with.</returns>
        public AverageWithLessonResultDetailsResponseModel CalculateAverageWithLessonAveragesDetails(string userId, string levelCode, string subjectCode)
        {
            AverageWithLessonResultDetailsResponseModel averageWithLessonResult = new AverageWithLessonResultDetailsResponseModel
            {
                LessonAverageList = new List<LessonAverageResponseModel>(),
            };

            UserQuizResultModel userQuizResultModelToFind = new UserQuizResultModel
            {
                UserId = userId,
                Level = levelCode,
                Subject = subjectCode,
                IsExam = "1",
            };

            List<UserQuizResultModel> userQuizResultModelList = new List<UserQuizResultModel>();
            UserQuizResultDAO userQuizResultDAO = new UserQuizResultDAO();
            userQuizResultModelList = userQuizResultDAO.Read(_connectionStringRead, userQuizResultModelToFind, false, false);

            // If nothing, returns null
            if (userQuizResultModelList.Count == 0)
            {
                return null;
            }

            double average = 0;
            // If there is nothing, then, just finish the process and return 0
            if (userQuizResultModelList.Count == 0)
            {
                averageWithLessonResult.Average = average.ToString();
                return averageWithLessonResult;
            }

            // If there is more, then group by quiz id, and calculate the highest scores
            var groupedQuizList = userQuizResultModelList.GroupBy(u => u.QuizId).Select(grp => grp.ToList()).ToList();

            // Now, for each quiz, we calculate the highest score.
            List<UserQuizResultModel> userQuizResultModelHIghestScore = new List<UserQuizResultModel>();
            List<double> scoreList = new List<double>(); // The high score list.
            for (int i = 0; i < groupedQuizList.Count; i++)
            {
                List<UserQuizResultModel> listOfQuizzesToSort = new List<UserQuizResultModel>();
                listOfQuizzesToSort = userQuizResultModelList.FindAll(x => x.QuizId == groupedQuizList[i][0].QuizId).OrderByDescending(x => double.Parse(x.TotalPercent)).ToList(); // Find and sort by highest score
                scoreList.Add(double.Parse(listOfQuizzesToSort[0].TotalPercent));
                UserQuizResultModel userQuizResultModel = listOfQuizzesToSort[0];
                userQuizResultModelHIghestScore.Add(userQuizResultModel);
            }

            // Now calculate the lessons average according to the user quiz results.
            var groupedLessonList = userQuizResultModelHIghestScore.GroupBy(x => x.LessonId).Select(grp => grp.ToList()).ToList();
            List<LessonAverageResponseModel> lessonAverageResponseWithDetails = new List<LessonAverageResponseModel>();
            for (int i = 0; i < groupedLessonList.Count; i++)
            {
                LessonAverageResponseModel lessonAverageResponseModel = new LessonAverageResponseModel();
                var lessonPercentList = new List<decimal>();

                // Calculate the average of each grouped lesson 
                for (int j = 0; j < userQuizResultModelHIghestScore.Count; j++)
                {
                    if (groupedLessonList[i][0].LessonId == userQuizResultModelHIghestScore[j].LessonId)
                    {
                        lessonPercentList.Add(decimal.Parse(userQuizResultModelHIghestScore[j].TotalPercent));
                    }
                }

                lessonAverageResponseModel.LessonId = groupedLessonList[i][0].LessonId;
                lessonAverageResponseModel.LessonAverage = AverageCalculator.CalculateAverage(lessonPercentList.ToArray(), true).ToString();
                lessonAverageResponseWithDetails.Add(lessonAverageResponseModel);
            }

            averageWithLessonResult.LessonAverageList = lessonAverageResponseWithDetails;

            var averageLessonArray = new List<decimal>();
            for (int i = 0; i < lessonAverageResponseWithDetails.Count; i++)
            {
                averageLessonArray.Add(decimal.Parse(lessonAverageResponseWithDetails[i].LessonAverage));
            }
            if (averageLessonArray.Count == 0)
            {
                averageWithLessonResult.Average = "0";
            }
            else
            {
                averageWithLessonResult.Average = AverageCalculator.CalculateAverage(averageLessonArray.ToArray(), true).ToString();
            }

            return averageWithLessonResult;
        }

        /// <summary>
        /// Returns the user quiz details plus the responses.
        /// </summary>
        /// <param name="userQuizId">The user quiz id to call.</param>
        /// <param name="webServiceURL">The url of the webservice. (To retrieve the files).</param>
        /// <param name="studentId">Optional: The student's id to get the student's responses.</param>
        /// <returns>Returns the list of user quiz results responses.</returns>
        public GSWebAPI.Domain.Models.UserQuiz.UserQuizResponseModel GetUserQuizResponse(string userQuizId, string webServiceURL, string studentId = null)
        {
            UserQuizResultModel userQuizResultModelToFind = new UserQuizResultModel()
            {
                Id = userQuizId,
                UserId = studentId,
            };

            UserQuizResultDAO userQuizResultDAO = new UserQuizResultDAO();
            List<UserQuizResultModel> userQuizResultModelList = userQuizResultDAO.Read(_connectionStringRead, userQuizResultModelToFind, false, false);

            // Now get the User Quiz Result Responses we want to see 
            List<UserResponseModel> userResponseModelsToFindList = new List<UserResponseModel>();
            for (int i = 0; i < userQuizResultModelList.Count; i++)
            {
                UserResponseModel userResponseModel = new UserResponseModel
                {
                    UserQuizResultId = userQuizResultModelList[i].Id,
                };
                userResponseModelsToFindList.Add(userResponseModel);
            }

            List<UserQuizFileModel> userQuizFileModels = new UserQuizFileDAO().Read(_connectionStringRead, new UserQuizFileModel()
            {
                QuizId = userQuizResultModelList[0].QuizId,
                UserId = userQuizResultModelList[0].UserId,
            }, false, false);

            foreach (UserQuizFileModel userQuizFileModel in userQuizFileModels)
            {
                userQuizFileModel.FileUrl = webServiceURL + userQuizFileModel.FileUrl;
            }

            UserResponseDAO userResponseDAO = new UserResponseDAO();

            // Now connect everything
            UserQuizResponseModel userQuizResponseModel = new UserQuizResponseModel
            {
                UserQuizResult = userQuizResultModelList[0],
                UserResponsesList = userResponseDAO.Read(_connectionStringRead, userResponseModelsToFindList),
                UserQuizFileModelList = userQuizFileModels,
            };

            return userQuizResponseModel;
        }

        /// <summary>
        /// Returns the lesson average.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="lessonId">The lesson id.</param>
        /// <returns>Returns the average.</returns>
        public decimal CalculateUserLessonAverage(string userId, string lessonId)
        {
            UserQuizResultModel userQuizResultModel = new UserQuizResultModel
            {
                UserId = userId,
                LessonId = lessonId,
                IsExam = "1",
            };
            return CalculateAverageWithRule(userQuizResultModel);
        }

        /// <summary>
        /// To get the user lesson averages. Added on 2020.05.20. Was outside from original designed.
        /// This was created because this will be used on a list that does not depend on the subject and grade.
        /// The teacher can choose whatever grade or subject, and can have a long list, which will be appear
        /// in the student record.
        /// This is why this was created. Everytime the user does a quiz, this table is updated with the highest score.
        /// </summary>
        /// <param name="userLessonAveragesToFind">User lesson to find.</param>
        /// <returns>Returns the user lesson average list.</returns>
        private List<UserLessonAverageModel> GetUserLessonAverageModels(List<UserLessonAverageModel> userLessonAveragesToFind)
        {
            return new UserLessonAverageDAO().Read(_connectionStringRead, userLessonAveragesToFind);
        }

        /// <summary>
        /// Creates the User Lesson Average.
        /// </summary>
        /// <param name="userLessonAverageModel">UserLessonAverage model to insert.</param>
        /// <param name="userSessionId">User in session.</param>
        /// <returns>Returns a response model with user details.</returns>
        private GSWebAPI.Domain.Models.UserQuiz.ResponseModel CreateUserLessonAverage(UserLessonAverageModel userLessonAverageModel, string userSessionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userLessonAverageModel.Average) || string.IsNullOrWhiteSpace(userLessonAverageModel.LessonId) ||
                    string.IsNullOrWhiteSpace(userLessonAverageModel.UserId))
                {
                    return new GSWebAPI.Domain.Models.UserQuiz.ResponseModel()
                    {
                        TransactionValid = false,
                    };
                }

                userLessonAverageModel.ModifiedBy = userSessionId;
                UserLessonAverageDAO userLessonAverageDAO = new UserLessonAverageDAO();
                // Check if exists. If exist, enter
                List<UserLessonAverageModel> userLessonAverageModels = userLessonAverageDAO.Read(
                    _connectionStringRead,
                    new UserLessonAverageModel()
                    {
                        UserId = userLessonAverageModel.UserId,
                        LessonId = userLessonAverageModel.LessonId,
                    }, true, true);

                if (userLessonAverageModels.Count == 0)
                {
                    userLessonAverageDAO.Create(_connectionString, userLessonAverageModel);
                }
                else
                {
                    userLessonAverageDAO.Update(_connectionString, userLessonAverageModel, new UserLessonAverageModel()
                    {
                        UserId = userLessonAverageModel.UserId,
                        LessonId = userLessonAverageModel.LessonId,
                    });
                }

                return new GSWebAPI.Domain.Models.UserQuiz.ResponseModel()
                {
                    TransactionValid = true,
                };
            }
            catch (Exception)
            {
                return new GSWebAPI.Domain.Models.UserQuiz.ResponseModel()
                {
                    TransactionValid = false,
                };
            }

        }

        /// <summary>
        /// Updates the user's lesson average and completion status.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="levelCode">The level of the lesson.</param>
        /// <param name="subjectCode">The subject of the lesson.</param>
        private void UpdateUserLessonAverageAndCompletion(string userId, string lessonId, string subjectCode, string levelCode)
        {
            var userLessonAverageModelsToCheck = GetUserLessonAverageModels(
                new List<UserLessonAverageModel> { new UserLessonAverageModel { UserId = userId, LessonId = lessonId } });

            bool areLessonQuizzesCompleted = true;

            var lessonQuizzes = AthenasDetailsHandler.GetLessonQuizzes(lessonId)?.Quizzes
                .Where(q => q.IsExam != ((int)QuizType.Practice).ToString()) // Remove practices
                .Where(q => _gwpDpkToEvaluate.Any(dpk => dpk.Equals(q.LevelOfKnowledge)))
                .ToList();

            // Also, remove the ones with Available = 0;
            lessonQuizzes = lessonQuizzes?.Where(q => q.Available == "1").ToList();

            if (lessonQuizzes == null || !lessonQuizzes.Any())
            {
                areLessonQuizzesCompleted = false; // No quizzes to evaluate or an error occurred
            }

            var userCompletedQuizzes = GetUserQuizResults(new UserQuizResultModel { UserId = userId, LessonId = lessonId });

            if (userCompletedQuizzes == null || userCompletedQuizzes.Count == 0)
            {
                areLessonQuizzesCompleted = false;
            }
            else
            {
                // Check if all required quizzes (exams, pre, post) are completed
                foreach (var quizInLesson in lessonQuizzes)
                {
                    if(quizInLesson.IsExam == ((int)QuizType.PreTest).ToString() ||
                        quizInLesson.IsExam == ((int)QuizType.PostTest).ToString())
                    {
                        // Check if the user has completed this quiz
                        continue; // Skip pre and post tests for this check
                    }

                    if (!userCompletedQuizzes.Any(x => x.QuizId == quizInLesson.Id))
                    {
                        areLessonQuizzesCompleted = false;
                        break;
                    }
                }
            }

            // Check if pre & post were finished specifically
            var prePostResults = UserResults.GetUserPrePostResults(userId, lessonId);
            bool preFinished = prePostResults?.Find(x => x.IsExam == ((int)QuizType.PreTest).ToString()) != null;
            bool postFinished = prePostResults?.Find(x => x.IsExam == ((int)QuizType.PostTest).ToString()) != null;

            bool lessonFullyCompleted = areLessonQuizzesCompleted && preFinished && postFinished;

            decimal currentAverage = CalculateUserLessonAverage(userId, lessonId);

            var newUserLessonAverage = new UserLessonAverageModel
            {
                UserId = userId,
                Average = currentAverage.ToString(),
                LessonId = lessonId,
                Completed = lessonFullyCompleted ? "1" : "0",
                SubjectCode = subjectCode,
                LevelCode = levelCode
            };

            CreateUserLessonAverage(newUserLessonAverage, userId);

            // If the lesson is fully completed, update the user lesson average and completion status.
            if (lessonFullyCompleted)
            {
                new UserLessonAverageDAO().Update(_connectionString, new UserLessonAverageModel()
                {
                    UserId = userId,
                    ModifiedBy = userId,
                    LessonId = lessonId,
                    Completed = "1",
                }, new UserLessonAverageModel()
                {
                    UserId = userId,
                    LessonId = lessonId
                });
            }
        }

        /// <summary>
        /// To insert the user quiz responses.
        /// </summary>
        /// <param name="questionRequestModelAthenasList">The questions list from Athenas.</param>
        /// <param name="userQuizResultModelToInsert">THe user quiz result to insert.</param>
        /// <param name="userResponseModelList">The user response list.</param>
        /// <param name="saveResults">True to save results, false if not.</param>
        /// <param name="totalQuizValue">The total quiz value.</param>
        /// <returns>Returs the user quiz result with the results details.</returns>
        private UserQuizResultModel InsertUserQuizAndResponsesFullProccess(
            List<Domain.Models.Lesson.Request.QuestionRequestModel> questionRequestModelAthenasList,
            UserQuizResultModel userQuizResultModelToInsert, List<UserResponseModel> userResponseModelList, bool saveResults, decimal totalQuizValue)
        {

            var learningSheetIds = new List<string>();

            // Set how many value per auto correct question. 
            decimal totalValuePerQuestion = totalQuizValue / questionRequestModelAthenasList.Count;

            // Set how many the student has acumulated
            decimal studentAcumulatedTotal = 0;

            var response = new GSWebAPI.Domain.Models.UserQuiz.ResponseModel()
            {
                TransactionValid = true,
            };

            int totalCorrectOfTheUser = 0;

            // First, we compare the user responses and the questions responses.
            // Now, evaluate the results.
            // A. Confirm the user responses, so we are really sure that he or she doesn't cheat.
            for (int i = 0; i < userResponseModelList.Count; i++)
            {
                try
                {
                    // First, let's find the question we want to work with.
                    var questionRequestModel = questionRequestModelAthenasList.Find(x => x.QuizQuestionModel.Id == userResponseModelList[i].QuestionId);

                    // Now, check for the correct responses.
                    if (questionRequestModel == null)
                    {
                        userResponseModelList[i].Correct = "1";
                        totalCorrectOfTheUser++;
                        studentAcumulatedTotal += totalValuePerQuestion;
                    }
                    else
                    {
                        bool wasResponseFound = false;

                        for (int j = 0; j < questionRequestModel.QuizQuestionResponseModelList.Count; j++)
                        {
                            userResponseModelList[i].Correct = "0";
                            // If the answer matches...
                            if (questionRequestModel.QuizQuestionResponseModelList[j].Id == userResponseModelList[i].ResponseId)
                            {
                                // If correct, count as correct.
                                if (questionRequestModel.QuizQuestionResponseModelList[j].Correct == "1")
                                {
                                    userResponseModelList[i].Correct = "1";
                                    totalCorrectOfTheUser++;
                                    studentAcumulatedTotal += totalValuePerQuestion;
                                }
                                else
                                {
                                    // If incorrect, count as incorrect and set the learning sheet id.
                                    userResponseModelList[i].Correct = "0";
                                    if (!string.IsNullOrWhiteSpace(questionRequestModel.QuizQuestionModel.LearningSheetId))
                                    {
                                        learningSheetIds.Add(questionRequestModel.QuizQuestionModel.LearningSheetId);
                                    }
                                }
                                wasResponseFound = true;
                            }

                            if (wasResponseFound)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Mark as correct, and give it to the user, because the user cannot pay the consequences of a teacher changer or the error handling.
                    userResponseModelList[i].Correct = "1";
                    studentAcumulatedTotal += totalValuePerQuestion;
                }
            }

            // Now we stablished the corrects and incorrects, we calculate the final result.
            try
            {
                // First we verified if there is another try.
                UserQuizResultModel userQuizResultModelToCheck = new UserQuizResultModel
                {
                    UserId = userQuizResultModelToInsert.UserId,
                    QuizId = userQuizResultModelToInsert.QuizId,
                };
                List<UserQuizResultModel> userQuizResultModelsExistsList = GetUserQuizResults(userQuizResultModelToCheck);

                // If there is another try, just sum it, else, set as one.
                if (userQuizResultModelsExistsList.Count > 0)
                {
                    userQuizResultModelToInsert.TryNo = (int.Parse(userQuizResultModelsExistsList[0].TryNo) + 1).ToString();
                }
                else
                {
                    userQuizResultModelToInsert.TryNo = "1";
                }

                // Commented on 05.24.2020 by POR. Martinez wants that the teacher specifies how many the quiz value is. userQuizResultModelToInsert.TotalPercent = AverageCalculator.GetTotalGrade(totalCorrectOfTheUser, questionRequestModelAthenasList.Count, true).ToString();
                // The following line is the new one 05.24.2020, where each question is assigned a value.
                userQuizResultModelToInsert.TotalPercent =
            AverageCalculator.FixTwoDecimalsAfterPoint(
                (studentAcumulatedTotal / totalQuizValue) * 100m
            ).ToString();
                // End of 05.24.2020 petitions.

                userQuizResultModelToInsert.TotalCorrect = totalCorrectOfTheUser.ToString();
                userQuizResultModelToInsert.TotalQuestions = questionRequestModelAthenasList.Count.ToString();

                // Now we insert it.
                if (saveResults)
                {
                    userQuizResultModelToInsert = InsertUserQuizResult(userQuizResultModelToInsert);

                    // Set the IDs for each result.
                    for (int i = 0; i < userResponseModelList.Count; i++)
                    {
                        userResponseModelList[i].UserQuizResultId = userQuizResultModelToInsert.Id;
                    }

                    response.UserQuizResultModel = userQuizResultModelToInsert;
                    InsertUserResponseQuestionsList(userResponseModelList);
                    userQuizResultModelToInsert.Valid = true;
                }

                MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
                userQuizResultModelToInsert.Messages = new List<string>()
                {
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_code_success"]),
                };
                userQuizResultModelToInsert.LearningSheetIds = learningSheetIds;

                return userQuizResultModelToInsert;
            }
            catch (Exception)
            {
                return new UserQuizResultModel()
                {
                    Valid = false,
                };
            }
        }
    }
}
