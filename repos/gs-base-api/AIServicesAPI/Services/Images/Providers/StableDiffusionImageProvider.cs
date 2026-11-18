using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Models.Images;

namespace GSAIServicesAPI.Services.Images.Providers
{
    /// <summary>
    /// Generates images with the Stable Diffusion endpoint (<c>sdapi/v1/txt2img</c>) through the
    /// existing <see cref="ImageGenerationFeignAPI"/>.
    /// </summary>
    public sealed class StableDiffusionImageProvider : IImageGenerationProvider
    {
        public const string StableDiffusionProviderName = "StableDiffusion";

        private readonly ImageGenerationFeignAPI _stableDiffusionFeignApi;
        private readonly ILogger<StableDiffusionImageProvider> _logger;

        public string ProviderName => StableDiffusionProviderName;

        public StableDiffusionImageProvider(
            ImageGenerationFeignAPI stableDiffusionFeignApi,
            ILogger<StableDiffusionImageProvider> logger)
        {
            _stableDiffusionFeignApi = stableDiffusionFeignApi ?? throw new ArgumentNullException(nameof(stableDiffusionFeignApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ImageGenerationProviderResult> GenerateImagesAsync(
            GenerateImageRequest imageRequest,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var stableDiffusionResponse = await _stableDiffusionFeignApi.TextToImageGeneration(imageRequest, cancellationToken);

                if (stableDiffusionResponse is null || stableDiffusionResponse.Images.Count == 0)
                {
                    _logger.LogWarning("Stable Diffusion image provider returned no images.");
                    return ImageGenerationProviderResult.Failed();
                }

                return ImageGenerationProviderResult.Success(stableDiffusionResponse);
            }
            catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning(exception, "Stable Diffusion image provider call failed.");
                return ImageGenerationProviderResult.Failed();
            }
        }
    }
}
