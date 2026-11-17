using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.PersonalDetails
{
    /// <summary>
    /// To handle details related to the address of the user.
    /// </summary>
    public class AddressManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code.</param>
        public AddressManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// To get the users' address.
        /// </summary>
        /// <param name="userAddressModels">The users address list to filter.</param>
        /// <returns>Return a list of user address model.</returns>
        public List<UserAddressModel> GetUserAddress(List<UserAddressModel> userAddressModels)
        {
            return new UserAddressDAO().Read(this._connectionStringRead, userAddressModels);
        }

        /// <summary>
        /// Updates a bulk of user address. The index between the the lists must be equal. 
        /// </summary>
        /// <param name="userAddressModelList">The user address list to update.</param>
        /// <param name="userAddressModelListWhere">The user addreess list to update (with the id of the user).</param>
        public void UpdateAddressBulk(List<UserAddressModel> userAddressModelList, List<UserAddressModel> userAddressModelListWhere)
        {
            UserAddressDAO userAddressDAO = new UserAddressDAO();
            userAddressDAO.Update(this._connectionString, userAddressModelList, userAddressModelListWhere);
        }

        /// <summary>
        /// Updates the user address (single user).
        /// </summary>
        /// <param name="userAddressModel">The user address model to update.</param>
        /// <param name="validateCity">To validate city when required.</param>
        /// <param name="validateZipCode">To validate zip code when required.</param>
        /// <param name="validatePersonAddress">To validate person address when required.</param>
        /// <param name="validateCountry">To validate country when required.</param>
        /// <param name="validateAddressLines">To validate address lines when required.</param>
        /// <returns>Returns the transaction details.</returns>
        public ResponseModel UpdateUserAddress(UserAddressModel userAddressModel, 
            bool validateAddressLines, bool validateZipCode, bool validateCity,
            bool validatePersonAddress,
            bool validateCountry
            )
        {
            var responseModel = this.ValidateAddress(userAddressModel, 
                validateAddressLines,  validateZipCode, validateCity,
                validatePersonAddress,
                validateCountry);
            if (!responseModel.Valid)
            {
                return responseModel;
            }

            this.SetOtherAddressesAsNotMain(userAddressModel.UserId);

            // We update the address (which is not an update, because we have old transactions with that address 
            UserAddressModel userAddressModelToUpdate = new UserAddressModel();
            UserAddressModel userAddressModelWhere = new UserAddressModel();
            string addressId = userAddressModel.Id;
            userAddressModelToUpdate.Enable = "0";
            userAddressModelWhere.Id = addressId;
            userAddressModelWhere.UserId = userAddressModel.UserId;

            UserAddressDAO userAddressDAO = new UserAddressDAO();
            userAddressDAO.Update(this._connectionString, userAddressModelToUpdate, userAddressModelWhere);


            userAddressModel.Id = null; // We set it null, because this must not be inserted 
            InsertUserAddress(userAddressModel);

            // Return success 
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            responseModel.Details = new List<string>
            {
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:adrs_message_succesfully_saved"])
            };

            return responseModel;

        }

        /// <summary>
        /// Validates the user address (AddressLine1, City, Person Address Name, Zip Code, Country (and Country ISO 2) are always required.
        /// </summary>
        /// <param name="userAddressModel">The user address model to validate. </param>
        /// <param name="validateAddressLines">To validate address lines when required.</param>
        /// <param name="validateCity">To validate city when required.</param>
        /// <param name="validateCountry">To validate country when required.</param>
        /// <param name="validatePersonAddress">To validate person address when required.</param>
        /// <param name="validateZipCode">To validate zip code when required.</param>
        /// <returns>Returns a response with transaction valid and the message list with error or success messages</returns>
        public ResponseModel ValidateAddress(UserAddressModel userAddressModel,
            bool validateAddressLines, bool validateZipCode, bool validateCity, 
            bool validatePersonAddress,
            bool validateCountry)
        {
            MessageManager messageManager = new MessageManager(this._connectionString, this._langCode);
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true
            };
            List<string> messageCodes = new List<string>();

            // Trim everything 
            if (!string.IsNullOrWhiteSpace(userAddressModel.AddressLine1))
                userAddressModel.AddressLine1 = userAddressModel.AddressLine1.Trim();
            if (!string.IsNullOrWhiteSpace(userAddressModel.AddressLine2))
                userAddressModel.AddressLine2 = userAddressModel.AddressLine2.Trim();
            if (!string.IsNullOrWhiteSpace(userAddressModel.City))
                userAddressModel.City = userAddressModel.City.Trim();
            if (!string.IsNullOrWhiteSpace(userAddressModel.PersonAddressName))
                userAddressModel.PersonAddressName = userAddressModel.PersonAddressName.Trim();
            if (!string.IsNullOrWhiteSpace(userAddressModel.State))
                userAddressModel.State = userAddressModel.State?.Trim(); // Optional
            if (!string.IsNullOrWhiteSpace(userAddressModel.Zip))
                userAddressModel.Zip = userAddressModel.Zip.Trim();
            if (!string.IsNullOrWhiteSpace(userAddressModel.Country))
                userAddressModel.Country = userAddressModel.Country.Trim();
            if (!string.IsNullOrWhiteSpace(userAddressModel.CountryIsoCode2))
                userAddressModel.CountryIsoCode2 = userAddressModel.CountryIsoCode2.Trim();
            userAddressModel.MainAddress = "1";
            userAddressModel.Enable = "1";


            // Validate 
            if (validateAddressLines && userAddressModel.AddressLine1.Length <= 0)
            {
                responseModel.Valid = false;
                messageCodes.Add(AppConfiguration.Configuration["AppSettings:adrs_message_code_address_line_1_required"]);
            }

            if (validateCity && userAddressModel.City.Length <= 0)
            {
                responseModel.Valid = false;
                messageCodes.Add(AppConfiguration.Configuration["AppSettings:adrs_message_code_address_city_required"]);
            }

            if (validatePersonAddress && userAddressModel.PersonAddressName.Length <= 0)
            {
                responseModel.Valid = false;
                messageCodes.Add(AppConfiguration.Configuration["AppSettings:adrs_message_code_address_person_name_required"]);
            }

            if (validateZipCode && userAddressModel.Zip.Length <= 0)
            {
                responseModel.Valid = false;
                messageCodes.Add(AppConfiguration.Configuration["AppSettings:adrs_message_code_address_zip_required"]);
            }

            if (validateCountry && userAddressModel.Country.Length <= 0 || userAddressModel.CountryIsoCode2.Length <= 0)
            {
                responseModel.Valid = false;
                messageCodes.Add(AppConfiguration.Configuration["AppSettings:adrs_message_code_address_country_required"]);
            }

            responseModel.Details = new List<string>();
            if (messageCodes.Count > 0)
                responseModel.Details = messageManager.GetMessageDetails(messageCodes);

            return responseModel;
        }


        /// <summary>
        /// Inserts the user address.
        /// </summary>
        /// <param name="userAddressModel">The details of the user address (model) to insert.</param>
        /// <returns>Returns the transaction's details.</returns>
        public void InsertUserAddress(UserAddressModel userAddressModel)
        {
            // Set other addresses as not main 
            this.SetOtherAddressesAsNotMain(userAddressModel.UserId);
            // Insert
            new UserAddressDAO().Create(this._connectionString, userAddressModel);
        }

        /// <summary>
        /// Set other user's addresses as not main. 
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public void SetOtherAddressesAsNotMain(string userId)
        {
            UserAddressModel userAddressModelToUpdate = new UserAddressModel
            {
                MainAddress = "0"
            };
            UserAddressModel userAddressModelWhere = new UserAddressModel
            {
                UserId = userId
            };
            new UserAddressDAO().Update(this._connectionString, userAddressModelToUpdate, userAddressModelWhere);
        }

        /// <summary>
        /// Returns the user main address.
        /// </summary>
        /// <param name="userIdsList">The user id list.</param>
        /// <returns>Returns a list of user's addresses models.</returns>
        public List<UserAddressModel> GetUsersMainAddress(List<string> userIdsList)
        {
            List<UserAddressModel> userAddressModelList = new List<UserAddressModel>();
            for (int i = 0; i < userIdsList.Count; i++)
            {
                UserAddressModel userAddressModel = new UserAddressModel
                {
                    UserId = userIdsList[i],
                    Enable = "1"
                };
                userAddressModelList.Add(userAddressModel);
            }
            return new UserAddressDAO().Read(this._connectionStringRead, userAddressModelList);
        }

        /// <summary>
        /// To get the user's main address.
        /// </summary>
        /// <param name="userId">The user's id to check.</param>
        /// <returns>Returns the main address model.</returns>
        public UserAddressModel GetUserMainAddress(string userId)
        {
            List<UserAddressModel> userAddressModelListResult = new UserAddressDAO().Read(this._connectionStringRead,
                 new UserAddressModel
                 {
                     UserId = userId,
                     Enable = "1",
                     MainAddress = "1"
                 }, true, true);

            if (userAddressModelListResult.Count <= 0)
                return null;
            else
                return userAddressModelListResult[0];
        }
    }
}
