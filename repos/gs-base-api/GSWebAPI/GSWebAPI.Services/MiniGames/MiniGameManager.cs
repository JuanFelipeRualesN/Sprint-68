using System.Collections.Generic;
using System.Configuration;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Domain.Models.MiniGame;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.MiniGames;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;
using GSWebAPI.CustomClasses;

namespace GSWebAPI.GSWebAPI.Services.MiniGames
{
    /// <summary>
    /// Manages the mini games (get all the mini games, get all the mini games evaluating if the user has unlock them, the transaction of insert a new mini game unlocked, etc...) 
    /// </summary>
    public class MiniGameManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniGameManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code.</param>
        public MiniGameManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get all mini games.
        /// </summary>
        /// <returns>Returns mini game details.</returns>
        public List<GameResponseModel> GetAllMiniGames()
        {
            GameModel gameModelToFind = new GameModel();
            GameDAO gameDAO = new GameDAO();
            GameLangDAO gameLangDAO = new GameLangDAO();

            // Retrieve the mini games
            List<GameModel> gameModelResultList = new List<GameModel>();
            gameModelResultList = gameDAO.Read(_connectionStringRead, gameModelToFind, false, false);

            // Now retrieve the lang details.
            List<GameLangModel> gameLangModelToFindList = new List<GameLangModel>();
            for (int i = 0; i < gameModelResultList.Count; i++)
            {
                GameLangModel gameLangModel = new GameLangModel
                {
                    GameCode = gameModelResultList[i].Code,
                    LangCode = _langCode,
                };
                gameLangModelToFindList.Add(gameLangModel);
            }

            List<GameLangModel> gameLangModelResultList = new List<GameLangModel>();
            gameLangModelResultList = gameLangDAO.Read(_connectionStringRead, gameLangModelToFindList);

            // Now prepare the GameResponseModelList 
            List<GameResponseModel> gameResponseList = new List<GameResponseModel>();

            for (int i = 0; i < gameLangModelResultList.Count; i++)
            {
                GameModel gameModelToValues = gameModelResultList.Find(x => x.Code == gameLangModelResultList[i].GameCode);

                GameResponseModel gameResponseModel = new GameResponseModel
                {
                    Code = gameLangModelResultList[i].GameCode,
                    Description = gameLangModelResultList[i].Desc,
                    Title = gameLangModelResultList[i].Title,
                    UnlockedType = gameModelToValues.UnlockedType,
                    UnlockedValue = gameModelToValues.UnlockedValue,
                    PlayValue = gameModelToValues.PlayValue.ToString(),
                };
                gameResponseList.Add(gameResponseModel);
            }

            return gameResponseList;
        }

        /// <summary>
        /// To get all the mini games unlocked by the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns the game response model.</returns>
        public List<GameResponseModel> GetAllMiniGamesWithUnlockedMiniGamesByUser(string userId)
        {

            // Get the games unlocked by the user 
            List<UserUnlockedGameModel> userUnlockedGameModelList = new List<UserUnlockedGameModel>();
            UserUnlockedGameModel userUnlockedGameModelToFind = new UserUnlockedGameModel
            {
                UserId = userId,
            };
            UserUnlockedGameDAO userUnlockedGameDAO = new UserUnlockedGameDAO();
            userUnlockedGameModelList = userUnlockedGameDAO.Read(_connectionStringRead, userUnlockedGameModelToFind, false, false);

            // Now we get the games details.
            GameModel gameModelToFind = new GameModel();
            GameDAO gameDAO = new GameDAO();
            GameLangDAO gameLangDAO = new GameLangDAO();

            // Retrieve the mini games.
            List<GameModel> gameModelResultList = new List<GameModel>();
            gameModelResultList = gameDAO.Read(_connectionStringRead, gameModelToFind, false, false);

            // Now retrieve the lang details.
            List<GameLangModel> gameLangModelToFindList = new List<GameLangModel>();
            for (int i = 0; i < gameModelResultList.Count; i++)
            {
                GameLangModel gameLangModel = new GameLangModel
                {
                    GameCode = gameModelResultList[i].Code,
                    LangCode = _langCode,
                };
                gameLangModelToFindList.Add(gameLangModel);
            }

            List<GameLangModel> gameLangModelResultList = new List<GameLangModel>();
            gameLangModelResultList = gameLangDAO.Read(_connectionStringRead, gameLangModelToFindList);

            // Now prepare the GameResponseModelList.
            List<GameResponseModel> gameResponseList = new List<GameResponseModel>();

            for (int i = 0; i < gameLangModelResultList.Count; i++)
            {
                GameModel gameModelToValues = gameModelResultList.Find(x => x.Code == gameLangModelResultList[i].GameCode);

                UserUnlockedGameModel checkIfUnlockedGameModel = null;
                if (userUnlockedGameModelList.Count > 0)
                {
                    checkIfUnlockedGameModel = userUnlockedGameModelList.Find(x => x.GameCode == gameLangModelResultList[i].GameCode); // Check if the game was unlocked
                }

                GameResponseModel gameResponseModel = new GameResponseModel
                {
                    Code = gameLangModelResultList[i].GameCode,
                    Description = gameLangModelResultList[i].Desc,
                    Title = gameLangModelResultList[i].Title,
                    UnlockedType = gameModelToValues.UnlockedType,
                    UnlockedValue = gameModelToValues.UnlockedValue,
                    PlayValue = gameModelToValues.PlayValue,
                };

                if (checkIfUnlockedGameModel != null)
                {
                    gameResponseModel.UnlockedByTheUser = true;
                }

                gameResponseList.Add(gameResponseModel);
            }

            return gameResponseList;

        }

