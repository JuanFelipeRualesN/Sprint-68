using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ReadingAPI.ReadingAPI.Domain.DTOs.Generals;
using ReadingAPI.CustomClasses;

namespace ReadingAPI.ReadingAPI.Services.FeignAPIs
{
    public class GetUserDetails
    {
        private readonly static string baseAddress = AppConfiguration.Configuration["AppSettings:gus_web_service_url"];
        private readonly static string usersApiPermissions = AppConfiguration.Configuration["AppSettings:gus_endpoint_permission_session_api"];

        /// <summary>
        /// To evaluate if token stills valid and to check if the user is Authorized or not. 
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="permissionName">User permission name.</param>
        /// <returns>Returns the respons emodel with user details.</returns>
        public static async Task<ValidationResponseDto> GetUserIdRequestOnOtherAPI(HttpContext context, string permissionName)
        {
            string token = RequestHandler.GetToken(context);
            string langCode = RequestHandler.GetLanguage(context);
            string ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                return new ValidationResponseDto
                {
                    SessionValid = false
                };
            }

            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Token", token);

            var jsonPostModel = new PermissionSessionRequestDto
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

                var modelObject = JsonConvert.DeserializeObject<ValidationResponseDto>(responseString,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                modelObject.IpAddress = ipAddress;
                modelObject.Token = token;
                modelObject.LangCode = langCode;
                return modelObject;
            }

            return new ValidationResponseDto
            {
                SessionValid = false
            };
        }
    }
}
