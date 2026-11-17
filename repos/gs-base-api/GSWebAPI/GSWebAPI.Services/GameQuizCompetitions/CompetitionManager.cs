using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameQuizCompetitions;
using GSWebAPI.GSWebAPI.Services.Avatars;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.GameQuizCompetitions.CustomClasses;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;
using GSWebAPI.CustomClasses;

namespace GSWebAPI.GSWebAPI.Services.GameQuizCompetitions
{
    /// <summary>
    /// Manages the competition between students. The competitions are against the results of previous competitions of other users.
    /// The competitions are not live.
    /// </summary>
    public class CompetitionManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitionManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code.</param>
        public CompetitionManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Inserts the user response list of a single user and single quiz (DO NOT send multiple users).
        /// </summary>
        /// <param name="userResponseModelList">The User response to insert.</param>
        /// <returns>Returns if the transaction was successful or not.</returns>
        public Domain.Models.GameQuizCompetition.ResponseModel InsertUserResponses(List<UserResponseModel> userResponseModelList)
        {
            UserResponseDAO userResponseDAO = new UserResponseDAO();

            // First, we check if exists already. We are going to save just the last try.
            UserResponseModel userResponseModelToCheckIfExists = new UserResponseModel
            {
                UserId = userResponseModelList[0].UserId,
                QuizId = userResponseModelList[0].QuizId,
            };

            List<UserResponseModel> userResponseModelListCheckIfExists = new List<UserResponseModel>();
            userResponseModelListCheckIfExists = userResponseDAO.Read(_connectionStringRead, userResponseModelToCheckIfExists, true, true);

            // If exists, delete it. We just want the last response 
            if (userResponseModelListCheckIfExists.Count > 0)
            {
                UserResponseModel userToDelete = new UserResponseModel
                {
                    UserId = userResponseModelList[0].UserId,
                    QuizId = userResponseModelList[0].QuizId,
                };

                userResponseDAO.Delete(_connectionString, userToDelete);
            }

            var responseModel = new Domain.Models.GameQuizCompetition.ResponseModel
            {
                TransactionValid = true,
            };
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

            try
            {
                userResponseDAO.Create(_connectionString, userResponseModelList);
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gqz_message_code_success"]));
            }
            catch (Exception)
            {
                responseModel.TransactionValid = false;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gqz_message_code_success"]));
            }
            return responseModel;
        }

        /// <summary>
        /// To get the user opponent responses.
        /// </summary>
        /// <param name="questionRequestModelList">The questions to response.</param>
        /// <param name="userId">The user id of the competition.</param>
        /// <param name="fakeRivalId">The user id of the fake rival.</param>
        /// <param name="levelCode">The level code. Example (K, 1, 2, 3...).</param>
        /// <param name="subjectCode">Th subject code. Example: MAT-SP, SP, EN...</param>
        /// <returns>Returns the user responses.</returns>
        public List<UserResponseModel> GetUserOpponentResponses(List<QuestionRequestModel> questionRequestModelList, string userId, string fakeRivalId, string levelCode, string subjectCode)
        {
            // First, we check if there is any user that has Ready made that quizId.
            List<UserResponseModel> userResponseModelList = new List<UserResponseModel>();
            UserResponseDAO userResponseDAO = new UserResponseDAO();
            UserResponseModel userResponseModelToCheck = new UserResponseModel
            {
                QuizId = questionRequestModelList[0].QuizQuestionModel.QuizId,
            };
            userResponseModelList = userResponseDAO.Read(_connectionStringRead, userResponseModelToCheck, false, false);

            // First, we check if we can send a real one.
            var matchingValues = userResponseModelList.Where(x => x.UserId != userId).ToList();
            matchingValues.Shuffle();  // Shuffle the list to get a random user.

            var groupByUsersList = userResponseModelList.GroupBy(x => x.UserId).ToList();  // Group by User Id.

            if (matchingValues.Count == 0 || groupByUsersList.Count < int.Parse(AppConfiguration.Configuration["AppSettings:gqz_fake_profiles_counter_for_new_one"]))
            {
                return GenerateFakeUserResponse(questionRequestModelList, fakeRivalId, subjectCode, levelCode);
            }
            else
            {
                // Return a real one
                List<UserResponseModel> userResponseModelListToReturn = new List<UserResponseModel>();
                for (int i = 0; i < matchingValues.Count; i++)
                {
                    // Now we push the other details.
                    if (matchingValues[i].QuizId == userResponseModelToCheck.QuizId && matchingValues[i].UserId == matchingValues[0].UserId)
                    {
                        userResponseModelListToReturn.Add(matchingValues[i]);
                    }

                    if (userResponseModelListToReturn.Count >= questionRequestModelList.Count) // We break because this means that we have the same questions and response quantity 
                    {
                        break;
                    }
                }

                return userResponseModelListToReturn;
            }
        }

