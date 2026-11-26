
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingAPI.Domain.DTOs;
using ReadingAPI.ReadingAPI.Services.Story;

namespace ReadingAPI.Controllers
{
    [ApiController]
    [Route("readingapi/pronunciation")]
    public class PronunciationController : ControllerBase
    {
        private readonly SpeechService _speechService;
        private readonly UserStoryResultsService _userStoryResultsService;
        private readonly ILogger<PronunciationController> _logger;

        public PronunciationController(SpeechService speechService, UserStoryResultsService userStoryResultsService, ILogger<PronunciationController> logger)
        {
            _speechService = speechService;
            _userStoryResultsService = userStoryResultsService;
            _logger = logger;
        }

        // --- New Text-to-Speech Function ---
        /// <summary>
        /// Generates audio from text for correct pronunciation example.
        /// </summary>
        /// <param name="text">The text to synthesize.</param>
        /// <param name="language">Optional (default en-US; try es-MX/es-ES).</param>
        /// <param name="voiceName">Optional (e.g., en-US-JennyNeural).</param>
        /// <returns>An audio file (WAV).</returns>
        [HttpGet("speak")]
        public async Task<IActionResult> Speak(
    [FromQuery] string text,
    [FromQuery] string? language,
    [FromQuery] string? voiceName,
    [FromQuery] bool wordBoundaries = false)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("Missing 'text' query parameter.");

            try
            {
                var lang = string.IsNullOrWhiteSpace(language) ? "en-US" : language!;

                if (wordBoundaries)
                {
                    var r = await _speechService.SynthesizeTextWithBoundariesAsync(text, lang, voiceName);
                    if (r.Audio == null || r.Audio.Length == 0)
                        return StatusCode(500, "Speech synthesis failed to produce audio.");

                    return Ok(new
                    {
                        audio = Convert.ToBase64String(r.Audio),
                        words = r.Words   // [{ offset, duration, text }]
                    });
                }

                var audioData = await _speechService.SynthesizeTextToAudioAsync(text, lang, voiceName);
                if (audioData == null || audioData.Length == 0)
                    return StatusCode(500, "Speech synthesis failed to produce audio.");

                return File(audioData, "audio/wav");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during speech synthesis: {ex.ToString()}");
            }
        }

