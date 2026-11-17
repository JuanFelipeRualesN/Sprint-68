using Microsoft.AspNetCore.Mvc;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle rewards.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class RewardsController : ControllerBase
    {

        /// <summary>
        /// To remove rewards when the student uses them for avatars, mini-games, etc.
        /// </summary>
        /// <param name="rewardsCodeUseRequestModel">Rewards details to remove.</param>
        /// <returns>Returns an HTTP response with current rewards if 200, else a message list with transaction's details.</returns>
        [HttpPost]
        [Route("rewards/remove")]
        public async Task<IActionResult> RewardsRemove(RewardsCodeUseRequestModel rewardsCodeUseRequestModel)
        {
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameRewardsRemoveUserRewards");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            GameUserRewardsService gameUserRewardsManager = new GameUserRewardsService(
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                responseSession.LangCode);

            // Remove coins
            var responseRewardsModel = new GSWebAPI.Domain.Models.GameUserReward.ResponseModel();

            // Check if we need to remove coins
            if (rewardsCodeUseRequestModel.CoinsQuantity > 0)
            {
                responseRewardsModel = gameUserRewardsManager.RemoveCoins(responseSession.UserId, rewardsCodeUseRequestModel.CoinsQuantity);
            }

            if (rewardsCodeUseRequestModel.DiamondsQuantity > 0)
            {
                responseRewardsModel = gameUserRewardsManager.RemoveDiamonds(responseSession.UserId, rewardsCodeUseRequestModel.DiamondsQuantity);
            }

            if (!responseRewardsModel.TransactionValid)
            {
                return BadRequest(new GeneralResponseModel()
                {
                    Messages = responseRewardsModel.MessageList,
                });
            }

            return Ok(gameUserRewardsManager.GetUserRewards(responseSession.UserId));
        }

        /// <summary>
        /// To get the user's rewards (user evaluated with the access token).
        /// </summary>
        /// <returns>Returns an HTTP Response Message with user's rewards.</returns>
        [HttpGet]
        [Route("rewards/user/rewards")]
        public async Task<IActionResult> GetUserRewards()
        {

            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameRewardsUserRewards");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(new GameUserRewardsService(
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                responseSession.LangCode).GetUserRewards(responseSession.UserId));
        }

        /// <summary>
        /// To redeem a reward or insert new rewards.
        /// </summary>
        /// <param name="rewardsRedeemRequestModel">Rewards' details to redeem.</param>
        /// <returns>Returns an HTTP Response Message with rewards' details or transaction details.</returns>
        [HttpPost]
        [Route("rewards/redeem")]
        public async Task<IActionResult> RedeemRewards(RewardsRedeemRequestModel rewardsRedeemRequestModel)
        {

            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GameRewardsUserUpdateRewards");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            GameUserRewardsService gameUserRewardsManager = new GameUserRewardsService(
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], responseSession.LangCode);


            // Just insert by default
            var responseModelRewardInAppPurchase = gameUserRewardsManager.InsertUserReward(responseSession.UserId, rewardsRedeemRequestModel.CoinsQuantity.ToString(), rewardsRedeemRequestModel.DiamondsQuantity.ToString());

            if (!responseModelRewardInAppPurchase.TransactionValid)
            {
                return BadRequest(new GeneralResponseModel()
                {
                    Messages = responseModelRewardInAppPurchase.MessageList,
                });
            }

            return Ok(responseModelRewardInAppPurchase.UserRewardModel);
        }
    }
}
