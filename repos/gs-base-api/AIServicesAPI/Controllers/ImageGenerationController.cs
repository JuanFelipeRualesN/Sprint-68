using GSAIServicesAPI.Filters;
using GSAIServicesAPI.Models.Images;
using GSAIServicesAPI.Models.Quotas;
using GSAIServicesAPI.Services.Images;
using GSAIServicesAPI.Services.Quotas;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Handles AI image generation requests.
    /// All endpoints require a valid session token scoped to the authenticated user.
    /// </summary>
    /// <remarks>
    /// The upstream generation service returns every image as a Base64 payload in a single
    /// response, so this endpoint replies with one JSON document rather than streaming.
    /// Inherits session validation and localization from <see cref="BaseController"/>.
    /// </remarks>
    [ApiController]
    [Route("aiservicesapi/[controller]")]
    public class ImageGenerateController : BaseController
    {
        private readonly IImageGenerationService _imageGenerationService;
        private readonly QuotaService _quotaService;

        /// <summary>
        /// The module name forwarded to the external auth service on every session validation call.
        /// </summary>
        private const string Module = "AIGenerateImages";

        /// <summary>
        /// Amount held before the request runs
        /// </summary>
        private const long AssumedImageCount = 1;

        /// <summary>
        /// Initializes the controller for image generation.
        /// </summary>
        /// <param name="config">
        /// The application configuration passed to <see cref="BaseController"/> to support
        /// localized message resolution.
        /// </param>
        /// <param name="imageGenerationService">Client for the upstream generation service.</param>
        /// <param name="quotaService">Meters and enforces the caller's image allowance.</param>
        public ImageGenerateController(IConfiguration config, IImageGenerationService imageGenerationService, QuotaService quotaService) : base(config)
        {
            _imageGenerationService = imageGenerationService
                ?? throw new ArgumentNullException(nameof(imageGenerationService));
            _quotaService = quotaService
                ?? throw new ArgumentNullException(nameof(quotaService));
        }

        /// <summary>
        /// Generates one or more images from the supplied prompt and returns them as Base64.
        /// Rejects the request when the user has no image generation credits remaining.
        /// </summary>
        /// <param name="request">The request containing the prompt and image quantity.</param>
        /// <param name="cancellationToken">Token used to cancel the outbound call.</param>
        /// <returns>The generated images, Base64 encoded.</returns>
        [HttpPost]
        [RequiresAiEnabled]
        [ProducesResponseType(typeof(GeneratedImagesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> Post([FromBody] GenerateImageRequest request, CancellationToken cancellationToken)
        {
            // The full session is needed rather than just the user id, because quota
            // resolution takes the caller's role into account.
            var session = await GetAuthenticatedSessionAsync(Module);
            if (session == null)
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });

            if (request is null || string.IsNullOrWhiteSpace(request.prompt))
                return BadRequest(new { error = GetLocalizedMessage("MissingModelOrPrompt") });

            // Hold a credit before calling the provider so an out-of-credit user is rejected
            // without ever incurring generation cost.
            var reservation = await _quotaService.ReserveAsync(
                session.UserId,
                session.RoleId,
                session.Token,
                QuotaResourceType.ImageCount,
                AssumedImageCount,
                Module);

            if (!reservation.Allowed)
                return StatusCode(StatusCodes.Status429TooManyRequests, new
                {
                    error = GetLocalizedMessage(reservation.DenialMessageKey ?? "QuotaExceeded"),
                    remaining = reservation.RemainingAmount,
                    resetsAt = reservation.PeriodEnd
                });

            try
            {
                var result = await _imageGenerationService
                    .GenerateAndStoreAsync(request, cancellationToken);

                // Nothing was generated, return the hold.
                if (!result.UpstreamSucceeded)
                {
                    await _quotaService.ReleaseAsync(reservation);
                    return StatusCode(
                        StatusCodes.Status502BadGateway,
                        new { error = GetLocalizedMessage("ImageGenerationFailed") });
                }

                // Storage failed but the images were generated,
                // so the caller is still charged
                if (result.StorageFailed)
                {
                    await _quotaService.CommitAsync(reservation, result.GeneratedCount);
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new { error = GetLocalizedMessage("ImageStorageFailed") });
                }

                await _quotaService.CommitAsync(reservation, result.GeneratedCount);
                return Ok(new GeneratedImagesResponse { ImageUrls = result.ImageUrls.ToList() });
            }
            catch (OperationCanceledException)
            {
                // Client disconnected before a result came back, release the hold.
                await _quotaService.ReleaseAsync(reservation);
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                await _quotaService.ReleaseAsync(reservation);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { error = ex.Message });
            }
        }
    }
}