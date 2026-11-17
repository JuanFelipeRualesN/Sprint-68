using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.MiniGame;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.MiniGames;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle mini-games' details. The mini-games are games built with Unity, for Unity's app, where the student can play some educational and non-educational games. (For entertainment).
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class MiniGameController : ControllerBase
    {
        /// <summary>
        /// Toggles mini game between lock or unlock.
        /// </summary>
        /// <param name="lockGame">true to lock the games, false to unlock them.</param>
        /// <returns>Returns a response with mini-games lock status.</returns>
        [HttpPost]
        [Route("mini-games/lock/modify/{lockGame}")]
        public async Task<IActionResult> ToggleLockMiniGame(bool lockGame)
        {
            try
            {
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "MiniGameLockCreate");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                /* End of Token Validation in Users API */

                // Now lock the game
                MiniGameLockManager miniGameLockManager = new MiniGameLockManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);
                miniGameLockManager.LockMiniGames(responseSession.UserId, lockGame ? "1" : "0");

                return Ok(lockGame);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Reads if mini games are locked or not.
        /// </summary>
        /// <param name="fromParentApp">If the call is from admin/parent/teacher's app,
        /// then this should be true. If it is from the student's app, this should be false.</param>
        /// <returns>true if locked, false if not.</returns>
        [HttpGet]
        [Route("mini-games/lock/status/{fromParentApp}")]
        public async Task<IActionResult> ReadLock(bool fromParentApp)
        {
            try
            {
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "MiniGamesBlockReader");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                /* End of Token Validation in Users API */

                MiniGameLockManager miniGameLockManager = new MiniGameLockManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode);
                
                // Verify if the account parent
                if (fromParentApp)
                {
                    // Verified for the parent
                    return Ok(miniGameLockManager.AreMiniGamesLockedParent(responseSession.UserId));
                }
                else
                {
                    return Ok(miniGameLockManager.AreMiniGamesLockedChild(responseSession.Token, responseSession.IpAddress));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To read the mini-games information.
        /// </summary>
        /// <returns>Returns a list of mini-games on the HTTP Response.</returns>
        [HttpGet]
        [Route("mini-games/games")]
        public async Task<IActionResult> ReadMiniGames()
        {
            try
            {
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "MiniGameAllGamesRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                /* End of Token Validation in Users API */
                return Ok(new MiniGameManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode).GetAllMiniGames());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To read the mini-games unlocked by the user.
        /// </summary>
        /// <returns>Returns a list of mini-games on the HTTP Response.</returns>
        [HttpGet]
        [Route("mini-games/games/unlocked")]
        public async Task<IActionResult> ReadMiniGamesUnlockedByUser()
        {
            try
            {
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "MiniGameUnlockedGamesRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                /* End of Token Validation in Users API */

                return Ok(new MiniGameManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], responseSession.LangCode).GetAllMiniGamesWithUnlockedMiniGamesByUser(responseSession.UserId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To unlock a mini-game. (When the user uses sapphire or coins to buy them).
        /// </summary>
        /// <param name="miniGameCode">The mini-game code to unlock.</param>
        /// <returns>Returns an HTTP Response with transaction details.</returns>
        [HttpPost]
        [Route("mini-games/games/unlock/{miniGameCode}")]
        public async Task<IActionResult> UnlockMiniGame(string miniGameCode)
        {
            try
            {
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "MiniGameUnlockGame");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                /* End of Token Validation in Users API */
                var responseInsertion = new MiniGameManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    responseSession.LangCode).InsertUnlockedMiniGame(responseSession.UserId, miniGameCode);
                if (!responseInsertion.TransactionValid)
                {
                    return BadRequest(new GeneralResponseModel() { Messages = responseInsertion.MessageList });
                }
                else
                {
                    return Created(string.Empty, new GeneralResponseModel() { Messages = responseInsertion.MessageList });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
