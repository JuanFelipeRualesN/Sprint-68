using System;
using System.Collections.Generic;
using System.Configuration;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameUserRewards;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals;
using GSWebAPI.CustomClasses;

namespace GSWebAPI.GSWebAPI.Services.GameUserRewards
{
    /// <summary>
    /// Manages the user rewards.
    /// </summary>
    public class GameUserRewardsService
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameUserRewardsService"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code.</param>
        public GameUserRewardsService(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the user's rewards.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the user reward.</returns>
        public UserRewardModel GetUserRewards(string userId)
        {
            UserRewardModel userRewardModel = new UserRewardModel
            {
                UserId = userId,
            };

            UserRewardModel userRewardModelReturn = new UserRewardModel();

            UserRewardDAO userRewardDAO = new UserRewardDAO();
            List<UserRewardModel> userRewardModelList = userRewardDAO.Read(_connectionStringRead, userRewardModel, true, false);
            if (userRewardModelList.Count == 0)
            {
                userRewardModelReturn.DiamondsTotal = "0";
                userRewardModelReturn.CoinsTotal = "0";
                return userRewardModelReturn;
            }

            userRewardModelReturn.CoinsTotal = userRewardModelList[0].CoinsTotal;
            userRewardModelReturn.DiamondsTotal = userRewardModelList[0].DiamondsTotal;

            // If less than 0, update it to 0
            bool updateRequired = false;
            if (int.Parse(userRewardModelReturn.CoinsTotal) < 0)
            {
                userRewardModelReturn.CoinsTotal = "0";
                updateRequired = true;
            }
            if (int.Parse(userRewardModelReturn.DiamondsTotal) < 0)
            {
                userRewardModelReturn.DiamondsTotal = "0";
                updateRequired = true;
            }

            if (updateRequired)
            {
                UserRewardModel userRewardModelToUpdate = new UserRewardModel
                {
                    CoinsTotal = userRewardModelReturn.CoinsTotal,
                    DiamondsTotal = userRewardModelReturn.DiamondsTotal,
                };
                UserRewardModel userRewardModelWhere = new UserRewardModel
                {
                    UserId = userId,
                };

                userRewardDAO.Update(_connectionString, userRewardModelToUpdate, userRewardModelWhere);
            }

            return userRewardModelReturn;
        }

        /// <summary>
        /// To remove diamonds.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="quantity">The quantity to call.</param>
        /// <returns>Returns the transaction details.</returns>
        public ResponseModel RemoveDiamonds(string userId, int quantity)
        {
            UserRewardModel userRewardModel = GetUserRewards(userId);

            ResponseModel responseModel = new ResponseModel
            {
                TransactionValid = false,
            };

            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

            if (int.Parse(userRewardModel.DiamondsTotal) == 0)
            {
                responseModel.TransactionValid = true;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gr_message_success_transaction"]));
                return responseModel;
            }
            else
            {
                int newQuantity = int.Parse(userRewardModel.DiamondsTotal) - quantity;
                if (newQuantity < 0)
                {
                    newQuantity = 0;
                }

                UserRewardModel userRewardModelToUpdate = new UserRewardModel
                {
                    UserId = userId,
                    DiamondsTotal = newQuantity.ToString(),
                };
                UserRewardModel userRewardModelWhere = new UserRewardModel
                {
                    UserId = userId,
                };

                new UserRewardDAO().Update(_connectionString, userRewardModelToUpdate, userRewardModelWhere);
                responseModel.TransactionValid = true;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gr_message_success_transaction"]));
                return responseModel;
            }
        }

        /// <summary>
        /// To remove the user coins.
        /// </summary>
        /// <param name="userId">The user id to remove coins.</param>
        /// <param name="quantity">The quantity to remove.</param>
        /// <returns>Returns the transaction details.</returns>
        public ResponseModel RemoveCoins(string userId, int quantity)
        {
            UserRewardModel userRewardModel = GetUserRewards(userId);

            ResponseModel responseModel = new ResponseModel
            {
                TransactionValid = false,
            };

            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

            if (int.Parse(userRewardModel.CoinsTotal) == 0)
            {
                responseModel.TransactionValid = true;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gr_message_success_transaction"]));
                return responseModel;
            }
            else
            {
                int newQuantity = int.Parse(userRewardModel.CoinsTotal) - quantity;
                if (newQuantity < 0)
                {
                    newQuantity = 0;
                }

                UserRewardModel userRewardModelToUpdate = new UserRewardModel
                {
                    UserId = userId,
                    CoinsTotal = newQuantity.ToString(),
                };
                UserRewardModel userRewardModelWhere = new UserRewardModel
                {
                    UserId = userId,
                };

                UserRewardDAO userRewardDAO = new UserRewardDAO();
                userRewardDAO.Update(_connectionString, userRewardModelToUpdate, userRewardModelWhere);
                responseModel.TransactionValid = true;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gr_message_success_transaction"]));
                return responseModel;
            }
        }

        /// <summary>
        /// Insert or updates the user rewards.
        /// </summary>
        /// <param name="userId">The user id to update the reward.</param>
        /// <param name="userCoins">The user coins to update.</param>
        /// <param name="userDiamonds">The user diamonds to update.</param>
        /// <returns>Returns a transaction details.</returns>
        public ResponseModel InsertUserReward(string userId, string userCoins, string userDiamonds)
        {
            ResponseModel responseModel = new ResponseModel
            {
                TransactionValid = false,
            };

            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

            int userCoinsInt = 0;
            int userDiamondsInt = 0;

            // Convert the values to int
            if (string.IsNullOrWhiteSpace(userCoins))
            {
                userCoinsInt = 0;
            }
            else
            {
                try
                {
                    userCoinsInt = int.Parse(userCoins);
                }
                catch (Exception)
                {
                    userCoinsInt = 0;
                }
            }

            if (string.IsNullOrWhiteSpace(userDiamonds))
            {
                userDiamondsInt = 0;
            }
            else
            {
                try
                {
                    userDiamondsInt = int.Parse(userDiamonds);
                }
                catch (Exception)
                {
                    userDiamondsInt = 0;
                }
            }

            if(userDiamondsInt < 0)
            {
                userDiamondsInt = 0;
            }
            if (userCoinsInt < 0)
            {
                userCoinsInt = 0;
            }

            // Now check how many the user hard.
            UserRewardDAO userRewardDAO = new UserRewardDAO();
            List<UserRewardModel> userRewardModelList = userRewardDAO.Read(_connectionStringRead, new UserRewardModel
            {
                UserId = userId,
            }, true, false);

            if (userRewardModelList.Count == 0)
            {
                // Insert
                UserRewardModel userRewardModelToInsert = new UserRewardModel
                {
                    UserId = userId,
                    CoinsTotal = userCoinsInt.ToString(),
                    DiamondsTotal = userDiamondsInt.ToString(),
                };
                userRewardDAO.Create(_connectionString, userRewardModelToInsert);
                responseModel.TransactionValid = true;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gr_message_success_transaction"]));
                responseModel.UserRewardModel = new UserRewardModel
                {
                    CoinsTotal = userRewardModelToInsert.CoinsTotal,
                    DiamondsTotal = userRewardModelToInsert.DiamondsTotal,
                };
                return responseModel;
            }
            else
            {
                // Update
                UserRewardModel userRewardModelToUpdate = new UserRewardModel
                {
                    UserId = userId,
                    CoinsTotal = (int.Parse(userRewardModelList[0].CoinsTotal) + userCoinsInt).ToString(),
                    DiamondsTotal = (int.Parse(userRewardModelList[0].DiamondsTotal) + userDiamondsInt).ToString(),
                };
                UserRewardModel userRewardModelWhere = new UserRewardModel
                {
                    UserId = userId,
                };

                userRewardDAO.Update(_connectionString, userRewardModelToUpdate, userRewardModelWhere);
                responseModel.TransactionValid = true;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:gr_message_success_transaction"]));
                responseModel.UserRewardModel = new UserRewardModel
                {
                    CoinsTotal = userRewardModelToUpdate.CoinsTotal,
                    DiamondsTotal = userRewardModelToUpdate.DiamondsTotal,
                };
                return responseModel;
            }

        }

        /// <summary>
        /// To generate a reward for the user.
        /// </summary>
        /// <param name="userId">The user id to generate the reward.</param>
        /// <param name="priceNo">The number to calculate.</param>
        /// <param name="diamondUnlocked">True if diamond unlocked.</param>
        /// <param name="tryNo">The try number of the quiz.</param>
        /// <returns>Returns the user reward.</returns>
        public UserRewardModel GenerateRewardForUser(string userId, int priceNo, bool diamondUnlocked, int tryNo)
        {
            UserRewardModel userRewardModel = new UserRewardModel
            {
                UserId = userId,
                DiamondsTotal = "0",
                CoinsTotal = "0",
            };

            // Get the rewards array 
            string[] rewardsArrayStr = AppConfiguration.Configuration["AppSettings:gr_rewards_type_array_luck"].Split(',');
            int[] rewardsArrayInt = Array.ConvertAll(rewardsArrayStr, int.Parse);

            // If diamonds unlocked, then, give to the user the diamonds (Be careful. The student should not have make a diamond twice per lesson 
            if (diamondUnlocked)
            {
                userRewardModel.DiamondsTotal = "1";
                // Finally insert it
                InsertUserReward(userRewardModel.UserId, userRewardModel.CoinsTotal, userRewardModel.DiamondsTotal);
                return userRewardModel;
            }

            userRewardModel.CoinsTotal = Convert.ToInt32(int.Parse(AppConfiguration.Configuration["AppSettings:gr_rewards_value_coins_default"]) * priceNo / (tryNo * 4)).ToString(); // Do the function and round to int   
            if (int.Parse(userRewardModel.CoinsTotal) < 0)
            {
                userRewardModel.CoinsTotal = "0";
            }

            // Finally insert it
            InsertUserReward(userRewardModel.UserId, userRewardModel.CoinsTotal, userRewardModel.DiamondsTotal);

            return userRewardModel;
        }

        /// <summary>
        /// To generate random rewards for the user.
        /// </summary>
        /// <param name="userId">The user id that will receive a random reward.</param>
        /// <returns>The user reward details.</returns>
        public UserRewardModel GenerateRandomRewardForUser(string userId)
        {
            UserRewardModel userRewardModel = new UserRewardModel
            {
                UserId = userId,
                DiamondsTotal = "0",
                CoinsTotal = "0",
            };

            // Get the rewards array.
            string[] rewardsArrayStr = AppConfiguration.Configuration["AppSettings:gr_rewards_type_array_luck"].Split(',');
            int[] rewardsArrayInt = Array.ConvertAll(rewardsArrayStr, int.Parse);

            // Now
            Random random = new Random();
            int[] userLuckArray = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3 };

            int userLuck = userLuckArray[random.Next(0, userLuckArray.Length)];
            int rewardType = rewardsArrayInt[random.Next(0, rewardsArrayInt.Length)];

            int totalRewardWithLesserValue = 0;

            // The rewards with lesser value.
            if (rewardType == 1)
            {
                // Coins rewards
                totalRewardWithLesserValue = int.Parse(AppConfiguration.Configuration["AppSettings:gr_rewards_value_coins_default"]) * userLuck;
                userRewardModel.CoinsTotal = totalRewardWithLesserValue.ToString();
            }
            else
            {
                userRewardModel.DiamondsTotal = userLuck.ToString();
            }

            // Finally insert it
            InsertUserReward(userRewardModel.UserId, userRewardModel.CoinsTotal, userRewardModel.DiamondsTotal);

            return userRewardModel;
        }
       
