using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.FeignAPIs.Models.Lessons.Requests;
using GSAIServicesAPI.Models.Images;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GSAIServicesAPI.FeignAPIs
{
    /// <summary>
    /// Handles image generation Feign API calls. Sends a prompt to the upstream
    /// generation service and returns the generated image(s) as Base64 payloads.
    /// </summary>
    public class ImageGenerationFeignAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        private readonly string _imgEndpoint;
        private readonly IMemoryCache _cache;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ImageGenerationFeignAPI(
            HttpClient httpClient,
            IConfiguration configuration,
            IMemoryCache cache)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

            _baseUrl = configuration["AppSettings:AiImageGenerationApiUrl"]
                ?? throw new InvalidOperationException("Missing config: AiImageGenerationApiUrl");
            _imgEndpoint = configuration["AppSettings:ai_image_text_to_img_endpoint"]
                ?? throw new InvalidOperationException("Missing config: ai_image_text_to_img_endpoint");
            _apiKey = configuration["AppSettings:AiImageGenerationApiKey"]
                ?? throw new InvalidOperationException("Missing config: AiImageGenerationApiKey");
        }

        /// <summary>
        /// Generates an image from the supplied prompt.
        /// </summary>
        /// <param name="generateImageRequest">The prompt, step count and related options.</param>
        /// <param name="cancellationToken">Token used to cancel the outbound call.</param>
        /// <returns>The deserialized response containing the Base64 image(s), or null on failure.</returns>
        public async Task<ImageGenerationResponseModel?> TextToImageGeneration(
            GenerateImageRequest generateImageRequest,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(generateImageRequest);

            var cacheKey = BuildCacheKey(generateImageRequest);
            if (_cache.TryGetValue(cacheKey, out ImageGenerationResponseModel? cached) && cached is not null)
                return cached;

            var fullUrl = _baseUrl + _imgEndpoint;
            using var request = BuildPostRequest(fullUrl, generateImageRequest);
            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content
                .ReadFromJsonAsync<ImageGenerationResponseModel>(JsonOptions, cancellationToken);

            if (result is not null)
                _cache.Set(cacheKey, result, CacheDuration);

            return result;
        }

        private HttpRequestMessage BuildPostRequest<TBody>(string endpoint, TBody body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(body)
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("x-api-key", _apiKey);

            return request;
        }

        private static string BuildCacheKey(GenerateImageRequest request) =>
            $"img-gen:{JsonSerializer.Serialize(request, JsonOptions)}";
    }
}