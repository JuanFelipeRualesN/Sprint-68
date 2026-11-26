using GSAIServicesAPI.Models.Images;

namespace GSAIServicesAPI.Services.Images
{
    /// <summary>
    /// Generates images from a caller-supplied request and stores them, returning references to the
    /// stored results rather than the image bytes.
    /// </summary>
    /// <remarks>
    /// Implementations must be safe for concurrent use, since a single instance is expected to serve
    /// requests for the lifetime of the application. Generation typically involves a remote model
    /// provider, so calls may be long-running and are subject to that provider's quota and rate
    /// limits; callers should always pass a cancellation token bound to the request lifetime.
    /// </remarks>
    public interface IImageGenerationService
    {
        /// <summary>
        /// Generates the images described by <paramref name="request"/> and persists each one,
        /// returning the stored locations.
        /// </summary>
        /// <param name="request">Describes what to generate, including the prompt and image count.</param>
        /// <param name="cancellationToken">Token used to abandon generation and storage.</param>
        /// <returns>
        /// A <see cref="GeneratedImagesResult"/> holding a reference to each stored image. Never
        /// <see langword="null"/> on successful completion.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The generation provider could not be reached or returned a failure status. Callers can
        /// distinguish a transient fault worth retrying from a rejected prompt only by inspecting the
        /// status code.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was signalled before the call completed.
        /// </exception>
        /// <remarks>
        /// Generation and storage are a single unit from the caller's point of view but are not
        /// atomic: if storage fails part-way, images that were already stored are not removed and
        /// their locations are not reported. Implementations are not idempotent — repeating a call
        /// with the same request produces new images and new locations, and consumes provider quota
        /// again, so retries after an ambiguous failure may leave orphaned images.
        /// </remarks>
        Task<GeneratedImagesResult> GenerateAndStoreAsync(
            GenerateImageRequest request,
            CancellationToken cancellationToken = default);
    }
}