        // --- New Text-to-Speech Function ---
        /// <summary>
        /// Generates audio from text for correct pronunciation example.
        /// </summary>
        /// <param name="text">The text to synthesize.</param>
        /// <param name="language">Optional (default en-US; try es-MX/es-ES).</param>
        /// <param name="voiceName">Optional (e.g., en-US-JennyNeural).</param>
        /// <returns>An audio file (WAV).</returns>
        [HttpGet("lessons")]
        public async Task<IActionResult> GetLessons(
            [FromQuery] int fromPage = 0,
            [FromQuery] int? limiter = 10,
            [FromQuery] string? subjectCode = null,
            [FromQuery] string? levelCode = null)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            try
            {
                // --- Guardrails ---
                var pageIndex = Math.Max(0, fromPage);
                var pageSize = Math.Clamp(limiter ?? 10, 1, 200);

                // --- Data source (sync -> async bridge) ---
                var allLessons = await Task.Run(() => AthenasFeignAPI.GetReadingLessons() ?? new List<LessonUnitDto>());

                // --- Filters (case-insensitive) ---
                if (!string.IsNullOrWhiteSpace(subjectCode))
                {
                    allLessons = allLessons
                        .Where(x => string.Equals(x.SubjectCode, subjectCode, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(levelCode))
                {
                    allLessons = allLessons
                        .Where(x => string.Equals(x.LevelCode, levelCode, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // --- Sort (stable, predictable) ---
                var ordered = allLessons
                    .OrderBy(x => x.LevelCode)
                    .ThenBy(x => x.SubjectCode)
                    .ThenBy(x => x.ConstructedFullCode)
                    .ThenBy(x => x.LessonTitle);

                // --- Paging ---
                var total = ordered.Count();
                var totalPages = (int)Math.Ceiling(total / (double)pageSize);
                var items = ordered.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                // --- Response shape (Feign-friendly) ---
                var result = new
                {
                    meta = new
                    {
                        pageIndex,
                        pageSize,
                        totalItems = total,
                        totalPages
                    },
                    items
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Optional: log ex
                return BadRequest(new
                {
                    messages = new[] { ex.Message }
                });
            }
        }



        [HttpPost("assess/level")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 25_000_000)]
        [RequestSizeLimit(25_000_000)]
        public async Task<IActionResult> AssessLevel([FromForm] PronunciationAssessForm form)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (form.Audio == null || form.Audio.Length == 0)
                return BadRequest("Missing audio file.");
            if (string.IsNullOrWhiteSpace(form.ReferenceText))
                return BadRequest("Missing referenceText.");

            try
            {
                var lang = string.IsNullOrWhiteSpace(form.Language) ? "en-US" : form.Language!;
                var threshold = form.MinAccuracy is > 0 and <= 100 ? form.MinAccuracy.Value : 85.0;

                await using var stream = form.Audio.OpenReadStream();
                var contentType = form.Audio.ContentType ?? "audio/webm";

                // 1. Get detailed DTO results from the Azure AI Speech service
                var azureResult = await _speechService.AssessAndSummarizeAsync(
                    stream, contentType, form.ReferenceText, lang, threshold, flagAnyErrorType: true);

                // 2. Determine the custom language level using the business logic
                //    We pass the Azure results, the language, and the original reference text.
                var levelEvaluator = new LanguageLevelEvaluator();
                var result = levelEvaluator.DetermineLevel(azureResult, lang, form.ReferenceText, form.StudentGrade);

                // 3. Construct the final response DTO: the DEPR level in the request language, and the story level
                var finalResponse = new FinalAssessmentResponse
                {
                    AzureResult = azureResult,
                    ReadingLevel = result.Level.Number,
                    LanguageLevel = result.Level.Label(lang),
                    LevelDescription = result.Level.Description(lang),
                    StoryLevel = result.StoryLevel
                };

                // React frontend receives this comprehensive JSON object.
                return Ok(finalResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during pronunciation level test");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during assessment.");
            }
        }

        /// <summary>
        /// multipart/form-data:
        ///  - audio: file (webm/ogg/mp3/wav)
        ///  - referenceText: required
        ///  - language: optional (default en-US; try es-MX/es-ES)
        ///  - minAccuracy: optional (default 85)
        /// </summary>
        [HttpPost("assess")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 25_000_000)]
        [RequestSizeLimit(25_000_000)]
        public async Task<IActionResult> Assess([FromForm] PronunciationAssessForm form)
        {
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var userId = int.Parse(responseSession.UserId);

            try
            {
                if (form.Audio == null || form.Audio.Length == 0)
                    return BadRequest("Missing audio.");
                if (string.IsNullOrWhiteSpace(form.ReferenceText))
                    return BadRequest("Missing referenceText.");

                var langRegion = string.IsNullOrWhiteSpace(form.Language) ? "en-US" : form.Language!;
                var langCode = langRegion.Split('-').FirstOrDefault();

                var threshold = form.MinAccuracy is > 0 and <= 100 ? form.MinAccuracy.Value : 85.0;

                await using var stream = form.Audio.OpenReadStream();
                var contentType = string.IsNullOrWhiteSpace(form.Audio.ContentType) ? "audio/webm" : form.Audio.ContentType;

                var summary = await _speechService.AssessAndSummarizeAsync(
                    stream, contentType, form.ReferenceText, langRegion, threshold, flagAnyErrorType: true);

                if (!summary.SpeechRecognized)
                    return Ok(summary);

                // TODO: persist (summary + audio) to DB/blob storage if desired.
                if (form.StoryId != 0)
                {
                    try
                    {
                        await _userStoryResultsService.SaveUserStoryResults(userId, form.StoryId, form.SlideId, langCode, summary);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during pronunciation assessment (storyId={StoryId}, slideId={SlideId}).", form.StoryId, form.SlideId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during assessment.");
            }
        }
    }
}