        /// <summary>
        /// To insert an unlocked mini game.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="gameCodeToInsert">The game code to insert.</param>
        /// <returns>Returns the response transaction.</returns>
        public Domain.Models.MiniGame.ResponseModel InsertUnlockedMiniGame(string userId, string gameCodeToInsert)
        {
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
            Domain.Models.MiniGame.ResponseModel responseModel = new Domain.Models.MiniGame.ResponseModel
            {
                TransactionValid = true,
            };

            // Check if the user doesn't have the item already.
            // Get the games unlocked by the user.
            List<UserUnlockedGameModel> userUnlockedGameModelList = new List<UserUnlockedGameModel>();
            UserUnlockedGameModel userUnlockedGameModelToFind = new UserUnlockedGameModel
            {
                UserId = userId,
                GameCode = gameCodeToInsert,
            };
            UserUnlockedGameDAO userUnlockedGameDAO = new UserUnlockedGameDAO();
            userUnlockedGameModelList = userUnlockedGameDAO.Read(_connectionStringRead, userUnlockedGameModelToFind, true, true);
            if (userUnlockedGameModelList.Count > 0)
            {
                responseModel.TransactionValid = false;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_already_exists_in_inventory"]));
                return responseModel;
            }

            // Now we check if the has any funds. If not, we just terminate the operation.
            GameUserRewardsService gameUserRewardsManager = new GameUserRewardsService(_connectionString, _connectionStringRead, _langCode);
            UserRewardModel userRewardModel = new UserRewardModel();
            userRewardModel = gameUserRewardsManager.GetUserRewards(userId);
            if (userRewardModel == null)
            {
                responseModel.TransactionValid = false;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_not_enough_funds"]));
                return responseModel;
            }

            // Now we get the details of the game code to be unlocked 
            List<GameModel> gameModelToUnlockList = new List<GameModel>();
            GameModel gameModelToCheck = new GameModel
            {
                Code = gameCodeToInsert,
            };
            GameDAO gameDAO = new GameDAO();
            gameModelToUnlockList = gameDAO.Read(_connectionStringRead, gameModelToCheck, true, true);

            // if doesn't exists, just cut the operation.
            if (gameModelToUnlockList.Count == 0)
            {
                responseModel.TransactionValid = false;
                responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_mini_game_doesnt_exist"]));
                return responseModel;
            }

            // Now we check if the it is by diamonds and if the user has the enought diamonds to unlock it
            if (gameModelToUnlockList[0].UnlockedType == AppConfiguration.Configuration["AppSettings:mg_diamonds_code"])
            {
                if (int.Parse(userRewardModel.DiamondsTotal) < int.Parse(gameModelToUnlockList[0].UnlockedValue))
                {
                    responseModel.TransactionValid = false;
                    responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_not_enough_funds"]));
                    return responseModel;
                }
                else
                {
                    // Unlock the item 
                    UserUnlockedGameModel userUnlockedGame = new UserUnlockedGameModel
                    {
                        GameCode = gameCodeToInsert,
                        UserId = userId,
                    };
                    userUnlockedGameDAO.Create(_connectionString, userUnlockedGame);

                    // Remove the diamonds 
                    gameUserRewardsManager.RemoveDiamonds(userId, int.Parse(gameModelToUnlockList[0].UnlockedValue));
                    responseModel.TransactionValid = true;
                    responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_mini_game_successful_transaction"]));
                    return responseModel;
                }
            }
            else
            {
                if (int.Parse(userRewardModel.CoinsTotal) < int.Parse(gameModelToUnlockList[0].UnlockedValue))
                {
                    responseModel.TransactionValid = false;
                    responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_not_enough_funds"]));
                    return responseModel;
                }
                else
                {
                    // Unlock the item 
                    UserUnlockedGameModel userUnlockedGame = new UserUnlockedGameModel
                    {
                        GameCode = gameCodeToInsert,
                        UserId = userId,
                    };
                    userUnlockedGameDAO.Create(_connectionString, userUnlockedGame);

                    // Remove the coins
                    gameUserRewardsManager.RemoveCoins(userId, int.Parse(gameModelToUnlockList[0].UnlockedValue));
                    responseModel.TransactionValid = true;
                    responseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:mg_message_code_item_mini_game_successful_transaction"]));
                    return responseModel;
                }
            }
        }
    }
}