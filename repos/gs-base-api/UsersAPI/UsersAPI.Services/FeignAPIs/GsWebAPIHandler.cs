using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace UsersAPI.UsersAPI.Services.FeignAPIs
{
    /// <summary>
    /// API that handle general settings.
    /// </summary>
    public class GsWebAPIHandler
    {
        /// <summary>
        /// To check if the invitation student status is enabled or not.
        /// </summary>
        /// <returns></returns>
        public static bool GetInvitationStudentStatus()
        {

            // If there is nothing in the cache, call the api.
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gs_web_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:gs_web_api_platform_setting_status"] + "?platformSetting=InvitationTeacherToStudentManagementTeacher").Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<bool>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
                else
                {
                    return false;
                }
            }
        }
    }
}