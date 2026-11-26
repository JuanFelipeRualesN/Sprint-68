using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.FeignAPIs.Utils;
    using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GSAIServicesAPI.FeignAPIs
{
    /// <summary>
    /// This class is responsible for retrieving user details from an external API. It provides a method to validate a session token and check if the user is authorized based on the provided permission name. The class uses HttpClient to send HTTP requests and JsonSerializer to handle JSON serialization and deserialization. It also incorporates caching to improve performance by storing user access details in memory, reducing the need for repeated API calls. The class supports a development mode that allows mocking user details for faster development without calling the external API.
    /// </summary>
    public class UserFeignAPI
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private static readonly IMemoryCache SchoolsCache = new MemoryCache(new MemoryCacheOptions());

        private static readonly bool IsSchoolCacheEnabled = AppConfiguration.Configuration["AppSettings:cache_on_off"] != "0";

        private static readonly int SchoolCacheExpirationHours =
            int.TryParse(AppConfiguration.Configuration["AppSettings:cache_token_expiration"], out var hours)
                ? hours
                : 2;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Evaluates whether the token is still valid and checks if the user is authorized.
        /// </summary>
        /// <param name="token">Session token.</param>
        /// <param name="ipAddress">User IP Address.</param>
        /// <param name="permissionName">User permission name.</param>
        /// <param name="langCode">Language code.</param>
        /// <returns>Returns the response model with user details.</returns>
        public static async Task<ValidationResponseModel> GetUserIdRequestOnOtherAPIAsync(
            string token, string ipAddress, string permissionName, string langCode)
        {
            // If we are developing this API, we can mock the user details to avoid calling the external API for faster development. 
            // Remember to set the "development_mode_session_off" to "0" in the appsettings.json file to enable this feature.
            var developmentModeSessionOff = AppConfiguration.Configuration["AppSettings:development_mode_session_off"];
            if(developmentModeSessionOff == "1")
            {
                return new ValidationResponseModel
                {
                    SessionValid = true,
                    UserId = "1",
                    IsStaff = true,
                    IsDirector = false,
                    Token = token,
                    RoleId = "2",
                    IpAddress = ipAddress,
                    LangCode = langCode,
                    Messages = new List<string> { "Development mode: user details are mocked." }
                };
            }

            if (string.IsNullOrWhiteSpace(token))
                return new ValidationResponseModel { SessionValid = false };

            // Check cache first
            var authenticationCache = new UsersAPICache();
            var cachedResponse = authenticationCache.GetUserAccessDetails(token, permissionName, ipAddress);
            if (cachedResponse != null)
                return cachedResponse;

            var baseUrl = AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"];
            var endpoint = AppConfiguration.Configuration["AppSettings:gus_endpoint_permission_session_api"];

            var requestModel = new PermissionSessionRequestModel
            {
                Token = token,
                IpAddress = ipAddress,
                PermissionName = permissionName,
                LangCode = langCode
            };

            var json = JsonSerializer.Serialize(requestModel, JsonOptions);
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(baseUrl), endpoint));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Token", token);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return new ValidationResponseModel { SessionValid = false };

            var responseStream = await response.Content.ReadAsStreamAsync();
            var modelObject = await JsonSerializer.DeserializeAsync<ValidationResponseModel>(responseStream, JsonOptions);

            if (modelObject is not null)
                authenticationCache.SetAccessDetails(token, permissionName, ipAddress, modelObject);

            return modelObject ?? new ValidationResponseModel { SessionValid = false };
        }

        /// <summary>
        /// Returns the schoold ids of the user, or an empty list when none can be resolved.
        /// </summary>
        public static async Task<IReadOnlyCollection<string>> GetUserSchoolIdsAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Array.Empty<string>();

            // Mock for development mode.
            var developmentModeSessionOff = AppConfiguration.Configuration["AppSettings:development_mode_session_off"];
            if (developmentModeSessionOff == "1")
            {
                return new[] { "104", "2448" };
            }
                

            if (IsSchoolCacheEnabled && SchoolsCache.TryGetValue(BuildCacheKey(userId), out IReadOnlyCollection<string>? cached) && cached is not null)
                return cached;

            var baseUrl = AppConfiguration.Configuration["AppSettings:gus_endpoint_web_service_url"];
            var path = AppConfiguration.Configuration["AppSettings:gus_endpoint_user_schools_api"];

            if (string.IsNullOrWhiteSpace(baseUrl))
                return Array.Empty<string>();

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(baseUrl), path));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Token", token);

                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return Array.Empty<string>();

                // The endpoint answers with an array of school ids
                var stream = await response.Content.ReadAsStreamAsync();
                var schoolIds = await JsonSerializer.DeserializeAsync<List<string>>(stream, JsonOptions);

                var ids = schoolIds is null
                    ? Array.Empty<string>()
                    : schoolIds
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .Distinct()
                        .ToArray();

                if (IsSchoolCacheEnabled)
                    SchoolsCache.Set(BuildCacheKey(userId), (IReadOnlyCollection<string>)ids, TimeSpan.FromHours(SchoolCacheExpirationHours));

                return ids;
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        private static string BuildCacheKey(string userId) => $"schools:{userId}";
    }
}