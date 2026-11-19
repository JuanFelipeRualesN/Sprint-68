using DocumentFormat.OpenXml.Bibliography;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Models.DTOs;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Exposes endpoints to query educational codes and lessons from the Athenas API.
    /// Results are cached in memory; lessons additionally support free-text search.
    /// </summary>
    [ApiController]
    [Route("aiservicesapi/[controller]")]
    [Produces("application/json")]
    public class AthenasCurriculumController : BaseController
    {
        private const string Module = "AIAthenas";

        private readonly CodesService   _codesService;
        private readonly LessonsService _lessonsService;
        private readonly ILogger<AthenasCurriculumController> _logger;
        private readonly AthenasFeignAPI _athenasFeignAPI;

        public AthenasCurriculumController(
            CodesService codesService,
            LessonsService lessonsService,
            AthenasFeignAPI athenasFeignAPI,
            ILogger<AthenasCurriculumController> logger, IConfiguration config) : base(config)
        {
            _codesService   = codesService;
            _lessonsService = lessonsService;
            _logger         = logger;
            _athenasFeignAPI = athenasFeignAPI;
        }

        // ── Codes ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Retrieves a paginated list of curriculum codes filtered by subject and level.
        /// Results are cached for 30 minutes; subsequent calls with the same parameters
        /// are served instantly from memory.
        /// </summary>
        /// <param name="request">Filter parameters including subject codes, level codes, and pagination.</param>
        /// <returns>A paginated list of <c>CodeHandleModel</c> items.</returns>
        /// <response code="200">Codes retrieved successfully (may be an empty page).</response>
        /// <response code="400">Request body is null or invalid.</response>
        /// <response code="500">Unexpected server error.</response>
        /// <summary>
        /// Retrieves a paginated list of curriculum codes filtered by subject and level via query parameters.
        /// Results are cached for 30 minutes; subsequent calls with the same parameters
        /// are served instantly from memory.
        /// </summary>
        /// <param name="request">Filter parameters including subject codes, level codes, and pagination provided in the query string.</param>
        /// <param name="typeId">Athenas code type: 1 for standards (default), 7 for expectations.</param>
        /// <returns>A paginated list of <c>CodeHandleModel</c> items.</returns>
        /// <response code="200">Codes retrieved successfully (may be an empty page).</response>
        /// <response code="400">Request parameters are invalid.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("codes")] // Changed from HttpPost to HttpGet
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCodesAsync([FromQuery] string subjectCode, string levelCode, int typeId = CodesService.StandardTypeId)
        {
            var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
            if (!isValid)
                return Unauthorized();

            // Note: [FromQuery] usually initializes the object even if no parameters are passed, 
            // so we check if the request object itself is null or if essential params are missing if needed.
            if (subjectCode is null || levelCode is null)
                return BadRequest("Request parameters are required.");

            try
            {
                var result = await _codesService.GetCodesAsync(
                    [subjectCode],
                    [levelCode],
                    0,
                    200,
                    false,
                    RequestHandler.GetLanguage(HttpContext),
                    typeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving codes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred retrieving codes.");
            }
        }

        /// <summary>
        /// To get the subjects from Athenas. (with name translated).  
        /// </summary>
        /// <returns>Returns the list of subjects.</returns>
        [HttpGet("subjects")] // Changed from HttpPost to HttpGet
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubjectssAsync() // Changed from FromBody to FromQuery
        {
            var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
            if (!isValid)
                return Unauthorized();

            try
            {
                var result = await _athenasFeignAPI.GetSubjects(RequestHandler.GetLanguage(HttpContext));
                // Order by name for better user experience
                result = result.OrderBy(s => s.Name).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving codes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred retrieving codes.");
            }
        }

        /// <summary>
        /// To get the subjects from Athenas. (with name translated).  
        /// </summary>
        /// <returns>Returns the list of subjects.</returns>
        [HttpGet("levels")] // Changed from HttpPost to HttpGet
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLevelsAsync() // Changed from FromBody to FromQuery
        {
            var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
            if (!isValid)
                return Unauthorized();

            try
            {
                var result = await _athenasFeignAPI.GetLevels(RequestHandler.GetLanguage(HttpContext));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving codes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred retrieving codes." + ex.ToString());
            }
        }


        // ── Lessons ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Retrieves lessons filtered by subject and level codes with optional full-text search.
        /// The full lesson set is cached for 30 minutes. The <c>searchTerm</c>, when provided,
        /// is applied in-memory against Title, Objective, Description, and related fields,
        /// so re-searching with a different term does not trigger a new upstream API call.
        /// </summary>
        /// <param name="request">
        /// Filter parameters. Set <c>SearchTerm</c> to narrow results by keyword.
        /// </param>
        /// <returns>A <c>LessonsResponseModel</c> containing the matched lessons.</returns>
        /// <response code="200">Lessons retrieved (and optionally filtered) successfully.</response>
        /// <response code="400">Request body is null or invalid.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("lessons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLessonsAsync([FromQuery] string? levelCode = null, string? subjectCode = null, string ? queryText = null)
        {

            try
            {
                var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
                if (!isValid)
                    return Unauthorized();

                var result = await _lessonsService.GetLessonsAsync(
                    subjectCodes: subjectCode != null ? [subjectCode] : null,
                    levelCodes: levelCode != null ? [levelCode] : null,
                    searchTerm: queryText);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lessons.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred retrieving lessons." + ex.ToString());
            }
        }

        /// <summary>
        /// Searches lessons already cached for a given subject/level combination.
        /// This is a GET convenience endpoint for quick keyword searches without a request body.
        /// The lesson collection must have been pre-cached by a prior <c>POST /lessons</c> call;
        /// if not cached, it is fetched on demand.
        /// </summary>
        /// <param name="subjectCodes">Comma-separated subject codes (e.g. <c>MAT-SP,SCI-EN</c>).</param>
        /// <param name="levelCodes">Comma-separated level codes (e.g. <c>k,1,2</c>).</param>
        /// <param name="searchTerm">Keyword to search within lessons.</param>
        /// <param name="langCode">Language code. Defaults to <c>en</c>.</param>
        /// <returns>Lessons whose searchable fields match the keyword.</returns>
        [HttpGet("lessons/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchLessonsAsync(
            [FromQuery] string subjectCodes,
            [FromQuery] string levelCodes,
            [FromQuery] string searchTerm,
            [FromQuery] string langCode = "en")
        {
            if (string.IsNullOrWhiteSpace(subjectCodes) || string.IsNullOrWhiteSpace(levelCodes))
                return BadRequest("subjectCodes and levelCodes query parameters are required.");

            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("searchTerm query parameter is required.");

            var subjects = subjectCodes.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            var levels   = levelCodes.Split(',',   StringSplitOptions.RemoveEmptyEntries).ToList();

            _logger.LogInformation(
                "SearchLessons – subjects: [{Subjects}], levels: [{Levels}], term: \"{Term}\"",
                subjectCodes, levelCodes, searchTerm);

            try
            {
                var result = await _lessonsService.GetLessonsAsync(subjects, levels, searchTerm, langCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching lessons.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred searching lessons.");
            }
        }
    }
}
