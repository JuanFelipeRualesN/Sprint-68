using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas;

namespace GSWebAPI.GSWebAPI.Services.Athenas
{
    public class AthenasLessonsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public AthenasLessonsService()
        {
            _apiUrl = AppConfiguration.Configuration["AppSettings:athenas_web_service_url"];
            _apiKey = AppConfiguration.Configuration["AppSettings:AthenasWebServiceApiAccessKey"];

            // Configurar cliente HTTP
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl, UriKind.Absolute)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
        }

        public async Task<AthenasLessonResponseByLessonModel> GetLessonByLessonModels(LessonModelSpecificsRequestModel request, bool filteredByPublished)
        {
            try 
            { 
                // Implementation to call the Athenas API and return the lesson details
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_by_lesson_model"];
                apiEndpoint = apiEndpoint.Replace("{filteredByPublished}", filteredByPublished.ToString());
                var response = await _httpClient.PostAsync(apiEndpoint, new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json"));


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                AthenasLessonResponseByLessonModel lessonByLessonModels = JsonConvert.DeserializeObject<AthenasLessonResponseByLessonModel>(responseContent);
                return lessonByLessonModels;

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
