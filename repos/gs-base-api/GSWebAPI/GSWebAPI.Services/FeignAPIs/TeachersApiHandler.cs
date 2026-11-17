using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle details related to teacher's API.
    /// </summary>
    public static class TeachersApiHandler
    {
        /// <summary>
        /// To insert the pre or post exam in teacher's API.
        /// </summary>
        /// <param name="token">The token to insert.</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="planLessonPrePost">The model details to insert.</param>
        public static void InsertPrePostResults(string token, string lessonId, PlanLessonPrePostUserResult planLessonPrePost)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:teachers_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                string endpointUrl = AppConfiguration.Configuration["AppSettings:teachers_endpoint_student_submit_lesson_pre_post_results"];
                endpointUrl = endpointUrl.Replace("{lessonId}", lessonId);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(planLessonPrePost), Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpointUrl, stringContent).Result;
            }
        }

        /// <summary>
        /// To insert the learning gap in teacher's API.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <param name="learningGapQuizSubmitRequestModel">To submit learning gap result.</param>
        public static void InsertLearningGap(string token, LearningGapQuizSubmitRequestModel learningGapQuizSubmitRequestModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:teachers_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                string endpointUrl = AppConfiguration.Configuration["AppSettings:teachers_endpoint_student_submit_learning_gap_results"];
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(learningGapQuizSubmitRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpointUrl, stringContent).Result;
            }
        }

        /// <summary>
        /// Inserts the results in the TeachersAPI.
        /// </summary>
        /// <param name="token">The user access token.</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="planLessonQuizUserResult">The plan lesson quiz user result object to insert.</param>
        public static void InsertExamResults(string token, string lessonId, PlanLessonQuizUserResult planLessonQuizUserResult)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:teachers_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                string endpointUrl = AppConfiguration.Configuration["AppSettings:teachers_endpoint_student_submit_lesson_quiz_results"];
                endpointUrl = endpointUrl.Replace("{lessonId}", lessonId);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(planLessonQuizUserResult), Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpointUrl, stringContent).Result;
            }
        }

        /// <summary>
        /// To check if exam can be submitted or not (for students from schools).
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="quizId">The quiz's id.</param>
        /// <returns>Returns true if can be submitted, else returns false.</returns>
        public static bool CheckIfExamCanbeSubmitted(string token, string quizId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:teachers_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                string endpoint = AppConfiguration.Configuration["AppSettings:teachers_endpoint_student_submit_availability"];
                endpoint = endpoint.Replace("{quizId}", quizId);
                var response = client.GetAsync(endpoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<bool>(
                        responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });
                }
                else
                {
                    return true; // If the servers are down, just allow it.
                }
            }
        }
    }
}