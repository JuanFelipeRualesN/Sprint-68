using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAnalysisAPI.Utils;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.FeignAPIs;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;

namespace DataAnalysisAPI.Controllers
{
    /// <summary>
    /// Handles requests related to results.
    /// </summary>
    [ApiController]
    [Route("dataanalysisapi")]
    public class ResultsController : ControllerBase
    {
        /// <summary>
        /// Gets the results list (paged/filtered).
        /// </summary>
        /// <param name="schoolId">Optional: School ID. If the requester is a director, it may be applied automatically.</param>
        /// <param name="subject">Optional: Subject code (e.g., MAT-SP).</param>
        /// <param name="level">Optional: Level code (e.g., 2).</param>
        /// <param name="sie">Optional: SIE identifier.</param>
        /// <param name="oreId">Optional: ORE ID. If the requester is an ORE admin, it will be applied automatically.</param>
        /// <param name="fromDate">Optional: From date filter. Defaults to a configured minimum date.</param>
        /// <param name="toDate">Optional: To date filter. Defaults to a configured maximum date.</param>
        /// <param name="limit">Optional: Page size. Default 10.</param>
        /// <param name="offset">Optional: Page offset. Default 0.</param>
        [HttpGet("results-list")]
        public IActionResult GetResultsList(
            [FromQuery] int? schoolId = null,
            [FromQuery] string? subject = null,
            [FromQuery] string? level = null,
            [FromQuery] string? sie = null,
            [FromQuery] int? oreId = null,
            [FromQuery] string? fromDate = null,
            [FromQuery] string? toDate = null,
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            try
            {
                // Session/identity lookup adapted to ASP.NET Core (uses HttpContext)
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "results-list");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // If the user has an ORE, force that filter
                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                // Safe date parsing with fallbacks
                var dateFrom = !string.IsNullOrWhiteSpace(fromDate) && DateTime.TryParse(fromDate, out var df)
                    ? df
                    : Configurations.GetMinDateForQueries();

                var dateTo = !string.IsNullOrWhiteSpace(toDate) && DateTime.TryParse(toDate, out var dt)
                    ? dt
                    : Configurations.GetMaxDatesForQueries();

                var result = DataAnalysisAPI.Services.Results.GetGradeSubjectTotals(
                    userIdRequester: int.Parse(session.UserId),
                    isDirector: session.IsDirector,
                    isStaff: session.IsStaff,
                    isStudent: session.RoleId == Configurations.GetStudentRoleId(),
                    isTeacher: session.RoleId == Configurations.GetTeacherRoleId(),
                    sie: sie,
                    schoolIds: (schoolId.HasValue && schoolId.Value > 0) ? new List<int> { schoolId.Value } : null,
                    userIds: null,
                    oreId: oreId,
                    subjectCodes: !string.IsNullOrWhiteSpace(subject) ? new List<string> { subject } : null,
                    levelCodes: !string.IsNullOrWhiteSpace(level) ? new List<string> { level } : null,
                    fromDatePeriod: dateFrom,
                    toDatePeriod: dateTo,
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
        /// Gets the results document (Excel) with totals.
        /// </summary>
        /// <param name="schoolId">Optional: School ID. If the requester is a director, it may be applied automatically.</param>
        /// <param name="subject">Optional: Subject code (e.g., MAT-SP).</param>
        /// <param name="level">Optional: Level code (e.g., 2).</param>
        /// <param name="sie">Optional: SIE identifier.</param>
        /// <param name="oreId">Optional: ORE ID. If the requester is an ORE admin, it will be applied automatically.</param>
        /// <param name="fromDate">Optional: From date filter. Defaults to a configured minimum date.</param>
        /// <param name="toDate">Optional: To date filter. Defaults to a configured maximum date.</param>
        [HttpGet("results-document")]
        public async Task<IActionResult> GetResultsDocument(
            [FromQuery] int? schoolId = null,
            [FromQuery] string? subject = null,
            [FromQuery] string? level = null,
            [FromQuery] string? sie = null,
            [FromQuery] int? oreId = null,
            [FromQuery] string? fromDate = null,
            [FromQuery] string? toDate = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "results-document");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                var dateFrom = !string.IsNullOrWhiteSpace(fromDate) && DateTime.TryParse(fromDate, out var df)
                    ? df
                    : Configurations.GetMinDateForQueries();

                var dateTo = !string.IsNullOrWhiteSpace(toDate) && DateTime.TryParse(toDate, out var dt)
                    ? dt
                    : Configurations.GetMaxDatesForQueries();

                var doc = await DataAnalysisAPI.Services.Results.GetGradeSubjectTotalsDocument(
                    userIdRequester: int.Parse(session.UserId),
                    isDirector: session.IsDirector,
                    isStaff: session.IsStaff,
                    isStudent: session.RoleId == Configurations.GetStudentRoleId(),
                    isTeacher: session.RoleId == Configurations.GetTeacherRoleId(),
                    sie: sie,
                    schoolIds: (schoolId.HasValue && schoolId.Value > 0) ? new List<int> { schoolId.Value } : null,
                    userIds: null,
                    oreId: oreId,
                    subjectCodes: !string.IsNullOrWhiteSpace(subject) ? new List<string> { subject } : null,
                    levelCodes: !string.IsNullOrWhiteSpace(level) ? new List<string> { level } : null,
                    fromDatePeriod: dateFrom,
                    toDatePeriod: dateTo,
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

        /// <summary>
        /// Gets the student's score report for a specific academic year.
        /// </summary>
        /// <param name="academicYearId">Academic year ID.</param>
        /// <param name="studentId">Optional: Student ID (when requester is not the student).</param>
        [HttpGet("grades-academic")]
        public IActionResult GetGradesAcademics(
            [FromQuery] int academicYearId,
            [FromQuery] int? studentId = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "grades-academic");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // If requester is not staff and is requesting another student's report, validate relationship.
                if (!session.IsStaff && studentId.HasValue && studentId.Value > 0 && int.Parse(session.UserId) != studentId.Value)
                {
                    var token = RequestHandler.GetToken(HttpContext);
                    if (!GetUserDetails.CanUserAManageUserB(session.UserId, studentId.Value.ToString(), token))
                        return Unauthorized();
                }

                var result = DataAnalysisAPI.Services.Results.PrepareStudentAnnualScore(
                    userIdRequester: int.Parse(session.UserId),
                    isDirector: session.IsDirector,
                    isStaff: session.IsStaff,
                    isStudent: session.RoleId == Configurations.GetStudentRoleId(),
                    studentId: studentId ?? int.Parse(session.UserId),
                    academicYearId: academicYearId
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
        /// Gets the student's score report (PDF) for a specific academic year.
        /// </summary>
        /// <param name="academicYearId">Academic year ID.</param>
        /// <param name="studentId">Optional: Student ID (when requester is not the student).</param>
        [HttpGet("grades-academic-pdf")]
        public async Task<IActionResult> GetGradesAcademicsPDF(
            [FromQuery] int academicYearId,
            [FromQuery] int? studentId = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "grades-academic-pdf");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // If requester is not staff and is requesting another student's report, validate relationship.
                if (!session.IsStaff && studentId.HasValue && studentId.Value > 0 && int.Parse(session.UserId) != studentId.Value)
                {
                    var token = RequestHandler.GetToken(HttpContext);
                    if (!GetUserDetails.CanUserAManageUserB(session.UserId, studentId.Value.ToString(), token))
                        return Unauthorized();
                }

                var doc = await DataAnalysisAPI.Services.Results.GetStudentAnnualScoreDocument(
                    userIdRequester: int.Parse(session.UserId),
                    isDirector: session.IsDirector,
                    isStaff: session.IsStaff,
                    isStudent: session.RoleId == Configurations.GetStudentRoleId(),
                    studentId: studentId ?? int.Parse(session.UserId),
                    academicYearId: academicYearId,
                    logoUrl: Configurations.GetLogoUrl(),
                    langCode: RequestHandler.GetLanguage(HttpContext)
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
