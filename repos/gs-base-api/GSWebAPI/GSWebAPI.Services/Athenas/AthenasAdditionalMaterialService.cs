
using Newtonsoft.Json;
using System.Net.Http.Headers;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;

namespace GSWebAPI.GSWebAPI.Services.Athenas
{
    public class AthenasAdditionalMaterialService
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public AthenasAdditionalMaterialService()
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

        public async Task<AthenasAdditionalMaterialResponseModel> GetAdditionalMaterialService(AdditionalMaterialRequestModel request)
        {
            try
            {
                // Implementation to call the Athenas API and return the lesson details
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_additional_material"];
                var response = await _httpClient.PostAsync(apiEndpoint, new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json"));


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                AthenasAdditionalMaterialResponseModel additionalMaterialResponse = JsonConvert.DeserializeObject<AthenasAdditionalMaterialResponseModel>(responseContent);
                return additionalMaterialResponse;

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


        public async Task<AthenasAdditionalMaterialResponseModel> GetLessonAditionalMaterialService(string lessonId, int fromPage, int limit)
        {
            try
            {
                // Implementation to call the Athenas API and return the lesson details
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_lesson_additional_material"];
                apiEndpoint = apiEndpoint.Replace("{lessonId}", lessonId)
                                         .Replace("{fromPage}", fromPage.ToString())
                                         .Replace("{limit}", limit.ToString());
                var response = await _httpClient. GetAsync(apiEndpoint);


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                AthenasAdditionalMaterialResponseModel lessonAdditionalMaterialResponse = JsonConvert.DeserializeObject<AthenasAdditionalMaterialResponseModel>(responseContent);
                return lessonAdditionalMaterialResponse;

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

        public async Task<List<AdditionalMaterialModel>> GetAdditionalMaterialByAddtionalMaterialIdService(string additionalMaterialId)
        {
            try
            {
                // Implementation to call the Athenas API and return the lesson details
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_additional_material_by_additional_material_id"];
                apiEndpoint = apiEndpoint.Replace("{additionalMaterialId}", additionalMaterialId);
                var response = await _httpClient.GetAsync(apiEndpoint);


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                List<AdditionalMaterialModel> lessonAdditionalMaterialResponse = JsonConvert.DeserializeObject<List<AdditionalMaterialModel>>(responseContent);
                return lessonAdditionalMaterialResponse;

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

        public async Task<string> PrintStudentLessonsService(string lessonId, string quizId, string outPutFormat = "pdf")
        {
            try
            {
                // Implementation to call the Athenas API and return the lesson details
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_offline_material"];
                apiEndpoint = apiEndpoint.Replace("{lessonId}", lessonId)
                                         .Replace("{quizId}", quizId)
                                         .Replace("{outPutFormat}", outPutFormat);
                var response = await _httpClient.GetAsync(apiEndpoint);


                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                string offlineMaterialResponse = JsonConvert.DeserializeObject<string>(responseContent);
                return offlineMaterialResponse;

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
