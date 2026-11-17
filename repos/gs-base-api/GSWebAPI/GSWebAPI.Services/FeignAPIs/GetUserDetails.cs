using Amazon.Runtime;
using GSWebAPI.CustomClasses;
using GSWebAPI.CustomClasses.CacheHandler;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>
    /// Handle user details to users API.
    /// </summary>
    public class GetUserDetails
    {
        private UsersAPICache _userApiCache;
        private readonly static string baseAddress = AppConfiguration.Configuration["AppSettings:gus_web_service_url"];
        private readonly static string usersApiPermissions = AppConfiguration.Configuration["AppSettings:gus_endpoint_permission_session_api"];


        /// <summary>
        /// To evaluate if token stills valid and to check if the user is Authorized or not.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="permissionName">User permission name.</param>
        /// <returns>Returns the respons emodel with user details.</returns>
        public static async Task<ValidationResponseModel> GetUserIdRequestOnOtherAPI(HttpContext context, string permissionName)
        {
            string token = RequestHandler.GetToken(context);
            string langCode = RequestHandler.GetLanguage(context);
            string ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                return new ValidationResponseModel
                {
                    SessionValid = false
                };
            }

            // If cache not valid, call external API
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Token", token);

            var jsonPostModel = new PermissionSessionRequestModel
            {
                Token = token,
                IpAddress = ipAddress,
                PermissionName = permissionName,
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(usersApiPermissions, stringContent);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                var modelObject = JsonConvert.DeserializeObject<ValidationResponseModel>(responseString,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                modelObject.IpAddress = ipAddress;
                modelObject.Token = token;
                modelObject.LangCode = langCode;
                return modelObject;
            }

            return new ValidationResponseModel
            {
                SessionValid = false
            };
        }

        /// <summary>
        /// Search for the rival generic details.
        /// </summary>
        /// <param name="token">User token.</param>
        /// <param name="ipAddress">Ip Address.</param>
        /// <param name="rivalUserId">The ID of the rival to search.</param>
        /// <param name="langCode">The language code.</param>
        /// <returns>Returns an object with user details.</returns>
        public static UserRivalResponseModel GetUserRivalGeneralDetails(string token, string ipAddress, string rivalUserId, string langCode)
        {
            if (AppConfiguration.Configuration["AppSettings:debug_on"] == "1")
            {
                return new UserRivalResponseModel()
                {
                    SessionValid = true,
                    TransactionValid = true,
                    UserId = "1",
                };
            }

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
                    PermissionName = "GameQuizCompetitionSearchRival",
                    LangCode = langCode,
                    UserId = rivalUserId,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_rival_api"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    UserRivalResponseModel modelObject = JsonConvert.DeserializeObject<UserRivalResponseModel>(
                        responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                    return modelObject;
                }
                else
                {
                    return new UserRivalResponseModel()
                    {
                        SessionValid = false,
                        TransactionValid = false,
                    };
                }
            }
        }

        /// <summary>
        /// Validates if the user a can manage the user b.
        /// </summary>
        /// <param name="userAlphaId">User A Id (Alpha, as teacher, parent, etc).</param>
        /// <param name="userBetaId">User B Id (Beta, the student).</param>
        /// <param name="token">The User Token.</param>
        /// <returns>Returns true if can be managed, else returns false.</returns>
        public bool CanUserAManageUserB(HttpContext context, string userAlphaId, string userBetaId, string token)
        {

            try
            {
                _userApiCache = context.RequestServices.GetRequiredService<UsersAPICache>();
                if (_userApiCache.GetUserCanHandleUser(userAlphaId, userBetaId))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // Ignore an continue. 
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_a_manage_b"] + userAlphaId + "/" + userBetaId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    bool modelObject = JsonConvert.DeserializeObject<bool>(
                        responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });
                    try
                    {
                        _userApiCache.SetUserCanHandleUser(userAlphaId, userBetaId, modelObject);
                    }
                    catch (Exception)
                    {

                    }
                    return modelObject;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the profile main account user id.
        /// </summary>
        /// <param name="token">The user token.</param>
        /// <param name="ipAddress">The IP Address.</param>
        /// <returns>Returns the ID of the main account of the profile.</returns>
        public static AssociatedAccountModel GetProfileMainAccountUserId(string token, string ipAddress)
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
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_get_user_main_account_id"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var modelObject = JsonConvert.DeserializeObject<AssociatedAccountModel>(
                        responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
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
        /// Get the shared details of the users who the user is related with.
        /// </summary>
        /// <param name="token">The user token.</param>
        /// <param name="userIds">The ids of the users to find.</param>
        /// <param name="ipAddress">The IP Address.</param>
        /// <returns>Return a list of shared details.</returns>
        public static List<UserResponse> GetSharedDetails(string token, List<string> userIds, string ipAddress)
        {
            try
            {
                // Call the api for that
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", token);
                    UserRequestModel jsonPostModel = new UserRequestModel
                    {
                        UserIds = userIds,
                        IpAddress = ipAddress,
                    };

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_shared_details"], stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        UserResponseModel modelObject = JsonConvert.DeserializeObject<UserResponseModel>(
                            responseString,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                            });

                        return modelObject.UserInfo;
                    }
                    else
                    {
                        return new List<UserResponse>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<UserResponse>();
            }
        }

        /// <summary>
        /// Get main account user profiles. (For regular client).
        /// </summary>
        /// <param name="token">Token of the user in session.</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <param name="limiter">A limiter, because this could affect the performance if the client has a large quantity of users.</param>
        /// <returns>Return a list of profile users.</returns>
        public static List<UserResponse> GetMainAccountUserProfilesSharedDetails(string token, string ipAddress, int limiter)
        {
            try
            {
                // Call the api for that
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", token);
                    UserRequestModel jsonPostModel = new UserRequestModel
                    {
                        IpAddress = ipAddress,
                        PageFrom = 0,
                        Limiter = limiter,
                        LastSessionFrom = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"),
                        LastSessionTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    };

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_main_account_profile_details"], stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        UserResponseModel modelObject = JsonConvert.DeserializeObject<UserResponseModel>(
                            responseString,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                            });

                        return modelObject.UserInfo;
                    }
                    else
                    {
                        return new List<UserResponse>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<UserResponse>();
            }
        }
    }
}