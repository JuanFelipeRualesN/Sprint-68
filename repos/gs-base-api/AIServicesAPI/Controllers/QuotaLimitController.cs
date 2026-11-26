using GSAIServicesAPI.Models.DTOs.Quotas;
using GSAIServicesAPI.Services.Quotas;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Management of quota limits that override the global default for a specific role or user. 
    /// The global default itself is managed through /api/quota/settings
    /// <br></br>
    /// The resolution order applied at request time is
    /// User -> School -> Role -> Global: the most specific matching limit wins,
    /// and removing an override makes its target inherit from the level above it again.
    /// A user can belong to several schools, so when more than one has a limit the most generous applies
    /// </summary>
    [Route("aiservicesapi/quota/limits")]
    public class QuotaLimitController : BaseController
    {
        private readonly QuotaConfigService _quotaConfigService;

        public QuotaLimitController(QuotaConfigService quotaConfigService, IConfiguration config) : base(config)
        {
            _quotaConfigService = quotaConfigService;
        }

        /// <summary>
        /// Returns the configured role, user and school overrides
        /// </summary>
        /// <param name="scope">Optional scope filter (Role, User, School).</param>
        /// <param name="scopeId">Optional target filter (a specific role, user or school id)</param>
        /// <param name="currentPage">The current page index</param>
        /// <param name="limiter">Rows per page</param>
        [HttpGet]
        public async Task<IActionResult> GetLimits([FromQuery] string? scope, [FromQuery] string? scopeId, [FromQuery] int currentPage = 0, [FromQuery] int limiter = 10)
        {
            var responseSession = await GetAuthenticatedSessionAsync("AIQuotaSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            var limits = await _quotaConfigService.GetScopedLimitsAsync(scope, scopeId, currentPage, limiter);
            return Ok(limits);
        }

        /// <summary>
        /// Returns one page of the users who have consumed a resource in the period running now,
        /// each with the limit governing them and the scope it came from.
        /// <br></br>
        /// Complements the limits list: a School or Role limit keeps no counter of its own, so this
        /// is how an admin sees who is spending under one. Ordered by consumption, highest first.
        /// </summary>
        /// <param name="resourceTypeCode">Optional resource filter; omit for all.</param>
        /// <param name="scope">Optional filter by the governing limit's scope (Role, School, User, Global).</param>
        /// <param name="scopeId">Optional filter by the governing limit's target</param>
        /// <param name="userId">Optional filter by the consuming user</param>
        /// <param name="currentPage">The current page index.</param>
        /// <param name="limiter">Rows per page.</param>
        [HttpGet("usage")]
        public async Task<IActionResult> GetUserUsage([FromQuery] string? resourceTypeCode, [FromQuery] string? scope,
            [FromQuery] string? scopeId, [FromQuery] string? userId, [FromQuery] int currentPage = 0, [FromQuery] int limiter = 10)
        {
            var responseSession = await GetAuthenticatedSessionAsync("AIQuotaSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            var usage = await _quotaConfigService.GetUserUsageAsync(resourceTypeCode, scope, scopeId, userId, currentPage, limiter);

            return Ok(usage);
        }

        /// <summary>
        /// Creates or updates the override for one resource and target.
        /// <br></br>
        /// A single upsert rather than separate create and update routes, because the
        /// identity of an override is the (resource, scope, target) triple.
        /// Saving the same target twice adjusts the existing row
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveLimit([FromBody] QuotaScopedLimitDto request)
        {
            var responseSession = await GetAuthenticatedSessionAsync("AIQuotaSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            if (request == null)
            {
                return BadRequest(new { error = GetLocalizedMessage("InvalidSettings") });
            }

            var (ok, errorKey, saved) = await _quotaConfigService.SaveScopedLimitAsync(request);

            if (!ok)
            {
                return BadRequest(new { error = GetLocalizedMessage(errorKey!) });
            }

            return Ok(saved);
        }

        /// <summary>
        /// Removes an override, so its target goes back to inheriting the limit from the
        /// level above it.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLimit(int id)
        {
            var responseSession = await GetAuthenticatedSessionAsync("AIQuotaSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            var (ok, errorKey) = await _quotaConfigService.DeleteScopedLimitAsync(id);

            if (!ok)
            {
                return BadRequest(new { error = GetLocalizedMessage(errorKey!) });
            }

            return Ok(new { deleted = true });
        }
    }
}
