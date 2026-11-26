using GSAIServicesAPI.Models.Images;

namespace GSAIServicesAPI.Services.Images.Providers
{
    /// <summary>
    /// A single text-to-image model that <see cref="ImageGenerationService"/> can try, in the order
    /// given by <c>ImageGeneration:ProviderOrder</c>. Adding a backup model means implementing this
    /// interface, registering it in the container and listing its <see cref="ProviderName"/> in that order.
    /// </summary>
    /// <remarks>
    /// Implementations must not throw for provider-side failures; they report them through
    /// <see cref="ImageGenerationProviderResult"/> so the caller can move on to the next provider.
    /// Only cancellation of the supplied token is expected to escape.
    /// </remarks>
    public interface IImageGenerationProvider
    {
        /// <summary>Identifier matched against <c>ImageGeneration:ProviderOrder</c>.</summary>
        string ProviderName { get; }

        /// <summary>
        /// Generates the images described by <paramref name="imageRequest"/>.
        /// </summary>
        /// <param name="imageRequest">The prompt and options sent by the client.</param>
        /// <param name="cancellationToken">Token used to cancel the outbound call.</param>
        /// <returns>The outcome of the call; never <see langword="null"/>.</returns>
        Task<ImageGenerationProviderResult> GenerateImagesAsync(
            GenerateImageRequest imageRequest,
            CancellationToken cancellationToken = default);
    }
}
