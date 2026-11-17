using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;
using UsersAPI.UsersAPI.Services.PersonalDetails;
using UsersAPI.UsersAPI.Services.Roles;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle the user account modifications.
    /// </summary>
    public class UserModifyManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language's code.</param>
        public UserModifyManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

       /// <summary>
       /// To update an username.
       /// </summary>
       /// <param name="userId">The user's id.</param>
       /// <param name="username">The new username.</param>
       /// <param name="modifiedBy">The user's id of the user that is modifying the record.</param>
       /// <returns>Returns the transaction's details.</returns>
        public UsersAPI.Domain.Models.Accounts.ResponseModel UpdateUsername(string userId, string username, string modifiedBy)
        {
            // Validate username and password 
            var responseModel = new UsersAPI.Domain.Models.Accounts.ResponseModel();
            ValidationManager validationManager = new ValidationManager(this._connectionStringRead, this._langCode);
            responseModel = validationManager.ValidateUsername(username, true);
            if (!responseModel.Valid)
            {
                responseModel.Details = new List<string>();
                responseModel.Details.AddRange(new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(responseModel.ErrorCodes));
                return responseModel;
            }
            else
            {
                // Update the username 
                new UserAccountDAO().Update(this._connectionString, new UserAccountModel()
                {
                    Username = username,
                    ModifiedBy = modifiedBy
                }, new UserAccountModel()
                {
                    Id = userId
                });
                responseModel.Valid = true;
                responseModel.Details = new List<string>
                {
                    new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_username_updated_success"])
                };
                return responseModel;
            }

        }

        /// <summary>
        /// To update the user's email.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="email">The email of the user (the new email).</param>
        /// <param name="modifiedBy">The user's id of the user that is modifying the record.</param>
        /// <returns>Returns the transaction's details.</returns>
        public UsersAPI.Domain.Models.Accounts.ResponseModel UpdateEmailProcess(string userId, string email, string modifiedBy)
        {
            email = email.Trim();
            // First, we validate the email, including if the emails is available
            CustomClasses.ValidationManager validationManager = new CustomClasses.ValidationManager(this._connectionString, this._langCode);
            UsersAPI.Domain.Models.Accounts.ResponseModel responseModel = validationManager.ValidateEmail(email, true, userId);
            if (!responseModel.Valid)
            {
                responseModel.Details = new List<string>();
                responseModel.Details.AddRange(new MessageManager(this._connectionString, this._langCode).GetMessageDetails(responseModel.ErrorCodes));
                return responseModel;
            }
            else
            {
                // Update the email 
                UserEmailDAO userEmailDAO = new UserEmailDAO();
                List<UserEmailModel> userEmailExisting = userEmailDAO.Read(this._connectionStringRead, new UserEmailModel() { UserId = userId }, true, true);
                // If there are not emails for the user, just create it. 
                if (userEmailExisting.Count == 0)
                {
                    userEmailDAO.Create(this._connectionString, new UserEmailModel()
                    {
                        Email = email,
                        ModifiedBy = modifiedBy,
                        Validated = "0",
                        UserId = userId,
                    });
                }
                else
                {
                    userEmailDAO.Update(this._connectionString, new UserEmailModel()
                    {
                        Email = email,
                        ModifiedBy = modifiedBy,
                        Validated = "0"
                    }, new UserEmailModel()
                    {
                        UserId = userId
                    });
                }

                // Send success 
                responseModel.Valid = true;
                responseModel.Details = new List<string>
                {
                    new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_email_successfully_updated"])
                };
                return responseModel;
            }
        }

        /// <summary>
        /// To update the user account's status.
        /// </summary>
        /// <param name="userAccountStatus">The user's account status to update.</param>
        public void UserAccountStatusUpdate(UserAccountStatusModel userAccountStatus)
        {
            new UserAccountStatusDAO().Update(this._connectionString, new UserAccountStatusModel()
            {
                StatusId = userAccountStatus.StatusId,
                ModifiedBy = userAccountStatus.ModifiedBy
            }, new UserAccountStatusModel()
            {
                UserId = userAccountStatus.UserId
            });
        }

        /// <summary>
        /// To register a profile.
        /// </summary>
        /// <param name="username">The profile's username.</param>
        /// <param name="password">The profile's password.</param>
        /// <param name="passwordRetype">The profile's password retype.</param>
        /// <param name="ipAddress">The profile's IP Address.</param>
        /// <param name="role">The profile's role.</param>
        /// <param name="name">The profile's given name.</param>
        /// <param name="surname">The profile's surname.</param>
        /// <param name="dob">The profile's date of birth.</param>
        /// <param name="addressLine1">The profile's address.</param>
        /// <param name="addressLine2">The profile's address line 2.</param>
        /// <param name="city">The profile's city.</param>
        /// <param name="state">The profile's state.</param>
        /// <param name="country">The profile's country.</param>
        /// <param name="zipCode">The profile's zip code.</param>
        /// <param name="countryIso">The profile's country ISO.</param>
        /// <param name="gender">The profile's gender.</param>
        /// <param name="genderRequired">If gender is required.</param>
        /// <param name="dobRequired">If date of birth is required.</param>
        /// <param name="userIdCreatedBy">The user that is creating the row.</param>
        /// <param name="email">The profile's email.</param>
        /// <returns>Returs the transaction's details.</returns>
        public UsersAPI.Domain.Models.Accounts.ResponseModel RegisterSubProfile(string username, string password, string passwordRetype, string ipAddress, string role, string name, string surname, string dob, string addressLine1, string addressLine2, string city, string state, string country, string zipCode, string countryIso, string gender, bool genderRequired, bool dobRequired, string userIdCreatedBy, string email)
        {
            var response = new UsersAPI.Domain.Models.Accounts.ResponseModel
            {
                Details = new List<string>(),
                Valid = true
            };

            var userAddressManager = new AddressManager(this._connectionString, this._connectionStringRead, this._langCode);

            #region useraccount
            var errorCodeList = new List<string>();

            // Validate username and password 
            var validationManager = new CustomClasses.ValidationManager(this._connectionString, this._langCode);

            // Validate username and password 
            var responseModel = validationManager.ValidateUsername(username, true);
            if (!responseModel.Valid)
            {
                response.Valid = false;
                errorCodeList.AddRange(responseModel.ErrorCodes);
            }

            responseModel = validationManager.ValidatePassword(password, passwordRetype);
            if (!responseModel.Valid)
            {
                response.Valid = false;
                errorCodeList.AddRange(responseModel.ErrorCodes);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                responseModel = validationManager.ValidateEmail(email, true);
                if (!responseModel.Valid)
                {
                    response.Valid = false;
                    errorCodeList.AddRange(responseModel.ErrorCodes);
                }
            }

            var messageManager = new MessageManager(this._connectionString, this._langCode);
            if (errorCodeList.Count > 0)
            {
                // Search the error of user accounts 
                response.Details = messageManager.GetMessageDetails(errorCodeList);
            }
            #endregion useraccount

            #region userAddress
            // Validates the user address
            var responseAddressModel = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
            var userAddressModel = new UsersAPI.Domain.Models.PersonalDetails.UserAddressModel
            {
                City = city,
                Country = country,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                Zip = zipCode,
                State = state,
                PersonAddressName = name + " " + surname,
                CountryIsoCode2 = countryIso
            };



            responseAddressModel = userAddressManager.ValidateAddress(userAddressModel, false, false, false, false, false);
            if (!responseAddressModel.Valid)
            {
                response.Valid = false;
                response.Details.AddRange(responseAddressModel.Details);
            }
            #endregion userAddress

            #region role
            // Validate the Role 
            UsersAPI.Services.Roles.CustomClasses.ValidationManager validateUserRole = new UsersAPI.Services.Roles.CustomClasses.ValidationManager(this._connectionStringRead, this._langCode);
            UsersAPI.Domain.Models.Roles.ResponseModel responseRoleModel = new UsersAPI.Domain.Models.Roles.ResponseModel();
            responseRoleModel = validateUserRole.ValidateRole(role);

            if (!responseRoleModel.Valid)
            {
                response.Valid = false;
                MessageManager messageManagerRole = new MessageManager(this._connectionStringRead, this._langCode);
                response.Details.AddRange(messageManagerRole.GetMessageDetails(responseRoleModel.ErrorCodes));
            }
            #endregion role

            #region personaldetails
            // Validate name 
            var responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateName(name);

            var errorPersonalDetailsList = new List<string>();
            if (!responseModelPersonalDetails.Valid)
            {
                response.Valid = false;
                errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
            }
            responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateSurname(surname);
            if (!responseModelPersonalDetails.Valid)
            {
                response.Valid = false;
                errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
            }

            if (dobRequired || !String.IsNullOrWhiteSpace(dob))
            {
                // Validate dob 
                responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateDOB(dob);
                if (!responseModelPersonalDetails.Valid)
                {
                    response.Valid = false;
                    errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
                }
            }

            if (genderRequired || !String.IsNullOrWhiteSpace(gender))
            {
                // Validate Gender 
                responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateGender(gender);
                if (!responseModelPersonalDetails.Valid)
                {
                    response.Valid = false;
                    errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
                }
            }

            if (errorPersonalDetailsList.Count > 0)
            {
                // Search the error message manager 
                var messageManagerPersonalDetails = new MessageManager(this._connectionString, this._langCode);
                response.Details.AddRange(messageManagerPersonalDetails.GetMessageDetails(errorPersonalDetailsList));
            }
            #endregion userdetails

            // If invalid, return the response
            if (!response.Valid)
                return response;
            else
            {
                // Do the insertions 
                var userAccountModel = this.RegisterUserAccount(username, password, userIdCreatedBy);
                this.SetLoginAttemtps(userAccountModel.Id);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    this.RegisterEmail(email, userAccountModel.Id, userIdCreatedBy);
                }
                var statusAccountModel = this.GetStatusRegistration();
                this.SetStatusUserAccount(userAccountModel.Id, statusAccountModel.Id, userIdCreatedBy);
                this.GetUserRegistrationMessage(statusAccountModel.Id);
                this.RegisterIpAddress(ipAddress, userAccountModel.Id);
                userAddressModel.UserId = userAccountModel.Id;
                userAddressModel.Enable = "1";
                userAddressModel.MainAddress = "1";
                if (string.IsNullOrWhiteSpace(userAddressModel.City))
                {
                    userAddressModel.City = " ";
                }
                userAddressManager.InsertUserAddress(userAddressModel);

                // Insert Role 
                this.RegisterRole(role, userAccountModel.Id);

                // Insert Personal Details (if gender is not null, insert it. If dob is not null, send it 
                this.RegisterUserDetails(userAccountModel.Id, name, surname, null, String.IsNullOrWhiteSpace(dob) ? null : dob, String.IsNullOrWhiteSpace(gender) ? null : gender, userIdCreatedBy);

                // Send success message 
                response.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_success_registration"]));
                response.UserId = userAccountModel.Id;
            }

            return response;
        }

        /// <summary>
        /// To register the user without the full validation. Should be validated already here.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="role">The user's role.</param>
        /// <param name="name">The user's name (first name).</param>
        /// <param name="surname">The user's surname (last name).</param>
        /// <param name="phone">The user's phone.</param>
        /// <param name="dob">The user's date of birth.</param>
        /// <param name="addressLine1">The user's address line 1.</param>
        /// <param name="addressLine2">The user's address line 2.</param>
        /// <param name="city">The user's city.</param>
        /// <param name="state">The user's state.</param>
        /// <param name="country">The user's country.</param>
        /// <param name="zipCode">The user's zip code.</param>
        /// <param name="countryIso">The user's country ISO. Example: PRI.</param>
        /// <param name="gender">The user's gender.</param>
        /// <param name="stateIsoCode">The user's state ISO code.</param>
        /// <param name="userIdCreatedBy">The user's id of the user that is creating the user.</param>
        /// <param name="isAddressBasicValidation">IP address validation.</param>
        /// <returns>Returns the response transaction details.</returns>
        public UsersAPI.Domain.Models.Accounts.ResponseModel RegisterUserFullProccessWithoutValidation(string username, string password, string ipAddress, string email, string role, string name, string surname, string phone, string dob, string addressLine1, string addressLine2, string city, string state, string country, string zipCode, string countryIso, string gender, string stateIsoCode, string userIdCreatedBy, bool isAddressBasicValidation)
        {
            UsersAPI.Domain.Models.Accounts.ResponseModel response = new UsersAPI.Domain.Models.Accounts.ResponseModel
            {
                Details = new List<string>(),
                Valid = true
            };

            // Do the insertions 
            UserAccountModel userAccountModel = this.RegisterUserAccount(username, password, userIdCreatedBy);
            this.SetLoginAttemtps(userAccountModel.Id);
            StatusAccountModel statusAccountModel = this.GetStatusRegistration();
            this.SetStatusUserAccount(userAccountModel.Id, statusAccountModel.Id, userIdCreatedBy);
            this.RegisterIpAddress(ipAddress, userAccountModel.Id);
            UserAddressModel userAddressModel = new UserAddressModel
            {
                City = city,
                Country = country,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                Zip = zipCode,
                State = state,
                PersonAddressName = name + " " + surname,
                StateIsoCode = stateIsoCode,
                CountryIsoCode2 = countryIso,
                UserId = userAccountModel.Id,
                Enable = "1",
                MainAddress = "1",
            };
            AddressManager userAddressManager = new AddressManager(this._connectionString, this._connectionStringRead, this._langCode);
            userAddressManager.InsertUserAddress(userAddressModel);
            this.RegisterEmail(email, userAccountModel.Id, userIdCreatedBy);

            // Insert Role 
            this.RegisterRole(role, userAccountModel.Id);

            // Insert Personal Details 
            this.RegisterUserDetails(userAccountModel.Id, name, surname, phone, dob, gender, userIdCreatedBy);

            // Send success message 
            response.Details.Add(new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_success_registration"]));
            response.UserId = userAccountModel.Id;
            return response;
        }

        /// <summary>
        /// To register the user with full process (including validations).
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="passwordRetype">The user's password retype.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="role">The user's role.</param>
        /// <param name="name">The user's name (first name).</param>
        /// <param name="surname">The user's surname (last name).</param>
        /// <param name="phone">The user's phone.</param>
        /// <param name="dob">The user's date of birth.</param>
        /// <param name="addressLine1">The user's address line 1.</param>
        /// <param name="addressLine2">The user's address line 2.</param>
        /// <param name="city">The user's city.</param>
        /// <param name="state">The user's state.</param>
        /// <param name="country">The user's country.</param>
        /// <param name="zipCode">The user's zip code.</param>
        /// <param name="countryIso">The user's country ISO. Example: PRI.</param>
        /// <param name="gender">The user's gender.</param>
        /// <param name="stateIsoCode">The user's state ISO code.</param>
        /// <param name="userIdCreatedBy">The user id of the user that is creating the new user.</param>
        /// <param name="dobRequired">If date of birth is required.</param>
        /// <param name="genderRequired">If gender is required.</param>
        /// <returns>Returns the transaction details.</returns>
        public UsersAPI.Domain.Models.Accounts.ResponseModel RegisterUserFullProccess(string username, string password, string passwordRetype, string ipAddress, string email, string role, string name, string surname, string phone, string dob, string addressLine1, string addressLine2, string city, string state, string country, string zipCode, string countryIso, string gender, bool dobRequired, bool genderRequired, string stateIsoCode, string userIdCreatedBy)
        {
            UsersAPI.Domain.Models.Accounts.ResponseModel response = new UsersAPI.Domain.Models.Accounts.ResponseModel
            {
                Details = new List<string>(),
                Valid = true
            };

            #region useraccount
            List<string> errorCodeList = new List<string>();

            // Validate username and password 
            CustomClasses.ValidationManager validationManager = new CustomClasses.ValidationManager(this._connectionStringRead, this._langCode);

            // Validate username and password 
            UsersAPI.Domain.Models.Accounts.ResponseModel responseModel = new UsersAPI.Domain.Models.Accounts.ResponseModel();
            responseModel = validationManager.ValidateUsername(username, true);
            if (!responseModel.Valid)
            {
                response.Valid = false;
                errorCodeList.AddRange(responseModel.ErrorCodes);
            }

            responseModel = new UsersAPI.Domain.Models.Accounts.ResponseModel();
            responseModel = validationManager.ValidatePassword(password, passwordRetype);
            if (!responseModel.Valid)
            {
                response.Valid = false;
                errorCodeList.AddRange(responseModel.ErrorCodes);
            }

            // Validate emails 
            responseModel = new UsersAPI.Domain.Models.Accounts.ResponseModel();
            responseModel = validationManager.ValidateEmail(email, true);
            if (!responseModel.Valid)
            {
                response.Valid = false;
                errorCodeList.AddRange(responseModel.ErrorCodes);
            }

            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            if (errorCodeList.Count > 0)
            {
                // Search the error of user accounts 
                response.Details = messageManager.GetMessageDetails(errorCodeList);
            }

            #endregion useraccount

            #region userAddress
            // Validates the user address

            UsersAPI.Domain.Models.PersonalDetails.ResponseModel responseAddressModel = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
            AddressManager addressManager = new AddressManager(this._connectionString, this._connectionStringRead, this._langCode);
            UserAddressModel userAddressModel = new UsersAPI.Domain.Models.PersonalDetails.UserAddressModel
            {
                City = city,
                Country = country,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                Zip = zipCode,
                State = state,
                PersonAddressName = name + " " + surname,
                StateIsoCode = stateIsoCode,
                CountryIsoCode2 = countryIso
            };
            AddressManager userAddressManager = new AddressManager(this._connectionString, this._connectionStringRead, this._langCode);
            responseAddressModel = userAddressManager.ValidateAddress(userAddressModel, false, 
                true, true, true, true);
            if (!responseAddressModel.Valid)
            {
                response.Valid = false;
                response.Details.AddRange(responseAddressModel.Details);
            }
            #endregion userAddress

            #region role
            // Validate the Role 
            UsersAPI.Services.Roles.CustomClasses.ValidationManager validateUserRole = new UsersAPI.Services.Roles.CustomClasses.ValidationManager(this._connectionStringRead, this._langCode);
            UsersAPI.Domain.Models.Roles.ResponseModel responseRoleModel = new UsersAPI.Domain.Models.Roles.ResponseModel();
            responseRoleModel = validateUserRole.ValidateRole(role);

            if (responseRoleModel.Valid == false)
            {
                response.Valid = false;
                MessageManager messageManagerRole = new MessageManager(this._connectionStringRead, this._langCode);
                response.Details.AddRange(messageManagerRole.GetMessageDetails(responseRoleModel.ErrorCodes));
            }
            #endregion role

            #region personaldetails
            // Validate name 
            
            UsersAPI.Domain.Models.PersonalDetails.ResponseModel responseModelPersonalDetails = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
            responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateName(name);

            List<string> errorPersonalDetailsList = new List<string>();
            if (responseModelPersonalDetails.Valid == false)
            {
                response.Valid = false;
                errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
            }
            responseModelPersonalDetails = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
            responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateSurname(surname);
            if (responseModelPersonalDetails.Valid == false)
            {
                response.Valid = false;
                errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
            }

            // If dob is required, then validate it
            if (dobRequired)
            {
                responseModelPersonalDetails = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
                responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateDOB(dob);
                if (!responseModelPersonalDetails.Valid)
                {
                    response.Valid = false;
                    errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
                }

                if (errorPersonalDetailsList.Count > 0)
                {
                    // Search the error message manager 
                    MessageManager messageManagerPersonalDetails = new MessageManager(this._connectionString, this._langCode);
                    response.Details.AddRange(messageManagerPersonalDetails.GetMessageDetails(errorPersonalDetailsList));
                }
            }

            // If gender is sent, then validate it
            if (genderRequired)
            {
                responseModelPersonalDetails = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
                responseModelPersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateGender(gender);
                if (!responseModelPersonalDetails.Valid)
                {
                    response.Valid = false;
                    errorPersonalDetailsList.AddRange(responseModelPersonalDetails.ErrorCodes);
                }

                if (errorPersonalDetailsList.Count > 0)
                {
                    // Search the error message manager 
                    MessageManager messageManagerPersonalDetails = new MessageManager(this._connectionString, this._langCode);
                    response.Details.AddRange(messageManagerPersonalDetails.GetMessageDetails(errorPersonalDetailsList));
                }
            }


            #endregion userdetails

            if (response.Valid)
            {

                response = RegisterUserFullProccessWithoutValidation(username, password, ipAddress, email, role, name, surname, phone, dob, addressLine1, addressLine2, city, state, country, zipCode, countryIso, gender, stateIsoCode, userIdCreatedBy, false);

                /** Delete the following if tests valid 
                // Do the insertions 
                UserAccountModel userAccountModel = this.RegisterUserAccount(username, password, userIdCreatedBy);
                this.SetLoginAttemtps(userAccountModel.Id);
                StatusAccountModel statusAccountModel = this.GetStatusRegistration();
                this.SetStatusUserAccount(userAccountModel.Id, statusAccountModel.Id, userIdCreatedBy);
                this.RegisterIpAddress(ipAddress, userAccountModel.Id);
                userAddressModel.UserId = userAccountModel.Id;
                userAddressModel.Enable = "1";
                userAddressModel.MainAddress = "1";
                userAddressManager.InsertUserAddress(userAddressModel, false);
                this.RegisterEmail(email, userAccountModel.Id, userIdCreatedBy);

                // Insert Role 
                this.RegisterRole(role, userAccountModel.Id);

                // Insert Personal Details 
                this.RegisterUserDetails(userAccountModel.Id, name, surname, phone, dob, gender, userIdCreatedBy);

                // Send success message 
                response.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_success_registration"]));
                response.UserId = userAccountModel.Id;
                this.SendEmailRegistrarion(email, userAccountModel);  **/
            }

            return response;
        }

        /// <summary>
        /// Updates a list of user account wuthout any validation. The validation must be made already. Use it with caution. 
        /// Recommended only for admin area. 
        /// THE PASSWORD MUST BE ALREADY HASHED.
        /// </summary>
        /// <param name="userAccountModelListToUpdate">The user account to update.</param>
        /// <param name="userAccountModelListToUpdateWhere">The user account where list (with the required ids).</param>
        public void UpdateUserAccountListWithoutValidation(List<UserAccountModel> userAccountModelListToUpdate, List<UserAccountModel> userAccountModelListToUpdateWhere)
        {
            new UserAccountDAO().Update(this._connectionString, userAccountModelListToUpdate, userAccountModelListToUpdateWhere);
        }

        /// <summary>
        /// Creates a username list without validation.
        /// </summary>
        /// <param name="userEmailModelList">User Email to insert.</param>
        public void InsertEmailModelWithoutValidation(List<UserEmailModel> userEmailModelList)
        {
            new UserEmailDAO().Create(this._connectionString, userEmailModelList);
        }

        /// <summary>
        /// Updates a list of user emails. The emails should be validated already. Use it with caution. 
        /// Recommended only for admin area.
        /// </summary>
        /// <param name="userEmailModelsToUpdate">User email list to update.</param>
        /// <param name="userEmailModelsToUpdateWhere">User email list where (with the required ids).</param>
        public void UpdateUserEmailListWithoutValidation(List<UserEmailModel> userEmailModelsToUpdate, List<UserEmailModel> userEmailModelsToUpdateWhere)
        {
            new UserEmailDAO().Update(this._connectionString, userEmailModelsToUpdate, userEmailModelsToUpdateWhere);
        }

        /// <summary>
        /// To create the user account basics.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="userIdCreatedBy">The user that is creating the record.</param>
        /// <returns>Returns the user account details that should include the user's id.</returns>
        private UserAccountModel RegisterUserAccount(string username, string password, string userIdCreatedBy)
        {
            // Generate Token 
            UserTokenManager userTokenManager = new UserTokenManager();

            // Registers the user and assign an id
            return new UserAccountDAO().Create(this._connectionString, new UserAccountModel()
            {
                Username = username,
                Password = PasswordManager.HashedPassword(password),
                ModifiedBy = userIdCreatedBy,
                Token = userTokenManager.GenerateTokenManager(),
            })[0];
        }

        /// <summary>
        /// To register the user's email.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="userAccountModelId">The user's id.</param>
        /// <param name="userIdCreatedBy">The user's id of the user that is creating the row.</param>
        private void RegisterEmail(string email, string userAccountModelId, string userIdCreatedBy)
        {
            // Now we assign the email 
            new UserEmailDAO().Create(this._connectionString, new UserEmailModel
            {
                Email = email,
                UserId = userAccountModelId,
                Validated = "0", // Email is not valdated yet. This is optional. You should see more in the login  
                ModifiedBy = userIdCreatedBy
            });
        }

        /// <summary>
        /// To set the login attempts for the account.
        /// </summary>
        /// <param name="userAccoundId">The user's id.</param>
        private void SetLoginAttemtps(string userAccoundId)
        {
            UserLoginAttemptDAO UserLoginAttemptDAO = new UserLoginAttemptDAO();
            // Check if exists 
            UserLoginAttemptModel userLoginAttempsModelCheckIfExists = new UserLoginAttemptModel
            {
                UserId = userAccoundId
            };

            List<UserLoginAttemptModel> userLoginAttempsMOdelList = new List<UserLoginAttemptModel>();
            userLoginAttempsMOdelList = UserLoginAttemptDAO.Read(this._connectionStringRead, userLoginAttempsModelCheckIfExists, true, false);

            if (userLoginAttempsMOdelList.Count > 0) // update
            {
                UserLoginAttemptModel userLoginAttempsModelToUpdate = new UserLoginAttemptModel
                {
                    Attempt = "0"
                };

                UserLoginAttemptModel userLoginAttempsModelWhere = new UserLoginAttemptModel
                {
                    UserId = userAccoundId
                };

                UserLoginAttemptDAO.Update(this._connectionString, userLoginAttempsModelToUpdate, userLoginAttempsModelWhere);
            }
            else
            {
                UserLoginAttemptModel UserLoginAttemptModel = new UserLoginAttemptModel
                {
                    UserId = userAccoundId,
                    Attempt = "0",
                    LastLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                UserLoginAttemptDAO.Create(this._connectionString, UserLoginAttemptModel);
            }
        }

        /// <summary>
        /// Returns the status registration id when an account is created.
        /// </summary>
        /// <returns>Returns the status id.</returns>
        private StatusAccountModel GetStatusRegistration()
        {
            StatusAccountModel statusAccountModel = new StatusAccountModel
            {
                Code = AppConfiguration.Configuration["AppSettings:ul_status_account_initially_created"]
            };
            StatusAccountDAO statusAccountDAO = new StatusAccountDAO();
            return statusAccountDAO.Read(this._connectionStringRead, statusAccountModel, true, true)[0];
        }

        /// <summary>
        /// Sets the status of the user account.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="statusId">The status' id.</param>
        /// <param name="userIdCreatedBy">The user that is modifying the row.</param>
        private void SetStatusUserAccount(string userId, string statusId, string userIdCreatedBy)
        {
            new UserAccountStatusDAO().Create(this._connectionString, new UserAccountStatusModel()
            {
                UserId = userId,
                StatusId = statusId,
                ModifiedBy = userIdCreatedBy
            });
        }

        /// <summary>
        /// Returns the user registration message.
        /// </summary>
        /// <param name="statusId">The status id of the registration.</param>
        /// <returns>Returns a response with the registration message.</returns>
        private UsersAPI.Domain.Models.Accounts.ResponseModel GetUserRegistrationMessage(string statusId)
        {
            UsersAPI.Domain.Models.Accounts.ResponseModel response = new UsersAPI.Domain.Models.Accounts.ResponseModel
            {
                Details = new List<string>()
            };

            List<StatusAccountLangModel> statusLangsDetails = new StatusAccountLangDAO().Read(this._connectionStringRead, new StatusAccountLangModel
            {
                StatusId = statusId,
                LangCode = this._langCode
            }, true, true);


            response.Details.Add(statusLangsDetails != null && statusLangsDetails.Count > 0 ? statusLangsDetails[0].Desc : "");

            return response;
        }

        /// <summary>
        /// Registers the IP address of the user. 
        /// </summary>
        /// <param name="ipAddress">The user's IP.</param>
        /// <param name="userAccountId">The user's id.</param>
        private void RegisterIpAddress(string ipAddress, string userAccountId)
        {
            IpAddressUserModel ipAddressUserModel = new IpAddressUserModel
            {
                UserId = userAccountId,
                IpAddress = ipAddress
            };

            IpAddressUserDAO ipAddressUserDAO = new IpAddressUserDAO();
            ipAddressUserDAO.Create(this._connectionString, ipAddressUserModel);
        }

        /// <summary>
        /// To register the user's role.
        /// </summary>
        /// <param name="roleId">The role's id.</param>
        /// <param name="userAccountId">The user's id.</param>
        private void RegisterRole(string roleId, string userAccountId)
        {
            UserRolesManager userRolesManager = new UserRolesManager(this._connectionString, this._connectionStringRead, this._langCode);
            userRolesManager.RegisterUserRole(userAccountId, roleId);
        }

        /// <summary>
        /// To register the user's details.
        /// </summary>
        /// <param name="userAccountId">The user's id.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastname">The last name of the user.</param>
        /// <param name="phone">The phone of the user.</param>
        /// <param name="dob">The date of birth of the user.</param>
        /// <param name="gender">The gender of the user.</param>
        /// <param name="userIdCreatedBy">The user's id that is creating the row.</param>
        private void RegisterUserDetails(string userAccountId, string name, string lastname, string phone, string dob, string gender, string userIdCreatedBy)
        {
            UserPersonalDetailsManager userPersonalDetailsManager = new UserPersonalDetailsManager(this._connectionString, this._connectionStringRead, this._langCode);
            userPersonalDetailsManager.InsertUserFullNameDetails(name, lastname, "", userAccountId);
            if (!String.IsNullOrWhiteSpace(phone))
            {
                userPersonalDetailsManager.InsertUserPhone(new UserPhoneNumberModel()
                {
                    UserId = userAccountId,
                    PhoneNumber = phone,
                    ModifiedBy = userIdCreatedBy
                });
            }
            if (!String.IsNullOrWhiteSpace(dob))
            {
                userPersonalDetailsManager.InsertUserDobProcess(new UserDobModel()
                {
                    UserId = userAccountId,
                    Dob = dob
                });
            }
            if (!String.IsNullOrWhiteSpace(gender))
            {
                userPersonalDetailsManager.InsertUserGenderProccess(new UserGenderModel()
                {
                    UserId = userAccountId,
                    Gender = gender
                });
            }
        }
    }
}
