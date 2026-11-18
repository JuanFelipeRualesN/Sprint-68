using GSAIServicesAPI.Models.DTOs.Quotas;
using GSAIServicesAPI.Services.Quotas;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Configuration for the AI quota system. 
    /// Lets a user read and update the AI availability switch, the quota master switch, 
    /// and the per-resource limits (text tokens, generated images, and any future resource)
    /// </summary>
    [Route("aiservicesapi/quota/settings")]
    public class QuotaConfigController : BaseController
    {
        private readonly QuotaConfigService _quotaConfigService;

        public QuotaConfigController(QuotaConfigService quotaConfigService, IConfiguration config) : base(config)
        {
            _quotaConfigService = quotaConfigService;
        }

        /// <summary>
        /// Returns the current quota configuration
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetConfig()
        {
            var responseSession = await GetAuthenticatedSessionAsync("AIQuotaSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            var config = await _quotaConfigService.GetConfigAsync();
            return Ok(config);
        }

        /// <summary>
        /// Validates and saves the supplied quota configuration.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateConfig([FromBody] QuotaConfigDto request)
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

            var (ok, errorKey) = await _quotaConfigService.SaveConfigAsync(request);

            if (!ok)
            {
                return BadRequest(new { error = GetLocalizedMessage(errorKey!) });
            }

            var saved = await _quotaConfigService.GetConfigAsync();
            return Ok(saved);
        }
    }
}
