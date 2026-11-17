using Microsoft.AspNetCore.Mvc;
using TeachersAPI.Domain.Models.Generals.Requests;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Teachers;
using System.Net;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// Controller for managing assignments.
    /// </summary>
    [ApiController]
    [Route("teachersapi/assignments")]
    public class AssignmentsController : ControllerBase
    {
        /// <summary>
        /// Gets the pending assignments to be corrected from a teacher.
        /// </summary>
        /// <param name="detailsRequest">The general details. Includes th subject, level code, from page for pagination, limiter, etc. </param>
        /// <returns>All pending assignments with student assigned.</returns>
        [HttpPost("pendings")]
        public async Task<IActionResult> GetPendingAssignments([FromBody] DetailsRequestModel detailsRequest)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherPendingAssignments");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get the pending assignments
                PendingAssignmentsResponseModel teacherPendingAssignmentsResponse =
                    TeacherPendingAssignmentsHandler.GetPendingAssignments(responseSession.UserId, detailsRequest.FromPage, detailsRequest.Limiter, responseSession.Token, responseSession.IpAddress);

                // return response
                return Ok(teacherPendingAssignmentsResponse);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets the total count pending assignments to be corrected from a teacher.
        /// </summary>
        /// <returns>The count of total pending assignments</returns>
        [HttpGet("pendings/total")]
        public async Task<IActionResult> GetTotalPendingAssignments()
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherPendingAssignments");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get the total count pending assignments
                int totalPendingAssignments = TeacherPendingAssignmentsHandler.GetTotalPendingAssignments(responseSession.UserId);

                // return response
                return Ok(new { TotalPendingAssignments = totalPendingAssignments });
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }
    }
}