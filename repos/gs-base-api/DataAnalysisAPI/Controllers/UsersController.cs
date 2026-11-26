using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAnalysisAPI.Utils;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.FeignAPIs;

namespace DataAnalysisAPI.Controllers
{
    /// <summary>
    /// Handles requests related to users information.
    /// </summary>
    [ApiController]
    [Route("dataanalysisapi")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Gets users information.
        /// </summary>
        /// <param name="schoolId">Optional: School ID. If requester is a director, it may be applied automatically.</param>
        /// <param name="sie">Optional: User SIE.</param>
        /// <param name="oreId">Optional: ORE ID. If requester is an ORE admin, it will be applied automatically.</param>
        /// <param name="roleId">Optional: Role ID.</param>
        /// <param name="status">Optional: User Status.</param>
        /// <param name="limit">Optional: Items per page. Default 10.</param>
        /// <param name="offset">Optional: Page offset. Default 0.</param>
        [HttpGet("users-list")]
        public IActionResult GetUsersList(
            [FromQuery] int? schoolId = null,
            [FromQuery] string? sie = null,
            [FromQuery] int? oreId = null,
            [FromQuery] int? roleId = null,
            [FromQuery] string? status = null,
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            try
            {
                // Session/identity lookup adapted to ASP.NET Core (uses HttpContext)
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "users-list");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // If the user has an ORE, enforce that filter
                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                var result = Users.GetUsers(
                    userIdRequester: int.Parse(session.UserId),
                    isDirector: session.IsDirector,
                    isStaff: session.IsStaff,
                    sie: sie,
                    roleIds: roleId.HasValue ? new List<int> { roleId.Value } : null,
                    schoolIds: schoolId.HasValue ? new List<int> { schoolId.Value } : null,
                    status: status,
                    oreId: oreId,
                    fromPage: offset,
                    limiter: limit
                );

                return Ok(result);
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>
        /// Gets users information as a document (e.g., Excel).
        /// </summary>
        /// <param name="schoolId">Optional: School ID. If requester is a director, it may be applied automatically.</param>
        /// <param name="sie">Optional: User SIE.</param>
        /// <param name="oreId">Optional: ORE ID. If requester is an ORE admin, it will be applied automatically.</param>
        /// <param name="roleId">Optional: Role ID.</param>
        /// <param name="status">Optional: User Subscription Status.</param>
        [HttpGet("users-document")]
        public async Task<IActionResult> GetUsersDocument(
            [FromQuery] int? schoolId = null,
            [FromQuery] string? sie = null,
            [FromQuery] int? oreId = null,
            [FromQuery] int? roleId = null,
            [FromQuery] string? status = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "users-document");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                var doc = await Users.GetUsersDocument(
                    userIdRequester: int.Parse(session.UserId),
                    isDirector: session.IsDirector,
                    isStaff: session.IsStaff,
                    sie: sie,
                    roleIds: roleId.HasValue ? new List<int> { roleId.Value } : null,
                    schoolIds: schoolId.HasValue ? new List<int> { schoolId.Value } : null,
                    status: status,
                    oreId: oreId,
                    languageCode: session.LangCode
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
