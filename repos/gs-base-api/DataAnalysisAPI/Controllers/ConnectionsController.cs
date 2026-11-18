using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.FeignAPIs;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;
using DataAnalysisAPI.Utils;

namespace DataAnalysisAPI.Controllers
{
    /// <summary>
    /// To handle connections.
    /// </summary>
    [ApiController]
    [Route("dataanalysisapi")]
    public class ConnectionsController : ControllerBase
    {
        /// <summary>
        /// Gets the list of connected users.
        /// </summary>
        /// <param name="fromDate">Optional: start date (filter).</param>
        /// <param name="toDate">Optional: end date (filter).</param>
        /// <param name="oreId">Optional: ORE id; if the user has an ORE it will be used automatically.</param>
        /// <param name="schoolId">Optional: school id; if not provided and the user is a principal, their school will be used.</param>
        /// <param name="status">Optional: User Status.</param>
        /// <param name="limit">Optional: page size (default 10).</param>
        /// <param name="offset">Optional: page index/offset (default 0).</param>
        [HttpGet("connected-list")]
        public IActionResult GetConnectedList(
            [FromQuery] string? fromDate = null,
            [FromQuery] string? toDate = null,
            [FromQuery] int? oreId = null,
            [FromQuery] string? status = null,
            [FromQuery] int? schoolId = null,
            [FromQuery] int? limit = 10,
            [FromQuery] int? offset = 0)
        {
            try
            {
              
                var responseSession =
                    GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "connected-list");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // If user has ORE
                if (!string.IsNullOrWhiteSpace(responseSession.OreId) &&
                    int.TryParse(responseSession.OreId, out var oreParsed))
                {
                    oreId = oreParsed;
                }

                // Dates
                var dateFrom = !string.IsNullOrWhiteSpace(fromDate) && DateTime.TryParse(fromDate, out var df)
                    ? df.Date
                    : Configurations.GetMinDateForQueries();

                var dateTo = !string.IsNullOrWhiteSpace(toDate) && DateTime.TryParse(toDate, out var dt)
                    ? dt.Date.AddDays(1).AddSeconds(-1)
                    : Configurations.GetMaxDatesForQueries().Date;

                var result = Connections.GetUsersConnections(
                    userIdRequester: int.Parse(responseSession.UserId),
                    isDirector: responseSession.IsDirector,
                    isStaff: responseSession.IsStaff,
                    dateFrom: dateFrom,
                    dateTo: dateTo,
                    schoolIds: schoolId.HasValue ? new List<int> { schoolId.Value } : null,
                    status: status,
                    userIds: null,
                    oreId: oreId,
                    fromPage: offset ?? 0,
                    limiter: limit ?? 10
                );

                return Ok(result);
            }
            catch (Exception)
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>
        /// Gets the document (Excel) with the connection details.
        /// </summary>
        /// <param name="fromDate">Optional: start date (filter).</param>
        /// <param name="toDate">Optional: end date (filter).</param>
        /// <param name="oreId">Optional: ORE id; if the user has an ORE it will be used automatically.</param>
        /// <param name="schoolId">Optional: school id.</param>
        /// <param name="status">Optional: User Subscription Status.</param>
        [HttpGet("connected-document")]
        public async Task<IActionResult> GetConnectedDocument(
            [FromQuery] string? fromDate = null,
            [FromQuery] string? toDate = null,
            [FromQuery] int? oreId = null,
            [FromQuery] string? status = null,
            [FromQuery] int? schoolId = null)
        {
            try
            {
                var responseSession =
                    GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "connected-document");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId) &&
                    int.TryParse(responseSession.OreId, out var oreParsed))
                {
                    oreId = oreParsed;
                }

                var dateFrom = !string.IsNullOrWhiteSpace(fromDate) && DateTime.TryParse(fromDate, out var df)
                    ? df.Date
                    : Configurations.GetMinDateForQueries();

                var dateTo = !string.IsNullOrWhiteSpace(toDate) && DateTime.TryParse(toDate, out var dt)
                    ? dt.Date.AddDays(1).AddSeconds(-1)
                    : Configurations.GetMaxDatesForQueries().Date;

                var doc = await Connections.GetUsersConnectionsDocument(
                    userIdRequester: int.Parse(responseSession.UserId),
                    isDirector: responseSession.IsDirector,
                    isStaff: responseSession.IsStaff,
                    dateFrom: dateFrom,
                    dateTo: dateTo,
                    schoolIds: schoolId.HasValue ? new List<int> { schoolId.Value } : null,
                    status: status,
                    userIds: null,
                    oreId: oreId,
                    languageCode: responseSession.LangCode
                );

                return Ok(doc);
            }
            catch (Exception ex)
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(ex.ToString());
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }
    }
}
