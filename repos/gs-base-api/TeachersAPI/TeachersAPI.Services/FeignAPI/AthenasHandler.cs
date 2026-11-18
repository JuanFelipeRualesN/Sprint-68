using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas;
using TeachersAPI.Domain.Models.Generals.Requests.APIs;
using TeachersAPI.CustomClasses.CacheHandler;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.Domain.Models.Courses;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Codes;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Lessons.Response;
using LessonsResponseModel = TeachersAPI.Domain.Models.Generals.Responses.APIs.LessonsResponseModel;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.FeignAPI
{
    /// <summary>
    /// Handles Athenas details.
    /// </summary>
    public class AthenasHandler
    {
        /// <summary>
        /// Get the quizzes of a lesson.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson's quizzes.</returns>
        public static LessonQuizzesResponseModel GetLessonQuizzes(string lessonId)
        {
            // First check the cache.
           // AthenasAPICache athenasCache = new AthenasAPICache();
            //LessonQuizzesResponseModel cacheObject = athenasCache.GetLessonQuizzess(lessonId);

            // If the lesson in the cache is not null, then return it.
            //if (cacheObject != null)
           // {
            //    return cacheObject;
            //}

            using (var clientLessonQuizzes = new HttpClient())
            {
                clientLessonQuizzes.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                clientLessonQuizzes.DefaultRequestHeaders.Accept.Clear();
                clientLessonQuizzes.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientLessonQuizzes.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

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
                   // athenasCache.SetLessonQuizzess(lessonId, responseAPI);

                    return responseAPI;
                }

                return new LessonQuizzesResponseModel();
            }
        }

                 /// <summary>
        /// To get the quiz from Athenas by quiz's id.
        /// </summary>
        /// <param name="quizId">The quiz's id.</param>
        /// <param name="token">The user access token.</param>
        /// <returns>Returns the AthenasJSONQuizResponseModel with quiz's details.</returns>
        public static AthenasJSONQuizResponseModel GetQuizById(string quizId, string token)
        {
            // Continue with the operation
            // First, we call Athenas, and we get the Quiz
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
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
                    return modelObject;
                }
                else
                {
                    return new AthenasJSONQuizResponseModel();
                }
            }
        }


        /// <summary>
        /// Gets Athenas lesson by lesson model to call.
        /// </summary>
        /// <param name="lessonModelsToCall">Lesson model to calls.</param>
        /// <param name="published">true if only published, false if not.</param>
        /// <returns>Return a Lesson Response Model.</returns>
        public static LessonsResponseModel GetAthenasLessons(List<LessonModel> lessonModelsToCall, bool published)
        {
            // First, read from the Cache so we can avoid to call unnecesary network call 
           // AthenasAPICache athenasCache = new AthenasAPICache();
            // List<LessonModel> lessonModelsInCache = athenasCache.GetLessonModels(lessonModelsToCall);

           // if (lessonModelsInCache.Count > 0)
           // {
                // Remove the ones that we do not have to call 
            //    lessonModelsToCall.RemoveAll(x => lessonModelsInCache.Any(y => y.Id == x.Id));
           // }

            LessonsResponseModel lessonResponse = new LessonsResponseModel();

            if (lessonModelsToCall.Count > 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                    LessonModelSpecificsRequestModel lessonModelSpecificsRequestModel = new LessonModelSpecificsRequestModel()
                    {
                        LessonModelList = lessonModelsToCall,
                    };

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(lessonModelSpecificsRequestModel), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(published ? AppConfiguration.Configuration["AppSettings:athenas_web_api_lessons_by_lessons"] : AppConfiguration.Configuration["AppSettings:athenas_web_api_lessons_by_lessons_without_filter_published"], stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;


                        lessonResponse = JsonConvert.DeserializeObject<LessonsResponseModel>(responseString,
                         new JsonSerializerSettings
                         {
                             NullValueHandling = NullValueHandling.Ignore
                         });

                        // Save the new lessons in Cache 
                      //  athenasCache.SetLessonModels(lessonResponse.LessonModelList);
                    }
                }
            }

           // lessonResponse.LessonModelList.AddRange(lessonModelsInCache);

            return lessonResponse;
        }

        /// <summary>
        /// Returns Athenas subjects
        /// </summary>
        /// <returns></returns>
        public static List<SubjectModel> GetAthenasSubjects(string langCode)
        {
            try
            {
                // First, read from the Cache so we can avoid to call unnecesary network call 
                AthenasAPICache athenasCache = new AthenasAPICache();
                try
                {
                    List<SubjectModel> subjectsInCache = athenasCache.GetAthenasSubjects();

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
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                    var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:athenas_web_api_get_subjects_not_published"]).Result;
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
                            athenasCache.SetAthenasSubjects(subjects);
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
        /// To get the PDF details.
        /// </summary>
        /// <param name="langCode">Language code.</param>
        ///  <param name="lessonId">If of the lesson.</param>
        /// <param name="isForStudent">If its for a student.</param>
        /// <returns>Returns the PDF response details.</returns>
        public static string GetAthenasLessonPDFUrl(string langCode, string lessonId, bool isForStudent)
        {
            // First, check in the Cache 
            AthenasAPICache athenasCache = new AthenasAPICache();
            string locationCache = null;

            if (isForStudent)
                locationCache = athenasCache.GetLessonPDFStudentLocation(lessonId);
            else
                locationCache = athenasCache.GetLessonPDFTeacherLocation(lessonId);

            if (!string.IsNullOrWhiteSpace(locationCache))
                return locationCache;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                var response = client.GetAsync(isForStudent ? string.Concat(AppConfiguration.Configuration["AppSettings:athenas_web_api_get_students_pdf"], lessonId) : string.Concat(AppConfiguration.Configuration["AppSettings:athenas_web_api_get_pdf"], lessonId)).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    string location = JsonConvert.DeserializeObject<string>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    // Store in the cache 
                    if (isForStudent)
                        athenasCache.SetLessonPDFStudentLocation(lessonId, location);
                    else
                        athenasCache.SetLessonPDFTeacherLocation(lessonId, location);

                    return location;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// To get the Athenas quizzess by id list.
        /// </summary>
        /// <param name="quizIdsAthenasModels">The quiz id list.</param>
        /// <returns>Returns the quiz Athenas model. </returns>
        public static List<QuizModel> GetQuizAthenasModelList(List<string> quizIdsAthenasModels)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(quizIdsAthenasModels), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:athenas_api_get_quizzes_by_list"], stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        var quizzes = JsonConvert.DeserializeObject<List<QuizModel>>(responseString,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });


                        return quizzes;
                    }
                    else
                    {
                        return new List<QuizModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<QuizModel>();
            }
        }

        /// <summary>
        /// To get the diagnostic test from Athenas.
        /// </summary>
        /// <returns>Returns the athenas diagnostics test model list.</returns>
        public static List<AthenasDiagnosticTestModel> GetAthenasDiagnosticTest(List<string> diagnosticIds)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                    string url = AppConfiguration.Configuration["AppSettings:athenas_api_get_diagnostic_test"];
                    var stringContent = new StringContent(JsonConvert.SerializeObject(diagnosticIds), Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        var diagnosticTest = JsonConvert.DeserializeObject<List<AthenasDiagnosticTestModel>>(responseString,
                                                       new JsonSerializerSettings
                                                       {
                                NullValueHandling = NullValueHandling.Ignore
                            });

                        return diagnosticTest;
                    }
                    else
                    {
                        return new List<AthenasDiagnosticTestModel>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<AthenasDiagnosticTestModel>();
            }
        }

        /// <summary>
        /// Get Athenas diagnostic details.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <returns>Returns the Athenas Diagnostic details.</returns>
        public static AthenasDiagnosticTestModel GetAthenasDiagnosticDetails(string subjectCode, string levelCode)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                    string url = AppConfiguration.Configuration["AppSettings:athenas_api_get_diagnostic_test_detail"];
                    // Substitute the subject code and level code
                    url = url.Replace("{subjectCode}", subjectCode);
                    url = url.Replace("{levelCode}", levelCode);
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        var diagnosticTest = JsonConvert.DeserializeObject<AthenasDiagnosticTestModel>(responseString,
                                                       new JsonSerializerSettings
                                                       {
                                                           NullValueHandling = NullValueHandling.Ignore
                                                       });

                        return diagnosticTest;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// To get the learning sheet by incorrect questions ids.
        /// </summary>
        /// <param name="learningSheetIds">The learning sheets ids to find.</param>
        /// <returns>Returns the learning sheet models.</returns>
        public static List<LearningSheetModel> GetLearningSheetByList(List<string> learningSheetIds)
        {
            // Return empty if there are no questions.
            if (learningSheetIds == null || learningSheetIds.Count == 0)
            {
                return new List<LearningSheetModel>();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(learningSheetIds), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:athenas_api_get_learning_sheets_list"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<LearningSheetModel> learningSheets = JsonConvert.DeserializeObject<List<LearningSheetModel>>(responseString,
                                               new JsonSerializerSettings
                                               {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return learningSheets;
                }
                else
                {
                    return new List<LearningSheetModel>();
                }
            }
        }

        /// <summary>
        /// Returns the grades/levels from Athenas API
        /// </summary>
        /// <returns>A list of grades/levels</returns>
        public static async Task<List<LevelModel>> GetAthenasGradesAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                    string url = AppConfiguration.Configuration["AppSettings:athenas_web_api_get_grades"];

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }

                    string responseString = await response.Content.ReadAsStringAsync();

                    var grades = JsonConvert.DeserializeObject<List<LevelModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return grades;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for the grades.", ex);
            }

        }

        /// <summary>
        /// Returns the specified codes from Athenas API (Standards, expectations, etc).
        /// </summary>
        /// <returns>A list with the codes</returns>
        public static async Task<PaginationModel<CodeHandleModel>> GetAthenasCodesAsync(CodeHandleModel codeHandleModel, int fromPage, int limiter, bool isExpectationType)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                    string baseUrl = AppConfiguration.Configuration["AppSettings:athenas_web_api_get_codes"];
                    string url = isExpectationType
                        ? $"{baseUrl}?fromPage={fromPage}&limiter={limiter}&uniqueExpectations=true"
                        : $"{baseUrl}?fromPage={fromPage}&limiter={limiter}";

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(codeHandleModel), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, stringContent);


                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }

                    string responseString = await response.Content.ReadAsStringAsync();

                    var codes = JsonConvert.DeserializeObject<PaginationModel<CodeHandleModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return codes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for the grades.", ex);
            }
        }

        /// To get the lessons detail from Athenas lesson ids.
        /// </summary>
        /// <param name="token">The user access token.</param>
        /// <param name="lessonIds"></param>
        /// <returns>Returns the LessonResponseModel list with lesson's details.</returns>
        public static List<LessonResponseModel> GetLessonsDetail(string token, params string[] lessonIds)
        {
            // Continue with the operation
            // First, we call Athenas, and we get the Quiz
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
                client.DefaultRequestHeaders.Add("Token", token);

                string url = AppConfiguration.Configuration["AppSettings:athenas_api_lessons_lesson"];

                List<LessonResponseModel> lessonResponseModels = new List<LessonResponseModel>(); 
                foreach (var lessonId in lessonIds)
                {
                    LessonRequestModel lessonRequestModel = new LessonRequestModel()
                    {
                        AddQuizDetails = true,
                        IsForOutsideSource = true,
                        LessonId = lessonId
                    };

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(lessonRequestModel), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, stringContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        LessonResponseModel modelObject = JsonConvert.DeserializeObject<LessonResponseModel>(responseString, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                        lessonResponseModels.Add(modelObject);
                    }
                }

                return lessonResponseModels;
            }
        }

        /// <summary>
        /// To get the HTML response of a lesson from Athenas.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="langCode">The language code.</param>
        /// <returns>Returns the HTML of the lesson.</returns>
        public static HTMLViewerResponseModel GetHTMLResponse(string lessonId, string langCode)
        {
            // Continue with the operation
            // First, we call Athenas, and we get the Quiz
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                string url = AppConfiguration.Configuration["AppSettings:athenas_api_lesson_html"];
                url = url.Replace("{lessonId}", lessonId);

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    var modelObject = JsonConvert.DeserializeObject<HTMLViewerResponseModel>(responseString, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });
                    return modelObject;
                }
                else
                {
                    return new HTMLViewerResponseModel();
                }
            }
        }

        /// <summary>
        /// To get the PDF details.
        /// </summary>
        /// <param name="langCode">Language code.</param>
        ///  <param name="lessonId">If of the lesson.</param>
        /// <param name="quizId">If its for a student.</param>
        /// <param name="outPutFormat"></param>
        /// <returns>Returns the PDF response details.</returns>
        public static string GetAthenasLessonQuizPDFUrl(string langCode, string lessonId, string quizId, string outPutFormat)
        {
            // First, check in the Cache 
            AthenasAPICache athenasCache = new AthenasAPICache();
            string locationCache = null;

            locationCache = athenasCache.GetLessonPDFTeacherLocation(lessonId);

            if (!string.IsNullOrWhiteSpace(locationCache))
                return locationCache;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);

                string apiEndpoint = string.Concat(
                    AppConfiguration.Configuration["AppSettings:athenas_web_api_get_students_pdf"],
                    lessonId, "/", quizId, "/" , outPutFormat
                );

                var response = client.GetAsync(apiEndpoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    string location = JsonConvert.DeserializeObject<string>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    // Store in the cache 
                    athenasCache.SetLessonPDFTeacherLocation(lessonId, location);

                    return location;
                }
                else
                {
                    return null;
                }
            }
        }

        /// Get lessons joined with their unit info (unit name + description, with
        /// Spanish translation fallback) for a given set of lesson ids.
        /// Used by the teacher events calendar.
        /// </summary>
        /// <param name="lessonIds">The lesson ids to filter by.</param>
        /// <param name="langCode">The user's selected language.</param>
        /// <returns>Returns the lessons' unit details.</returns>
        public static List<LessonWithUnitsModel> GetLessonsWithUnits(List<string> lessonIds, string langCode)
        {
            using (var clientLessonsWithUnits = new HttpClient())
            {
                clientLessonsWithUnits.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                clientLessonsWithUnits.DefaultRequestHeaders.Accept.Clear();
                clientLessonsWithUnits.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientLessonsWithUnits.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
                clientLessonsWithUnits.DefaultRequestHeaders.Add("Accept-Language", langCode);
                string url = AppConfiguration.Configuration["AppSettings:athenas_api_get_lessons_with_units"];

                var content = new StringContent(
                    JsonConvert.SerializeObject(lessonIds),
                    Encoding.UTF8,
                    "application/json");

                var responseLessonsWithUnits = clientLessonsWithUnits.PostAsync(url, content).Result;
                if (responseLessonsWithUnits.IsSuccessStatusCode)
                {
                    string responseStringLessonsWithUnits = responseLessonsWithUnits.Content.ReadAsStringAsync().Result;
                    var responseAPI = JsonConvert.DeserializeObject<List<LessonWithUnitsModel>>(
                        responseStringLessonsWithUnits,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });
                    return responseAPI;
                }
                return new List<LessonWithUnitsModel>();
            }
        }
    }
}