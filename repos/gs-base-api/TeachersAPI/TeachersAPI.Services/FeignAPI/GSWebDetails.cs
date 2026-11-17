using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.FeignAPI
{
    /// <summary>
    /// To handle details related to GS Web.
    /// </summary>
    public static class GSWebDetails
    {
        /// <summary>
        /// To get the user results from GS Web.
        /// </summary>
        /// <param name="token">The user's token (the user's making the request.)</param>
        /// <param name="usquiResultId">The usqui result id.</param>
        /// <returns>Returns the user quiz response model.</returns>
        public static UserQuizResponseModel GetUserResults(string token, string usquiResultId)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gs_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);
                
                // Get the gs web usqui result from web config
                var usquiResultUrl = AppConfiguration.Configuration["AppSettings:gs_endpoint_usqui_result"];
                usquiResultUrl = usquiResultUrl.Replace("{userQuizResultId}", usquiResultId);
                // replace the usqui result id

                var response = client.GetAsync(usquiResultUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var results = JsonConvert.DeserializeObject<UserQuizResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return results;
                }
                else
                {
                    return new UserQuizResponseModel();
                }
            }
        }

        /// <summary>
        /// To get the user results per lesson for homeschoolers.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns the list of lesson quiz results.</returns>
        public static List<LessonQuizResponseModel> GetStudentHomeschoolResultsPerLesson(string token)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:gs_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                // Get the gs web usqui result from web config
                var usquiResultUrl = AppConfiguration.Configuration["AppSettings:gs_endpoint_user_homeschool_exams"];
                // replace the usqui result id

                var response = client.GetAsync(usquiResultUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var results = JsonConvert.DeserializeObject<List<LessonQuizResponseModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return results;
                }
                else
                {
                    return new List<LessonQuizResponseModel>();
                }
            }
        }
    }
}