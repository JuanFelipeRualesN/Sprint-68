using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle the users' related details (read).
    /// </summary>
    public class UserDetailsManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

       /// <summary>
       /// Constructor.
       /// </summary>
       /// <param name="connectionStringRead">The connection string (reader).</param>
       /// <param name="langCode">The user's language code.</param>
        public UserDetailsManager(string connectionStringRead, string langCode)
        {
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// Return the statuses list.
        /// </summary>
        /// <param name="statusModels">The statuses' model list to filter.</param>
        /// <returns>Returns a list of StatusAccountModel.</returns>
        public List<StatusAccountModel> GetStatus(List<StatusAccountModel> statusModels)
        {
            return new StatusAccountDAO().Read(this._connectionStringRead, statusModels); 
        }

        /// <summary>
        /// Returns the user account details list according to the user account model list with the filters.
        /// </summary>
        /// <param name="userAccountDetails">The user account details that works like a where query.</param>
        /// <returns>Returns a list of user accound models.</returns>
        public List<UserAccountModel> GetUserAccountDetails(List<UserAccountModel> userAccountDetails)
        {
            return new UserAccountDAO().Read(this._connectionStringRead, userAccountDetails); 
        }

        /// <summary>
        /// To get the user id by the user's email.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>Returns the transaction with the user's id.</returns>
        public UsersAPI.Domain.Models.Accounts.ResponseModel GetUserIdByEmail(string email)
        {
            UsersAPI.Domain.Models.Accounts.ResponseModel responseModel = new UsersAPI.Domain.Models.Accounts.ResponseModel();

            List<UserEmailModel> userEmailModelList = new UserEmailDAO().Read(this._connectionStringRead, new UserEmailModel
            {
                Email = email
            }, true, true); 

            if (userEmailModelList.Count == 0)
            {
                responseModel.Valid = false;
                responseModel.Details = new List<string>();
                MessageManager messageManager = new MessageManager(this._connectionStringRead, this._langCode);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_email_doesnt_exists"]));
                return responseModel;
            }
            else
            {
                responseModel.UserId = userEmailModelList[0].UserId;
                responseModel.Valid = true;
                return responseModel;
            }
        }

        /// <summary>
        /// To get the user's email.
        /// </summary>
        /// <param name="userId">The user id to get the email.</param>
        /// <returns>Returns the email string.</returns>
        public string GetUserEmail(string userId)
        {
            List<UserEmailModel> userEmailModelRead = new UserEmailDAO().Read(this._connectionStringRead, new UserEmailModel
            {
                UserId = userId
            }, true, true);

            if (userEmailModelRead.Count > 0)
                return userEmailModelRead[0].Email;
            else
                return null; 
        }

        /// <summary>
        /// Returns the user emails by the user emails model list to filter.
        /// </summary>
        /// <param name="userEmailsToFind">The user email model list to filter.</param>
        /// <returns>Returns a list of user email models.</returns>
        public List<UserEmailModel> GetUsersEmailByUserEmailList(List<UserEmailModel> userEmailsToFind)
        {
            return new UserEmailDAO().Read(this._connectionStringRead, userEmailsToFind); 
        }
    }
}
