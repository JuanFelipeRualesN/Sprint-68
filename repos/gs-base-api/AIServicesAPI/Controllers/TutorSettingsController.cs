using GSAIServicesAPI.Models.DTOs;
using GSAIServicesAPI.Services.Tutor;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Configuration for AI tutor routing. It allows a user to choose which
    /// lesson grades are answered by the RAG engine (every other grade uses the
    /// generative engine). The settings are read by TutorController.
    [Route("aiservicesapi/tutor/settings")]
    public class TutorSettingsController : BaseController
    {
        private readonly TutorSettingsService _tutorSettingsService;

        public TutorSettingsController(TutorSettingsService settings, IConfiguration config) : base(config)
        {
            _tutorSettingsService = settings;
        }

        /// <summary>
        /// Returns the current RAG grade list.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRagGrades()
        {
            var responseSession = await GetAuthenticatedSessionAsync("AITutorSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            var rawGrades = await _tutorSettingsService.GetRagGradesRawAsync();
            return Ok(new TutorSettingsDto { RagGrades = SplitToList(rawGrades) });
        }

        /// <summary>
        /// Update the RAG grade list with the supplied values.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateRagGrades([FromBody] TutorSettingsDto request)
        {
            var responseSession = await GetAuthenticatedSessionAsync("AITutorSettings");

            if (responseSession == null)
            {
                return Unauthorized(new { error = GetLocalizedMessage("Unauthorized") });
            }

            if (request?.RagGrades == null)
            {
                return BadRequest(new { error = GetLocalizedMessage("InvalidSettings") });
            }

            var csv = string.Join(",", request.RagGrades);
            await _tutorSettingsService.SaveRagGradesAsync(csv);

            var raw = await _tutorSettingsService.GetRagGradesRawAsync();
            return Ok(new TutorSettingsDto { RagGrades = SplitToList(raw) });
        }

        /// <summary>
        /// Splits the stored comma-separated grade string into a trimmed list, dropping empty entries.
        /// </summary>
        private static List<string> SplitToList(string csv) =>
            string.IsNullOrWhiteSpace(csv)
                ? new List<string>()
                : csv.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(g => g.Trim())
                     .Where(g => g.Length > 0)
                     .ToList();
    }
}
