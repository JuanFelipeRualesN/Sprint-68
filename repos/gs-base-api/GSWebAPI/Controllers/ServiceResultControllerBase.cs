using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.AspNetCore.Mvc;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// Shared plumbing for the controllers of the EF Core modules: session validation through the Users API (same
    /// call as the other controllers) and mapping of <see cref="ServiceResult{T}"/> to HTTP responses.
    /// </summary>
    public abstract class ServiceResultControllerBase : ControllerBase
    {
        /// <summary>Validates the token and permission in the Users API. Returns null if the session is not valid.</summary>
        /// <param name="permissionName">Permission the caller must have for the endpoint.</param>
        /// <returns>The caller, or null when the token is invalid or lacks the permission.</returns>
        protected async Task<RequestUser?> GetRequestUserAsync(string permissionName)
        {
            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, permissionName);
            return RequestUser.FromSession(session);
        }

        /// <summary>
        /// Maps a service result to an HTTP response: on success 200 (or 201 when <paramref name="created"/>) with the data
        /// as body; on failure 400 (Validation), 404, 403, 409 or 503 with a body holding Valid and Messages.
        /// </summary>
        /// <typeparam name="T">Data type of the result.</typeparam>
        /// <param name="result">The service result to map.</param>
        /// <param name="created">True to answer 201 Created instead of 200 OK on success.</param>
        /// <returns>The HTTP response.</returns>
        protected IActionResult FromResult<T>(ServiceResult<T> result, bool created = false)
        {
            if (result.Valid)
            {
                return created ? StatusCode(201, result.Data) : Ok(result.Data);
            }

            var statusCode = result.ErrorKind switch
            {
                ServiceErrorKind.NotFound => 404,
                ServiceErrorKind.Forbidden => 403,
                ServiceErrorKind.Conflict => 409,
                ServiceErrorKind.Unavailable => 503,
                _ => 400,
            };
            return StatusCode(statusCode, new { result.Valid, result.Messages });
        }
    }
}
