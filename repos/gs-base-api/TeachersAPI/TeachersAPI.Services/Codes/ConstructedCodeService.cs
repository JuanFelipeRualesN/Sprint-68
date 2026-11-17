using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Courses;
using TeachersAPI.Domain.Models.Codes;
using TeachersAPI;
namespace TeachersAPI.TeachersAPI.Services.Codes
{
    /// <summary>
    /// Service class to handle code operations.
    /// </summary>
    public class ConstructedCodeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the ConstructedCodeService class.
        /// </summary>
        public ConstructedCodeService()
        {
            _httpClient = new HttpClient();
            _apiUrl = AppConfiguration.Configuration["AppSettings:athenas_web_service_url"];
            _apiKey = AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"];

            // Configurar cliente HTTP
            _httpClient.BaseAddress = new Uri(_apiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
        }

        /// <summary>
        /// Gets the constructed code list.
        /// </summary>
        /// <param name="lessonId">string of lessons ids</param>
        /// <returns>Returns the constructed code list.</returns>
        public async Task<List<ConstructedCodeHandleModel>> ConstructedCodesServiceAsync(string lessonId)
        {
            try
            {
                string apiEndpoint = AppConfiguration.Configuration["AppSettings:athenas_api_get_constructed-codes_lessons"];
                apiEndpoint = apiEndpoint.Replace("{lessonId}", lessonId);
                var response = await _httpClient.GetAsync(apiEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error in API call: {response.StatusCode} - {errorMessage}");
                }

                // Deserialize the response content into a List<ConstructedCodeModel>
                var responseContent = await response.Content.ReadAsStringAsync();
                List<ConstructedCodeHandleModel> constructedCodes = JsonConvert.DeserializeObject<List<ConstructedCodeHandleModel>>(responseContent);
                return constructedCodes;
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