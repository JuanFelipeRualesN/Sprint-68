using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GSWebAPI.CustomClasses.CacheHandler;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle Athenas' details.
    /// </summary>
    public class AthenasDetailsHandler
    {
        private readonly AthenasAPICache _athenasApiCache;
        /// <summary>
        /// To get the quiz from Athenas by quiz's id.
        /// </summary>
        /// <param name="quizId">The quiz's id.</param>
        /// <param name="token">The user access token.</param>
        /// <returns>Returns the AthenasJSONQuizResponseModel with quiz's details.</returns>
        public static AthenasJSONQuizResponseModel GetQuizById(string quizId, string token)
        {
            // First check the cache.
            /*CustomClasses.CacheHandler.AthenasAPICache athenasCache = new CustomClasses.CacheHandler.AthenasAPICache();
            AthenasJSONQuizResponseModel cacheObject = athenasCache.GetQuiz(quizId);

            // If the lesson in the cache is not null, then return it.
            if (cacheObject != null)
            {
                return cacheObject;
            } */

            // Continue with the operation
            // First, we call Athenas, and we get the Quiz
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);
                client.DefaultRequestHeaders.Add("Token", token);

                string url = AppConfiguration.Configuration["AppSettings:athenas_api_quiz"];
                url = url.Replace("{0}", quizId);

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    AthenasJSONQuizResponseModel modelObject = JsonConvert.DeserializeObject<AthenasJSONQuizResponseModel>(responseString, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });
                    // athenasCache.SetQuiz(quizId, modelObject);
                    return modelObject;
                }
                else
                {
                    return new AthenasJSONQuizResponseModel();
                }
            }
        }

        /// <summary>
        /// Get the quizzes of a lesson.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson's quizzes.</returns>
        public static LessonQuizzesResponseModel GetLessonQuizzes(string lessonId)
        {
            // First check the cache.
            /*
            CustomClasses.CacheHandler.AthenasAPICache athenasCache = new CustomClasses.CacheHandler.AthenasAPICache();
            LessonQuizzesResponseModel cacheObject = athenasCache.GetLessonQuizzess(lessonId);

            // If the lesson in the cache is not null, then return it.
            if (cacheObject != null)
            {
                return cacheObject;
            } */

            using (var clientLessonQuizzes = new HttpClient())
            {
                clientLessonQuizzes.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);
                clientLessonQuizzes.DefaultRequestHeaders.Accept.Clear();
                clientLessonQuizzes.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientLessonQuizzes.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);

                string url = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_quizzes"];
                url = url.Replace("{0}", lessonId);
                var responseLessons = clientLessonQuizzes.GetAsync(url).Result;
                if (responseLessons.IsSuccessStatusCode)
                {
                    string responseStringLessonQuizzes = responseLessons.Content.ReadAsStringAsync().Result;
                    LessonQuizzesResponseModel responseAPI = JsonConvert.DeserializeObject<LessonQuizzesResponseModel>(
                        responseStringLessonQuizzes,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });
                   //  athenasCache.SetLessonQuizzess(lessonId, responseAPI);

                    return responseAPI;
                }

                return new LessonQuizzesResponseModel();
            }
        }

        /// <summary>
        /// Get a random quiz from Athenas.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns the quiz's details.</returns>
        public static AthenasJSONQuizResponseModel GetRandomQuiz(string subjectCode, string levelCode, string token)
        {
            using (var clientLessonQuizzes = new HttpClient())
            {
                clientLessonQuizzes.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);
                clientLessonQuizzes.DefaultRequestHeaders.Add("Token", token);
                clientLessonQuizzes.DefaultRequestHeaders.Accept.Clear();
                clientLessonQuizzes.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientLessonQuizzes.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);

                string url = AppConfiguration.Configuration["AppSettings:athenas_api_quiz_read_random"];
                url = url.Replace("{$subjectCode}", subjectCode);
                url = url.Replace("{$levelCode}", levelCode);
                var responseLessons = clientLessonQuizzes.GetAsync(url).Result;
                if (responseLessons.IsSuccessStatusCode)
                {
                    string responseStringLessonQuizzes = responseLessons.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<AthenasJSONQuizResponseModel>(
                        responseStringLessonQuizzes,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });
                }

                return new AthenasJSONQuizResponseModel();
            }
        }

        /// <summary>
        /// To get Athenas lesson by id.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson's details.</returns>
        public static AthenasResponseLessonModel GetLessonById(string lessonId)
        {
            // First check the cache.
            /*
            CustomClasses.CacheHandler.AthenasAPICache athenasCache = new CustomClasses.CacheHandler.AthenasAPICache();
            AthenasResponseLessonModel athenasResponseLessonModel = athenasCache.GetLesson(lessonId);

            // If the lesson in the cache is not null, then return it.
            if (athenasResponseLessonModel != null)
            {
                return athenasResponseLessonModel;
            } */

            // Read from the endpoint.
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AthenasJSONPostModel athenasJSONPostModel = new AthenasJSONPostModel
                {
                    LangCode = null,
                    IsForOutsideSource = true,
                    LessonId = lessonId,
                    IsForGame = true,
                    ShowUrlInstead = true,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(athenasJSONPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:athenas_api_lesson_read_v2"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    AthenasResponseLessonModel lesson = JsonConvert.DeserializeObject<AthenasResponseLessonModel>(responseString, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });
                    // athenasCache.SetLesson(lessonId, lesson);
                    return lesson;
                }
            }

            return new AthenasResponseLessonModel();
        }

        /// <summary>
        /// To get athenas subjects
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public List<SubjectModel> GetAthenasSubjects(string langCode)
        {
            try
            {
                // First, read from the Cache so we can avoid to call unnecesary network call 
                try
                {
                    List<SubjectModel> subjectsInCache = _athenasApiCache.GetAthenasSubjects();

                    if (subjectsInCache != null && subjectsInCache.Count > 0)
                    {
                        return subjectsInCache;
                    }
                }
                catch (Exception ex)
                {
                    // Ignore
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);

                    var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:athenas_web_api_get_subjects"]).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        List<SubjectModel> subjects = JsonConvert.DeserializeObject<List<SubjectModel>>(responseString,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        try
                        {
                            _athenasApiCache.SetAthenasSubjects(subjects);
                        }
                        catch (Exception ex)
                        {
                            // Ignore
                        }

                        return subjects;
                    }
                    else
                    {
                        return new List<SubjectModel>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<SubjectModel>();
            }
        }

        /// <summary>
        /// To get the lesson models by lesson ids.
        /// </summary>
        /// <param name="lessonIds">The ids of the lessons to call.</param>
        /// <returns>The response. </returns>
        public static LessonResponseModel GetLessonModels(List<string> lessonIds)
        {
            // Prepare the request 
            var lessonRequestModel = new LessonResponseModel()
            {
                LessonModelList = new List<LessonModel>()
            };

            foreach (var lessonId in lessonIds)
            {
                lessonRequestModel.LessonModelList.Add(new LessonModel()
                {
                    Id = lessonId,
                });
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_service_url"]);
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(lessonRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:athenas_api_lessons_models_read"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    LessonResponseModel lessons = JsonConvert.DeserializeObject<LessonResponseModel>(responseString, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });
                    // athenasCache.SetLesson(lessonId, lesson);
                    return lessons;
                }
            }

            return new LessonResponseModel()
            {
                LessonModelList = new List<LessonModel>()
            };
        }
    }
}