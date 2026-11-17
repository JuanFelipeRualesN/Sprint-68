using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.FeignAPIs;
using DataAnalysisAPI.Utils;
using Microsoft.AspNetCore.Http;
using DataAnalysisAPI;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs
{
    public class GetUserDetails
    {
        /// <summary>
        /// To evaluate if the token is valid or not.
        /// </summary>
        /// <param name="httpContext">The request with the token and language.</param>
        /// <param name="permissionName">The permission's name.</param>
        /// <returns>Returns the validation details.</returns>
        public static ValidationResponseModel GetUserIdRequestOnOtherAPI(HttpContext httpContext, string permissionName)
        {

            string token = RequestHandler.GetToken(httpContext);
            string langCode = RequestHandler.GetLanguage(httpContext);
            string ipAddress = IPAddressDetector.GetClientIpAddress(httpContext.Request);

            if (string.IsNullOrWhiteSpace(token))
            {
                return new ValidationResponseModel()
                {
                    SessionValid = false,
                };
            }

            // If cache not valid, then check in the API
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gus_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                PermissionSessionRequestModel jsonPostModel = new PermissionSessionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                    PermissionName = permissionName,
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

                    modelObject.IpAddress = ipAddress;
                    modelObject.Token = token;
                    modelObject.LangCode = langCode;

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
        /// Validates if the user a can manage the user b.
        /// </summary>
        /// <param name="userAlphaId">User A Id (Alpha, as teacher, parent, etc).</param>
        /// <param name="userBetaId">User B Id (Beta, the student).</param>
        /// <param name="token">User token.</param>
        /// <returns>Returns true if can be managed, else returns false.</returns>
        public static bool CanUserAManageUserB(string userAlphaId, string userBetaId, string token)
        {

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
                   
                    return modelObject;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}