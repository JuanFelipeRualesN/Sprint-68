using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SchoolsAPI.SchoolsAPI.Services.Schools;
using SchoolsAPI.CustomClasses;
using SchoolsAPI.SchoolsAPI.Services.FeignAPIs;

namespace SchoolsAPI.Controllers
{
    /// <summary>
    /// To handle the terms that an academic year is divided. For example, a trimester system is divided in three sessions.
    /// </summary>
    [ApiController]
    [Route("schoolsapi/terms")]
    public class TermsController : ControllerBase
    {
        /// <summary>
        /// To read the terms as semester, trimester, etc, and the details about them.
        /// </summary>
        /// <returns>Returns the list of terms with an HTTP Response code 200 if valid permissions.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTerms()
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                 HttpContext,
                "SubscriptionAdvanceRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            // Use dependency injection to get the connection string.
            var terms = new SchoolsReader(
                  Settings.CONNECTION_STRING_READ,
                  RequestHandler.GetLanguage(HttpContext)).GetTerms(
                        new List<TermModel>());

            return Ok(terms);
        }
    }
}