        /// <summary>
        /// To generate the reward according to the results.
        /// </summary>
        /// <param name="quizResult">The user quiz result to evaluate.</param>
        /// <param name="score">The score.</param>
        /// <param name="isExam">The type (isExam = 1 then is an exam, if 0 is a practice, if 2 a pre, if 3 a post).</param>
        /// <param name="maxRewardsManager">Max Rewards Manager.</param>
        /// <returns>Returns the user General results.</returns>
        public UserRewardModel GenerateReward(Domain.Models.UserQuiz.UserQuizResultModel quizResult, double score, string isExam)
        {
            var reward = new UserRewardModel();
            int tryNo = int.Parse(quizResult.TryNo);

            if (score >= int.Parse(AppConfiguration.Configuration["AppSettings:usqui_min_diamond_score"]))
            {
                if (isExam == "1")
                {
                    bool diamondUnlocked = CheckIfUserUnlockedQuizDiamond(quizResult.QuizId, quizResult.UserId);
                    if (!diamondUnlocked)
                    {
                        reward = GenerateRewardForUser(quizResult.UserId, 0, true, tryNo);
                        CreateUserUnlockedQuizDiamond(quizResult.QuizId, quizResult.UserId);
                    }
                    else
                    {
                        reward = GenerateRewardForUser(quizResult.UserId, 4, false, tryNo);
                    }
                }
                else
                {
                    reward = GenerateRewardForUser(quizResult.UserId, 4, false, tryNo);
                }
            }
            else if (score >= 90)
            {
                reward = GenerateRewardForUser(quizResult.UserId, 3, false, tryNo);
            }
            else if (score >= 80)
            {
                reward = GenerateRewardForUser(quizResult.UserId, 2, false, tryNo);
            }
            else if (score >= 70)
            {
                reward = GenerateRewardForUser(quizResult.UserId, 1, false, tryNo);
            }
            else
            {
                reward.CoinsTotal = "0";
                reward.DiamondsTotal = "0";
            }

            return reward;
        }

        /// <summary>
        /// Verifies if the user unlocked the lesson diamond.
        /// </summary>
        /// <param name="quizId">The quiz ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>Returns true if unlocked, else, false.</returns>
        public bool CheckIfUserUnlockedQuizDiamond(string quizId, string userId)
        {
            return new UserRewardMaxPricePerQuizDAO().Read(
                _connectionStringRead,
                new UserRewardMaxPricePerQuizModel
                {
                    QuizId = quizId,
                    UserId = userId,
                },
                true,
                true).Count > 0;
        }

        /// <summary>
        /// Creates the user unlocked lesson diamond.
        /// </summary>
        /// <param name="quizId">The quiz ID.</param>
        /// <param name="userId">The user ID.</param>
        public void CreateUserUnlockedQuizDiamond(string quizId, string userId)
        {
            new UserRewardMaxPricePerQuizDAO().Create(_connectionString, new UserRewardMaxPricePerQuizModel
            {
                QuizId = quizId,
                UserId = userId,
            });
        }
    }
}
