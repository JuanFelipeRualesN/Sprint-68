using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameQuizCompetitions;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;

namespace GSWebAPI.GSWebAPI.Services.GameQuizCompetitions
{
    /// <summary>
    /// Manages the high scores, as the top 10 users by subject and level, and so on.
    /// </summary>
    public class HighScoreManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighScoreManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringRead">The connection string read.</param>
        /// <param name="langCode">The language code.</param>
        public HighScoreManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To check if the user got a bonus. This is a bonus by connecting to the APP.
        /// </summary>
        /// <param name="rewardType">Type of reward. If sapphire (2) or coins (1).</param>
        /// <param name="rewardQuantity">How many coins or sapphires.</param>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the user's bonus.</returns>
        public UserBonusLogModel CheckIfUserReceiveBonus(string rewardType, string rewardQuantity, string userId)
        {
            UserBonusLogModel userBonusLogModelToReturn = new UserBonusLogModel();

            // First we check if the user received a bonus in the las 24 hours.
            UserBonusLogDAO userBonusLogDAO = new UserBonusLogDAO();
            List<UserBonusLogModel> userBonusLogModelListResult = userBonusLogDAO.Read(_connectionStringRead, new UserBonusLogModel
            {
                UserId = userId,
            }, true, true);

            if (userBonusLogModelListResult.Count > 0)
            {
                // If the user has receive bonus in the last 24 hours, then don't add new ones 
                if (DateTime.Parse(userBonusLogModelListResult[0].CreatedAt) > DateTime.Now.AddDays(-1))
                {
                    userBonusLogModelToReturn.RewardType = "0";
                    userBonusLogModelToReturn.RewardQuantity = "0";
                    return userBonusLogModelToReturn;
                }
            }

            // else, add some rewards.
            UserBonusLogModel userBonusLogModelToAdd = new UserBonusLogModel
            {
                UserId = userId,
                RewardQuantity = rewardQuantity,
                RewardType = rewardType,
            };

            UserRewardModel userRewardModelToAdd = new UserRewardModel
            {
                UserId = userId,
            };

            if (AppConfiguration.Configuration["AppSettings:mg_diamonds_code"] == rewardType)
            {
                userRewardModelToAdd.DiamondsTotal = rewardQuantity;
                userRewardModelToAdd.CoinsTotal = "0";
            }
            else
            {
                userRewardModelToAdd.CoinsTotal = rewardQuantity;
                userRewardModelToAdd.DiamondsTotal = "0";
            }

            GameUserRewardsService gameUserRewardsManager = new GameUserRewardsService(_connectionString, _connectionStringRead, _langCode);
            gameUserRewardsManager.InsertUserReward(userId, userRewardModelToAdd.CoinsTotal, userRewardModelToAdd.DiamondsTotal);

            userBonusLogModelToReturn.RewardType = rewardType;
            userBonusLogModelToReturn.RewardQuantity = rewardQuantity;

            userBonusLogDAO.Create(_connectionString, userBonusLogModelToAdd);
            return userBonusLogModelToReturn;
        }

