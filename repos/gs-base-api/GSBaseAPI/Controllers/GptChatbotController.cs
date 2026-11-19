using GSBaseAPI.GPTModule.Models;
using GSBaseAPI.GPTModule.Services;
using GSBaseAPI.GPTModule.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GSBaseAPI.GPTModule.Controllers
{
    [ApiController]
    [Route("baseapi/gpt-chatbot")]
    public class GptChatbotController : ControllerBase
    {
        private readonly DeprTutorService _tutorService;

        // Internal system defaults (NOT exposed to student)
        private const string DefaultPlanningId = "1966";
        private const string DefaultDueDate = "2026-03-04 00:00:00";
        private const string DefaultLangCode = "es";

        public GptChatbotController(DeprTutorService tutorService)
        {
            _tutorService = tutorService;
        }

        /// <summary>
        /// Student chatbot endpoint.
        /// Student sends ONLY message + conversation id.
        /// Everything else is resolved internally.
        /// </summary>
        [HttpPost("ask")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request, string language = "es")
        {
            // -----------------------------------------
            // 1) Validate session
            // -----------------------------------------
            /* TODO: Uncomment and implement session validation when auth is set up
             * var responseSession = await GetUserDetails
                .GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");

            if (responseSession == null || responseSession.SessionValid == false)
                return Unauthorized("Invalid or expired session."); */

            if (request == null || string.IsNullOrWhiteSpace(request.Query))
                return BadRequest("Query is required.");

            // -----------------------------------------
            // 2) Build internal tutor request
            // (student does NOT know these values)
            // -----------------------------------------
            var tutorReq = new StudentRequest
            {
                ConversationId = request.ConversationId,
                StudentPrompt = request.Query,
                LangCode = language,
                GradeLevel = "3",          // unknown
                LessonId = "",           // unknown -> service must resolve/search
                PlanningId = DefaultPlanningId,
                DueDate = DefaultDueDate
            };

            // -----------------------------------------
            // 3) Execute DEPR tutor pipeline
            // -----------------------------------------
            StudentReply reply;

            try
            {
                reply = await _tutorService.HandleStudentAsync(tutorReq);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Chatbot error: {ex.Message}");
            }

            // -----------------------------------------
            // 4) Return response to student
            // -----------------------------------------
            return Ok(new
            {
                conversationId = reply.ConversationId,
                message = reply.Message,
                language = reply.Language,
                assignmentCreated = !string.IsNullOrEmpty(reply.GeneratedQuizId)
            });
        }

        [HttpPost("quiz-jobs")]
        public async Task<IActionResult> StartQuizJob([FromBody] StudentRequest req,
    [FromServices] IQuizJobQueue queue,
    [FromServices] IMemoryCache cache,
    [FromServices] DeprTutorService tutor)
        {
            var job = new QuizJob();
            cache.Set(job.JobId, job, TimeSpan.FromMinutes(30));

            await queue.EnqueueAsync(async ct =>
            {
                job.Status = QuizJobStatus.Running;
                cache.Set(job.JobId, job, TimeSpan.FromMinutes(30));

                try
                {
                    // IMPORTANT: call a method that does ONLY the slow quiz pipeline
                    var quizId = await tutor.CreatePublishAttachQuizAsync(req, ct);
                    job.QuizId = quizId;
                    job.Status = QuizJobStatus.Succeeded;
                }
                catch (Exception ex)
                {
                    job.Status = QuizJobStatus.Failed;
                    job.Error = ex.Message;
                }

                cache.Set(job.JobId, job, TimeSpan.FromMinutes(30));
            });

            return Accepted(new { jobId = job.JobId, status = job.Status.ToString() });
        }

        [HttpGet("quiz-jobs/{jobId}")]
        public IActionResult GetQuizJob(string jobId, [FromServices] IMemoryCache cache)
        {
            if (!cache.TryGetValue(jobId, out QuizJob? job) || job == null)
                return NotFound();

            return Ok(new { jobId = job.JobId, status = job.Status.ToString(), quizId = job.QuizId, error = job.Error });
        }
    }
}