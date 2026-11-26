using ReadingAPI.Domain.DTOs;
using ReadingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;

namespace ReadingAPI.Controllers
{
    [ApiController]
    [Route("readingapi/reading-diagnostics")]
    [Produces("application/json")]
    public class ReadingDiagnosticsController : ControllerBase
    {
        private readonly ReadingDiagnosticsService _service;

        public ReadingDiagnosticsController()
        {
            _service = new ReadingDiagnosticsService();
        }

        // --- Request/Response models for the API ---

        public class CreateCustomReadingDiagnosticRequest
        {
            public string Title { get; set; }
            public string SubjectCode { get; set; }
            public string LevelCode { get; set; }
            public bool Published { get; set; }
            public List<int> QuestionIds { get; set; } = new();
        }

        public class UpdateCustomReadingDiagnosticRequest
        {
            public string Title { get; set; }
            public string SubjectCode { get; set; }
            public string LevelCode { get; set; }
            public bool Published { get; set; }
            public List<int> QuestionIds { get; set; } = new();
        }

        public class CustomReadingDiagnosticDetailResponse
        {
            public CustomReadingDiagnosticDto Diagnostic { get; set; }
            public List<CustomReadingDiagnosticQuestionDto> Questions { get; set; } = new();
        }

        // --- CREATE ---

        /// <summary>
        /// Creates a new custom reading diagnostic with its associated questions.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> CreateDiagnostic([FromBody] CreateCustomReadingDiagnosticRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            // Validate the user session
            var sessionDetails = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
            if (sessionDetails == null || sessionDetails.SessionValid == false)
            {
                return Unauthorized();
            }

            var currentUserId = sessionDetails.UserId;

            var diagnostic = new CustomReadingDiagnosticDto
            {
                Title = request.Title,
                SubjectCode = request.SubjectCode,
                LevelCode = request.LevelCode,
                TeacherId = currentUserId,
                Published = request.Published,
                ModifiedBy = currentUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var id = _service.CreateDiagnosticWithQuestions(
                diagnostic,
                request.QuestionIds,
                currentUserId
            );

            // Return the new ID and location
            return CreatedAtAction(nameof(GetDiagnosticById), new { id }, id);
        }

        // --- READ: Get by ID ---

        /// <summary>
        /// Gets a custom reading diagnostic by ID. Optionally includes questions.
        /// Only the owner (teacher) can access it.
        /// </summary>
        /// <param name="id">Diagnostic ID.</param>
        /// <param name="includeQuestions">Whether to include questions in the response.</param>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomReadingDiagnosticDetailResponse>> GetDiagnosticById(
            int id,
            [FromQuery] bool includeQuestions = true)
        {
            // Validate the user session
            var sessionDetails = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
            if (sessionDetails == null || sessionDetails.SessionValid == false)
            {
                return Unauthorized();
            }

            var currentUserId = sessionDetails.UserId;

            var diagnostic = _service.GetDiagnostic(id, includeQuestions, out var questions);

            if (diagnostic == null)
            {
                return NotFound();
            }

            // Ensure the authenticated user is the owner
            if (!string.Equals(diagnostic.TeacherId, currentUserId, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            var response = new CustomReadingDiagnosticDetailResponse
            {
                Diagnostic = diagnostic,
                Questions = questions ?? new List<CustomReadingDiagnosticQuestionDto>()
            };

            return Ok(response);
        }

        // --- READ: Get by teacher ---

        /// <summary>
        /// Gets all custom reading diagnostics created by the authenticated teacher.
        /// The teacherId in the route is ignored for security; the session user is used instead.
        /// </summary>
        /// <param name="teacherId">Teacher ID (ignored; kept for backward compatibility).</param>
        [HttpGet("by-teacher/")]
        public async Task<ActionResult<List<CustomReadingDiagnosticDto>>> GetDiagnosticsByTeacher()
        {
            try
            {
                // Validate the user session
                var sessionDetails = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (sessionDetails == null || sessionDetails.SessionValid == false)
                {
                    return Unauthorized();
                }

                var currentUserId = sessionDetails.UserId;

                // Always use the user from session, not from route
                var diagnostics = _service.GetDiagnosticsByTeacher(currentUserId) ?? new List<CustomReadingDiagnosticDto>();
                return Ok(diagnostics);
            }
            catch(Exception ex)
            {
                return StatusCode(400, $"An error occurred: {ex.Message}");
            }
        }

        // --- UPDATE ---

        /// <summary>
        /// Updates an existing custom reading diagnostic and replaces its question list.
        /// Only the owner (teacher) can update it.
        /// </summary>
        /// <param name="id">Diagnostic ID.</param>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDiagnostic(
            int id,
            [FromBody] UpdateCustomReadingDiagnosticRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            // Validate the user session
            var sessionDetails = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
            if (sessionDetails == null || sessionDetails.SessionValid == false)
            {
                return Unauthorized();
            }

            var currentUserId = sessionDetails.UserId;

            var existing = _service.GetDiagnostic(id, includeQuestions: false, out _);
            if (existing == null)
            {
                return NotFound();
            }

            // Ensure the authenticated user is the owner
            if (!string.Equals(existing.TeacherId, currentUserId, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            existing.Title = request.Title;
            existing.SubjectCode = request.SubjectCode;
            existing.LevelCode = request.LevelCode;
            existing.Published = request.Published;
            existing.ModifiedBy = currentUserId;

            _service.UpdateDiagnosticWithQuestions(
                existing,
                request.QuestionIds,
                currentUserId
            );

            return NoContent();
        }

        // --- DELETE ---

        /// <summary>
        /// Deletes a custom reading diagnostic and all its associated questions.
        /// Only the owner (teacher) can delete it.
        /// </summary>
        /// <param name="id">Diagnostic ID.</param>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDiagnostic(int id)
        {
            // Validate the user session
            var sessionDetails = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
            if (sessionDetails == null || sessionDetails.SessionValid == false)
            {
                return Unauthorized();
            }

            var currentUserId = sessionDetails.UserId;

            var existing = _service.GetDiagnostic(id, includeQuestions: false, out _);
            if (existing == null)
            {
                return NotFound();
            }

            // Ensure the authenticated user is the owner
            if (!string.Equals(existing.TeacherId, currentUserId, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            _service.DeleteDiagnostic(id);
            return NoContent();
        }
    }
}
