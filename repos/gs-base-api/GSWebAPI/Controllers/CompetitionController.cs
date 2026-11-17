using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using GSWebAPI.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;

using GSWebAPI.GSWebAPI.Services.Avatars;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.GameQuizCompetitions;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;
using GSWebAPI.GSWebAPI.Services.Generals;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle the competitions.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class CompetitionController : ControllerBase
    {
        /// <summary>
        /// To check if the user got a bonus. This is a bonus by connecting to the APP.
        /// </summary>
        /// <returns>Returns the HTTP response code with user bonus if any. </returns>
        [HttpGet]
        [Route("competition/bonus/check")]
        public async Task<IActionResult> CheckBonusCompetition()
        {
            /* End of general values to process the request */
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameQuizCompetitionCheckUserBonus");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var result = new HighScoreManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], responseSession.LangCode).CheckIfUserReceiveBonus(
                AppConfiguration.Configuration["AppSettings:gqz_reward_type_for_bonus"],
                new Random().Next(1, int.Parse(AppConfiguration.Configuration["AppSettings:gqz_reward_bonus_max"])).ToString(),
                responseSession.UserId);

            return Ok(result);
        }

        /// <summary>
        /// To submit the competitions' results. The competitions are not live. It is against the users' previous competition submitions results.
        /// </summary>
        /// <param name="generalRequestModel">Users' competitions' results to submit.</param>
        /// <returns>Returns the HTTP response with the submitted results' details.</returns>
        [HttpPost]
        [Route("competition/results/submit")]
        public async Task<IActionResult> SubmitCompetitionResults(CompetitionUserAnswersRequestModel generalRequestModel)
        {
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameQuizCompetitionCreateUserResponse");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Continue with the operation.
            CompetitionManager competitionManager = new CompetitionManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"], 
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], responseSession.LangCode);
            for (int i = 0; i < generalRequestModel.UserResponseModelList.Count; i++)
            {
                generalRequestModel.UserResponseModelList[i].UserId = responseSession.UserId;
            }

            competitionManager.InsertUserResponses(generalRequestModel.UserResponseModelList);

            AthenasJSONQuizResponseModel modelObject = AthenasDetailsHandler.GetQuizById(generalRequestModel.UserResponseModelList[0].QuizId, responseSession.Token);

            CompetitionResultsResponseModel generalResponseModel = new CompetitionResultsResponseModel();

            // Compare the responses
            // First, we call the Quiz to check the user answers.

            // Then, we compare them with the rival responses.
            // If both of them are correct, give it to the user with the lesser time

            // Finally, if the user won, insert some rewards.

            // Compares the user responses
            CompetitionResponseModel competitionResponseModel =
                    competitionManager.CompareUserResponses(modelObject.QuizRequestModel.QuestionRequestModelList, generalRequestModel.UserResponseModelList, generalRequestModel.RivalResponseModelList, responseSession.UserId);
            generalResponseModel.CompetitionResultsModel = new CompetitionResultsModel
            {
                Draw = competitionResponseModel.Draw,
                RivalPoints = competitionResponseModel.RivalTotalPoints,
                RivalWon = competitionResponseModel.RivalWon,
                UserPoints = competitionResponseModel.UserTotalPoints,
                UserWon = competitionResponseModel.UserWon,
            };

            // If the user won, then give some rewards
            if (competitionResponseModel.UserWon)
            {
                GameUserRewardsService gameUserRewardsManager = new GameUserRewardsService(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);
                generalResponseModel.UserRewardModel = gameUserRewardsManager.GenerateRandomRewardForUser(responseSession.UserId);
                if (!string.IsNullOrWhiteSpace(generalResponseModel.UserRewardModel.CoinsTotal))
                {
                    generalResponseModel.CompetitionResultsModel.RewardType = AppConfiguration.Configuration["AppSettings:gr_coins_code"];
                    generalResponseModel.CompetitionResultsModel.RewardQuantity = int.Parse(generalResponseModel.UserRewardModel.CoinsTotal);
                }
                else if (!string.IsNullOrWhiteSpace(generalResponseModel.UserRewardModel.DiamondsTotal))
                {
                    generalResponseModel.CompetitionResultsModel.RewardType = AppConfiguration.Configuration["AppSettings:gr_diamond_code"];
                    generalResponseModel.CompetitionResultsModel.RewardQuantity = int.Parse(generalResponseModel.UserRewardModel.DiamondsTotal);
                }
            }
            else
            {
                generalResponseModel.CompetitionResultsModel.RewardType = "0";
                generalResponseModel.CompetitionResultsModel.RewardQuantity = 0;
            }

            return Created(string.Empty, generalResponseModel);
        }

        /// <summary>
        /// To get the top students of each subject and level by competitions' scores.
        /// </summary>
        /// <param name="subjectCode">The subject's code. Example: MAT-SP, EN, SP, etc.</param>
        /// <param name="levelCode">The level's code. Example: K, 1, 2, 3, etc.</param>
        /// <returns>Returns a HTTP Response with a list of top scores.</returns>
        [HttpGet]
        [Route("competition/top/{subjectCode}/{levelCode}")]
        public async Task<IActionResult> GetTopStudentsInCompetitions(string subjectCode, string levelCode)
        {
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameQuizCompetitionGetTopScores");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(
                new HighScoreManager(
                        AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                        responseSession.LangCode).
                        GetTopHighScore(int.Parse(AppConfiguration.Configuration["AppSettings:gqz_top_to_find"]), levelCode, subjectCode));
        }

        /// <summary>
        /// To search a rival for competition.
        /// </summary>
        /// <param name="subjectCode">The subject's code. Example: MAT-SP, EN, SP, etc.</param>
        /// <param name="levelCode">The level's code. Example: K, 1, 2, 3, etc.</param>
        /// <returns>Returns a HTTP Response message with the rival details.</returns>
        [HttpGet]
        [Route("competition/rival/search/{subjectCode}/{levelCode}")]
        public async Task<IActionResult> SearchRival(string subjectCode, string levelCode)
        {

            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameQuizCompetitionSearchRival");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok( new CompetitionManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode).GenerateFullCompetitionRivalProcess(subjectCode, levelCode,
                    responseSession.UserId, responseSession.Token, responseSession.IpAddress, responseSession.LangCode));
        }
    }
}
