using System;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;


namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle details related to login process.
    /// </summary>
    public class LoginManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

       /// <summary>
       /// Constructor.
       /// </summary>
       /// <param name="connectionString">The connection string (write).</param>
       /// <param name="connectionStringRead">The connection string (read).</param>
       /// <param name="langCode">The user's language code.</param>
        public LoginManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode; 
        }

        /// <summary>
        /// To block an account.
        /// </summary>
        /// <param name="userAccountModel">The user's account.</param>
        public void BlockAccount(UserAccountModel userAccountModel)
        {
            UserAccountStatusModel userAccountStatusModelSet = new UserAccountStatusModel();

            // Get the status ID 
            StatusAccountModel statusAccountModelMaxAttemtps = new StatusAccountModel
            {
                Code = AppConfiguration.Configuration["AppSettings:account_max_login_attempts_reached"]
            };
            StatusAccountDAO statusAccountDAO = new StatusAccountDAO();
            statusAccountModelMaxAttemtps = statusAccountDAO.Read(this._connectionStringRead, statusAccountModelMaxAttemtps, true, true)[0];
            userAccountStatusModelSet.StatusId = statusAccountModelMaxAttemtps.Id;

            UserAccountStatusModel userAccountStatusWhere = new UserAccountStatusModel
            {
                UserId = userAccountModel.Id
            };

            UserAccountStatusDAO userAccountStatusDAO = new UserAccountStatusDAO();
            userAccountStatusDAO.Update(this._connectionString, userAccountStatusModelSet, userAccountStatusWhere);
        }

        /// <summary>
        /// Sets the login attempts.
        /// </summary>
        /// <param name="userAccountModel">The user's account.</param>
        /// <param name="quantitySet">How many attempts to set.</param>
        public void SetLoginAttempts(UserAccountModel userAccountModel, int quantitySet)
        {
            UserLoginAttemptModel userLogingAttemptsModel = new UserLoginAttemptModel
            {
                Attempt = quantitySet.ToString()
            };

            if (quantitySet == 0)
                userLogingAttemptsModel.LastLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            else
            {
                // Get the last successful login date
                UserLoginAttemptModel userLoginAttemptLastLoginModel = new UserLoginAttemptModel
                {
                    UserId = userAccountModel.Id
                };
                UserLoginAttemptDAO userLoginAttemptLastLoginDAO = new UserLoginAttemptDAO();
                userLoginAttemptLastLoginModel = userLoginAttemptLastLoginDAO.Read(this._connectionStringRead, userLoginAttemptLastLoginModel, true, true)[0];
                userLogingAttemptsModel.LastLogin = DateTime.Parse(userLoginAttemptLastLoginModel.LastLogin).ToString("yyyy-MM-dd HH:mm:ss"); 
            }

            UserLoginAttemptModel userLoginAttemptsWhereModel = new UserLoginAttemptModel
            {
                UserId = userAccountModel.Id
            };
            UserLoginAttemptDAO userLoginAttempsDAO = new UserLoginAttemptDAO();
            userLoginAttempsDAO.Update(this._connectionString, userLogingAttemptsModel, userLoginAttemptsWhereModel);
        }
    }
}
