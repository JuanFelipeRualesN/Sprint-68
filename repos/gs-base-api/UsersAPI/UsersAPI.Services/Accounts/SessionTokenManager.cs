using System.Collections.Generic;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;


namespace UsersAPI.UsersAPI.Services.Accounts
{
     /// <summary>
     /// To handle details related to the access tokens.
     /// </summary>
    public class SessionTokenManager
    {
        private readonly string _connectionString, _connectionStringRead;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string to the DB</param>
        /// <param name="connectionStringRead">The connection string to read.</param>
        public SessionTokenManager(string connectionString, string connectionStringRead)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// Delete the session token relationated with the user (this log outs the user for the next operation) 
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public void DeleteAllSessionsOfTheUser(string userId)
        {
            SessionModel sessionModel = new SessionModel
            {
                UserId = userId
            };
            SessionDAO sessionDAO = new SessionDAO();
            List<SessionModel> sessionModelToBeDeleted = sessionDAO.Read(this._connectionStringRead, sessionModel, false, false);
            sessionDAO.Delete(this._connectionString, sessionModelToBeDeleted);
        }

    }
}
