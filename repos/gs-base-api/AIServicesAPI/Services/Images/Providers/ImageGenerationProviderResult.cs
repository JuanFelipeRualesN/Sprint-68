using GSAIServicesAPI.FeignAPIs.Models;

namespace GSAIServicesAPI.Services.Images.Providers
{
    /// <summary>
    /// Outcome of a single <see cref="IImageGenerationProvider"/> call.
    /// </summary>
    /// <param name="GeneratedImages">The generated Base64 images, or <see langword="null"/> when the call failed.</param>
    /// <param name="WasRateLimited">The provider answered 429 (requests-per-minute limit reached).</param>
    public sealed record ImageGenerationProviderResult(
        ImageGenerationResponseModel? GeneratedImages,
        bool WasRateLimited = false)
    {
        /// <summary>True when the provider returned at least one image.</summary>
        public bool HasImages => GeneratedImages is { Images.Count: > 0 };

        /// <summary>A failed call that should fall through to the next provider.</summary>
        public static ImageGenerationProviderResult Failed() => new(GeneratedImages: null);

        /// <summary>A 429 response that should fall through to the next provider.</summary>
        public static ImageGenerationProviderResult RateLimited() => new(GeneratedImages: null, WasRateLimited: true);

        /// <summary>A successful call.</summary>
        public static ImageGenerationProviderResult Success(ImageGenerationResponseModel generatedImages) => new(generatedImages);
    }
}
