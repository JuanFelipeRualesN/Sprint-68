using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;
using DataAnalysisAPI.Utils;

namespace DataAnalysisAPI.Controllers
{
    [ApiController]
    [Route("dataanalysisapi")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Smoke-test endpoint that generates the users-connections document
        /// using fixed credentials (userId=1, staff=true, director=false).
        /// </summary>
        /// <param name="fromDate">Optional from-date filter (parsed with fallback).</param>
        /// <param name="toDate">Optional to-date filter (parsed with fallback).</param>
        /// <param name="oreId">Optional ORE ID.</param>
        /// <param name="schoolId">Optional school ID.</param>
        [HttpGet("test-db-blob")]
        public async Task<IActionResult> GetConnectedDocument(
            [FromQuery] string? fromDate = null,
            [FromQuery] string? toDate = null,
            [FromQuery] int? oreId = null,
            [FromQuery] int? schoolId = null)
        {
            try
            {
                // Safe date parsing with sensible fallbacks
                var dateFrom = !string.IsNullOrWhiteSpace(fromDate) && DateTime.TryParse(fromDate, out var df)
                    ? df
                    : Configurations.GetMinDateForQueries();

                var dateTo = !string.IsNullOrWhiteSpace(toDate) && DateTime.TryParse(toDate, out var dt)
                    ? dt.Date
                    : Configurations.GetMaxDatesForQueries().Date;

                // Keep the same hardcoded requester context as in the original code
                var doc = await Connections.GetUsersConnectionsDocument(
                    userIdRequester: 1,
                    isDirector: false,
                    isStaff: true,
                    dateFrom: dateFrom,
                    dateTo: dateTo,
                    schoolIds: schoolId.HasValue ? new List<int> { schoolId.Value } : null,
                    userIds: null,
                    oreId: oreId
                );

                return Ok(doc);
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }
    }
}
