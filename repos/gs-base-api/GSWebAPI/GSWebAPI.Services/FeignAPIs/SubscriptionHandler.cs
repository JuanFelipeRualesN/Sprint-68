using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    public static class SubscriptionHandler
    {
        /// <summary>
        /// To check if the student is a homeschool user.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns true if homeschool, else, false.</returns>
        public static bool CheckIStudentIsHomeschool(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_user_homeschool"]).Result;
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