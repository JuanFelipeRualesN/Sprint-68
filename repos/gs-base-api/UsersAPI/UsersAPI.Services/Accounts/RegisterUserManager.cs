using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;


namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle registrations and details related to the registration.
    /// </summary>
    public static class RegisterUserManager
    {
        /// <summary>
        /// Return the main address of the main profile account.
        /// </summary>
        /// <param name="userMainAddressId">The main address id of the user.</param>
        /// <param name="langCode">Language code.</param>
        /// <returns>Returns the user main address.</returns>
        public static UsersAPI.Domain.Models.PersonalDetails.UserAddressModel GetMainAccountMainAddress(string userMainAddressId, string langCode)
        {
            return new UsersAPI.Services.PersonalDetails.AddressManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"], 
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetUserMainAddress(userMainAddressId);
        }

        /// <summary>
        /// To register user-profiles such as students, teachers, and so on. 
        /// </summary>
        /// <param name="requestModel">Register with request details.</param>
        /// <param name="userSessionId">User current session.</param>
        /// <param name="ipAddress">IP Address.</param>
        /// <param name="userAddressModel">User address, which will be the same as main subscription.</param>
        /// <param name="subscriptionDetails">Subscription Details.</param>
        /// <param name="roleId">Role ID.</param>
        /// <param name="langCode">The language code of the user that is making the request.</param>
        /// <returns>Returns the details.</returns>
        public static RegisterProfileResponseModel RegisterUserProfile(
            RegisterRequestModel requestModel,
            string userSessionId,
            string ipAddress,
            UsersAPI.Domain.Models.PersonalDetails.UserAddressModel userAddressModel,
            UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionResponseModel subscriptionDetails,
            string roleId,
            string langCode)
        {
            RegisterProfileResponseModel generalResponseModel = new RegisterProfileResponseModel();

            // Now, continue with the child registration
            UserModifyManager userModifyManager = new UserModifyManager(
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                langCode);

            UsersAPI.Domain.Models.Accounts.ResponseModel userChildRegisterResponseModel = userModifyManager.RegisterSubProfile(
                requestModel.Username,
                requestModel.Password,
                requestModel.PasswordRetype,
                ipAddress,
                roleId,
                requestModel.FirstName,
                requestModel.LastName,
                requestModel.DoB,
                userAddressModel.AddressLine1,
                userAddressModel.AddressLine2,
                userAddressModel.City,
                userAddressModel.State,
                userAddressModel.Country,
                userAddressModel.Zip,
                userAddressModel.CountryIsoCode2,
                requestModel.Gender,
                AppConfiguration.Configuration["AppSettings:gus_profile_gender_required"] == "1",
                AppConfiguration.Configuration["AppSettings:gus_profile_dob_required"] == "1",
                userSessionId,
                requestModel.Email);

            // Return the response
            generalResponseModel.UserId = userChildRegisterResponseModel.UserId;
            generalResponseModel.TransactionValid = userChildRegisterResponseModel.Valid;
            generalResponseModel.Messages = userChildRegisterResponseModel.Details;
            generalResponseModel.SpacesInSubscriptionLeft = int.Parse(subscriptionDetails.Subscriptions[0].ProfilesTotalQuantity) > subscriptionDetails.Subscriptions[0].SubscriptionProfiles.Count + 1; // The new space available
            return generalResponseModel;
        }
    }
}