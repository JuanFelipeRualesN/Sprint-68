using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.Models.Images;
using GSAIServicesAPI.Services.Images.Providers;

namespace GSAIServicesAPI.Services.Images
{
    /// <summary>
    /// Default <see cref="IImageGenerationService"/> that tries each registered
    /// <see cref="IImageGenerationProvider"/> in the order given by <c>ImageGeneration:ProviderOrder</c>
    /// and persists the first successful result via <see cref="IImageBlobStorage"/>.
    /// </summary>
    /// <remarks>
    /// A provider is skipped and the next one tried when it fails, answers 429, returns no images
    /// or does not answer within <c>ImageGeneration:ProviderTimeoutSeconds</c>.
    /// <para>
    /// Failure handling is asymmetric by design. A failure to generate is reported through the
    /// returned result, whereas a failure to persist is logged and reported as a generated-but-not-
    /// stored result rather than thrown — with the sole exception of cancellation, which propagates.
    /// The caller therefore never learns why storage failed beyond what reaches the log.
    /// </para>
    /// </remarks>
    public sealed class ImageGenerationService : IImageGenerationService
    {
        /// <summary>Order used when <c>ImageGeneration:ProviderOrder</c> is not configured, which keeps the previous behaviour.</summary>
        private static readonly string[] DefaultProviderOrder = { StableDiffusionImageProvider.StableDiffusionProviderName };

        private const int DefaultProviderTimeoutInSeconds = 30;

        /// <summary>Providers to try, already sorted by the configured order.</summary>
        private readonly IReadOnlyList<IImageGenerationProvider> _imageProvidersInFallbackOrder;

        /// <summary>Backing store that receives the generated Base64 payloads.</summary>
        private readonly IImageBlobStorage _blobStorage;

        /// <summary>
        /// Signer used to generate signed URLs for the uploaded blobs.
        /// </summary>
        private readonly IBlobUrlSigner _signer;

        /// <summary>Logger used to record fallbacks and storage failures that are not surfaced as exceptions.</summary>
        private readonly ILogger<ImageGenerationService> _logger;

        /// <summary>Maximum time a single provider gets before the next one is tried.</summary>
        private readonly TimeSpan _providerTimeout;

        /// <summary>
        /// Initialises the service with the generation providers, blob store and logger it delegates to.
        /// </summary>
        /// <param name="registeredImageProviders">Every registered provider; only those listed in the configured order are used.</param>
        /// <param name="blobStorage">Store that the generated payloads are written to.</param>
        /// <param name="signer">The signer used to generate signed URLs for the uploaded blobs.</param>
        /// <param name="configuration">Source of the <c>ImageGeneration</c> section.</param>
        /// <param name="logger">Logger for fallbacks and storage failures.</param>
        public ImageGenerationService(
            IEnumerable<IImageGenerationProvider> registeredImageProviders,
            IImageBlobStorage blobStorage,
            IBlobUrlSigner signer,
            IConfiguration configuration,
            ILogger<ImageGenerationService> logger)
        {
            _blobStorage = blobStorage;
            _signer = signer;
            _logger = logger;

            var configuredProviderOrder = configuration.GetSection("ImageGeneration:ProviderOrder").Get<string[]>();
            if (configuredProviderOrder is null || configuredProviderOrder.Length == 0)
                configuredProviderOrder = DefaultProviderOrder;

            var imageProvidersByName = registeredImageProviders
                .ToDictionary(imageProvider => imageProvider.ProviderName, StringComparer.OrdinalIgnoreCase);

            _imageProvidersInFallbackOrder = configuredProviderOrder
                .Where(imageProvidersByName.ContainsKey)
                .Select(providerName => imageProvidersByName[providerName])
                .ToList();

            _providerTimeout = TimeSpan.FromSeconds(
                configuration.GetValue("ImageGeneration:ProviderTimeoutSeconds", DefaultProviderTimeoutInSeconds));
        }

