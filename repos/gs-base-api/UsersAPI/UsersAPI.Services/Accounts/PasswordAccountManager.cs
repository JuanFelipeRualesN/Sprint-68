using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;
using UsersAPI.UsersAPI.Services.Generals;


namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle details related to the password.
    /// </summary>
    public class PasswordAccountManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language.</param>
        public PasswordAccountManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// To retrieve that the email that was sent to the user for password reset is valid.
        /// </summary>
        /// <param name="emailValidationUserModel">The email user validation to call.</param>
        /// <returns>Returns a list of emails validations.</returns>
        public List<EmailValidationUserModel> GetEmailValidationByEmailValidation(EmailValidationUserModel emailValidationUserModel)
        {
            EmailValidationUserDAO emailValidationUserDAO = new EmailValidationUserDAO();
            List<EmailValidationUserModel> emailValidationUserModelList = emailValidationUserDAO.Read(this._connectionStringRead, new EmailValidationUserModel()
            {
                Token = emailValidationUserModel.Token
            }, true, true);

            return emailValidationUserModelList;
        }

        /// <summary>
        /// Updates the password by recovery token.
        /// </summary>
        /// <param name="recoveryToken">The recovery token that was sent in the email.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newPasswordRetype">The new password retyped.</param>
        /// <returns>Returns the response model with the transaction details.</returns>
        public ResponseModel UpdatePasswordByRecoveryToken(string recoveryToken, string newPassword, string newPasswordRetype)
        {
            ResponseModel response = new ResponseModel
            {
                Valid = true
            };
            // Error list with the error descriptions 
            List<string> messageList = new List<string>();
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);

            // Validate the passwords
            ValidationManager validationManager = new ValidationManager(this._connectionString, this._langCode);
            ResponseModel responseValidation = validationManager.ValidatePassword(newPassword, newPasswordRetype);
            if (!responseValidation.Valid)
            {
                response.Valid = false;
                response.Details = messageManager.GetMessageDetails(responseValidation.ErrorCodes);
                return response;
            }

            // Continue the operation
            // Get the user Id 
            EmailValidationUserModel emailValidationUserModel = new EmailValidationUserModel
            {
                Token = recoveryToken,
                Used = "0"
            };
            EmailValidationUserDAO emailValidationUserDAO = new EmailValidationUserDAO();
            List<EmailValidationUserModel> emailValidationUserModelList = new List<EmailValidationUserModel>();
            emailValidationUserModelList = emailValidationUserDAO.Read(this._connectionStringRead, emailValidationUserModel, true, true);

            if (emailValidationUserModelList.Count > 0)
            {
                // 1) We check if the user old password is valid 

                // a. Get Password DB
                UserAccountModel userAccountModel = new UserAccountModel
                {
                    Id = emailValidationUserModelList[0].UserId
                };

                UserAccountDAO userAccountDAOToCheckPassword = new UserAccountDAO();
                List<UserAccountModel> userAccountModelListToCheckPassword = new List<UserAccountModel>();

                userAccountModelListToCheckPassword = userAccountDAOToCheckPassword.Read(this._connectionStringRead, userAccountModel, true, true);

                if (userAccountModelListToCheckPassword.Count > 0)
                {

                    // Now we update the password 
                    UserAccountModel userAccountModelToUpdate = new UserAccountModel
                    {
                        Password = PasswordManager.HashedPassword(newPassword)
                    };

                    UserAccountModel userAccountModelWhere = new UserAccountModel
                    {
                        Id = userAccountModel.Id
                    };

                    UserAccountDAO userAccountDaoToUpdate = new UserAccountDAO();
                    List<UserAccountModel> userAccountModelList = userAccountDaoToUpdate.Update(this._connectionString, userAccountModelToUpdate, userAccountModelWhere);

                    // Change the status to valid 

                    // Reset the login attempts
                    UserLoginAttemptModel userLoginAttempsMOdel = new UserLoginAttemptModel
                    {
                        Attempt = "0"
                    };
                    UserLoginAttemptModel userLoginAttempsModelWhere = new UserLoginAttemptModel
                    {
                        UserId = userAccountModel.Id
                    };
                    UserLoginAttemptDAO UserLoginAttemptDAO = new UserLoginAttemptDAO();
                    UserLoginAttemptDAO.Update(this._connectionString, userLoginAttempsMOdel, userLoginAttempsModelWhere);

                    // If user was bloacked by max attempts, reset it 
                    UserAccountStatusModel userAccountStatusModelToCheck = new UserAccountStatusModel
                    {
                        StatusId = AppConfiguration.Configuration["AppSettings:user_account_status_max_login_attempts"],
                        UserId = userAccountModel.Id
                    };
                    UserAccountStatusDAO userAccountStatusDAO = new UserAccountStatusDAO();
                    List<UserAccountStatusModel> userAccountStatusResult = new List<UserAccountStatusModel>();
                    userAccountStatusResult = userAccountStatusDAO.Read(this._connectionStringRead, userAccountStatusModelToCheck, false, false);

                    if (userAccountStatusResult.Count > 0)
                    {
                        // Change the Status to valid 
                        UserAccountStatusModel userAccountStatusModel = new UserAccountStatusModel
                        {
                            StatusId = AppConfiguration.Configuration["AppSettings:user_account_status_valid"]
                        };
                        UserAccountStatusModel userAccountStatusModelWhere = new UserAccountStatusModel
                        {
                            UserId = userAccountModel.Id
                        };
                        userAccountStatusDAO.Update(this._connectionString, userAccountStatusModel, userAccountStatusModelWhere);
                    }

                    if (userAccountModelList != null)
                    {
                        // Mark token as used 
                        EmailValidationUserModel emailValidationUserModelToUpdate = new EmailValidationUserModel
                        {
                            Used = "1"
                        };

                        EmailValidationUserModel emailValidationUserModelWhere = new EmailValidationUserModel
                        {
                            Id = emailValidationUserModelList[0].Id
                        };
                        emailValidationUserDAO.Update(this._connectionString, emailValidationUserModelToUpdate, emailValidationUserModelWhere);

                        // OK, the transaction was completed
                        messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_success_transaction"]));
                        response.Details = messageList;
                        return response;
                    }
                    else
                    {
                        // An error occurred. Please, try again 
                        messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_error_ocurred_during_transaction"]));
                        response.Details = messageList;
                        return response;
                    }

                }
                else
                {
                    response.Valid = false;
                    messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_id_not_valid"]));
                    response.Details = messageList;
                    return response;
                }
            }
            else
            {
                response.Valid = false;
                messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_token_not_exists"]));
                response.Details = messageList;
                return response;
            }

        }

        /// <summary>
        /// Updates the password without entry the old/current password.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newPasswordRetype">The new password retyped.</param>
        /// <returns>Returns the response model with the transaction details.</returns>
        public ResponseModel UpdatePasswordWithoutEntryOld(string userId, string newPassword, string newPasswordRetype, string updatedBy = null)
        {
            ResponseModel response = new ResponseModel
            {
                Valid = true
            };
            // Error list with the error descriptions 
            List<string> messageList = new List<string>();
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);

            // Validate the passwords
            ValidationManager validationManager = new ValidationManager(this._connectionString, this._langCode);
            ResponseModel responseValidation = validationManager.ValidatePassword(newPassword, newPasswordRetype);
            if (!responseValidation.Valid)
            {
                response.Valid = false;
                response.Details = messageManager.GetMessageDetails(responseValidation.ErrorCodes);
                return response;
            }

            UserAccountModel userAccountModelToUpdate = new UserAccountModel
            {
                Password = PasswordManager.HashedPassword(newPassword),
                ModifiedBy = !string.IsNullOrWhiteSpace(updatedBy) ? updatedBy : userId
            };

            UserAccountModel userAccountModelWhere = new UserAccountModel
            {
                Id = userId
            };

            UserAccountDAO userAccountDaoToUpdate = new UserAccountDAO();
            List<UserAccountModel> userAccountModelList = userAccountDaoToUpdate.Update(this._connectionString, userAccountModelToUpdate, userAccountModelWhere);

            if (userAccountModelList.Count > 0)
            {
                // OK, the transaction was completed
                messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_success_transaction"]));
                response.Details = messageList;
                // Reset user account 
                ResetAccountStatusAndLoginAttempts(userId, !string.IsNullOrWhiteSpace(updatedBy) ? updatedBy : userId);

                return response;
            }
            else
            {
                // An error occurred. Please, try again 
                messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_error_ocurred_during_transaction"]));
                response.Details = messageList;
                return response;
            }

        }

        /// <summary>
        /// To update a password.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newPasswordRetype">The password retyped.</param>
        /// <returns>Returns the transaction details with the updated password.</returns>
        public ResponseModel UpdatePassword(string userId, string oldPassword, string newPassword, string newPasswordRetype)
        {
            ResponseModel response = new ResponseModel
            {
                Valid = true
            };
            // Error list with the error descriptions 
            List<string> messageList = new List<string>();
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);

            // Validate the passwords
            ValidationManager validationManager = new ValidationManager(this._connectionString, this._langCode);
            ResponseModel responseValidation = validationManager.ValidatePassword(newPassword, newPasswordRetype);
            if (!responseValidation.Valid)
            {
                response.Valid = false;
                response.Details = messageManager.GetMessageDetails(responseValidation.ErrorCodes);
                return response;
            }

            // Continue with the operation 
            // 1) We check if the user old password is valid 
            // a. Get Password DB
            UserAccountModel userAccountModel = new UserAccountModel
            {
                Id = userId
            };

            UserAccountDAO userAccountDAOToCheckPassword = new UserAccountDAO();
            List<UserAccountModel> userAccountModelListToCheckPassword = new List<UserAccountModel>();
            userAccountModelListToCheckPassword = userAccountDAOToCheckPassword.Read(this._connectionStringRead, userAccountModel, true, true);

            if (userAccountModelListToCheckPassword.Count > 0)
            {
                // 2) Now, we check if the password is valid comparing the one in the DB vs. the old one entered 
                if (PasswordManager.PasswordMatch(userAccountModelListToCheckPassword[0].Password, oldPassword))
                {
                    // Now we update the password 
                    UserAccountModel userAccountModelToUpdate = new UserAccountModel
                    {
                        Password = PasswordManager.HashedPassword(newPassword)
                    };

                    UserAccountModel userAccountModelWhere = new UserAccountModel
                    {
                        Id = userId
                    };

                    UserAccountDAO userAccountDaoToUpdate = new UserAccountDAO();
                    List<UserAccountModel> userAccountModelList = userAccountDaoToUpdate.Update(this._connectionString, userAccountModelToUpdate, userAccountModelWhere);

                    if (userAccountModelList != null)
                    {
                        // OK, the transaction was completed
                        messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_success_transaction"]));
                        response.Details = messageList;

                        // Reset user account 
                        ResetAccountStatusAndLoginAttempts(userId, userId);

                        return response;
                    }
                    else
                    {
                        // An error occurred. Please, try again 
                        messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_error_ocurred_during_transaction"]));
                        response.Details = messageList;
                        return response;
                    }
                }
                else
                {
                    // Send error password invalid 
                    response.Valid = false;
                    messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_invalid_password"]));
                    response.Details = messageList;
                    return response;
                }
            }
            else
            {
                response.Valid = false;
                messageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_id_not_valid"]));
                response.Details = messageList;
                return response;
            }
        }

        /// <summary>
        /// To reset the account status and the login attempts.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="modifiedBy">The user who modified the status.</param>
        private void ResetAccountStatusAndLoginAttempts(string userId, string modifiedBy)
        {
            // Reset user account 
            UserModifyManager userModifyManager = new UserModifyManager(this._connectionString, this._connectionStringRead, this._langCode);
            userModifyManager.UserAccountStatusUpdate(new UserAccountStatusModel()
            {
                StatusId = AppConfiguration.Configuration["AppSettings:user_account_status_valid"],
                UserId = userId,
                ModifiedBy = modifiedBy
            });

            UserLoginAttemptModel userLoginAttempsMOdel = new UserLoginAttemptModel
            {
                Attempt = "0"
            };
            UserLoginAttemptModel userLoginAttempsModelWhere = new UserLoginAttemptModel
            {
                UserId = userId
            };
            UserLoginAttemptDAO UserLoginAttemptDAO = new UserLoginAttemptDAO();
            UserLoginAttemptDAO.Update(this._connectionString, userLoginAttempsMOdel, userLoginAttempsModelWhere);
        }

    }
}