        /// <summary>
        /// Compare the user responses, insert the high score, and return the scores.
        /// </summary>
        /// <param name="questionRequestModelAthenasList">The question request model from Athenas.</param>
        /// <param name="userResponseModelList">The user responses</param>
        /// <param name="userResponseModelRivalList">The rival responses.</param>
        /// <param name="userId">The user's id.</param>
        /// <returns>Return the competition results.</returns>
        public CompetitionResponseModel CompareUserResponses(List<GSWebAPI.Domain.Models.Lesson.Request.QuestionRequestModel> questionRequestModelAthenasList, List<UserResponseModel> userResponseModelList, List<UserResponseModel> userResponseModelRivalList, string userId)
        {

            CompetitionResponseModel competitionResponseModel = new CompetitionResponseModel();
            // Now, evaluate the results.
            // A. Confirm the user responses, so we are really sure that he or she doesn't cheat.
            for (int i = 0; i < userResponseModelList.Count; i++)
            {
                userResponseModelList[i].UserId = userId;
                try
                {
                    // First, let's find the question we want to work with 
                    var questionRequestModel = questionRequestModelAthenasList.Find(x => x.QuizQuestionModel.Id == userResponseModelList[i].QuestionId);

                    // Now, check for the correct responses.
                    if (questionRequestModel == null)
                    {
                        userResponseModelList[i].Correct = "1";
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
                }
            }

            // B. Compare the user responses.
            int userPoints = 0;
            int rivalPoints = 0;
            int basePoints = int.Parse(AppConfiguration.Configuration["AppSettings:gqz_reward_high_score_standard_base_points"]);
            for (int i = 0; i < userResponseModelList.Count; i++)
            {
                // Find the rival user response.
                UserResponseModel rivalResponseToCheck = new UserResponseModel();
                rivalResponseToCheck = userResponseModelRivalList.Find(x => x.QuestionId == userResponseModelList[i].QuestionId);

                // If there is nothing to compare, just give it to the user.
                if (rivalResponseToCheck == null)
                {
                    userPoints += basePoints;
                }
                else
                {
                    // If both of them are correct.
                    if (rivalResponseToCheck.Correct == "1" && userResponseModelList[i].Correct == "1")
                    {
                        // Compare the times 
                        float userResponseTime = float.Parse(userResponseModelList[i].ResponseTime);
                        float rivalResponseTime = float.Parse(rivalResponseToCheck.ResponseTime);
                        // Give the points to the one that has the less time responding.
                        if (userResponseTime < rivalResponseTime)
                        {
                            userPoints += 1000;
                        }
                        else if (rivalResponseTime < userResponseTime)
                        {
                            rivalPoints += 1000;
                        }
                    }
                    else if (rivalResponseToCheck.Correct == "0" && userResponseModelList[i].Correct == "1")
                    {
                        userPoints += basePoints;
                    }
                    else if (rivalResponseToCheck.Correct == "1" && userResponseModelList[i].Correct == "0")
                    {
                        rivalPoints += basePoints;
                    }
                }
            }

            // Finally, declare a winner.
            if (userPoints > rivalPoints)
            {
                competitionResponseModel.UserWon = true;
                competitionResponseModel.UserTotalPoints = userPoints;
                competitionResponseModel.RivalTotalPoints = rivalPoints;

                // Sum to the highscore.
                // Get user High Score.
                HighScoreManager highScoreManager = new HighScoreManager(_connectionString, _connectionStringRead, _langCode);

                // Insert new High score.
                int userNewHighScore = competitionResponseModel.UserTotalPoints + highScoreManager.GetUserHighScore(userId, userResponseModelList[0].SubjectCode, userResponseModelList[0].LevelCode);
                HighScoreModel highScoreModel = new HighScoreModel
                {
                    UserId = userId,
                    Score = userNewHighScore.ToString(),
                    SubjectCode = userResponseModelList[0].SubjectCode,
                    LevelCode = userResponseModelList[0].LevelCode,
                };
                // Insert the user new High Score
                highScoreManager.InsertUserHighScore(highScoreModel);
                return competitionResponseModel;
            }
            else if (userPoints == rivalPoints)
            {
                // If draw, then nothing happengs, sorry, no insertion on points
                competitionResponseModel.Draw = true;
                competitionResponseModel.UserTotalPoints = userPoints;
                competitionResponseModel.RivalTotalPoints = rivalPoints;
                return competitionResponseModel;
            }
            else
            {
                competitionResponseModel.RivalWon = true;
                competitionResponseModel.UserTotalPoints = userPoints;
                competitionResponseModel.RivalTotalPoints = rivalPoints;
                return competitionResponseModel;
            }
        }

        /// <summary>
        /// To generate fake user responses.
        /// </summary>
        /// <param name="questionRequestModelList">The questions to response.</param>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="fakeRivalUserId">The user id of the fake rival.</param>
        /// <param name="subjectCode">The subject's code. Example: MAT-SP, SP, EN.</param>
        /// <param name="levelCode">The level's code. Example: PK, 1...</param>
        /// <returns>Returns the user responses.</returns>
        public List<UserResponseModel> GenerateFakeUserResponse(List<QuestionRequestModel> questionRequestModelList, string fakeRivalUserId, string subjectCode, string levelCode)
        {

            int minMilliSecondsResponse = int.Parse(AppConfiguration.Configuration["AppSettings:gqz_fake_answers_min_milliseconds"]);
            int maxMilliSecondsResponse = int.Parse(AppConfiguration.Configuration["AppSettings:gqz_fake_answers_max_milliseconds"]);

            Random random = new Random();
            string[] responseArray = { "0", "1", "1", "1", "1" }; // 2/5 to get it incorrect.
            int numberInRandom;
            int responseTimeRandom;

            // Create the fake responses 
            List<UserResponseModel> userResponseModelToInsertFakeResponsesList = new List<UserResponseModel>();
            for (int i = 0; i < questionRequestModelList.Count; i++)
            {
                // User Response Model 
                UserResponseModel userResponseModel = new UserResponseModel
                {
                    QuizId = questionRequestModelList[i].QuizQuestionModel.QuizId,
                    LevelCode = levelCode,
                    SubjectCode = subjectCode,
                    UserId = fakeRivalUserId,
                };

                // Set correct or incorrect in random
                numberInRandom = random.Next(0, 5);
                userResponseModel.Correct = responseArray[numberInRandom];
                userResponseModel.QuestionId = questionRequestModelList[i].QuizQuestionModel.Id;

                // Select the response id that is the same as the user response.
                for (int j = 0; j < questionRequestModelList[i].QuizQuestionResponseModelList.Count; j++)
                {
                    if (questionRequestModelList[i].QuizQuestionResponseModelList[j].Correct == userResponseModel.Correct)
                    {
                        userResponseModel.ResponseId = questionRequestModelList[i].QuizQuestionResponseModelList[j].Id;
                        break;
                    }
                }

                // Set the response time.
                responseTimeRandom = random.Next(minMilliSecondsResponse, maxMilliSecondsResponse);
                userResponseModel.ResponseTime = (responseTimeRandom * 0.0001).ToString(); // In seconds, since Unity takes the time in seconds
                userResponseModelToInsertFakeResponsesList.Add(userResponseModel);
            }

            // Now do the insertion.
            InsertUserResponses(userResponseModelToInsertFakeResponsesList);

            // And finally, return the user responses.
            return userResponseModelToInsertFakeResponsesList;
        }

        /// <summary>
        /// To generate the full competition details.
        /// </summary>
        /// <param name="subjectCode">The subject for the competition.</param>
        /// <param name="levelCode">The level for the competition.</param>
        /// <param name="userId">The id of the user making the request.</param>
        /// <param name="userToken">The token of the user making the request.</param>
        /// <param name="ipAddress">The ipAddress of the user making the request.</param>
        /// <param name="langCode">The language's code of the user making the request.</param>
        /// <returns>Returns the competition full details. </returns>
        public CompetitionRivalResponseModel GenerateFullCompetitionRivalProcess(string subjectCode,
            string levelCode, string userId, string userToken, string ipAddress, string langCode)
        {
            // First, identify the random quiz
            if (string.IsNullOrWhiteSpace(subjectCode) || subjectCode.ToLower() == "null")
            {
                subjectCode = "en";
            }
            if (string.IsNullOrWhiteSpace(levelCode) || levelCode.ToLower() == "null")
            {
                levelCode = "4";
            }
            // If levelCode is k, move to upper level. K is conflictive because it has a lot of videos, images, etc.
            if (levelCode.ToLower() == "k")
            {
                levelCode = "1";
            }

            var randomQuiz = IdentifyRandomQuiz(subjectCode, levelCode, userToken);

            var competitionRivalResponseModel = new CompetitionRivalResponseModel()
            {
                QuizId = randomQuiz.QuizRequestModel.QuestionRequestModelList[0].QuizQuestionModel.QuizId,
                UserRivalModel = new UserRivalModel()
                {
                    UserResponseModelList = new List<UserResponseModel>(),
                    UserAvatar3dModel = new AvatarV2JsonModel(),
                    UserAvatarModel = new UserAvatarModel(),
                }
            };

            // Now, identify a rival. First, we set a fake rival just in case. 
            string rivalId = new Generals.FakeProfileManager(_connectionString, _connectionStringRead).GetFakeProfileIdRandom();
            if (rivalId == userId || string.IsNullOrWhiteSpace(rivalId))
            {
                // Generate a random username
                competitionRivalResponseModel.UserRivalModel = GenerateRivalGeneralDetails();
                randomQuiz = IdentifyRandomQuiz(subjectCode, levelCode, userToken);
                competitionRivalResponseModel.QuizId = randomQuiz.QuizRequestModel.QuestionRequestModelList[0].QuizQuestionModel.QuizId;
                competitionRivalResponseModel.UserRivalModel.UserResponseModelList = GenerateFakeUserResponse(
                    randomQuiz.QuizRequestModel.QuestionRequestModelList, "1", subjectCode, levelCode);
                competitionRivalResponseModel.UserRivalModel.UserAvatar3dModel = null;
                return competitionRivalResponseModel;
            }

            // Get the rival responses 
            // If the quiz is real, then prepare the real responses.
            if (randomQuiz.QuizRequestModel.QuizModel != null && randomQuiz.QuizRequestModel.QuizModel.Id != "0")
            {
                competitionRivalResponseModel.UserRivalModel.UserResponseModelList =
                    GetUserOpponentResponses(
                    randomQuiz.QuizRequestModel.QuestionRequestModelList,
                    userId,
                    rivalId,
                    levelCode,
                    subjectCode);

                // Get Rival username, and personal details (sex, username, country)
                string userIdToFind = competitionRivalResponseModel.UserRivalModel.UserResponseModelList[0].UserId;

                var userRivalResponseModel = GetUserDetails.GetUserRivalGeneralDetails(userToken, ipAddress, userIdToFind, langCode);

                // Now get the other details
                competitionRivalResponseModel.UserRivalModel.Username = userRivalResponseModel.Username;

                // Get user Sex to generate the avatar in case is has to be generated
                string userSex = userRivalResponseModel.Gender;

                // Get the city and country
                competitionRivalResponseModel.UserRivalModel.City = string.Empty;
                competitionRivalResponseModel.UserRivalModel.Country = string.Empty;
                competitionRivalResponseModel.QuizId = competitionRivalResponseModel.UserRivalModel.UserResponseModelList[0].QuizId;

                // Get the rival avatar
                AvatarManager avatarManager = new AvatarManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], null);
                competitionRivalResponseModel.UserRivalModel.UserAvatarModel = avatarManager.GetRivalAvatar(userIdToFind, userSex);
                competitionRivalResponseModel.UserRivalModel.UserAvatar3dModel = new AvatarV2Manager(
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"], null)
                   .GetUser3dAvatar(userIdToFind, true);

                return competitionRivalResponseModel;
            }
            else
            {
                // Try to get any quiz then. 

                competitionRivalResponseModel.UserRivalModel = GenerateRivalGeneralDetails();
                randomQuiz = IdentifyRandomQuiz(subjectCode, levelCode, userToken);
                competitionRivalResponseModel.QuizId = randomQuiz.QuizRequestModel.QuestionRequestModelList[0].QuizQuestionModel.QuizId;
                competitionRivalResponseModel.UserRivalModel.UserResponseModelList = GenerateFakeUserResponse(
                    randomQuiz.QuizRequestModel.QuestionRequestModelList, "1", subjectCode, levelCode);
                competitionRivalResponseModel.UserRivalModel.UserAvatar3dModel = new AvatarV2Manager(
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"], null)
                   .GetUser3dAvatar("1", true);


                return competitionRivalResponseModel;
            }
        }

        /// <summary>
        /// To generate rival general details.
        /// </summary>
        /// <returns>Returns a generic random avatar.</returns>
        private UserRivalModel GenerateRivalGeneralDetails()
        {
            // Generate a random username
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] randomString = new char[5];
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                randomString[i] = characters[random.Next(characters.Length)];
            }
            // Set the generated rival
            return new UserRivalModel()
            {
                Username = new string(randomString),
                UserAvatarModel =
                   new AvatarManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], null).
                      GenerateRandomAvatar("1", "M"),
                UserResponseModelList = new List<Domain.Models.GameQuizCompetition.UserResponseModel>(),
            };
        }

        /// <summary>
        /// To identify a random quiz for the competition.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="userToken">The user's token.</param>
        /// <returns>Returns the Athenas JSON quiz details.</returns>
        private AthenasJSONQuizResponseModel IdentifyRandomQuiz(string subjectCode, string levelCode, string userToken)
        {
            AthenasJSONQuizResponseModel modelObject = new AthenasJSONQuizResponseModel();

            // Try at least with two subjects and grades. 
            if (string.IsNullOrWhiteSpace(subjectCode) || subjectCode.ToLower() == "null")
            {
                subjectCode = "en";
            }
            if (string.IsNullOrWhiteSpace(levelCode) || levelCode.ToLower() == "null")
            {
                levelCode = "4";
            }
            try
            {
                modelObject = AthenasDetailsHandler.GetRandomQuiz(subjectCode, levelCode, userToken);
            }
            catch (Exception)
            {
                try
                {
                    subjectCode = "sp";
                    levelCode = "3";
                    modelObject = AthenasDetailsHandler.GetRandomQuiz(subjectCode, levelCode, userToken);
                }
                catch (Exception)
                {
                    // Generate a fake quiz

                }
            }

            return modelObject;
        }

    }
}