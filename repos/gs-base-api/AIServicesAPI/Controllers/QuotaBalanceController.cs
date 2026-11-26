using GSAIServicesAPI.Services.Quotas;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Returns the user's remaining AI allowance for display
    /// </summary>
    [Route("aiservicesapi/quotas/me")]
    public class QuotaBalanceController : BaseController
    {
        private readonly QuotaService _quotaService;

        public QuotaBalanceController(QuotaService quotaService, IConfiguration config) : base(config)
        {
            _quotaService = quotaService;
        }

        /// <summary>
        /// Returns the caller's balance for every active resource type.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyBalances()
        {
            var responseSession = await GetAuthenticatedSessionAsync("AIGenerate");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            var balances = await _quotaService.GetBalancesAsync(
                responseSession.UserId,
                responseSession.RoleId,
                responseSession.Token);

            return Ok(balances);
        }
    }
}
