using Azure;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;
using DataAnalysisAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataAnalysisAPI.Controllers
{
    /// <summary>
    /// Handles requests related to PowerBI reports information.
    /// </summary>
    [ApiController]
    [Route("dataanalysisapi")]
    public class PowerBIController : ControllerBase
    {
        private readonly PowerBI _powerBI;

        public PowerBIController()
        {
            var tenantId = AppConfiguration.Configuration["AppSettings:powerbi_tenant_id"];
            var clientId = AppConfiguration.Configuration["AppSettings:powerbi_client_id"];
            var clientSecret = AppConfiguration.Configuration["AppSettings:PowerbiClientSecret"];
            _powerBI = new PowerBI(tenantId, clientId, clientSecret);
        }

        /// <summary>
        /// Gets PowerBI embedded information.
        /// </summary>
        /// <param name="reportId">The id for the requested report.</param>
        /// <param name="workspaceId">The id for the requested workspace.</param>
        [HttpGet("embed-powerbi-info")]
        public async Task<IActionResult> GetEmbedPowerBIInfo([FromQuery] string reportId, [FromQuery] string workspaceId)
        {
            if (string.IsNullOrEmpty(reportId) || string.IsNullOrEmpty(workspaceId))
            {
                return BadRequest("Missing reportId or workspaceId query parameter.");
            }

            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "embed-powerbi-info");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                int? oreId = null;
                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                var response = await _powerBI.EmbedPowerBIReport(reportId, workspaceId, oreId);
                if (!response.Success)
                {
                    BadRequest(Configurations.GetGenericMessageOnFail());
                }

                return Ok(response);
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }
    }
}