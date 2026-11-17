using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using SubscriptionsAPI.CustomClasses.CacheHandler;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle the users' details in Users API, as validations for access tokens and get users' details.
    /// </summary>
    public class GetUserDetails
    {
        /// <summary>
        /// To relate profile to main account.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <param name="mainAccountUserId">The main account user to relate.</param>
        /// <param name="profileId">The profile to relate.</param>
        public static void RelateProfileToMainAccount(string token, string mainAccountUserId, string profileId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                string endpointToCall = AppConfiguration.Configuration["AppSettings:gus_endpoint_relate_profile_to_main_account"];
                endpointToCall = endpointToCall.Replace("{userIdMainAccount}", mainAccountUserId);
                endpointToCall = endpointToCall.Replace("{userIdProfile}", profileId);

                client.PostAsync(endpointToCall, null);
            }
        }

        /// <summary>
        /// To evaluate if token stills valid and to check if the user is Authorized or not. 
        /// </summary>
        /// <param name="token">Session token</param>
        /// <param name="ipAddress">User IP Address.</param>
        /// <param name="permissionName">User permission name.</param>
        /// <param name="langCode">Language code.</param>
        /// <returns>Returns the respons emodel with user details.</returns>
        public static ValidationResponseModel GetUserIdRequestOnOtherAPI(string token, string ipAddress, string permissionName, string langCode)
        {

            if (string.IsNullOrWhiteSpace(token))
            {
                return new ValidationResponseModel()
                {
                    SessionValid = false,
                };
            }

            // Check Cache first 
            var authenticationCache = new UsersAPICache();
            ValidationResponseModel validationResponse = authenticationCache.GetUserAccessDetails(token, permissionName, ipAddress);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                PermissionSessionRequestModel jsonPostModel = new PermissionSessionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                    PermissionName = permissionName,
                    LangCode = langCode
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_permission_session_api"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    ValidationResponseModel modelObject = JsonConvert.DeserializeObject<ValidationResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    authenticationCache.SetAccessDetails(token, permissionName, ipAddress, modelObject);

                    return modelObject;
                }
                else
                {
                    return new ValidationResponseModel()
                    {
                        SessionValid = false,
                    };
                }
            }
        }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        /// <param name="token">The user token.</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <param name="langCode">The language code.</param>
        /// <param name="userId">User Id. If there is specific user Id that we want the email.</param>
        /// <returns>Returns the email if anything.</returns>
        public static string GetUserEmail(string token, string ipAddress, string langCode, string userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                PermissionSessionRequestModel jsonPostModel = new PermissionSessionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                    LangCode = langCode,
                    UserId = userId
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_client_email_api"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    string modelObject = JsonConvert.DeserializeObject<string>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// To get the user address.
        /// </summary>
        /// <param name="token">User token.</param>
        /// <param name="ipAddress">Ip Address.</param>
        /// <returns>Returns the main account address details.</returns>
        public static UserAddressModel GetUserAddress(string token, string ipAddress)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                PermissionSessionRequestModel jsonPostModel = new PermissionSessionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_address_api"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    UserAddressModel modelObject = JsonConvert.DeserializeObject<UserAddressModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// To get the user address by employee.
        /// </summary>
        /// <param name="token">User token.</param>
        /// <param name="ipAddress">Ip Address.</param>
        /// <param name="userId">User ID to find.</param>
        /// <returns>Returns the main account address details.</returns>
        public static UserAddressModel GetUserAddressByEmployee(string token, string ipAddress, string userId)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                PermissionSessionRequestModel jsonPostModel = new PermissionSessionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                    UserId = userId,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_address_by_employee_api"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    UserAddressModel modelObject = JsonConvert.DeserializeObject<UserAddressModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// To filter the users and get the users ids. 
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="ipAddress">Ip address.</param>
        /// <param name="userReportFilters">Includes the username, name and other details to filters.</param>
        /// <returns>Returns a list of string of users.</returns>
        public static UserAdvanceResponseModel FilterUsersIds(string token, string ipAddress, UserReportsFilterRequestModel userReportFilters)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                userReportFilters.Token = token;
                userReportFilters.IpAddress = ipAddress;

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userReportFilters), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_filters"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    UserAdvanceResponseModel modelObject = JsonConvert.DeserializeObject<UserAdvanceResponseModel>(responseString,
                       new JsonSerializerSettings
                       {
                           NullValueHandling = NullValueHandling.Ignore
                       });

                    return modelObject;
                }
                else
                {
                    return new UserAdvanceResponseModel();
                }
            }
        }

        /// <summary>
        /// To get the user roles 
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="ipAddress">Ip address.</param>
        /// <param name="userIds">The user ids to find..</param>
        /// <returns>Returns a list of string of users.</returns>
        public static List<UserRoleModel> GetUserRoles(string token, string ipAddress, List<string> userIds)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                UserReportsFilterRequestModel userReportsFilters = new UserReportsFilterRequestModel()
                {
                    Token = token,
                    IpAddress = ipAddress,
                    UserIds = userIds,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userReportsFilters), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_roles"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<UserRoleModel> modelObject = JsonConvert.DeserializeObject<List<UserRoleModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return new List<UserRoleModel>();
                }
            }
        }
    }
}