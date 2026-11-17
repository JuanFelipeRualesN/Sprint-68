using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Generals.Requests;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.UserQuizzes;
using TeachersAPI.TeachersAPI.Services.UserQuizzes.CustomClasses;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI.TeachersAPI.Services.Lessons;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.Utils;
using ResponseModel = TeachersAPI.Domain.Models.UserQuizzes.ResponseModel;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.UserQuizzes
{
    /// <summary>
    /// To handle the student quizzes results and related details to the quizzes..
    /// </summary>
    public class UserQuizManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code.</param>
        public UserQuizManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To insert the user quiz results. (Custom quizzes created by the teacher)
        /// </summary>
        /// <param name="questionRequestModelList">The questions of the quiz.</param>
        /// <param name="userQuizResultModelToInsert">The result details.</param>
        /// <param name="userResponseModelList">The user response.</param>
        /// <param name="saveResults">To save the quiz or not. (true to save, else false).</param>
        /// <param name="totalQuizValue">The total quiz value.</param>
        /// <returns>Returns the user quiz result details.</returns>
        public UserQuizResultModel InsertUserQuizAndResponsesFullProccess(List<QuestionRequestModel> questionRequestModelList, UserQuizResultModel userQuizResultModelToInsert, List<UserResponseModel> userResponseModelList, bool saveResults, decimal totalQuizValue)
        {
            // Set how many value per auto correct question. 
            decimal totalValuePerQuestion = totalQuizValue / questionRequestModelList.Count;

            // Set how many the student has acumulated
            decimal studentAcumulatedTotal = 0;

            ResponseModel response = new ResponseModel()
            {
                TransactionValid = true
            };

            int totalCorrectOfTheUser = 0;

            // First, we compare the user responses and the questions responses 
            // Now, evaluate the results 
            // A. Confirm the user responses, so we are really sure that he or she doesn't cheat 
            for (int i = 0; i < userResponseModelList.Count; i++)
            {
                try
                {
                    // First, let's find the question we want to work with 
                    QuestionRequestModel questionRequestModel = questionRequestModelList.Find(x => x.QuizQuestionModel.Id == userResponseModelList[i].QuestionId);

                    // Now, check for the correct responses 
                    if (questionRequestModel == null)
                    {
                        userResponseModelList[i].Correct = "1";
                        totalCorrectOfTheUser++;
                        studentAcumulatedTotal += totalValuePerQuestion;
                    }
                    else
                    {
                        // Now
                        bool wasResponseFound = false;

                        for (int j = 0; j < questionRequestModel.QuizQuestionResponseModelList.Count; j++)
                        {
                            userResponseModelList[i].Correct = "0";
                            if (questionRequestModel.QuizQuestionResponseModelList[j].Id == userResponseModelList[i].ResponseId && questionRequestModel.QuizQuestionResponseModelList[j].Correct == "1")
                            {
                                userResponseModelList[i].Correct = "1";
                                totalCorrectOfTheUser++;
                                wasResponseFound = true;
                                studentAcumulatedTotal += totalValuePerQuestion;
                            }

                            if (wasResponseFound)
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    // Mark as correct, and give it to the user, because the user cannot pay the consequences of a teacher changer or the error handling 
                    userResponseModelList[i].Correct = "1";
                    studentAcumulatedTotal += totalValuePerQuestion;
                }
            }

            // Now we stablished the corrects and incorrects, we calculate the final result 
            try
            {
                // First we verified if there is another try 
                UserQuizResultModel userQuizResultModelToCheck = new UserQuizResultModel
                {
                    UserId = userQuizResultModelToInsert.UserId,
                    QuizId = userQuizResultModelToInsert.QuizId
                };
                List<UserQuizResultModel> userQuizResultModelsExistsList = GetUserQuizResults(userQuizResultModelToCheck);
                // If there is another try, just sum it, else, set as one
                if (userQuizResultModelsExistsList.Count > 0)
                    userQuizResultModelToInsert.TryNo = (int.Parse(userQuizResultModelsExistsList[0].TryNo) + 1).ToString();
                else
                    userQuizResultModelToInsert.TryNo = "1";

                // Commented on 05.24.2020 by POR. Martinez wants that the teacher specifies how many the quiz value is. userQuizResultModelToInsert.TotalPercent = AverageCalculator.GetTotalGrade(totalCorrectOfTheUser, questionRequestModelAthenasList.Count, true).ToString();
                // The following line is the new one 05.24.2020, where each question is assigned a value. 
                userQuizResultModelToInsert.TotalPercent = AverageCalculator.FixTwoDecimalsAfterPoint(studentAcumulatedTotal / totalQuizValue * decimal.Parse(100.ToString())).ToString();
                // End of 05.24.2020 petitions.

                userQuizResultModelToInsert.TotalCorrect = totalCorrectOfTheUser.ToString();
                userQuizResultModelToInsert.TotalQuestions = questionRequestModelList.Count.ToString();

                // Now we insert it 
                if (saveResults)
                {
                    userQuizResultModelToInsert = InsertUserQuizResult(userQuizResultModelToInsert);
                    // Set the IDs for each result 
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
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_code_success"])
                };
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

        /// <summary>
        /// To correct a mix quiz (a quiz that has questions of different types).
        /// </summary>
        /// <param name="userId">The user's id (student's id).</param>
        /// <param name="authorizationCode">The authorization code.</param>
        /// <param name="studentQuizRequestModel">The student quiz request model with responses and so on.</param>
        /// <returns>Returns the transaction details.</returns>
        public ResponseModel CorrectUserQuiz(string userId, string authorizationCode, StudentQuizRequestModel studentQuizRequestModel)
        {
            try
            {
                // First, get the quiz id and user id from the authorization code. 
                var userQuizAuthorizationCodeModels = GetUserQuizAuthorizationCodeModels(
                    new List<UserQuizAuthorizationCodeModel>()
                    {
                        new UserQuizAuthorizationCodeModel()
                        {
                            AuthorizationToken = authorizationCode,
                            UserId = userId,
                        }
                    });

                // If the user is here, probably is tricking, since the authorization code is created before this, so we do not return too much info.
                if (userQuizAuthorizationCodeModels.Count == 0)
                {
                    return new ResponseModel()
                    {
                        TransactionValid = false,
                    };
                }

                // Check the plans that have the the assignment
                // First, find the plans of the user that have the homework. 
                var planQuizzesIds = TeacherPlans.GetPlanQuizIdsAccordingToQuizAndStudent(userId,
                    userQuizAuthorizationCodeModels[0].CustomQuizId);
                if (planQuizzesIds == null || planQuizzesIds.Count == 0)
                {
                    return new ResponseModel()
                    {
                        TransactionValid = false,
                    };
                }

                // Now, check if auto correction. If auto correction is false, just inset it. 
                // Call the quiz details 
                var quizReaderManager = new QuizReaderManager(
                    this._connectionStringRead, null);
                var quizRequestModel = quizReaderManager.GetQuiz(
                    userQuizAuthorizationCodeModels[0].CustomQuizId, true);
                
                // Check if the quiz can be corrected automatically by evaluating all the question types. 
                var questionTypes = new QuizQuestionTypesDAO().Read(this._connectionStringRead,
                    new QuizQuestionTypesModel(), false, false);
                
                // If the quiz has a question type that is not auto correctable, then we cannot correct it automatically.
                var correctAutomatically = true;
                foreach (var question in quizRequestModel.QuestionRequestModelList)
                {
                    var quizQuestionType = question.QuizQuestionModel.QuestionTypeId; 
                    var questionType = questionTypes.Find(x => x.Id == quizQuestionType);

                    if (questionType.AutoCorrect == "0")
                    {
                        correctAutomatically = false;
                        break;
                    }
                }
                
                // Prepare the user result
                var userQuizResultModelToInsert = new Domain.Models.UserQuizzes.UserQuizResultModel
                {
                    UserId = userId,
                    QuizId = userQuizAuthorizationCodeModels[0].CustomQuizId,
                    TotalQuestions = quizRequestModel.QuestionRequestModelList.Count.ToString(),
                    ModifiedBy = userId,
                    IsExam = "1",
                };

                // Prepare the user responses
                var userResponseModelListToInsert = new List<Domain.Models.UserQuizzes.UserResponseModel>();
                foreach (QuestionQuiz questionQuiz in studentQuizRequestModel.UserQuizFullDetailModel.QuestionQuizList)
                {
                    
                    var userResponseModelToInsert = new Domain.Models.UserQuizzes.UserResponseModel
                    {
                        QuestionId = questionQuiz.QuestionId,
                        ResponseId = questionQuiz.QuestionResponses[0].ResponseUserId,
                        ResponseExtra = questionQuiz.QuestionResponses[0].ResponseExtra,
                        ModifiedBy = userId
                    };
                    userResponseModelListToInsert.Add(userResponseModelToInsert);
                }

                // Can be corrected automatically 
                if (correctAutomatically)
                {
                    userQuizResultModelToInsert.Status = "C"; // Completed
                    var responseUserQuiz = InsertUserQuizAndResponsesFullProccess(
                        quizRequestModel.QuestionRequestModelList, userQuizResultModelToInsert,
                        userResponseModelListToInsert, true, decimal.Parse(quizRequestModel.QuizModel.CustomValue));

                    // Now prepare the results to insert 
                    var planAssignmentUserResultsToInsert = new List<PlanAssignmentUserResultModel>();
                    foreach (var teacherQuiz in planQuizzesIds)
                    {
                        planAssignmentUserResultsToInsert.Add(new PlanAssignmentUserResultModel()
                        {
                            PlanQuizId = teacherQuiz.Id,
                            UserId = userQuizResultModelToInsert.UserId,
                            UsquiUserResultId = responseUserQuiz.Id,
                            ModifiedBy = userId,
                            Score = responseUserQuiz.TotalAccumulated.ToString("F2"),
                            TotalPercent = responseUserQuiz.TotalPercent,
                        });
                    }

                    new PlanningModifierManager(
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                        null).AddPlanAssignmentUserResult(planAssignmentUserResultsToInsert, userId);
                }
                else
                {
                    // Just insert
                    userQuizResultModelToInsert.Status = "P"; // Pending to correction.
                    userQuizResultModelToInsert = InsertUserQuizResult(userQuizResultModelToInsert);
                    // Set the Id for each question 
                    foreach (var userResponse in userResponseModelListToInsert)
                    {
                        userResponse.UserQuizResultId = userQuizResultModelToInsert.Id;
                    }

                    InsertUserResponseQuestionsList(userResponseModelListToInsert);

                    var planAssignmentUserResultsToInsert = new List<PlanAssignmentUserResultModel>();
                    foreach (var teacherQuiz in planQuizzesIds)
                    {
                        planAssignmentUserResultsToInsert.Add(new PlanAssignmentUserResultModel()
                        {
                            PlanQuizId = teacherQuiz.Id,
                            UserId = userQuizResultModelToInsert.UserId,
                            UsquiUserResultId = userQuizResultModelToInsert.Id,
                            ModifiedBy = userId,
                        });
                    }

                    new PlanningModifierManager(
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                        null).AddPlanAssignmentUserResult(planAssignmentUserResultsToInsert, userId);
                }

                // Delete authorization code 
                DeleteAuthorizationCodeCustomQuizId(authorizationCode);
                return new ResponseModel()
                {
                    TransactionValid = true,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    TransactionValid = false,
                };
            }
        }

        /// <summary>
        /// Return a list of the user quiz results (the quizzes that already been completed by the user).
        /// </summary>
        /// <param name="userQuizResultModel">The user quiz result model to filter if anything.</param>
        /// <returns>Returns a list of UserQuizResultModel with the results details.</returns>
        public List<UserQuizResultModel> GetUserQuizResults(UserQuizResultModel userQuizResultModel)
        {
            return new UserQuizResultDAO().Read(_connectionStringRead, userQuizResultModel, false, true);
        }

        /// <summary>
        /// Return a list of the user quiz results (the quizzes that already been completed by the user).
        /// </summary>
        /// <param name="userQuizResultModels">A list of user quiz result model to filter if anything.</param>
        /// <returns>Returns a list of UserQuizResultModel with the results details.</returns>
        public List<UserQuizResultModel> GetUsersQuizResults(List<UserQuizResultModel> userQuizResultModels)
        {
            return new UserQuizResultDAO().Read(_connectionStringRead, userQuizResultModels);
        }

        /// <summary>
        /// Inserts the User Quiz Result and return the object with all details.
        /// </summary>
        /// <param name="userQuizResultModelToInsert">The User Quiz Result Model to Insert/</param>
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
        /// To correct a quiz manually for those quizzes that are not automatically corrected.
        /// </summary>
        /// <param name="userQuizResponseModels">The user responses.</param>
        /// <param name="userQuizId">The user quiz id.</param>
        /// <param name="userSessionId">The id of the user that is making the request.</param>
        /// <param name="totalQuizValue">The total value of the quiz.</param>
        /// <param name="feedback">A feedback made by the teacher if any.</param>
        /// <returns>Returns a response model with details.</returns>
        public UserQuizResultModel CorrectManualQuiz(string planQuizId, 
            List<UserResponseModel> userQuizResponseModels, 
            string userQuizId, string userSessionId, string feedback = null)
        {
            // Get the quiz by the plan quiz id
            var planningQuiz = new PlanningReaderManager(this._connectionStringRead, this._langCode).GetTeacherPlanQuizModels(new List<TeacherPlanQuizModel>()
            {
                new TeacherPlanQuizModel()
                {
                    Id = planQuizId,
                }
            });
            
            if(planningQuiz == null || planningQuiz.Count == 0)
            {
                return new UserQuizResultModel()
                {
                    Valid = false,
                    Messages = {
                        new MessageManager(_connectionStringRead, _langCode).
                            GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_quiz_not_found"])
                    }
                };
            }
            
            // First, lets call the quiz
            var quiz = new QuizReaderManager(this._connectionStringRead, this._langCode).GetQuiz(planningQuiz[0].QuizId, false, null,
                false);
            
            // If quiz is null, return an error
            if (quiz == null)
            {
                return new UserQuizResultModel()
                {
                    Valid = false,
                    Messages = {
                        new MessageManager(_connectionStringRead, _langCode).
                            GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_quiz_not_found"])
                    }
                };
            }
            
            var userResponseToUpdate = new List<UserResponseModel>();
            var userResponseToUpdateWhere = new List<UserResponseModel>();

            decimal assignedTotal = 0;
            decimal totalValue = quiz.QuestionRequestModelList.Sum(question => decimal.Parse(question.QuizQuestionModel.CustomValue));

            foreach (var userQuiz in userQuizResponseModels)
            {
                // Compare the user response value with the question value
                // Find question. 
                var question = quiz.QuestionRequestModelList.Find(x => x.QuizQuestionModel.Id == userQuiz.QuestionId);
                var assignedValue = decimal.Parse(userQuiz.AssignedValue);
                if(assignedValue > decimal.Parse(question.QuizQuestionModel.CustomValue))
                {
                    return new UserQuizResultModel()
                    {
                        Valid = false,
                        Messages = {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_assigned_value_more_than_question_value"])
                        }
                    };
                }
                
                if(assignedValue < 0)
                {
                    return new UserQuizResultModel()
                    {
                        Valid = false,
                        Messages = {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_assigned_value_less_than_zero"])
                        }
                    };
                }
                
                userResponseToUpdate.Add(new UserResponseModel()
                {
                    AssignedValue = userQuiz.AssignedValue,
                    ModifiedBy = userSessionId,
                });
                
                userResponseToUpdateWhere.Add(new UserResponseModel()
                {
                    QuestionId = userQuiz.QuestionId,
                    Id = userQuiz.Id,
                    UserQuizResultId = userQuiz.UserQuizResultId,
                });
                
                assignedTotal += assignedValue;
            }

            if (assignedTotal > totalValue)
            {
                return new UserQuizResultModel()
                {
                    Valid = false,
                    Messages = {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_assigned_value_more_than_total_value"])
                    }
                };
            }

            decimal totalPercent = assignedTotal / totalValue * 100;
            string totalToInsert = AverageCalculator.FixTwoDecimalsAfterPoint(totalPercent).ToString();
            
            new UserResponseDAO().Update(_connectionString, userResponseToUpdate, userResponseToUpdateWhere);
            
            new UserQuizResultDAO().Update(_connectionString, new UserQuizResultModel()
            {
                ModifiedBy = userSessionId,
                Status = "C",
                TotalPercent = totalToInsert,
                Feedback = feedback
            },
            new UserQuizResultModel()
            {
                Id = userQuizId,
            });
            

            // Read the updated user quiz 
            UserQuizResultModel userQuizResultToReturn = new UserQuizResultDAO().Read(_connectionStringRead, new UserQuizResultModel() { Id = userQuizId }, true, true)[0];
            userQuizResultToReturn.Valid = true;
            userQuizResultToReturn.Messages = new List<string>()
            {
                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_code_success"])
            };
            
            // Save the quiz in plan user quiz results. 
            // Now prepare the results to insert 
            var planAssignmentUserResultsToInsert = new List<PlanAssignmentUserResultModel>();
            planAssignmentUserResultsToInsert.Add(new PlanAssignmentUserResultModel()
            {
                PlanQuizId = planQuizId,
                UserId = userQuizResultToReturn.UserId,
                UsquiUserResultId = userQuizResultToReturn.Id,
                ModifiedBy = userSessionId,
                Score = assignedTotal.ToString("F2"),
                TotalPercent = totalToInsert,
            });
            
            new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null).
                AddPlanAssignmentUserResult(planAssignmentUserResultsToInsert, userSessionId);

            return userQuizResultToReturn;
        }

        /// <summary>
        /// Returns user quiz responses plus the result.
        /// </summary>
        /// <param name="userQuizId">The user quiz id.</param>
        /// <param name="webServiceURL">The web service URL to set a file full path if a file was attached to the quiz response.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns the user quiz response with all details.</returns>
        public UserQuizResponseModel GetUserQuizResponse(string userQuizId, string webServiceURL, string studentId = null)
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
                    UserQuizResultId = userQuizResultModelList[i].Id
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
                userQuizFileModel.FileUrl = AwsAssets.GetSignedUrlFronCloudFront(userQuizFileModel.FileUrl);
            }

            UserResponseDAO userResponseDAO = new UserResponseDAO();
            // Now connect everything 
            UserQuizResponseModel userQuizResponseModel = new UserQuizResponseModel
            {
                UserQuizResult = userQuizResultModelList[0],
                UserResponsesList = userResponseDAO.Read(_connectionStringRead, userResponseModelsToFindList),
                UserQuizFileModelList = userQuizFileModels
            };

            return userQuizResponseModel;
        }

        /// <summary>
        /// To correct a quiz manually for those quizzes that are not automatically corrected.
        /// </summary>
        /// <param name="userQuizResponseModels">The user responses.</param>
        /// <param name="userQuizId">The user quiz id.</param>
        /// <param name="userSessionId">The id of the user that is making the request.</param>
        /// <param name="totalQuizValue">The total value of the quiz.</param>
        /// <param name="feedback">A feedback made by the teacher if any.</param>
        /// <returns>Returns a response model with details.</returns>
        public UserQuizResultModel CorrectManualQuiz(List<UserResponseModel> userQuizResponseModels, string userQuizId, string userSessionId, decimal totalQuizValue, string feedback = null)
        {
            // Evaluate totals
            decimal assignedTotal = 0;
            foreach (UserResponseModel userQuiz in userQuizResponseModels)
            {
                assignedTotal += decimal.Parse(userQuiz.AssignedValue);
            }

            if (assignedTotal > totalQuizValue)
            {
                return new UserQuizResultModel()
                {
                    Valid = false,
                    Messages = {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:usqui_message_assigned_value_more_than_total_value"])
                    }
                };
            }

            decimal total = assignedTotal / totalQuizValue * 100;

            var userResponseToUpdate = new List<UserResponseModel>();
            var userResponseToUpdateWhere = new List<UserResponseModel>();
            foreach (var userResponseModel in userQuizResponseModels)
            {
                userResponseToUpdate.Add(new UserResponseModel()
                {
                    AssignedValue = userResponseModel.AssignedValue,
                    ModifiedBy = userSessionId,
                });
                userResponseToUpdateWhere.Add(new UserResponseModel()
                {
                    QuestionId = userResponseModel.QuestionId,
                    Id = userResponseModel.Id,
                    UserQuizResultId = userResponseModel.UserQuizResultId,
                });
            }

            new UserResponseDAO().Update(_connectionString, userResponseToUpdate, userResponseToUpdateWhere);

            new UserQuizResultDAO().Update(_connectionString, new UserQuizResultModel()
            {
                ModifiedBy = userSessionId,
                Status = "C",
                TotalPercent = CustomClasses.AverageCalculator.FixTwoDecimalsAfterPoint(total).ToString(),
                Feedback = feedback
            },
            new UserQuizResultModel()
            {
                Id = userQuizId,
            });

            // Read the updated user quiz 
            UserQuizResultModel userQuizResultToReturn = new UserQuizResultDAO().Read(_connectionStringRead, new UserQuizResultModel() { Id = userQuizId }, true, true)[0];

            userQuizResultToReturn.Valid = true;
            userQuizResultToReturn.Messages = new List<string>()
            {
                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_code_success"])
            };

            return userQuizResultToReturn;
        }

        /// <summary>
        /// Generates an authorization code that will allow the student to take the custom quiz/homework.
        /// </summary>
        /// <param name="userToTakeExamId">The student (user's) id.</param>
        /// <param name="quizId">The quiz id to take.</param>
        /// <param name="userSessionId">The user id that is making the request.</param>
        /// <returns>Returns a string with the authorizacion code.</returns>
        public string GenerateTokenAuthorizationForCustomQuiz(string userToTakeExamId, string quizId, string userSessionId)
        {
            try
            {
                string userToken = TokenGenerator.GenerateAlphanumericToken(50);

                new UserQuizAuthorizationCodeDAO().Create(_connectionString, new UserQuizAuthorizationCodeModel()
                {
                    UserId = userToTakeExamId,
                    CustomQuizId = quizId,
                    AuthorizationToken = userToken,
                    ModifiedBy = userSessionId,
                });

                return userToken;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the authorization code based on models.
        /// </summary>
        /// <param name="userQuizAuthorizationCode">User Authorization code model.</param>
        /// <returns>Returns a list of uthorization code model list.</returns>
        public List<UserQuizAuthorizationCodeModel> GetUserQuizAuthorizationCodeModels(List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCode)
        {
            return new UserQuizAuthorizationCodeDAO().Read(_connectionStringRead, userQuizAuthorizationCode);
        }

        /// <summary>
        /// Creates user quiz file.
        /// </summary>
        /// <param name="userQuizFileModel">User quiz file model details.</param>
        /// <returns>Returns the created User Quiz File.</returns>
        public List<UserQuizFileModel> CreateUserQuizFileModel(UserQuizFileModel userQuizFileModel)
        {
            return new UserQuizFileDAO().Create(_connectionString, userQuizFileModel);
        }

        /// <summary>
        /// Deletes the authorization code custom quiz id. If deleted, if the user tries to use this, it will fail.
        /// </summary>
        /// <param name="authorizationCode">The authorization code to delete.</param>
        /// <returns>Returns a response model if the transaction was valid or not.</returns>
        public ResponseModel DeleteAuthorizationCodeCustomQuizId(string authorizationCode)
        {
            try
            {
                new UserQuizAuthorizationCodeDAO().Delete(_connectionString, new UserQuizAuthorizationCodeModel()
                {
                    AuthorizationToken = authorizationCode,
                });

                return new ResponseModel()
                {
                    TransactionValid = true,
                };
            }
            catch (Exception)
            {
                return new ResponseModel()
                {
                    TransactionValid = false,
                };
            }
        }

    }
}
