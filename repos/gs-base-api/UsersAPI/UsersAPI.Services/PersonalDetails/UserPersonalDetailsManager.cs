using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.PersonalDetails
{
    /// <summary>
    /// To handle the user's personal details.
    /// </summary>
    public class UserPersonalDetailsManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's languages code.</param>
        public UserPersonalDetailsManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        #region UserDOB

        /// <summary>
        /// The process to insert the user's date of birth.
        /// </summary>
        /// <param name="userDobModelToInsert">The user's date of birth model. Must have the user's id and the dob.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel InsertUserDobProcess(UserDobModel userDobModelToInsert)
        {
            ResponseModel responseModelReturn = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            // First we check if exists 
            UserDobModel userDobModel = new UserDobModel();
            userDobModel = GetUserDob(userDobModelToInsert.UserId);
            if (userDobModel != null)
            {
                // Update if the data is not equal
                if (userDobModelToInsert.Dob != userDobModel.Dob)
                    return this.UpdateDOB(userDobModelToInsert);  // If not equal, update
                else
                {
                    MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
                    responseModelReturn.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                    return responseModelReturn;
                }
            }
            else
                return this.InsertDOB(userDobModelToInsert);  // If not exists, insert 
        }

        /// <summary>
        /// Insert or updates the user DOB with a full validation.
        /// </summary>
        /// <param name="userDobModelToInsert">The model including the user id and the dob<./param>
        /// <returns>Returns the transaction's details</returns>
        public ResponseModel InsertOrUpdateUserDobProcessWithValidation(UserDobModel userDobModelToInsert)
        {
            ResponseModel responseModelReturn = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            // First, lets validate 
            responseModelReturn = ValidationManager.ValidateDOB(userDobModelToInsert.Dob);

            if (!responseModelReturn.Valid)
            {
                responseModelReturn.Details = new MessageManager(this._connectionString, this._langCode).GetMessageDetails(responseModelReturn.ErrorCodes);
                return responseModelReturn;
            }

            // If valid, then continue
            UserDobDAO userDobDAO = new UserDobDAO();
            List<UserDobModel> userGenderModelsList = userDobDAO.Read(this._connectionStringRead, new UserDobModel() { UserId = userDobModelToInsert.UserId }, true, true);

            if (userGenderModelsList.Count > 0)
            {
                // Update
                try
                {
                    userDobDAO.Update(this._connectionString, new UserDobModel() { Dob = DateTime.Parse(userDobModelToInsert.Dob).ToString("yyyy-MM-dd HH:mm:ss") }, new UserDobModel() { UserId = userDobModelToInsert.UserId });
                    responseModelReturn.Valid = true;
                    responseModelReturn.Details.Add(new MessageManager(this._connectionString, this._langCode).
                        GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                    return responseModelReturn;
                }
                catch (Exception)
                {
                    responseModelReturn.Valid = false;
                    return responseModelReturn;
                }
            }
            else
            {
                // Insert 
                try
                {
                    userDobDAO.Create(this._connectionString, new UserDobModel()
                    {
                        Dob = DateTime.Parse(userDobModelToInsert.Dob).ToString("yyyy-MM-dd HH:mm:ss"),
                        UserId = userDobModelToInsert.UserId
                    });
                    responseModelReturn.Valid = true;
                    responseModelReturn.Details.Add(new MessageManager(this._connectionString, this._langCode).
                        GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                    return responseModelReturn;
                }
                catch (Exception)
                {
                    responseModelReturn.Valid = false;
                    return responseModelReturn;
                }
            }
        }

        /// <summary>
        /// Returns the users' date of birth by filtering by another dob.
        /// </summary>
        /// <param name="userDobModelList">The users' DOB list to filter.</param>
        /// <returns>Returns a list of users' DOBs.</returns>
        public List<UserDobModel> GetUserDobList(List<UserDobModel> userDobModelList)
        {
            UserDobDAO userDobDao = new UserDobDAO();
            return userDobDao.Read(this._connectionStringRead, userDobModelList);
        }
        #endregion UserDOB

        #region UserGender

        /// <summary>
        /// Get the user gender by the user id.
        /// </summary>
        /// <param name="userId">The user id to check.</param>
        /// <returns>Returns the user's gender model.</returns>
        public UserGenderModel GetUserGender(string userId)
        {
            // Get Gender if any 
            List<UserGenderModel> userGenderModelList = new UserGenderDAO().Read(this._connectionStringRead, new UserGenderModel
            {
                UserId = userId
            }, true, true);
            if (userGenderModelList.Count > 0)
                return userGenderModelList[0];
            else
                return null;
        }

        /// <summary>
        /// To insert or update the user's gender without validation.
        /// </summary>
        /// <param name="userGenderModelToInsert">The user's gender model to insert.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel InsertUserGenderProccess(UserGenderModel userGenderModelToInsert)
        {
            ResponseModel responseModelReturn = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            // First we check if exists 
            UserGenderModel userGenderModelToValidate = new UserGenderModel();
            userGenderModelToValidate = GetUserGender(userGenderModelToInsert.UserId);
            if (userGenderModelToValidate != null)
            {
                // Update if the data is not equal
                if (userGenderModelToInsert.Gender != userGenderModelToValidate.Gender)
                    return this.UpdateGender(userGenderModelToInsert);  // If not equal, update
                else
                {
                    MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
                    responseModelReturn.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                    return responseModelReturn;
                }
            }
            else
                return this.InsertGender(userGenderModelToInsert);  // If not exists, insert 
        }

        /// <summary>
        /// To insert or update the user's gender with validation.
        /// </summary>
        /// <param name="userGenderModelToInsert">The user's gender model to insert.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel InsertOrUpdateUserGenderProcessWithValidation(UserGenderModel userGenderModelToInsert)
        {
            ResponseModel responseModelReturn = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            // First, lets validate 
            responseModelReturn = ValidationManager.ValidateGender(userGenderModelToInsert.Gender);

            if (!responseModelReturn.Valid)
            {
                responseModelReturn.Details = new MessageManager(this._connectionString, this._langCode).GetMessageDetails(responseModelReturn.ErrorCodes);
                return responseModelReturn;
            }
            // If valid, then continue
            UserGenderDAO userGenderDAO = new UserGenderDAO();
            List<UserGenderModel> userGenderModelsList = userGenderDAO.Read(this._connectionStringRead, new UserGenderModel() { UserId = userGenderModelToInsert.UserId }, true, true);

            if (userGenderModelsList.Count > 0)
            {
                // Update
                try
                {
                    userGenderDAO.Update(this._connectionString, new UserGenderModel() { Gender = userGenderModelToInsert.Gender }, new UserGenderModel() { UserId = userGenderModelToInsert.UserId });
                    responseModelReturn.Valid = true;
                    responseModelReturn.Details.Add(new MessageManager(this._connectionString, this._langCode).
                        GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                    return responseModelReturn;
                }
                catch (Exception)
                {
                    responseModelReturn.Valid = false;
                    return responseModelReturn;
                }
            }
            else
            {
                // Insert 
                try
                {
                    userGenderDAO.Create(this._connectionString, new UserGenderModel()
                    {
                        Gender = userGenderModelToInsert.Gender,
                        UserId = userGenderModelToInsert.UserId
                    });
                    responseModelReturn.Valid = true;
                    responseModelReturn.Details.Add(new MessageManager(this._connectionString, this._langCode).
                        GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                    return responseModelReturn;
                }
                catch (Exception)
                {
                    responseModelReturn.Valid = false;
                    return responseModelReturn;
                }
            }
        }

        /// <summary>
        /// Returns the users' gender list by users gender list filtering. 
        /// </summary>
        /// <param name="userGenderModelList">The users' gender list to filter.</param>
        /// <returns>Returns the users' gender list.</returns>
        public List<UserGenderModel> GetUserGenderModelList(List<UserGenderModel> userGenderModelList)
        {
            UserGenderDAO userGenderDAO = new UserGenderDAO();
            return userGenderDAO.Read(this._connectionStringRead, userGenderModelList);
        }
        #endregion UserGender

        #region UserFullName 


        /// <summary>
        /// To insert or modify the user's full name.
        /// </summary>
        /// <param name="givenName">The given name (first name)./</param>
        /// <param name="surname">The surname (last name).</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns details of the transaction./returns>
        public ResponseModel InsertUserFullNameDetails(string givenName, string surname, string suffix, string userId)
        {
            ResponseModel responseModelReturn = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            if (!String.IsNullOrWhiteSpace(givenName))
                givenName = givenName.Trim();
            if (!String.IsNullOrWhiteSpace(surname))
                surname = surname.Trim();
            if (!String.IsNullOrWhiteSpace(suffix))
                suffix = suffix.Trim();

            // First, we check if exists alReady 
            UserFullNameModel userFullNameModel = this.GetUserFullname(userId);
            if (userFullNameModel == null)
            {
                // Insert
                responseModelReturn = this.InsertUserFullname(new UserFullNameModel
                {
                    GivenName = givenName,
                    Surname = surname,
                    Suffix = suffix,
                    UserId = userId
                });
                return responseModelReturn;
            }
            else
            {
                // Update: Process the update 
                responseModelReturn = this.UpdateUserFullname(new UserFullNameModel()
                {
                    Suffix = suffix,
                    GivenName = givenName,
                    Surname = surname,
                    UserId = userFullNameModel.UserId
                });
                return responseModelReturn;
            }
        }

        /// <summary>
        /// Returns the user's fullname by user's id.
        /// </summary>
        /// <param name="userId">The user's id to find the fullname.</param>
        /// <returns>Returns the user's full name model.</returns>
        public UserFullNameModel GetUserFullname(string userId)
        {
            // Get User full name
            List<UserFullNameModel> userFullNameModelList = new UserFullNameDAO().Read(this._connectionStringRead, new UserFullNameModel
            {
                UserId = userId
            }, true, true);
            if (userFullNameModelList.Count > 0)
                return userFullNameModelList[0];
            else
                return null;
        }

        #endregion UserFullName

        #region UserPhone 

        /// <summary>
        /// To insert the user's phone.
        /// </summary>
        /// <param name="userPhoneNumberModel">The user's phone number to insert.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel InsertUserPhone(UserPhoneNumberModel userPhoneNumberModel)
        {
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };
            try
            {

                if (string.IsNullOrWhiteSpace(userPhoneNumberModel.UserId) || string.IsNullOrWhiteSpace(userPhoneNumberModel.PhoneNumber))
                {
                    responseModel.Valid = false;
                    return responseModel;
                }

                // Clean the phone 
                userPhoneNumberModel.PhoneNumber = Regex.Replace(userPhoneNumberModel.PhoneNumber, @"[^0-9]", "");
                UserPhoneNumberDAO userPhoneNumberDAO = new UserPhoneNumberDAO();

                // Check if user exists already. 
                List<UserPhoneNumberModel> userPhoneNumberExists = userPhoneNumberDAO.Read(this._connectionStringRead,
                    new UserPhoneNumberModel()
                    {
                        UserId = userPhoneNumberModel.UserId,
                    }, false, false);

                if (userPhoneNumberExists.Count > 0)
                {
                    // Delete everything, and create the new one. 
                    userPhoneNumberDAO.Delete(this._connectionString, new UserPhoneNumberModel()
                    {
                        UserId = userPhoneNumberModel.UserId,
                    });
                }

                userPhoneNumberDAO.Create(this._connectionString, userPhoneNumberModel);


                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_phone_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;

            }
        }
        #endregion UserPhone


        /// <summary>
        /// Updates a list a user full names. It must be already validated.
        /// </summary>
        /// <param name="userFullNameModelsToUpdate">User full name to update.</param>
        /// <param name="userFullnameToUpdateWhere">User full name where list.</param>
        public void UpdateUserNameListWithoutValidation(List<UserFullNameModel> userFullNameModelsToUpdate, List<UserFullNameModel> userFullnameToUpdateWhere)
        {
            new UserFullNameDAO().Update(this._connectionString, userFullNameModelsToUpdate, userFullnameToUpdateWhere);
        }

        /// <summary>
        /// Updates a list of date of birth without validation. Must be already be validated.
        /// </summary>
        /// <param name="userDobModelsToUpdate">Users DOB to update.</param>
        /// <param name="userDobModelsToUpdateWhere">Users DOB where list.</param>
        public void UpdateDobListWithoutValidation(List<UserDobModel> userDobModelsToUpdate, List<UserDobModel> userDobModelsToUpdateWhere)
        {
            new UserDobDAO().Update(this._connectionString, userDobModelsToUpdate, userDobModelsToUpdateWhere);
        }

        /// <summary>
        /// Inserts a list of date of birth without validation. Must be already be validated.
        /// </summary>
        /// <param name="userDobModelsToInsert">Users DOB to insert.</param>
        public void InsertDobListWithoutValidation(List<UserDobModel> userDobModelsToInsert)
        {
            new UserDobDAO().Create(this._connectionString, userDobModelsToInsert);
        }

        /// <summary>
        /// Updates a list of gender without validation. Must be already be validated.
        /// </summary>
        /// <param name="userGenderModelsToUpdate">Users' gender to update.</param>
        /// <param name="userGenderModelsToUpdateWhere">Users' gender where list.</param>
        public void UpdateGenderListWithoutValidation(List<UserGenderModel> userGenderModelsToUpdate, List<UserGenderModel> userGenderModelsToUpdateWhere)
        {
            new UserGenderDAO().Update(this._connectionString, userGenderModelsToUpdate, userGenderModelsToUpdateWhere);
        }

        /// <summary>
        /// Inserts a list of gender without validation. Must be already be validated.
        /// </summary>
        /// <param name="userGendersModelsToInsert">Users Gender to insert.</param>
        public void InsertGenderListWithoutValidation(List<UserGenderModel> userGendersModelsToInsert)
        {
            new UserGenderDAO().Create(this._connectionString, userGendersModelsToInsert);
        }

        /// <summary>
        /// To update the user name details with validation.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="firstname">The user's first name.</param>
        /// <param name="lastname">The user's last name.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="userModifiedBy">The user's that is modifying the record.</param>
        /// <returns>Returns a response model with transaction details.</returns>
        public ResponseModel UpdateOrInsertUserNameAndLastnameWithValidation(string userId, string firstname, string lastname, string suffix, string userModifiedBy)
        {
            ResponseModel responseModel = new ResponseModel();

            firstname = firstname?.Trim();
            lastname = lastname?.Trim();
            suffix = suffix?.Trim();

            // Now validate 
            ResponseModel responseValidName = ValidationManager.ValidateName(firstname);
            ResponseModel responseValidLastName = ValidationManager.ValidateSurname(lastname);

            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);

            if (!responseValidName.Valid || !responseValidLastName.Valid)
            {
                List<string> errorCodeList = new List<string>();
                errorCodeList.AddRange(responseValidName.ErrorCodes);
                errorCodeList.AddRange(responseValidLastName.ErrorCodes);
                // Errors 
                responseModel.Valid = false;
                responseModel.Details = messageManager.GetMessageDetails(errorCodeList);
                return responseModel;
            }
            else
            {
                // Update 
                // Check if the user exists 
                UserFullNameDAO userFullNameDAO = new UserFullNameDAO();
                List<UserFullNameModel> userFullNameModelsIfExists = userFullNameDAO.Read(this._connectionStringRead, new UserFullNameModel() { UserId = userId }, true, true);

                if (userFullNameModelsIfExists.Count > 0)
                {
                    try
                    {
                        userFullNameDAO.Update(this._connectionString, new UserFullNameModel()
                        {
                            GivenName = firstname,
                            Surname = lastname,
                            Suffix = suffix,
                            ModifiedBy = userModifiedBy
                        },
                        new UserFullNameModel()
                        {
                            UserId = userId
                        });
                        responseModel.Valid = true;
                        responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_name_successfully_updated"]));
                        return responseModel;
                    }
                    catch (Exception)
                    {
                        responseModel.Valid = false;
                        return responseModel;
                    }
                }
                else // Create 
                {
                    try
                    {
                        userFullNameDAO.Create(this._connectionString,
                            new UserFullNameModel()
                            {
                                GivenName = firstname,
                                Surname = lastname,
                                Suffix = suffix,
                                UserId = userId
                            });
                        responseModel.Valid = true;
                        responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_name_successfully_updated"]));
                        return responseModel;
                    }
                    catch (Exception)
                    {
                        responseModel.Valid = false;
                        return responseModel;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the user fullname.
        /// </summary>
        /// <param name="userFullNameToUpdate">The user full name to update.</param>
        /// <returns>Returns the transaction's details.</returns>
        private ResponseModel UpdateUserFullname(UserFullNameModel userFullNameToUpdate)
        {
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };
            try
            {
                UserFullNameModel userFullNameModelWhere = new UserFullNameModel
                {
                    UserId = userFullNameToUpdate.UserId
                };
                UserFullNameDAO userFullNameDAO = new UserFullNameDAO();
                userFullNameDAO.Update(this._connectionString, userFullNameToUpdate, userFullNameModelWhere);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;
            }
        }

        /// <summary>
        /// Inserts the user's date of birth. 
        /// </summary>
        /// <param name="userDobModelToInsert">The user DOB model. Must have the users id and the dob.</param>
        /// <returns>Returns details of the transaction.</returns>
        private ResponseModel InsertDOB(UserDobModel userDobModelToInsert)
        {
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            try
            {
                UserDobDAO userDobDAO = new UserDobDAO();
                userDobDAO.Create(this._connectionString, userDobModelToInsert);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;
            }
        }

        /// <summary>
        /// Returns the user date of birth.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the user dob model.</returns>
        private UserDobModel GetUserDob(string userId)
        {
            // Get User DOB
            List<UserDobModel> userDobList = new UserDobDAO().Read(this._connectionStringRead, new UserDobModel
            {
                UserId = userId
            }, true, true);
            if (userDobList.Count > 0)
                return userDobList[0];
            else
                return null;
        }

        /// <summary>
        /// To update the date of birth.
        /// </summary>
        /// <param name="userDobModel">The user dob. Must have the dob and the user id.</param>
        /// <returns>Returns details of the transaction. </returns>
        private ResponseModel UpdateDOB(UserDobModel userDobModel)
        {
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };
            try
            {
                UserDobModel userDobModelWhere = new UserDobModel
                {
                    UserId = userDobModel.UserId
                };

                UserDobDAO userDobDAO = new UserDobDAO();
                userDobDAO.Update(this._connectionString, userDobModel, userDobModelWhere);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;
            }
        }

        /// <summary>
        /// To insert the user's gender.
        /// </summary>
        /// <param name="userGenderModel">The user's gender model.</param>
        /// <returns>Returns details of the transaction. If valid, then it was a success</returns>
        private ResponseModel InsertGender(UserGenderModel userGenderModel)
        {
            userGenderModel.Gender = userGenderModel.Gender.ToUpper();

            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            try
            {
                UserGenderDAO userGenderDAO = new UserGenderDAO();
                userGenderDAO.Create(this._connectionString, userGenderModel);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;
            }
        }

        /// <summary>
        /// To updates the user's gender.
        /// </summary>
        /// <param name="userGenderModel">The user gender model to update.</param>
        /// <returns>Returns details of the transaction. If valid, then it was a success</returns>
        private ResponseModel UpdateGender(UserGenderModel userGenderModel)
        {
            userGenderModel.Gender = userGenderModel.Gender.ToUpper();
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };
            try
            {
                UserGenderModel userGenderModelWhere = new UserGenderModel
                {
                    UserId = userGenderModel.UserId
                };
                UserGenderDAO userGenderDAO = new UserGenderDAO();
                userGenderDAO.Update(this._connectionString, userGenderModel, userGenderModelWhere);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;
            }
        }

        /// <summary>
        /// To insert the user's full name.
        /// </summary>
        /// <param name="userFullNameModelToInsert">The user's full name model to insert.</param>
        /// <returns>Returns the response transaction details.</returns>
        private ResponseModel InsertUserFullname(UserFullNameModel userFullNameModelToInsert)
        {
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                Details = new List<string>()
            };

            try
            {
                UserFullNameDAO userFullNameDAO = new UserFullNameDAO();
                userFullNameDAO.Create(this._connectionString, userFullNameModelToInsert);
                responseModel.Details.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_user_details_success"]));
                return responseModel;
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                return responseModel;
            }
        }
    }
}
