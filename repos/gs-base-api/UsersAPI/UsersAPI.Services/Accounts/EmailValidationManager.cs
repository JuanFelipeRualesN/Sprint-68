using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle validations of emails.
    /// </summary>
    public class EmailValidationManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Class to manage things related to email confirmation.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (write).</param>
        /// <param name="langCode">The language's code.</param>
        public EmailValidationManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// Verifies if email already exists or not. 
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>Returns true if exists, else false.</returns>
        public bool IsEmailAvailable(string email)
        {
            return (VerifyIfEmailExists(email) == null); 
        }

        /// <summary>
        /// To read the user email by email string as a param.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>Returns the user's email model.</returns>
        public UserEmailModel VerifyIfEmailExists(string email)
        {
            List<UserEmailModel> emailModelResultList = new UserEmailDAO().Read(this._connectionStringRead, new UserEmailModel(){ Email = email }, true, true);
            if (emailModelResultList.Count > 0)
                return emailModelResultList[0];
            else
                return null; 
        }

        /// <summary>
        /// To send the password reset.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>Returns the response about transaction details.</returns>
        public ResponseModel SendResetPasswordEmail(string email)
        {
            MessageManager messageManager = new MessageManager(this._connectionStringRead, this._langCode);
            ResponseModel responseModel = new ResponseModel(); 

            UserEmailModel emailModel = new UserEmailModel();
            emailModel = this.VerifyIfEmailExists(email);

            if(emailModel != null)
            {
                try {
                    // Get the email token type to retrieve the email type 
                    EmailTokenTypeModel emailTokenTypeModel = new EmailTokenTypeModel
                    {
                        Code = AppConfiguration.Configuration["AppSettings:token_type_password_reset_code"]
                    };
                    EmailTokenTypeDAO emailTokenTypeDAO = new EmailTokenTypeDAO();
                    emailTokenTypeModel = emailTokenTypeDAO.Read(this._connectionStringRead, emailTokenTypeModel, true, true)[0];

                    EmailValidationUserDAO emailValidationUserDAO = new EmailValidationUserDAO();

                    // Delete a previous token if exists 
                    EmailValidationUserModel emailValidationUserModelToDelete = new EmailValidationUserModel
                    {
                        Email = email,
                        EmailTypeId = emailTokenTypeModel.Id
                    };
                    emailValidationUserDAO.Delete(this._connectionString, emailValidationUserModelToDelete);

                    // If it is required, then we send the email 
                    string token = TokenManager.GenerateAlphanumericToken(32);

                    // Insert the token in the database for later validation 
                    EmailValidationUserModel emailValidationUserModel = new EmailValidationUserModel
                    {
                        UserId = emailModel.UserId,
                        Used = "0",
                        Token = token,
                        Email = email
                    };

                    // Get the email Type 
                    EmailValidationDetailModel emailValidationDetailModel = new EmailValidationDetailModel
                    {
                        EmailTokenTypeId = emailTokenTypeModel.Id,
                        LangCode = this._langCode
                    };
                    EmailValidationDetailDAO emailValidationEmailDAO = new EmailValidationDetailDAO();
                    emailValidationDetailModel = emailValidationEmailDAO.Read(this._connectionStringRead, emailValidationDetailModel, true, true)[0];
                    emailValidationUserModel.EmailTypeId = emailValidationDetailModel.EmailTokenTypeId;

                    // Insert the token 
                    emailValidationUserDAO.Create(this._connectionString, emailValidationUserModel);

                    // Send the email 
                    EmailSenderManager emailManager = new EmailSenderManager(this._connectionString, this._connectionStringRead, this._langCode);
                    Task.Factory.StartNew(() => emailManager.SendPasswordReset(emailValidationUserModel.Email, token));

                    responseModel.Valid = true;
                    responseModel.Details = new List<string>
                    {
                        messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_success_email_reset_sent"])
                    };
                }
                catch(Exception ex)
                {
                    responseModel.Valid = false;
                    responseModel.Details = new List<string>
                    {
                        ex.ToString()
                    };
                }
            }
            else
            {
                responseModel.Valid = false;
                responseModel.Details = new List<string>
                {
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_login_email_not_found"])
                };
            }

            return responseModel;
        }
    }
}