        /// <summary>
        /// Requests generation for <paramref name="request"/> and uploads each returned payload,
        /// reporting generation and storage outcomes separately in the result.
        /// </summary>
        /// <param name="request">Describes what to generate, including the prompt and image count.</param>
        /// <param name="cancellationToken">Token used to abandon generation and the subsequent uploads.</param>
        /// <returns>
        /// A <see cref="GeneratedImagesResult"/> describing one of three outcomes: no provider
        /// returned images; generation and storage both succeeded, in which case the URIs correspond
        /// positionally to the payloads returned by the provider; or generation succeeded and storage
        /// failed, in which case no URIs are reported even if some uploads had already completed.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was signalled during generation, before any images
        /// came back. Cancellation during the upload phase does not escape: the provider has already
        /// generated the images, so it is reported as a storage failure with the count of images generated.
        /// </exception>
        /// <remarks>
        /// The call is not atomic and not idempotent. When storage fails part-way, payloads already
        /// written to the store are neither removed nor reported, leaving blobs that nothing
        /// references; the same applies when cancellation interrupts an in-flight batch. Retrying
        /// consumes provider quota again and produces a fresh set of images.
        /// </remarks>
        public async Task<GeneratedImagesResult> GenerateAndStoreAsync(
         GenerateImageRequest request,
         CancellationToken cancellationToken = default)
        {
            var generatedImages = await GenerateWithFirstAvailableProviderAsync(request, cancellationToken);

            if (generatedImages is null)
                return new GeneratedImagesResult(false, Array.Empty<string>(), false);

            try
            {
                var blobNames = await _blobStorage.UploadBase64Async(generatedImages.Images, cancellationToken);
                var urls = blobNames.Select(_signer.ToSignedUrl).ToArray();
                return new GeneratedImagesResult(true, urls, false, generatedImages.Images.Count);
            }
            catch (Exception ex)
            {
                // Generation already completed and was billed by the provider, so a storage
                // failure, including a cancellation that lands while the upload is in flight
                // still counts the images as generated
                _logger.LogError(ex, "Generation succeeded but persisting the images failed.");
                return new GeneratedImagesResult(true, Array.Empty<string>(), true, generatedImages.Images.Count);
            }
        }

        /// <summary>
        /// Tries each provider in the configured order and returns the images of the first one that
        /// succeeds, or <see langword="null"/> when every provider failed.
        /// </summary>
        private async Task<ImageGenerationResponseModel?> GenerateWithFirstAvailableProviderAsync(
            GenerateImageRequest imageRequest,
            CancellationToken requestCancellationToken)
        {
            foreach (var imageProvider in _imageProvidersInFallbackOrder)
            {
                using var providerTimeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(requestCancellationToken);
                providerTimeoutTokenSource.CancelAfter(_providerTimeout);

                ImageGenerationProviderResult providerResult;
                try
                {
                    providerResult = await imageProvider.GenerateImagesAsync(imageRequest, providerTimeoutTokenSource.Token);
                }
                catch (OperationCanceledException) when (!requestCancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Image provider {ProviderName} timed out after {TimeoutInSeconds}s; trying the next one.",
                        imageProvider.ProviderName, _providerTimeout.TotalSeconds);
                    continue;
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    _logger.LogWarning(exception, "Image provider {ProviderName} threw; trying the next one.",
                        imageProvider.ProviderName);
                    continue;
                }

                if (providerResult.HasImages)
                {
                    _logger.LogInformation("Image generated by provider {ProviderName}.", imageProvider.ProviderName);
                    return providerResult.GeneratedImages;
                }

                _logger.LogWarning("Image provider {ProviderName} failed (rate limited: {WasRateLimited}); trying the next one.",
                    imageProvider.ProviderName, providerResult.WasRateLimited);
            }

            _logger.LogError("All image providers failed.");
            return null;
        }
    }
}
