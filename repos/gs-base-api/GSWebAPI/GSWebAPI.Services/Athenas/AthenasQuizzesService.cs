using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.Athenas
{
    public class AthenasQuizzesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public AthenasQuizzesService()
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

        public async Task<AthenasResponseLessonModel> GetQuizByQuizId(string quizId, bool randomResponses)
        {
            try
            {
                // Implementation to call the Athenas API and return the lesson details
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_quiz_by_quizid"];
                apiEndpoint = apiEndpoint.Replace("{quizId}", quizId);
                apiEndpoint = apiEndpoint.Replace("{randomResponses}", randomResponses.ToString());
                var response = await _httpClient.GetAsync(apiEndpoint);


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                AthenasResponseLessonModel getQuizByQuizId = JsonConvert.DeserializeObject<AthenasResponseLessonModel>(responseContent);
                return getQuizByQuizId;

            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the external quiz search API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for quizzes.", ex);
            }

        }
    }
}
