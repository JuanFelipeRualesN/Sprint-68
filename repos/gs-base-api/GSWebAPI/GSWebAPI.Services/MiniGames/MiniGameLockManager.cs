using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;

namespace GSWebAPI.GSWebAPI.Services.MiniGames
{
    /// <summary>
    /// Handles the lock of the mini games.
    /// </summary>
    public class MiniGameLockManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniGameLockManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringRead">The connection string read.</param>
        /// <param name="langCode">The language code.</param>
        public MiniGameLockManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Locks or Unlocks the mini games, whatever is sent by the parent in the Front-end.
        /// </summary>
        /// <param name="userId">The id of the user (parent).</param>
        /// <param name="locked">If locked or unlocked.</param>
        public void LockMiniGames(string userId, string locked)
        {
            ParentLockedGameDAO parentLockedGameDAO = new ParentLockedGameDAO();

            // Check if the user has already a row
            List<ParentLockedGameModel> parentLockedGameModelsListToVerify = parentLockedGameDAO.Read(_connectionStringRead, new ParentLockedGameModel { UserId = userId }, true, true);

            if (parentLockedGameModelsListToVerify.Count == 0)
            {
                // Insert it because the parent doesn't have the locker registered yet.
                parentLockedGameDAO.Create(_connectionString, new ParentLockedGameModel { UserId = userId, Locked = locked });
            }
            else
            {
                // Update it
                parentLockedGameDAO.Update(_connectionString, new ParentLockedGameModel { Locked = locked }, new ParentLockedGameModel { UserId = userId });
            }
        }

        /// <summary>
        /// Verifies if the parent has locked the games.
        /// </summary>
        /// <param name="parentId">The id of the parent.</param>
        /// <returns>Returns true if locked.</returns>
        public bool AreMiniGamesLockedParent(string parentId)
        {
            return new ParentLockedGameDAO().Read(_connectionStringRead, new ParentLockedGameModel { UserId = parentId, Locked = "1" }, true, true).Count > 0;
        }

        /// <summary>
        /// Verifies if the mini games are locked or not (By students).
        /// </summary>
        /// <param name="token">The user token.</param>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Returns true if locked else, false.</returns>
        public bool AreMiniGamesLockedChild(string token, string ipAddress)
        {
            // Check the related account for the user, the user must be related to one account
            // First, we check the related parents/teachers
            var mainAccount = GetUserDetails.GetProfileMainAccountUserId(token, ipAddress);

            // Now we check if the account if there is a parent with unlocked game
            return new ParentLockedGameDAO().Read(_connectionStringRead, new List<ParentLockedGameModel>()
            {
                new ParentLockedGameModel()
                {
                    UserId = mainAccount.MainUserAccount,
                    Locked = "1",
                },
            }).Count > 0;
        }
    }
}