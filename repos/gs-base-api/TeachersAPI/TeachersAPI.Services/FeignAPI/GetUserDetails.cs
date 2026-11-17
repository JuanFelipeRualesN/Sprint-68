using TeachersAPI.CustomClasses;
using TeachersAPI.CustomClasses.CacheHandler;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Requests.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.FeignAPI
{
    /// <summary>
    /// To handle the details related to Users API.
    /// </summary>
    public static class GetUserDetails
    {
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
        /// Returns the teachers student.
        /// </summary>
        /// <param name="generalRequestModel">The request with user ids, roles, etc.</param>
        /// <param name="token">Session token</param>
        /// <returns>Returns a </returns>
        public static UserResponseModel GetStudentsThatCanBeHandled(UserRequestModel generalRequestModel, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(generalRequestModel), Encoding.UTF8, "application/json");

                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_call_users_handle"], stringContent).Result;
                UserResponseModel profileConnectionResponseJSONModel = new UserResponseModel();
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    profileConnectionResponseJSONModel = JsonConvert.DeserializeObject<UserResponseModel>(responseString,
                      new JsonSerializerSettings
                      {
                          NullValueHandling = NullValueHandling.Ignore
                      });

                    return profileConnectionResponseJSONModel;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Get users shared details.
        /// </summary>
        /// <param name="token">User token.</param>
        /// <param name="ipAddress">Ip Address.</param>
        /// <param name="userList">User list to request.</param>
        /// <returns>Returns the user shared details.</returns>
        public static List<UserResponse> GetUsersSharedDetails(string token, string ipAddress, List<string> userList)
        {
            UsersAPICache userApiCache = new UsersAPICache();
            List<UserResponse> result = new List<UserResponse>();

            foreach (var userId in userList)
            {
                // Check if the user exists in Redis cache
                var cachedUser = userApiCache.GetUserGeneralInfoInCache(userId);

                if (cachedUser != null)
                {
                    // User found in cache, add to the result list
                    result.Add(cachedUser);
                }
            }

            if(result.Count == userList.Count)
            {
                return result;
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);
                UserListRequestModel userListRequestModel = new UserListRequestModel()
                {
                    UserIds = userList,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userListRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_get_user_shared_details_api"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    UserResponseModel modelObject = JsonConvert.DeserializeObject<UserResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject.UserInfo == null || modelObject.UserInfo.Count == 0)
                    {
                        return new List<UserResponse>();
                    }

                    // Save in the cache 
                    Task.Run(() =>
                    {
                        if (modelObject.UserInfo != null && modelObject.UserInfo.Count > 0)
                        {
                            foreach (var item in modelObject.UserInfo)
                            {
                                userApiCache.SetUserGeneralInfoInCache(item.UserId, item);
                            }
                            // Add the user to the result list
                            result.Add(modelObject.UserInfo[0]);
                        }
                    }); 

                    return modelObject.UserInfo;
                }
                else
                {
                    return new List<UserResponse>();
                }
            }
        }

        /// <summary>
        /// Get users full names of same school.
        /// </summary>
        /// <param name="token">User token.</param>
        /// <param name="ipAddress">Ip Address.</param>
        /// <param name="userList">User list to request.</param>
        /// <returns>Returns the user shared details.</returns>
        public static List<UserFullNameModel> GetUsersFullNamesSameSchool(string token, string ipAddress, List<string> userList)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userList), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_call_users_full_names_same_school"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<UserFullNameModel> modelObject = JsonConvert.DeserializeObject<List<UserFullNameModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject == null || modelObject.Count == 0)
                    {
                        return new List<UserFullNameModel>();
                    }

                    return modelObject;
                }
                else
                {
                    return new List<UserFullNameModel>();
                }
            }
        }

        /// <summary>
        /// Get user full names. This algorithm is different. Is simpler, because where it is used, it will hit the performance.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <param name="ipAddress">Ip Address</param>
        /// <param name="userList">The users' list to call.</param>
        /// <returns>Returns a list of user full names.</returns>
        public static List<UserFullNameModel> GetProfileConnectionsFullNames(string token, string ipAddress, List<string> userList)
        {
            using (var client = new HttpClient())
            {
                UserListRequestModel userListRequestModel = new UserListRequestModel()
                {
                    UserIds = userList,
                };
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userListRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_just_names"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<UserFullNameModel> modelObject = JsonConvert.DeserializeObject<List<UserFullNameModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject == null || modelObject.Count == 0)
                    {
                        return new List<UserFullNameModel>();
                    }

                    return modelObject;
                }
                else
                {
                    return new List<UserFullNameModel>();
                }
            }
        }

        /// <summary>
        /// To get the user full names withut any strict validation.
        /// </summary>
        /// <param name="token">Token of the user making the request.</param>
        /// <param name="ipAddress">Ip Address.</param>
        /// <param name="userList">The list of the users to call.</param>
        /// <returns>Returns the full names simple.</returns>
        public static List<UserFullNameModel> GetUserFullNamesSimple(string token, string ipAddress, List<string> userList)
        {
            UsersAPICache userApiCache = new UsersAPICache();
            var result = new List<UserFullNameModel>();

            foreach (var userId in userList)
            {
                // Check if the user exists in Redis cache
                var cachedUser = userApiCache.GetUserGeneralNameInCache(userId);

                if (cachedUser != null)
                {
                    // User found in cache, add to the result list
                    result.Add(cachedUser);
                }
            }

            if (result.Count == userList.Count)
            {
                return result;
            }

            using (var client = new HttpClient())
            {
                UserListRequestModel userListRequestModel = new UserListRequestModel()
                {
                    UserIds = userList,
                };
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userListRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_enpoint_user_full_names_general"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<UserFullNameModel> modelObject = JsonConvert.DeserializeObject<List<UserFullNameModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject == null || modelObject.Count == 0)
                    {
                        return new List<UserFullNameModel>();
                    }

                    // Save in the cache 
                    Task.Run(() =>
                    {
                        if (modelObject != null && modelObject.Count > 0)
                        {
                            foreach (var item in modelObject)
                            {
                                userApiCache.SetUserGeneralNameInCache(item.UserId, item);
                            }
                        }
                    });

                    return modelObject;
                }
                else
                {
                    return new List<UserFullNameModel>();
                }
            }
        }

        /// <summary>
        /// To get the users main groups.
        /// </summary>
        /// <param name="token">The user's token (the user making the request).</param>
        /// <param name="ipAddress">The user's ip address (the user making the request).</param>
        /// <param name="userList">List of the users.</param>
        /// <returns>Returns the users' main group list.</returns>
        public static List<UserMainGroupModel> GetUsersMainGroups(string token, string ipAddress, List<string> userList)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userList), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_schools_user_groups_list"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<UserMainGroupModel> modelObject = JsonConvert.DeserializeObject<List<UserMainGroupModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject == null || modelObject.Count == 0)
                    {
                        return new List<UserMainGroupModel>();
                    }

                    return modelObject;
                }
                else
                {
                    return new List<UserMainGroupModel>();
                }
            }
        }

        /// <summary>
        /// Returns the users that the account can handle.
        /// </summary>
        /// <param name="token">User session token.</param>
        /// <param name="ipAddress">The IP Address.</param>
        /// <returns>Returns the user shared details.</returns>
        public static List<UserResponse> GetUsersThatAccountCanHandle(string token, string ipAddress)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);
                PermissionSessionRequestModel permissionSessionRequestModel = new PermissionSessionRequestModel()
                {
                    IpAddress = ipAddress,
                    Token = token,
                    PermissionName = "ProfileConnections",
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(permissionSessionRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_call_users_handle"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    UserResponseModel modelObject = JsonConvert.DeserializeObject<UserResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    if (modelObject == null || modelObject.UserInfo.Count == 0)
                    {
                        return new List<UserResponse>();
                    }

                    return modelObject.UserInfo;
                }
                else
                {
                    return new List<UserResponse>();
                }
            }
        }

        /// <summary>
        /// Validates if the user a can manage the user b.
        /// </summary>
        /// <param name="userAlphaId">User A Id (Alpha, as teacher, parent, etc).</param>
        /// <param name="userBetaId">User B Id (Beta, the student).</param>
        /// <param name="token">User token.</param>
        /// <returns>Returns true if can be managed, else returns false.</returns>
        public static bool CanUserAManageUserB(string userAlphaId, string userBetaId, string token)
        {
            UsersAPICache userApiCache = new UsersAPICache();
            if (userApiCache.GetUserCanHandleUser(userAlphaId, userBetaId))
            {
                return true;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);
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
                    userApiCache.SetUserCanHandleUser(userAlphaId, userBetaId, modelObject);
                    return modelObject;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Check if users is from the same school of the user in session.
        /// </summary>
        /// <param name="token">The user token.</param>
        /// <param name="ipAddress">The user IP Address.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns a bool if from the same school.</returns>
        public static bool CheckIfUsersFromSameSchool(string token, string ipAddress, string userId)
        {
            using (var clientUserSchools = new HttpClient())
            {
                clientUserSchools.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);
                clientUserSchools.DefaultRequestHeaders.Accept.Clear();
                clientUserSchools.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientUserSchools.DefaultRequestHeaders.Add("Token", token);
                var responseUserSchools = clientUserSchools.GetAsync(AppConfiguration.Configuration["AppSettings:gus_endpoint_user_same_school"] + "/" + userId).Result;
                if (responseUserSchools.IsSuccessStatusCode)
                {
                    string responseStringUserSchools = responseUserSchools.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<bool>(responseStringUserSchools,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }); ;
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// To get the school ids of the user.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="userId">The id of the user we want to check.</param>
        /// <returns>Returns the schools' ids of the user.</returns>
        public static List<string> GetSchoolIdsOfUser(string token,string userId)
        {
            using (var clientUserSchools = new HttpClient())
            {
                clientUserSchools.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);
                clientUserSchools.DefaultRequestHeaders.Accept.Clear();
                clientUserSchools.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientUserSchools.DefaultRequestHeaders.Add("Token", token);
                var url = AppConfiguration.Configuration["AppSettings:gus_endpoint_user_school_ids"];
                // Replace {userId} with the actual userId in the url
                url = url.Replace("{userId}", userId);
                var responseUserSchools = clientUserSchools.GetAsync(url).Result;
                if (responseUserSchools.IsSuccessStatusCode)
                {
                    string responseStringUserSchools = responseUserSchools.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<List<string>>(responseStringUserSchools,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }); ;
                }
                else
                {
                    return new List<string>();
                }
            }

        }
    }
}