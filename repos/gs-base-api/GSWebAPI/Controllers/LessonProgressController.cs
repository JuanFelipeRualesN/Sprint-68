using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.LessonProgress;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.LessonProgress;
using GSWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// Tracks the student's progress through the sections of a lesson
    /// (concept, vocabulary, example, practice, exam, resources).
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class LessonProgressController : ControllerBase
    {
        private readonly ILogger<LessonProgressController> _logger;

        public LessonProgressController(ILogger<LessonProgressController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Marks a lesson section as completed for the student.
        /// </summary>
        [HttpPost]
        [Route("lessons/progress/section")]
        public async Task<IActionResult> MarkSectionCompleted([FromBody] MarkSectionRequest request)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (request == null || string.IsNullOrWhiteSpace(request.LessonId) ||
                    string.IsNullOrWhiteSpace(request.Section))
                {
                    return BadRequest("LessonId and Section are required.");
                }

                // Validate the marked section
                LessonProgressManager manager = new LessonProgressManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]);

                if (!manager.IsValidSection(request.Section))
                {
                    return BadRequest("Invalid section.");
                }

                manager.MarkSectionCompleted(responseSession.UserId, request.LessonId, request.Section);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking lesson section completed. LessonId: {LessonId}, Section: {Section}", request?.LessonId, request?.Section);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while marking the section.");
            }
        }

        /// <summary>
        /// Returns the sections the student has completed for a lesson.
        /// </summary>
        [HttpGet]
        [Route("lessons/{lessonId}/progress")]
        public async Task<IActionResult> GetLessonProgress(string lessonId)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                LessonProgressManager manager = new LessonProgressManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]);

                // Entering a lesson counts as activity for the day-streak.
                try
                {
                    manager.RecordActivity(responseSession.UserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error recording activity for streak. UserId: {UserId}", responseSession.UserId);
                }

                var completed = manager.GetCompletedSections(responseSession.UserId, lessonId);
                return Ok(completed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lesson progress. LessonId: {LessonId}", lessonId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while loading lesson progress.");
            }
        }

        /// <summary>
        /// Returns the student's current consecutive-day streak.
        /// </summary>
        [HttpGet]
        [Route("student/streak")]
        public async Task<IActionResult> GetStreak()
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "LessonDetailsWithStudentDetailsRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                LessonProgressManager manager = new LessonProgressManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]);

                int streak = manager.GetCurrentStreak(responseSession.UserId);

                return Ok(new { streak });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student streak.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while loading the streak.");
            }
        }
    }
}
