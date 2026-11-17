using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Requests;
using Microsoft.AspNetCore.Http;
using SchoolsAPI.CustomClasses;
using CacheHandlerLibrary;
using Newtonsoft.Json.Linq;

namespace SchoolsAPI.SchoolsAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle the user details request the User's API.
    /// </summary>
    public class GetUserDetails
    {
        private readonly static string baseAddress = AppConfiguration.Configuration["AppSettings:gus_web_service_url"];
        private readonly static string usersApiPermissions = AppConfiguration.Configuration["AppSettings:gus_endpoint_permission_session_api"];

        /// <summary>
        /// To evaluate if token stills valid and to check if the user is Authorized or not. 
        /// </summary>
        /// <param name="token">Session token</param>
        /// <param name="ipAddress">User IP Address.</param>
        /// <param name="permissionName">User permission name.</param>
        /// <param name="langCode">Language code.</param>
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

                // Parse into JToken instead of ValidationResponseModel
                var jsonToken = JToken.Parse(responseString);

                // Safely add extra fields
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
    }
}