        /// <summary>
        /// To get the top high scores.
        /// </summary>
        /// <param name="totalTop">The total top numbers to call. Example: 10 to Top 10.</param>
        /// <param name="levelCode">The level's code. (Example: K, 1, 2, 3...).</param>
        /// <param name="subjectCode">The subject's code. (Example: SP, EN, MAT-SP...).</param>
        /// <returns>Returns the top highest scores.</returns>
        public List<HighScoreResponseModel> GetTopHighScore(int totalTop, string levelCode, string subjectCode)
        {
            List<HighScoreResponseModel> highScoreResponseModelList = new List<HighScoreResponseModel>();
            HighScoreModel highScoreResponseModelToFind = new HighScoreModel
            {
                LevelCode = levelCode,
                SubjectCode = subjectCode,
            };
            HighScoreDAO highScoreDAO = new HighScoreDAO();
            List<HighScoreModel> highScoreModelResponseList = new List<HighScoreModel>();
            highScoreModelResponseList = highScoreDAO.Read(_connectionStringRead, highScoreResponseModelToFind, false, false);

            // Now get the top users.
            List<HighScoreModel> highScoreModelSorted = new List<HighScoreModel>();
            highScoreModelSorted = (from item in highScoreModelResponseList
                                    orderby int.Parse(item.Score) descending
                                    select item).ToList();

            // Now get just the top users to find
            int usersTotalToFind = totalTop;
            if (highScoreModelSorted.Count < usersTotalToFind)
            {
                usersTotalToFind = highScoreModelSorted.Count;
            }

            List<string> userIdList = new List<string>();

            // Now connect everything.
            for (int i = 0; i < usersTotalToFind; i++)
            {
                HighScoreResponseModel highScoreResponseModel = new HighScoreResponseModel
                {
                    Score = highScoreModelSorted[i].Score,
                    SubjectCode = highScoreModelSorted[i].SubjectCode,
                    LevelCode = highScoreModelSorted[i].LevelCode,
                    UserId = highScoreModelSorted[i].UserId,
                };

                highScoreResponseModelList.Add(highScoreResponseModel);
            }

            return highScoreResponseModelList;
        }

        /// <summary>
        /// Returns the user high score details.
        /// </summary>
        /// <param name="userId">The user Id we want to check.</param>
        /// <param name="subjectCode">The subject code to find (example: SCI-SP, MAT-SP, SP, EN, etc.).</param>
        /// <param name="levelCode">The level code (Example: K, 1, 2...)</param>
        /// <returns>Returns the User high score in 0.</returns>
        public int GetUserHighScore(string userId, string subjectCode, string levelCode)
        {
            HighScoreModel highScoreModelToFind = new HighScoreModel
            {
                UserId = userId,
                SubjectCode = subjectCode,
                LevelCode = levelCode,
            };

            HighScoreDAO highScoreDAO = new HighScoreDAO();
            List<HighScoreModel> highScoreModelList = new List<HighScoreModel>();
            highScoreModelList = highScoreDAO.Read(_connectionStringRead, highScoreModelToFind, true, true);

            if (highScoreModelList.Count == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(highScoreModelList[0].Score);
            }
        }

        /// <summary>
        /// To insert the high score.
        /// </summary>
        /// <param name="highScoreModel">The high score details to insert.</param>
        /// <returns>Returns the transaction details.</returns>
        public Domain.Models.GameQuizCompetition.ResponseModel InsertUserHighScore(HighScoreModel highScoreModel)
        {
            var responseModel = new Domain.Models.GameQuizCompetition.ResponseModel
            {
                TransactionValid = true,
            };

            try
            {
                // First, we check if the highscore exists
                HighScoreModel highScoreModelCheckToExists = new HighScoreModel
                {
                    UserId = highScoreModel.UserId,
                    LevelCode = highScoreModel.LevelCode,
                    SubjectCode = highScoreModel.SubjectCode,
                };

                HighScoreDAO highScoreDAO = new HighScoreDAO();
                List<HighScoreModel> highScoreModelCheckResultList = new List<HighScoreModel>();
                highScoreModelCheckResultList = highScoreDAO.Read(_connectionStringRead, highScoreModelCheckToExists, true, true);

                HighScoreModel highScoreModelWhere = new HighScoreModel
                {
                    UserId = highScoreModel.UserId,
                    LevelCode = highScoreModel.LevelCode,
                    SubjectCode = highScoreModel.SubjectCode,
                };

                if (highScoreModelCheckResultList.Count > 0)
                {
                    highScoreDAO.Update(_connectionString, highScoreModel, highScoreModelWhere);
                }
                else
                {
                    highScoreDAO.Create(_connectionString, highScoreModel); // Insert
                }

                return responseModel;
            }
            catch (Exception)
            {
                responseModel.TransactionValid = false;
                return responseModel;
            }
        }
    }
}
