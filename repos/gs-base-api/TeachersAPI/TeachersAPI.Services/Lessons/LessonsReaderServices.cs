using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TeachersAPI.Domain.Models.Lessons;
using LessonResponse = TeachersAPI.Domain.Models.Lessons.Response;
using FilterResponseModel = TeachersAPI.Domain.Models.Generals.Responses;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Lessons.LessonSearchModels;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI.Domain.Models.Codes;
using TeachersAPI;
namespace TeachersAPI.TeachersAPI.Services.Lessons
{
    /// <summary>
    /// Service class to handle lesson reading operations.
    /// </summary>
    public class LessonsReaderServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the LessonsReaderServices class.
        /// </summary>
        public LessonsReaderServices()
        {
            _httpClient = new HttpClient();
            _apiUrl = AppConfiguration.Configuration["AppSettings:athenas_web_service_url"];
            _apiKey = AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"];

            // Configurar cliente HTTP
            _httpClient.BaseAddress = new Uri(_apiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
        }

        /// <summary>
        /// To read the lessons applying many advanced filters.
        /// </summary>
        /// <param name="lessonFilterRequestModel">The lesson's request with filters to apply.</param>
        /// <return>Returns a LessonsResponseModel containing the search results.</return>
        public async Task<FilterResponseModel.LessonSearchResponseModel> SearchLessonsAsync(LessonFilterRequestModel lessonFilterRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonFilterRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_advanced_lesson_search"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }

                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FilterResponseModel.LessonSearchResponseModel>(responseString) ?? new FilterResponseModel.LessonSearchResponseModel();
                }   
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }


        /// <summary>
        /// To get lesson details by lesson id
        /// </summary>
        /// <param name="lessonId">string with lessonId</param>
        /// <returns>Return LessonModel details</returns>
        public async Task<LessonModel> LessonDetailsServiceAsync(string lessonId)
        {
            try
            {
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_details"];
                apiEndpoint = apiEndpoint.Replace("{lessonId}", lessonId);
                var response = await _httpClient.GetAsync(apiEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                LessonModel lessonModel = JsonConvert.DeserializeObject<LessonModel>(responseContent);
                return lessonModel;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }
        
        /// <summary>
        /// Gets the lesson description.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>
        /// <returns>Returns the lesson description.</returns>
        public async Task<string> LessonDescriptionServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_description"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }

                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<string>(responseString) ?? string.Empty;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson objective.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson objective.</returns>
        public async Task<List<LessonObjectiveModel>> LessonObjectiveServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_objective"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LessonObjectiveModel>>(responseString) ?? new List<LessonObjectiveModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson transversal theme.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson transversal theme.</returns>
        public async Task<List<LessonTransversalThemeModel>> LessonTransversalThemeServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_transversal-theme"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LessonTransversalThemeModel>>(responseString) ?? new List<LessonTransversalThemeModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson strategies.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson strategies.</returns>
        public async Task<List<LessonStrategyModel>> LessonStrategiesServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_strategies"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LessonStrategyModel>>(responseString) ?? new List<LessonStrategyModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson performance task.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson performance task.</returns>
        public async Task<List<LessonPerformanceTaskModel>> LessonPerformanceTaskServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_performance-task"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LessonPerformanceTaskModel>>(responseString) ?? new List<LessonPerformanceTaskModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson concept.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson concept.</returns>
        public async Task<string> LessonConceptServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_concept"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<string>(responseString) ?? string.Empty;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson definition.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson definition.</returns>
        public async Task<List<LessonDefinitionModel>> LessonDefinitionServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_definition"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LessonDefinitionModel>>(responseString) ?? new List<LessonDefinitionModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson example.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson example.</returns>
        public async Task<List<LessonExampleModel>> LessonExampleServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_example"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LessonExampleModel>>(responseString) ?? new List<LessonExampleModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }

        /// <summary>
        /// Gets the lesson quizzes details.
        /// </summary>
        /// <param name="lessonDetailsRequestModel">LessonDetailsRequestModel</param>"
        /// <returns>Returns the lesson quizzes details.</returns>
        public async Task<List<QuizLessonSearchRequestModel>> LessonQuizzesDetailsServiceAsync(LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(lessonDetailsRequestModel);
                using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                {
                    string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_quizzes-details"];
                    var response = await _httpClient.PostAsync(apiEndpoint, stringContent);
                    Console.WriteLine($"Response: {response}");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<QuizLessonSearchRequestModel>>(responseString) ?? new List<QuizLessonSearchRequestModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external lesson search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for lessons.", ex);
            }
        }
    }
}