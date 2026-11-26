using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.FeignAPIs.Models.Lessons;
using GSAIServicesAPI.FeignAPIs.Models.Lessons.Requests;
using GSAIServicesAPI.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace GSAIServicesAPI.FeignAPIs
{
    /// <summary>
    /// Handles Athenas Feign API calls. Provides methods to interact with the Athenas API,
    /// allowing retrieval of lessons and codes based on various filters and parameters.
    /// Supports authentication via API key and allows specifying language codes for localization.
    /// </summary>
    public class AthenasFeignAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Initializes a new instance of <see cref="AthenasFeignAPI"/>.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to send requests.</param>
        /// <param name="configuration">Application configuration for retrieving API settings.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if required configuration values are missing or empty.
        /// </exception>
        public AthenasFeignAPI(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _baseUrl = configuration["AppSettings:athenas_web_service_url"]
                ?? throw new InvalidOperationException("Missing config: athenas_web_service_url");

            _apiKey = configuration["AppSettings:AthenasWebServiceApiAccessKey"]
                ?? throw new InvalidOperationException("Missing config: AthenasWebServiceApiAccessKey");

            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Gets lessons from the Athenas API applying advanced filters.
        /// </summary>
        /// <param name="lessonAdvanceRequestModel">The lesson request with filters to apply.</param>
        /// <param name="langCode">Language code for the request headers.</param>
        /// <returns>The lessons response, or an empty model if the request fails.</returns>
        public async Task<LessonsFeignResponseModel> GetLessonsAdvanceAsync(
            LessonAdvanceRequestModel lessonAdvanceRequestModel, string langCode = "en")
        {
            ArgumentNullException.ThrowIfNull(lessonAdvanceRequestModel);

            const string endpoint = "api/lessons/advance/";

            using var request = BuildPostRequest(endpoint, lessonAdvanceRequestModel, langCode);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new LessonsFeignResponseModel();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<LessonsFeignResponseModel>(responseStream, JsonOptions)
                ?? new LessonsFeignResponseModel();
        }

        /// <summary>
        /// Gets codes from the Athenas API by filters.
        /// Codes are components used to construct standards, expectations, etc.
        /// </summary>
        /// <param name="codeHandleModel">The code details to filter by.</param>
        /// <param name="fromPage">The page index to start from (zero-based).</param>
        /// <param name="limiter">The number of items per page.</param>
        /// <param name="uniqueExpectations">If true, returns only unique expectations.</param>
        /// <param name="langCode">Language code for the request headers.</param>
        /// <returns>A paginated list of codes, or an empty page if the request fails.</returns>
        public async Task<PaginationModel<CodeHandleModel>> GetCodesAsync(
            CodeHandleModel codeHandleModel,
            int fromPage, int limiter, bool uniqueExpectations = false, string langCode = "en")
        {
            ArgumentNullException.ThrowIfNull(codeHandleModel);

            var endpoint = $"api/codes/query?fromPage={fromPage}&limiter={limiter}&uniqueExpectations={uniqueExpectations}";

            using var request = BuildPostRequest(endpoint, codeHandleModel, langCode);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new PaginationModel<CodeHandleModel>(0, 0, new List<CodeHandleModel>());

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<PaginationModel<CodeHandleModel>>(responseStream, JsonOptions)
                ?? new PaginationModel<CodeHandleModel>(0, 0, new List<CodeHandleModel>());
        }

        /// <summary>
        /// To get the subjects from the Athenas API. Subjects are used to categorize lessons and codes.
        /// Results are cached per language code.
        /// </summary>
        /// <param name="langCode">Language code for the request headers.</param>
        /// <returns>A list of subjects, or an empty list if the request fails.</returns>
        public Task<List<SubjectModel>> GetSubjects(string langCode = "en")
            => GetCachedList<SubjectModel>("subjects", "api/subjects", langCode);

        /// <summary>
        /// To get the levels (grades) from the Athenas API. Levels are used to categorize lessons and codes.
        /// Results are cached per language code.
        /// </summary>
        /// <param name="langCode">Language code for the request headers.</param>
        /// <returns>A list of levels, or an empty list if the request fails.</returns>
        public Task<List<LevelModel>> GetLevels(string langCode = "en")
            => GetCachedList<LevelModel>("levels", "api/grades", langCode);

        /// <summary>
        /// To get the curriculum units of a subject and level from the Athenas API.
        /// Results are cached per subject, level and language code.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="langCode">Language code for the request headers.</param>
        /// <returns>A list of units, or an empty list if the request fails.</returns>
        public Task<List<UnitModel>> GetUnits(string subjectCode, string levelCode, string langCode = "en")
            => GetCachedList<UnitModel>(
                $"units_{subjectCode}_{levelCode}",
                $"api/unit/grade-unit/{Uri.EscapeDataString(levelCode)}/{Uri.EscapeDataString(subjectCode)}",
                langCode);

        /// <summary>
        /// Core helper to build a base <see cref="HttpRequestMessage"/> with standard headers.
        /// </summary>
        private HttpRequestMessage BuildBaseRequest(HttpMethod method, string endpoint, string langCode)
        {
            var request = new HttpRequestMessage(method, new Uri(new Uri(_baseUrl), endpoint));

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Accept-Language", langCode);
            request.Headers.Add("X-API-KEY", _apiKey);

            return request;
        }

        /// <summary>
        /// Builds a POST <see cref="HttpRequestMessage"/> with the standard headers and serialized body.
        /// </summary>
        private HttpRequestMessage BuildPostRequest<T>(string endpoint, T body, string langCode)
        {
            var request = BuildBaseRequest(HttpMethod.Post, endpoint, langCode);

            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return request;
        }

        /// <summary>
        /// Builds a GET <see cref="HttpRequestMessage"/> with the standard headers.
        /// </summary>
        private HttpRequestMessage BuildGetRequest(string endpoint, string langCode)
        {
            return BuildBaseRequest(HttpMethod.Get, endpoint, langCode);
        }

        /// <summary>
        /// To get cached lists of data from the Athenas API. This method checks the cache first and fetches from the API if not present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyPrefix">The prefix for the cache key.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="langCode">The language code.</param>
        /// <returns>Returns the cached list of items.</returns>
        private async Task<List<T>> GetCachedList<T>(string cacheKeyPrefix, string endpoint, string langCode)
        {
            // Concurrent requests for an expired key share one call to Athenas
            var result = await _cache.GetOrLoadOnceAsync<List<T>?>($"{cacheKeyPrefix}_{langCode}", async () =>
            {
                using var request = BuildGetRequest(endpoint, langCode);
                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return null;

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<T>>(responseStream, JsonOptions) ?? new List<T>();
            }, CacheDuration);

            return result ?? new List<T>();
        }
    }
}