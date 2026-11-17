using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.Relations;

namespace UsersAPI.UsersAPI.Services.PersonalDetails
{
    /// <summary>
    /// To handle details related to the user's address.
    /// </summary>
    public static class AddressUserManager
    {
        /// <summary>
        /// Updates the user's address.
        /// </summary>
        /// <param name="addressLine1">The address line 1.</param>
        /// <param name="addressLine2">The address line 2.</param>
        /// <param name="city">The city.</param>
        /// <param name="countryIso">The country Iso/</param>
        /// <param name="country">The country.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <param name="state">The state.</param>
        /// <param name="stateIso">The state iso.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="langCode">The lang code of the user that is making the request.</param>
        /// <param name="modifiedBy">The id of the user that is making the request.</param>
        /// <returns>Returns a response with transaction's details.</returns>
        public static GeneralResponseModel UpdateUserAddress(string addressLine1, string addressLine2, string city, string countryIso, string country, string zipCode, string state, string stateIso, string userId, string langCode, string modifiedBy)
        {
            GeneralResponseModel generalResponseModel = new GeneralResponseModel();
            // Returns the country USPS name
            country = new AddressDetailsHandler().GetCountryStandardName(countryIso);

            // Get user first name and last name
            UserPersonalDetailsManager userPersonalDetailsManager
                = new UserPersonalDetailsManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode);
            UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel userFullNameModel = userPersonalDetailsManager.GetUserFullname(userId);

            UsersAPI.Domain.Models.PersonalDetails.UserAddressModel userAddressModel = new UsersAPI.Domain.Models.PersonalDetails.UserAddressModel
            {
                PersonAddressName = userFullNameModel.GivenName + " " + userFullNameModel.Surname,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                CountryIsoCode2 = countryIso,
                Country = country,
                Zip = zipCode,
                StateIsoCode = state ?? stateIso,
                State = stateIso ?? state,
                MainAddress = "1",
                UserId = userId,
                Enable = "1",
                ModifiedBy = modifiedBy,
            };

            AddressManager userAddressManager = new AddressManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"], 
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode);

            // Update the user address
            var responseAddressModel = userAddressManager.UpdateUserAddress(userAddressModel, 
                true, true, true, true, true);

            // Now, update every child address 
            // Get related accounts 
            RelatedAccountsManager relatedAccountsManager = new RelatedAccountsManager(
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                langCode);
            var associatedAccounts =
            relatedAccountsManager.GetMainAccountRelatedProfiles(userId);

            List<UsersAPI.Domain.Models.PersonalDetails.UserAddressModel> profileAddressListToUpdate = new List<UsersAPI.Domain.Models.PersonalDetails.UserAddressModel>();
            List<UsersAPI.Domain.Models.PersonalDetails.UserAddressModel> profileAddressListToUpdateWhere = new List<UsersAPI.Domain.Models.PersonalDetails.UserAddressModel>();
            for (int i = 0; i < associatedAccounts.Count; i++)
            {
                UsersAPI.Domain.Models.PersonalDetails.UserAddressModel userAddressModelToUpdate = new UsersAPI.Domain.Models.PersonalDetails.UserAddressModel
                {
                    UserId = associatedAccounts[i].ProfileUserAccount,
                    AddressLine1 = addressLine1,
                    AddressLine2 = addressLine2,
                    MainAddress = "1",
                    City = city,
                    Country = country,
                    CountryIsoCode2 = countryIso,
                    StateIsoCode = state ?? stateIso,
                    State = stateIso ?? state,
                    Zip = zipCode,
                    ModifiedBy = modifiedBy,
                };

                UsersAPI.Domain.Models.PersonalDetails.UserAddressModel userAddressModelWhere = new UsersAPI.Domain.Models.PersonalDetails.UserAddressModel
                {
                    UserId = associatedAccounts[i].ProfileUserAccount,
                };
                profileAddressListToUpdateWhere.Add(userAddressModelWhere);
                profileAddressListToUpdate.Add(userAddressModelToUpdate);
            }

            if (profileAddressListToUpdate.Count > 0 && profileAddressListToUpdateWhere.Count > 0)
            {
                userAddressManager.UpdateAddressBulk(profileAddressListToUpdate, profileAddressListToUpdateWhere);
            }

            // Return the response
            generalResponseModel.TransactionValid = responseAddressModel.Valid;
            generalResponseModel.Messages = responseAddressModel.Details;
            return generalResponseModel;
        }
    }
}