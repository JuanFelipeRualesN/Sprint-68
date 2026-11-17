using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Web;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.FeignAPI
{
    /// <summary>
    /// To handle details related to the subscription web service.
    /// </summary>
    public static class SubscriptionHandler
    {
        /// <summary>
        /// To get the user courses from the subscription web service.
        /// </summary>
        /// <param name="token">The user's token.</param>>
        /// <returns>Return the list of the courses.</returns>
        public static SubscriptionResponseModel GetUserCourses(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subs_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:subs_endpoint_call_courses"]).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var academicYears = JsonConvert.DeserializeObject<SubscriptionResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return academicYears;
                }
                else
                {
                    return new SubscriptionResponseModel();
                }
            }
        }

        /// <summary>
        /// To check if the student is a homeschool user.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns true if homeschool, else, false.</returns>
        public static bool CheckIStudentIsHomeschool(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subs_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:subs_endpoint_user_homeschool"]).Result;
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