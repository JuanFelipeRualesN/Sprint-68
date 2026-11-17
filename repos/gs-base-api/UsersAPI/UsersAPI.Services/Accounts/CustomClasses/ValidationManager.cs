using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;


namespace UsersAPI.UsersAPI.Services.Accounts.CustomClasses
{
    /// <summary>
    /// To handle general validations for users' accounts.
    /// </summary>
    public class ValidationManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code.</param>
        public ValidationManager(string connectionStringRead, string langCode)
        {
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// Verifies if username is available.
        /// </summary>
        /// <returns>True if valid, else returns false.</returns>
        private bool IsUsernameAvailable(string username)
        {
            List<UserAccountModel> userAccountModelUsernameVerifiedList = new UserAccountDAO().Read(this._connectionStringRead, new UserAccountModel
            {
                Username = username
            }, true, false);
            if (userAccountModelUsernameVerifiedList.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Verifies if email is available.
        /// </summary>
        /// <returns>True if available, else false.</returns>
        private bool IsEmailAvailable(string email)
        {
            List<UserEmailModel> emailModelList = new UserEmailDAO().Read(this._connectionStringRead, new UserEmailModel
            {
                Email = email
            }, true, false);
            if (emailModelList.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <param name="passwordRetype">Password retyped.</param>
        /// <returns>Returns the response model with validation details.</returns>
        public ResponseModel ValidatePassword(string password, string passwordRetype)
        {
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };
            FieldValidator fieldValidator = new FieldValidator();

            if (fieldValidator.IsPasswordEmpty(password) || fieldValidator.IsPasswordEmpty(passwordRetype))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:ul_message_password_required"]);
            }
            else
            {

                // Verify if passwords match 
                if (!PasswordManager.IsPasswordAndRetypeEqual(password, passwordRetype))
                {
                    responseModel.Valid = false;
                    responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_password_do_not_match"]);
                }
                else // Validate password 
                {
                    // Check if password too short 
                    if (PasswordManager.IsPasswordTooShort(password))
                    {
                        responseModel.Valid = false;
                        responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_password_less_5_char"]);
                    }

                    if (AppConfiguration.Configuration["AppSettings:password_creation_strict"] != null && AppConfiguration.Configuration["AppSettings:password_creation_strict"] == "1")
                    {
                        // Check if password has an  upper, lower and a digit 
                        if (!PasswordManager.DoesPasswordContainsUpperLowerAndDigit(password))
                        {
                            responseModel.Valid = false;
                            responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_password_not_upper_lower_number"]);
                        }
                    }
                }
            }

            return responseModel;
        }

        /// <summary>
        /// Validates the username.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="validateUniqueness">true to validate if unique, false if not.</param>
        /// <returns>Returns the response model. </returns>
        public ResponseModel ValidateUsername(string username, bool validateUniqueness)
        {

            FieldValidator fieldValidator = new FieldValidator();
            ResponseModel response = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            #region usernameValidation
            if (fieldValidator.IsUsernameEmpty(username))
            {
                response.Valid = false;
                response.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:ul_message_username_required"]);
            }
            else
            {

                // Verifies if username is available
                if (validateUniqueness && !IsUsernameAvailable(username))
                {
                    response.Valid = false;
                    response.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_username_taken"]);
                }
                // Validate if User name correct 

                // Verify if username too short
                if (fieldValidator.IsUsernameTooShort(username))
                {
                    response.Valid = false;
                    response.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_username_short"]);
                }
                // Verify if username too long
                if (fieldValidator.IsUserNameTooLong(username))
                {
                    response.Valid = false;
                    response.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_username_long"]);
                }
                // Verifify if username has valid characters
                if (!fieldValidator.IsUsernameValidCharacters(username))
                {
                    response.Valid = false;
                    response.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_username_invalid_characters"]);
                }
                #endregion usernameValidation 
            }

            return response;
        }

        /// <summary>
        /// Validates the email on update.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="validateUniqueness">To valid if unique or not.</param>
        /// <returns>Returns the response model with validation details.</returns>
        public ResponseModel ValidateEmailOnUpdate(string email, string userId, bool validateUniqueness)
        {

            ResponseModel responseModel = new ResponseModel
            {
                Valid = true
            };

            FieldValidator fieldValidator = new FieldValidator();
            if (fieldValidator.IsEmailEmpty(email))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:ul_message_email_required"]);
                return responseModel;
            }
            else
            {

                UserEmailModel userEmailModel = new UserEmailModel
                {
                    Email = email,
                    UserId = userId
                };
                List<UserEmailModel> userEmailModelList = new List<UserEmailModel>();
                UserEmailDAO userEmailDao = new UserEmailDAO();
                userEmailModelList = userEmailDao.Read(this._connectionStringRead, userEmailModel, false, false);

                if (userEmailModelList.Count > 0) // Email is the same
                    return responseModel;
                else
                {
                    // Is not the same email, so validate it 
                    return this.ValidateEmail(email, validateUniqueness);
                }
            }
        }
       
        /// <summary>
        /// Validates the email (if valid, if unique, etc.).
        /// </summary>
        /// <param name="email">Email to validate.</param>
        /// <param name="userId">User Id (optional). If sent, the validation will be if the email correspondence to that user.</param>
        /// <param name="validateUnique"> If validation for uniqueness is required.</param>
        /// <returns>Returns a response model with validation details.</returns>
        public ResponseModel ValidateEmail(string email, bool validateUnique, string userId = null)
        {
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            FieldValidator fieldValidator = new FieldValidator();

            if (fieldValidator.IsEmailEmpty(email))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:ul_message_email_required"]);
            }
            else
            {
                if (!fieldValidator.IsValidEmail(email))
                {
                    responseModel.Valid = false;
                    responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_email_not_valid"]);
                }
                else
                {

                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelList = new UserEmailDAO().Read(this._connectionStringRead, new UserEmailModel()
                        {
                            Email = email,
                        }, true, true);

                        if (userEmailModelList.Count > 0)
                        {
                            if (userEmailModelList[0].UserId != userId)
                            {
                                responseModel.Valid = false;
                                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_email_taken"]);
                            }
                        }
                    }
                    else
                    {
                        // Verify if email available 
                        if (validateUnique && !IsEmailAvailable(email))
                        {
                            responseModel.Valid = false;
                            responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_email_taken"]);
                        }
                    }
                }
            }
            return responseModel;
        }
    }
}
