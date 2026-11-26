using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Abstract base controller that provides shared authentication and localization utilities
    /// available to all controllers in the application.
    /// <br/>
    /// Any controller that requires session validation or localized messages should inherit
    /// from this class instead of <see cref="ControllerBase"/> directly.
    /// </summary>
    /// <remarks>
    /// <b>Authentication:</b> exposes <see cref="GetAuthenticatedUserAsync"/> to validate the
    /// session token and resolve the caller's user ID from the external auth service.
    /// <br/><br/>
    /// <b>Localization:</b> exposes <see cref="GetLocalizedMessage"/> to resolve user-facing
    /// strings from <c>appsettings.json</c> based on the language detected in the request.
    /// Supported ISO codes are <c>en</c> (English) and <c>es</c> (Spanish); defaults to <c>en</c>.
    /// </remarks>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes the base controller with the application configuration.
        /// </summary>
        /// <param name="config">
        /// The application configuration used to resolve localized user-facing messages
        /// from the <c>Messages</c> section in <c>appsettings.json</c>.
        /// </param>
        protected BaseController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Validates the session token from the incoming request and resolves the authenticated user ID
        /// by calling the external auth service.
        /// </summary>
        /// <param name="module">
        /// The application module name forwarded to the auth service for audit and access-control purposes
        /// (e.g. <c>"AIDocuments"</c>, <c>"AITools"</c>).
        /// </param>
        /// <returns>
        /// A tuple where <c>isValid</c> is <c>true</c> and <c>userId</c> is populated when the session
        /// is active; otherwise <c>isValid</c> is <c>false</c> and <c>userId</c> is <c>null</c>.
        /// </returns>
        protected async Task<(bool isValid, string? userId)> GetAuthenticatedUserAsync(string module)
        {
            var responseSession = await UserFeignAPI.GetUserIdRequestOnOtherAPIAsync(
                RequestHandler.GetToken(HttpContext),
                IPAddressDetector.GetClientIpAddress(HttpContext),
                module,
                RequestHandler.GetLanguage(HttpContext));

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                return (false, null);

            return (true, responseSession.UserId);
        }

        /// <summary>
        /// Validates the session token and returns the full session model from the auth service
        /// </summary>
        /// <returns>
        /// The session response
        /// </returns>
        protected async Task<ValidationResponseModel?> GetAuthenticatedSessionAsync(string permissionName)
        {
            var responseSession = await UserFeignAPI.GetUserIdRequestOnOtherAPIAsync(
                RequestHandler.GetToken(HttpContext),
                IPAddressDetector.GetClientIpAddress(HttpContext),
                permissionName,
                RequestHandler.GetLanguage(HttpContext));

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return null;
            }

            return responseSession;
        }

        /// <summary>
        /// Resolves a localized message from <c>appsettings.json</c> using the language detected
        /// from the current HTTP request. Falls back to English when the detected language has
        /// no corresponding entry, or when the message key does not exist.
        /// </summary>
        /// <param name="messageKey">
        /// The key under the <c>Messages</c> section in <c>appsettings.json</c>
        /// (e.g. <c>"DocumentNotFound"</c>).
        /// </param>
        /// <returns>
        /// The localized string for the detected language, the English fallback if the language
        /// is unsupported, or the raw key itself if neither entry exists.
        /// </returns>
        protected string GetLocalizedMessage(string messageKey)
        {
            var language = RequestHandler.GetLanguage(HttpContext);
            var isoCode = language?.ToLowerInvariant() == "es" ? "es" : "en";

            return _config[$"Messages:{messageKey}:{isoCode}"]
                ?? _config[$"Messages:{messageKey}:en"]
                ?? messageKey;
        }
    }
}