using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.Models.Images;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GSAIServicesAPI.Services.Images.Providers
{
    /// <summary>
    /// Generates images with the MAI image model deployed in Microsoft Foundry
    /// (<c>POST {AiFoundryImageGenerationApiUrl}{AiFoundryImageGenerationEndpoint}</c>).
    /// </summary>
    /// <remarks>
    /// Missing configuration does not fail startup: the provider reports every call as failed so
    /// the next provider in <c>ImageGeneration:ProviderOrder</c> takes over. The client's
    /// <c>steps</c> option has no equivalent in this API and is ignored.
    /// </remarks>
    public sealed class FoundryImageProvider : IImageGenerationProvider
    {
        public const string FoundryProviderName = "Foundry";

        private const string DefaultFoundryModelName = "MAI-Image-2.6-Flash";
        private const int DefaultImageSizeInPixels = 1024;
        private const int ImagesPerRequest = 1;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly ILogger<FoundryImageProvider> _logger;

        private readonly string? _imageGenerationUrl;
        private readonly string? _foundryApiKey;
        private readonly string _foundryModelName;
        private readonly int _imageWidthInPixels;
        private readonly int _imageHeightInPixels;

        public string ProviderName => FoundryProviderName;

        public FoundryImageProvider(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<FoundryImageProvider> logger)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var foundryBaseUrl = configuration["AppSettings:AiFoundryImageGenerationApiUrl"];
            var imageGenerationEndpoint = configuration["AppSettings:AiFoundryImageGenerationEndpoint"];
            _imageGenerationUrl = string.IsNullOrWhiteSpace(foundryBaseUrl) || string.IsNullOrWhiteSpace(imageGenerationEndpoint)
                ? null
                : foundryBaseUrl.TrimEnd('/') + "/" + imageGenerationEndpoint.TrimStart('/');

            _foundryApiKey = configuration["AppSettings:AiFoundryImageGenerationApiKey"];
            _foundryModelName = configuration["AppSettings:AiFoundryImageGenerationModel"] ?? DefaultFoundryModelName;
            _imageWidthInPixels = configuration.GetValue("AppSettings:AiFoundryImageGenerationWidth", DefaultImageSizeInPixels);
            _imageHeightInPixels = configuration.GetValue("AppSettings:AiFoundryImageGenerationHeight", DefaultImageSizeInPixels);
        }

        public async Task<ImageGenerationProviderResult> GenerateImagesAsync(
            GenerateImageRequest imageRequest,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(imageRequest);

            if (_imageGenerationUrl is null || string.IsNullOrWhiteSpace(_foundryApiKey))
            {
                _logger.LogWarning("Foundry image provider is not configured; skipping.");
                return ImageGenerationProviderResult.Failed();
            }

            var foundryRequestBody = new FoundryImageGenerationRequestBody(
                imageRequest.prompt,
                _imageWidthInPixels,
                _imageHeightInPixels,
                ImagesPerRequest,
                _foundryModelName);

            // Foundry rejects chunked bodies ("Content-Length header is required"), so the body is
            // serialized up front instead of using JsonContent, which streams without a length.
            using var foundryHttpRequest = new HttpRequestMessage(HttpMethod.Post, _imageGenerationUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(foundryRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };
            foundryHttpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foundryHttpRequest.Headers.Add("api-key", _foundryApiKey);

            try
            {
                using var foundryHttpResponse = await _httpClient.SendAsync(foundryHttpRequest, cancellationToken);

                if (foundryHttpResponse.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    _logger.LogWarning("Foundry image provider reached its requests-per-minute limit (429).");
                    return ImageGenerationProviderResult.RateLimited();
                }

                if (!foundryHttpResponse.IsSuccessStatusCode)
                {
                    var foundryErrorBody = await foundryHttpResponse.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("Foundry image provider returned {StatusCode}: {ErrorBody}",
                        (int)foundryHttpResponse.StatusCode, foundryErrorBody);
                    return ImageGenerationProviderResult.Failed();
                }

                var foundryResponseBody = await foundryHttpResponse.Content
                    .ReadFromJsonAsync<FoundryImageGenerationResponseBody>(JsonOptions, cancellationToken);

                var base64Images = foundryResponseBody?.GeneratedImageItems?
                    .Select(generatedImageItem => generatedImageItem.Base64Image)
                    .Where(base64Image => !string.IsNullOrEmpty(base64Image))
                    .Select(base64Image => base64Image!)
                    .ToList();

                if (base64Images is null || base64Images.Count == 0)
                {
                    _logger.LogWarning("Foundry image provider returned no images.");
                    return ImageGenerationProviderResult.Failed();
                }

                return ImageGenerationProviderResult.Success(new ImageGenerationResponseModel { Images = base64Images });
            }
            catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
            {
                // Network errors, malformed JSON and HttpClient timeouts fall through to the next
                // provider; only a cancellation of the supplied token propagates.
                _logger.LogWarning(exception, "Foundry image provider call failed.");
                return ImageGenerationProviderResult.Failed();
            }
        }

        private sealed record FoundryImageGenerationRequestBody(
            [property: JsonPropertyName("prompt")] string Prompt,
            [property: JsonPropertyName("width")] int WidthInPixels,
            [property: JsonPropertyName("height")] int HeightInPixels,
            [property: JsonPropertyName("n")] int NumberOfImages,
            [property: JsonPropertyName("model")] string ModelName);

        private sealed class FoundryImageGenerationResponseBody
        {
            [JsonPropertyName("data")]
            public List<FoundryGeneratedImageItem>? GeneratedImageItems { get; set; }
        }

        private sealed class FoundryGeneratedImageItem
        {
            [JsonPropertyName("b64_json")]
            public string? Base64Image { get; set; }
        }
    }
}
