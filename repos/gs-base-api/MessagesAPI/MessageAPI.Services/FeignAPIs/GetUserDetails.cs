using Amazon.Runtime.Credentials.Internal;
using CacheHandlerLibrary;
using MessagesAPI.CustomClasses;
using MessagesAPI.CustomClasses.CacheHandler;
using MessagesAPI.MessageAPI.Domain.Models.General;
using MessagesAPI.MessageAPI.Domain.Models.General.APIs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace MessagesAPI.MessageAPI.Services.FeignAPIs
{
    /// <summary>
    /// To get the user details in the Users API.
    /// </summary>
    public static class GetUserDetails
    {
        private readonly static string baseAddress = AppConfiguration.Configuration["AppSettings:gus_web_service_url"];
        private readonly static string usersApiFullNames = AppConfiguration.Configuration["AppSettings:gus_endpoint_get_user_fullnames_api"];
        private readonly static string usersApiPermissions = AppConfiguration.Configuration["AppSettings:gus_endpoint_permission_session_api"];
        private readonly static string usersApiAllowedInteractionEmails = AppConfiguration.Configuration["AppSettings:gus_endpoint_user_id_list_allowed_interaction_emails"];
        private readonly static string usersApiProfileConnections = AppConfiguration.Configuration["AppSettings:gus_endpoint_profile_connections_api"];
        private readonly static string usersApiUserEmails = AppConfiguration.Configuration["AppSettings:gus_endpoint_get_user_emails_api"];

        /// <summary>
        /// To get the user full names without any strict validation.
        /// </summary>
        /// <param name="token">Token of the user making the request.</param>
        /// <param name="userList">The list of the users to call.</param>
        /// <returns>Returns the full names simple.</returns>
        public static List<UserAdvance> GetUserFullNamesSimple(string token, List<string> userList)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userList), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_enpoint_user_full_names_general"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var modelObject = JsonConvert.DeserializeObject<List<UserAdvance>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject == null || modelObject.Count == 0)
                    {
                        return new List<UserAdvance>();
                    }

                    return modelObject;
                }
                else
                {
                    return new List<UserAdvance>();
                }
            }
        }

        /// <summary>
        /// To evaluate if the token is valid or not.
        /// </summary>
        /// <param name="request">The request with the token and language.</param>
        /// <param name="permissionName">The permission's name.</param>
        /// <returns>Returns the validation details.</returns>
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

            // Check cache first
            ExternalAPIsCache authenticationCache = new ExternalAPIsCache();
            var validationResponse = authenticationCache.GetUserAccessDetails(token, permissionName, ipAddress);
            if (validationResponse != null)
            {
                return validationResponse;
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

                var jsonToken = JToken.Parse(responseString);


                if (jsonToken is JObject jObj)
                {
                    jObj["IpAddress"] = ipAddress;
                    jObj["Token"] = token;
                    jObj["LangCode"] = langCode;
                }

                authenticationCache.SetAccessDetails(token, permissionName, ipAddress, jsonToken);

                return modelObject;
            }

            return new ValidationResponseModel
            {
                SessionValid = false
            };
        }

        /// <summary>
        /// To get the user ids that the user can handle by role.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <param name="roleId">The role's id to get the users.</param>
        /// <param name="pageFrom">The page from.</param>
        /// <param name="limiter">How many items to get per request.</param>
        /// <returns>Returns the user emails' list.</returns>
        public static List<UserEmailModel> GetUsersIdsThatUserCanHandleByRole(string token, string roleId, int pageFrom = 0, int limiter = 50)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new UserRequestModel() { RoleId = roleId, PageFrom = pageFrom, Limiter = limiter }),
                    Encoding.UTF8, "application/json");
                var response = client.PostAsync(usersApiAllowedInteractionEmails, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<UserEmailModel> modelObject = JsonConvert.DeserializeObject<List<UserEmailModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return new List<UserEmailModel>();
                }
            }
        }

        /// <summary>
        /// Returns the users that the user can interact with.
        /// </summary>
        /// <param name="token">The user token of the user making the request.</param>
        /// <returns>Returns a list of profile connections to interact with. </returns>
        public static List<ProfileConnectionModel> GetUsersThatUserCanInteractWith(string token)
        {
            var profileConnectionResponseJSONModel = new List<ProfileConnectionModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(usersApiProfileConnections).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    profileConnectionResponseJSONModel = JsonConvert.DeserializeObject<List<ProfileConnectionModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return profileConnectionResponseJSONModel;

                }
                else
                {
                    return new List<ProfileConnectionModel>();
                }
            }
        }

        /// <summary>
        /// To get the user email models. 
        /// </summary>
        /// <param name="userIds">The user ids to get the emails.</param>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns the email details.</returns>
        public static List<UserEmailModel> GetUserEmailModels(List<string> userIds, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userIds), Encoding.UTF8, "application/json");
                var response = client.PostAsync(usersApiUserEmails, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<UserEmailModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
                else
                {
                    return new List<UserEmailModel>();
                }
            }
        }
    }
}