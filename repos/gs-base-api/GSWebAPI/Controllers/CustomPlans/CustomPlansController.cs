using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSWebAPI.Controllers.CustomPlans
{
    /// <summary>
    /// Custom study plans of a client for one of the client's students.
    /// Permissions: CustomPlanRead (read) and CustomPlanHandler (write).
    /// </summary>
    [ApiController]
    [Route("gswebapi/custom-plans")]
    public class CustomPlansController : ServiceResultControllerBase
    {
        private const string ReadPermission = "CustomPlanRead";
        private const string WritePermission = "CustomPlanHandler";

        private readonly ICustomPlanService _service;
        private readonly ICustomPlanOverviewService _overview;
        private readonly ICustomPlanEvidenceService _evidences;
        private readonly ICustomPlanActivityService _activities;
        private readonly ICustomPlanAssistantService _assistant;

        /// <summary>Creates the controller.</summary>
        /// <param name="service">Plans: create, read, draft update, status, lesson progress and activities, delete.</param>
        /// <param name="overview">Overview of the plans by student.</param>
        /// <param name="evidences">Evidences and projects of a plan.</param>
        /// <param name="activities">Status and results of practices, exams and pre/post tests.</param>
        /// <param name="assistant">The AI study-plan assistant.</param>
        public CustomPlansController(ICustomPlanService service, ICustomPlanOverviewService overview, ICustomPlanEvidenceService evidences, ICustomPlanActivityService activities, ICustomPlanAssistantService assistant)
        {
            _assistant = assistant;
            _service = service;
            _overview = overview;
            _evidences = evidences;
            _activities = activities;
        }

        /// <summary>
        /// One turn of the AI study-plan assistant. Start with <c>{ "conversationId": null }</c> and keep sending the returned
        /// conversationId with the user's clicked option (selectedOption / selectedOptions) or text (message). The response
        /// has the step, message, options, answers, the plan preview and payload (confirm step) and the created plan
        /// (created step). The plan is created only after the user confirms the preview of the previous turn. Language:
        /// Accept-Language (es by default, en).
        /// </summary>
        /// <param name="request">The turn.</param>
        /// <returns>200 with the turn; 400 message too long or adjust mode; 403 not a client; 404 conversation not found or expired; 409 busy or too many turns; 401 without CustomPlanHandler.</returns>
        [HttpPost("assistant/messages")]
        public async Task<IActionResult> AssistantMessage([FromBody] CustomPlanAssistantRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _assistant.SendAsync(user, request ?? new CustomPlanAssistantRequestModel(), HttpContext.RequestAborted));
        }

        /// <summary>
        /// Lists the custom plan types (homeschool, reinforcements...) with the name in the session's language (Accept-Language),
        /// e.g. Refuerzos for es and Reinforcements for en. Use the id as customPlanTypeId when creating a plan.
        /// </summary>
        /// <returns>200 with the plan types; 401 without CustomPlanRead.</returns>
        [HttpGet("types")]
        public async Task<IActionResult> ListTypes()
        {
            var user = await GetRequestUserAsync(ReadPermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _service.ListTypesAsync(user.LangCode, HttpContext.RequestAborted));
        }

        /// <summary>
        /// The plans of the client's children (or the named students, for staff), by student, plan and subject, with every
        /// lesson area (learning, practices, exams, pre/post tests), dates, time zones, completion, scores, standards, Athenas
        /// titles and evidences. Filters are optional for clients (staff must send studentIds or studentId), e.g.
        /// <c>?studentIds=12&amp;studentIds=15&amp;dateFrom=2026-11-01&amp;dateTo=2026-11-30&amp;minScore=70&amp;statuses=active</c>,
        /// or <c>?studentId=12</c> for one student. <c>planDateFrom</c>/<c>planDateTo</c> keep only the plans whose start..end
        /// dates overlap that window (e.g. <c>?planDateFrom=2026-11-01</c> leaves out plans that ended before November).
        /// For a month calendar, <c>?month=2026-11</c> returns only the lessons with something in that month, from any plan
        /// (old ones included), and loads nothing else.
        /// </summary>
        /// <param name="filter">Students, plan statuses, date window and lesson score range.</param>
        /// <returns>200 with the students and their plans; 400 for invalid filters; 401 without CustomPlanRead.</returns>
        [HttpGet("overview")]
        public async Task<IActionResult> Overview([FromQuery] CustomPlanOverviewFilterModel filter)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _overview.GetOverviewAsync(user, filter ?? new CustomPlanOverviewFilterModel(), HttpContext.RequestAborted));
        }

        /// <summary>Lists the evidences and projects of a plan (the plan's client or staff). Uploaded files come with a 1-hour read link.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <returns>200 with the evidences, newest first; 404 when the plan is not found or not visible; 401 without CustomPlanRead.</returns>
        [HttpGet("{planId}/evidences")]
        public async Task<IActionResult> ListEvidences(ulong planId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _evidences.ListAsync(user, planId, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Adds an evidence or project (multipart/form-data): Title, Description, EvidenceType, PlanLessonId and either a
        /// <c>file</c> (uploaded to Blob Storage, max 25 MB) or an external Url. Only the plan's client; not on cancelled plans.
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">Evidence fields from the form.</param>
        /// <param name="file">Optional uploaded file; when present the Url is ignored.</param>
        /// <returns>201 with the evidence; 400 validation errors; 404 plan not found; 409 cancelled plan; 503 storage unavailable; 401 without CustomPlanHandler.</returns>
        [HttpPost("{planId}/evidences")]
        [RequestSizeLimit(CustomPlanEvidenceService.MaxFileBytes + 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = CustomPlanEvidenceService.MaxFileBytes + 1024 * 1024)]
        public async Task<IActionResult> CreateEvidence(ulong planId, [FromForm] CustomPlanEvidenceRequestModel request, IFormFile? file)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            if (file == null)
            {
                return FromResult(await _evidences.CreateAsync(user, planId, request, null, HttpContext.RequestAborted), created: true);
            }

            await using var stream = file.OpenReadStream();
            var upload = new UploadedFile(stream, file.FileName, file.ContentType, file.Length);
            return FromResult(await _evidences.CreateAsync(user, planId, request, upload, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Soft-deletes an evidence of a plan. Only the plan's client.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="evidenceId">Evidence id.</param>
        /// <returns>200 with true; 404 when the evidence is not found in the caller's plan; 401 without CustomPlanHandler.</returns>
        [HttpDelete("{planId}/evidences/{evidenceId}")]
        public async Task<IActionResult> DeleteEvidence(ulong planId, ulong evidenceId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _evidences.DeleteAsync(user, planId, evidenceId, HttpContext.RequestAborted));
        }

        /// <summary>Lists the plans of the client in session, optionally for one student.</summary>
        /// <param name="studentId">Optional student filter.</param>
        /// <returns>200 with the plan summaries, newest first; 401 without CustomPlanRead.</returns>
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ulong? studentId = null)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _service.ListAsync(user, studentId, HttpContext.RequestAborted));
        }

        /// <summary>Reads a plan with its full structure (the plan's client or staff).</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <returns>200 with the plan; 404 when not found or not visible; 401 without CustomPlanRead.</returns>
        [HttpGet("{planId}")]
        public async Task<IActionResult> Get(ulong planId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _service.GetAsync(user, planId, HttpContext.RequestAborted));
        }

        /// <summary>Creates a draft plan for a student of the client in session.</summary>
        /// <param name="request">Student, header and structure.</param>
        /// <returns>201 with the plan; 400 validation errors; 403 when the caller is not a client; 503 when Athenas or the Users API cannot be reached; 401 without CustomPlanHandler.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomPlanRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.CreateAsync(user, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Replaces the header and structure of a draft plan. Only the plan's client.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">New header and structure.</param>
        /// <returns>200 with the plan; 400 validation errors; 404 not found; 409 not a draft; 503 Athenas unavailable; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}")]
        public async Task<IActionResult> UpdateDraft(ulong planId, [FromBody] UpdateCustomPlanDraftRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.UpdateDraftAsync(user, planId, request, HttpContext.RequestAborted));
        }

        /// <summary>Changes the status of a plan (draft, active, paused, completed, cancelled). Only the plan's client.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">Target status.</param>
        /// <returns>200 with the plan; 404 not found; 409 transition not allowed or no lessons to activate; 401 without CustomPlanHandler.</returns>
        [HttpPost("{planId}/status")]
        public async Task<IActionResult> ChangeStatus(ulong planId, [FromBody] ChangeCustomPlanStatusRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.ChangeStatusAsync(user, planId, request.Status, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Replaces the weekly study days and time slots (days per week, start times, session durations) of a plan that is
        /// not completed or cancelled. Only the plan's client. Lessons are not changed.
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">The new days and time slots (an empty list clears the schedule).</param>
        /// <returns>200 with the plan; 400 validation errors; 404 not found; 409 plan completed or cancelled; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/schedule")]
        public async Task<IActionResult> UpdateSchedule(ulong planId, [FromBody] CustomPlanScheduleRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.UpdateScheduleAsync(user, planId, request, HttpContext.RequestAborted));
        }

        /// <summary>Adds a subject to a plan that is not completed or cancelled. Only the plan's client.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">The Athenas subject id.</param>
        /// <returns>200 with the plan; 400 invalid or already in the plan; 404 not found; 409 plan completed or cancelled; 401 without CustomPlanHandler.</returns>
        [HttpPost("{planId}/subjects")]
        public async Task<IActionResult> AddSubject(ulong planId, [FromBody] CustomPlanSubjectRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.AddSubjectAsync(user, planId, request.SubjectId, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Removes a subject from a plan that is not completed or cancelled. Only the plan's client. When the subject has
        /// lessons in the plan, send <c>?removeLessons=true</c> to remove them too; that is refused (409) when any of them
        /// has progress.
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="subjectId">Athenas subject id.</param>
        /// <param name="removeLessons">Also remove the subject's lessons (none may have progress).</param>
        /// <returns>200 with the plan; 404 plan or subject not found; 409 plan closed, subject has lessons, or a lesson has progress; 401 without CustomPlanHandler.</returns>
        [HttpDelete("{planId}/subjects/{subjectId}")]
        public async Task<IActionResult> RemoveSubject(ulong planId, ulong subjectId, [FromQuery] bool removeLessons = false)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _service.RemoveSubjectAsync(user, planId, subjectId, removeLessons, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Adds one selected Athenas lesson to a plan that is not completed or cancelled (draft, active or paused). The
        /// user fills the schedule: learning window (concept, definitions, examples), session dates, and the practices,
        /// exams and pre/post tests with their dates. Same rules as a lesson of the plan structure.
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">The lesson (LessonId, SubjectId, Sequence, optional StandardId) and its schedule.</param>
        /// <returns>201 with the new plan lesson; 400 validation errors (lesson or sequence already in the plan, subject not in the plan, dates, quizzes); 404 not found; 409 plan completed or cancelled; 503 Athenas unavailable; 401 without CustomPlanHandler.</returns>
        [HttpPost("{planId}/lessons")]
        public async Task<IActionResult> AddLesson(ulong planId, [FromBody] CustomPlanLessonRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.AddLessonAsync(user, planId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>
        /// Updates the progress of one lesson of an active plan. Marking it completed requires every practice and exam the
        /// lesson has in Athenas to be scheduled and completed (Athenas unreachable = 503).
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planLessonId">Id of the plan lesson.</param>
        /// <param name="request">Flags and lesson status to change.</param>
        /// <returns>200 with the lesson; 400 practices flag set while practices are scheduled; 404 not found; 409 plan not active or practices/exams pending; 503 Athenas unavailable; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/lessons/{planLessonId}/progress")]
        public async Task<IActionResult> UpdateLessonProgress(ulong planId, ulong planLessonId, [FromBody] CustomPlanLessonProgressRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.UpdateLessonProgressAsync(user, planId, planLessonId, request, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Adds the minutes the student studied a lesson to the lesson's and the plan's total (<c>totalStudyMinutes</c>).
        /// Called by the app at the end of a study session. The plan's client or its student, while the plan is active.
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planLessonId">Id of the plan lesson.</param>
        /// <param name="request">Minutes studied in this session (1-600).</param>
        /// <returns>200 with the new lesson and plan totals; 400 minutes out of range; 404 plan or lesson not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPost("{planId}/lessons/{planLessonId}/study-time")]
        public async Task<IActionResult> AddStudyTime(ulong planId, ulong planLessonId, [FromBody] CustomPlanStudyTimeRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.AddStudyTimeAsync(user, planId, planLessonId, request.Minutes, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Adds practices, exams or pre/post tests to one lesson of a plan that is not completed or cancelled (draft, active
        /// or paused), for example a quiz Athenas added after the plan was created. Same rules as the plan structure.
        /// </summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planLessonId">Id of the plan lesson.</param>
        /// <param name="request">Practices, exams and tests to add.</param>
        /// <returns>201 with the lesson; 400 validation errors; 404 not found; 409 plan closed; 503 Athenas unavailable; 401 without CustomPlanHandler.</returns>
        [HttpPost("{planId}/lessons/{planLessonId}/activities")]
        public async Task<IActionResult> AddLessonActivities(ulong planId, ulong planLessonId, [FromBody] CustomPlanLessonActivitiesRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _service.AddLessonActivitiesAsync(user, planId, planLessonId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Changes the status of a scheduled exam (not_started, in_progress, completed) of an active plan.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="examDateId">Id of the scheduled exam date.</param>
        /// <param name="request">New status.</param>
        /// <returns>200 with the exam; 404 not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/exams/{examDateId}/status")]
        public async Task<IActionResult> UpdateExamStatus(ulong planId, ulong examDateId, [FromBody] CustomPlanActivityStatusRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _activities.UpdateExamStatusAsync(user, planId, examDateId, request.Status, HttpContext.RequestAborted));
        }

        /// <summary>Links the result of a taken exam (usqui_user_quiz_results, is_exam = 1) and completes the exam.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="examDateId">Id of the scheduled exam date.</param>
        /// <param name="request">The usqui_user_quiz_results id of the student's attempt.</param>
        /// <returns>200 with the exam; 400 result not the student's attempt at that exam; 404 not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/exams/{examDateId}/result")]
        public async Task<IActionResult> LinkExamResult(ulong planId, ulong examDateId, [FromBody] LinkExamResultRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _activities.LinkExamResultAsync(user, planId, examDateId, request.GwpUserResultId, HttpContext.RequestAborted));
        }

        /// <summary>Changes the status of one practice (not_started, in_progress, completed) of an active plan.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planPracticeId">Id of the scheduled practice.</param>
        /// <param name="request">New status.</param>
        /// <returns>200 with the practice; 404 not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/practices/{planPracticeId}/status")]
        public async Task<IActionResult> UpdatePracticeStatus(ulong planId, ulong planPracticeId, [FromBody] CustomPlanActivityStatusRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _activities.UpdatePracticeStatusAsync(user, planId, planPracticeId, request.Status, HttpContext.RequestAborted));
        }

        /// <summary>Records the score of a practice attempt (kept by the plan, not in usqui_user_quiz_results) and completes the practice.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planPracticeId">Id of the scheduled practice.</param>
        /// <param name="request">Score (0-100) and optional correct / total questions.</param>
        /// <returns>200 with the practice; 400 invalid score; 404 not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/practices/{planPracticeId}/result")]
        public async Task<IActionResult> RecordPracticeResult(ulong planId, ulong planPracticeId, [FromBody] CustomPlanPracticeResultRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _activities.RecordPracticeResultAsync(user, planId, planPracticeId, request, HttpContext.RequestAborted));
        }

        /// <summary>Changes the status of a pre-test or post-test (not_started, in_progress, completed) of an active plan.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planTestId">Id of the scheduled test.</param>
        /// <param name="request">New status.</param>
        /// <returns>200 with the test; 404 not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/tests/{planTestId}/status")]
        public async Task<IActionResult> UpdateTestStatus(ulong planId, ulong planTestId, [FromBody] CustomPlanActivityStatusRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _activities.UpdateTestStatusAsync(user, planId, planTestId, request.Status, HttpContext.RequestAborted));
        }

        /// <summary>Links the result of a taken pre-test (is_exam = 2) or post-test (is_exam = 3) and completes the test.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planTestId">Id of the scheduled test.</param>
        /// <param name="request">The usqui_user_quiz_results id of the student's attempt.</param>
        /// <returns>200 with the test; 400 result not the student's attempt at that test; 404 not found; 409 plan not active; 401 without CustomPlanHandler.</returns>
        [HttpPut("{planId}/tests/{planTestId}/result")]
        public async Task<IActionResult> LinkTestResult(ulong planId, ulong planTestId, [FromBody] LinkExamResultRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            return FromResult(await _activities.LinkTestResultAsync(user, planId, planTestId, request.GwpUserResultId, HttpContext.RequestAborted));
        }

        /// <summary>Soft-deletes a plan. Only the plan's client.</summary>
        /// <param name="planId">Custom plan id.</param>
        /// <returns>200 with true; 404 not found; 401 without CustomPlanHandler.</returns>
        [HttpDelete("{planId}")]
        public async Task<IActionResult> Delete(ulong planId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return FromResult(await _service.DeleteAsync(user, planId, HttpContext.RequestAborted));
        }
    }
}
