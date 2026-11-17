using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GSWebAPI.CustomClasses;

using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.Avatars;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;

using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle the students' avatars.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class AvatarController : ControllerBase
    {
        /// <summary>
        /// To get the avatar parts. You can filter by gender, part, unlock type, etc. Just send the property with the desired filter.
        /// </summary>
        /// <param name="avatarItemModel">The avatar item model with the parts to filter.</param>
        /// <returns>Returns an HTTP response with the avatar parts array.</returns>
        [HttpPost]
        [Route("avatar/parts")]
        public async Task<IActionResult> GetAvatarParts(AvatarItemModel avatarItemModel)
        {
            try
            {
                if (avatarItemModel == null)
                {
                    avatarItemModel = new AvatarItemModel();
                }
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarPartsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var result = new AvatarManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext)).GetAllAvatarItems(avatarItemModel, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To buy an avatar item.
        /// </summary>
        /// <param name="itemId">The avatar's item's id to buy.</param>
        /// <param name="part">The avatar's part to buy.</param>
        /// <returns>Returns the HTTP Response Message with transaction's message.</returns>
        [HttpPost]
        [Route("avatar/parts/buy/{itemId}/{part}")]
        public async Task<IActionResult> BuyParts(string itemId, string part)
        {

            /* End of general values to process the request */
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarUserBuyPart");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            UserUnlockedItemModel responseModel = new AvatarManager(
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                       responseSession.LangCode
                       ).InsertUserUnlockedPart(itemId, part, responseSession.UserId);

            if (!responseModel.Valid)
            {
                return BadRequest(new GeneralResponseModel() { Messages = responseModel.MessageList });
            }

            return Created(string.Empty, new GeneralResponseModel() { Messages = responseModel.MessageList });
        }

        /// <summary>
        /// To create the avatar for the user.
        /// </summary>
        /// <param name="userAvatarModel">User avatar to create.</param>
        /// <returns>Returns the HTTP Response Message with transaction's message.</returns>
        [HttpPost]
        [Route("avatar/user/create")]
        public async Task<IActionResult> AvatarUserCreate(UserAvatarModel userAvatarModel)
        {
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarUserCreate");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            UserAvatarModel responseModel = new AvatarManager(
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                       responseSession.LangCode
                       ).InsertUserAvatar(userAvatarModel, responseSession.UserId);

            if (!responseModel.Valid)
            {
                return BadRequest(new GeneralResponseModel() { Messages = responseModel.MessageList });
            }

            return Created(string.Empty, new GeneralResponseModel() { Messages = responseModel.MessageList });
        }

        /// <summary>
        /// To get the user's avatar (user evaluated with the access token).
        /// </summary>
        /// <returns>Returns the user's avatar model.</returns>
        [HttpGet]
        [Route("avatar/user")]
        public async Task<IActionResult> GetUserAvatar()
        {

            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarUserRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(
                new AvatarManager(
                       AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                       responseSession.LangCode
                      ).GetUserAvatar(responseSession.UserId));
        }

        /// <summary>
        /// To get the user's unlocked parts.
        /// </summary>
        /// <returns>Returns an array with the parts that are unlocked by the user.</returns>
        [HttpGet]
        [Route("avatar/user/parts")]
        public async Task<IActionResult> GetUserAvatarParts()
        {
            /* Token validation in Users API */
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarUserUnlockedItems");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(
                             new AvatarManager(
                               AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                               AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                               responseSession.LangCode
                               ).GetUserUnlockedItems(responseSession.UserId));
        }

        /// <summary>
        /// To get the user's unlocked parts.
        /// </summary>
        /// <param name="gender">The selected gender to buy the part.</param>
        /// <param name="part">The part to buy (if Chest, Hair, etc.).</param>
        /// <param name="partIndex">The index of the part.</param>
        /// <returns>Returns an array with the parts that are unlocked by the user.</returns>
        [HttpPost]
        [Route("avatar/v2/parts/buy/{gender}/{part}/{partIndex}/")]
        public async Task<IActionResult> BuyAvatarPart3D(string gender, string part, int partIndex)
        {
            /* Token validation in Users API */
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarUserBuyPart");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var responseBuy = new AvatarV2Manager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    RequestHandler.GetLanguage(HttpContext))
                .BuyItemPart(responseSession.UserId, gender, part, partIndex);

            return Ok(responseBuy);
        }

        /// <summary>
        /// To get the user's avatars parts with an evaluation if unlocked or not.
        /// </summary>
        /// <param name="gender">The gender to get the parts and evaluate if unlocked or not.</param>
        /// <returns>Returns the user's avatars parts as expected for Unity 3D.</returns>
        [HttpGet]
        [Route("avatar/v2/parts/user")]
        public async Task<IActionResult> GetAvatarUserPartsItems(string gender)
        {
            /* Token validation in Users API */
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarPartsRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(
                new AvatarV2Manager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    RequestHandler.GetLanguage(HttpContext)).GetUserAvatarLockedAndUnlockedParts(responseSession.UserId, gender));
        }

        /// <summary>
        /// To get the user avatar.
        /// </summary>
        /// <returns>Returns the user avatar: Example: {"Gender":"M","Glasses":false,"Skin":0,"Parts":[30,27,22,11,8,5,2]}</returns>
        [HttpGet]
        [Route("avatar/v2/user")]
        public async Task<IActionResult> GetUserAvatar3D()
        {
            /* Token validation in Users API */
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarPartsRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(
                new AvatarV2Manager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    RequestHandler.GetLanguage(HttpContext)).GetUser3dAvatar(responseSession.UserId));
        }

        /// <summary>
        /// To create or update the user avatar.
        /// </summary>
        /// <returns>Returns a generic response model (if valid or not).</returns>
        [HttpPost]
        [Route("avatar/v2/user")]
        public async Task<IActionResult> PostUserAvatar3D(AvatarV2JsonModel avatarV2JsonModel)
        {
            // {"Gender":"M","Glasses":false,"Skin":0,"Parts":[30,27,22,11,8,5,2]}
            /* Token validation in Users API */
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AvatarPartsRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(
                new AvatarV2Manager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    RequestHandler.GetLanguage(HttpContext)).CreateOrUpdateUserV2AvatarModel(responseSession.UserId,
                    avatarV2JsonModel
                    ));
        }
    }
}