using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;

namespace GSWebAPI.GSWebAPI.Domain.Models.CustomPlans
{
    /// <summary>
    /// Creates a plan in draft status. Only clients can create plans; the client is always the user in session. If Athenas
    /// or the Users API cannot be reached the request fails (503).
    /// </summary>
    public class CreateCustomPlanRequestModel
    {
        /// <summary>custom_plan_types.id. Must exist.</summary>
        public byte CustomPlanTypeId { get; set; }

        /// <summary>
        /// ul_user_accounts.id of the student. Required (not 0), cannot be the client in session, and must be a student
        /// profile the client manages (checked in the Users API).
        /// </summary>
        public ulong StudentId { get; set; }

        /// <summary>Athenas level id. Required (not 0).</summary>
        public ulong LevelId { get; set; }

        /// <summary>curriculum_frameworks.id of an active framework. Null = no official standards alignment.</summary>
        public ushort? CurriculumFrameworkId { get; set; }

        /// <summary>Plan length in weeks. At least 1.</summary>
        public ushort PlanDurationWeeks { get; set; }

        /// <summary>First day of the plan.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Last day of the plan (inclusive). Not before StartDate. Defaults to StartDate + PlanDurationWeeks weeks.</summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>Subjects, weekly schedule and lessons of the plan.</summary>
        public CustomPlanStructureRequestModel Structure { get; set; } = new();
    }

    /// <summary>
    /// Replaces the header fields and the structure of a draft plan (only the plan's client; only while it is draft). The
    /// student cannot change. Same rules as <see cref="CreateCustomPlanRequestModel"/>.
    /// </summary>
    public class UpdateCustomPlanDraftRequestModel
    {
        /// <summary>custom_plan_types.id. Must exist.</summary>
        public byte CustomPlanTypeId { get; set; }

        /// <summary>Athenas level id. Required (not 0).</summary>
        public ulong LevelId { get; set; }

        /// <summary>curriculum_frameworks.id of an active framework. Null = no official standards alignment.</summary>
        public ushort? CurriculumFrameworkId { get; set; }

        /// <summary>Plan length in weeks. At least 1.</summary>
        public ushort PlanDurationWeeks { get; set; }

        /// <summary>First day of the plan.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Last day of the plan (inclusive). Not before StartDate. Defaults to StartDate + PlanDurationWeeks weeks.</summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// The full desired structure. Existing rows are matched by their natural key (subject id, day of week, start
        /// time, lesson id, practice id, exam id + assigned date, test type + test id) and keep their id, status and
        /// results; rows not sent are removed.
        /// </summary>
        public CustomPlanStructureRequestModel Structure { get; set; } = new();
    }

    /// <summary>The editable structure of a plan: subjects, days and times, lessons and their dates and exams.</summary>
    public class CustomPlanStructureRequestModel
    {
        /// <summary>Athenas subject ids of the plan. No duplicates.</summary>
        public List<ulong> SubjectIds { get; set; } = new();

        /// <summary>Weekly study days. One entry per day of week.</summary>
        public List<CustomPlanDayRequestModel> Days { get; set; } = new();

        /// <summary>Lessons of the plan. Lesson ids and sequences must be unique within the plan.</summary>
        public List<CustomPlanLessonRequestModel> Lessons { get; set; } = new();
    }

    /// <summary>
    /// Body of <c>PUT gswebapi/custom-plans/{planId}/schedule</c>: the new weekly study days and their time slots (how
    /// many days a week and how long each session). Replaces the current schedule; lessons are not changed.
    /// </summary>
    public class CustomPlanScheduleRequestModel
    {
        /// <summary>Weekly study days, one entry per day of week. Days left out are removed; an empty list clears the schedule.</summary>
        public List<CustomPlanDayRequestModel> Days { get; set; } = new();
    }

    /// <summary>Body of <c>POST gswebapi/custom-plans/{planId}/subjects</c>.</summary>
    public class CustomPlanSubjectRequestModel
    {
        /// <summary>Athenas subject id to add to the plan. Not 0 and not already in the plan.</summary>
        public ulong SubjectId { get; set; }
    }

    /// <summary>A weekly study day and its time slots.</summary>
    public class CustomPlanDayRequestModel
    {
        /// <summary>ISO 8601: 1 = Monday ... 7 = Sunday.</summary>
        public byte DayOfWeek { get; set; }

        /// <summary>Time slots of the day. Start times must be unique within the day.</summary>
        public List<CustomPlanDayTimeRequestModel> Times { get; set; } = new();
    }

    /// <summary>A study time slot within a weekly day.</summary>
    public class CustomPlanDayTimeRequestModel
    {
        /// <summary>Local time in <see cref="Timezone"/>.</summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>Length of the slot in minutes. Greater than 0.</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>IANA time zone, e.g. America/Puerto_Rico. Must be valid.</summary>
        public string Timezone { get; set; } = null!;
    }

    /// <summary>Study time to add to a lesson (and to its plan).</summary>
    public class CustomPlanStudyTimeRequestModel
    {
        /// <summary>Minutes studied in this session. Required: 1 to 600 (10 hours) per report.</summary>
        public ushort Minutes { get; set; }
    }

    /// <summary>A lesson of the plan with its learning window, session dates and scheduled activities.</summary>
    public class CustomPlanLessonRequestModel
    {
        /// <summary>Athenas lesson id. Required (not 0) and unique within the plan.</summary>
        public ulong LessonId { get; set; }

        /// <summary>Athenas standard id. Must be null when the plan has no curriculum framework; otherwise it must belong to that framework.</summary>
        public ulong? StandardId { get; set; }

        /// <summary>Athenas subject id. Must be one of the plan's SubjectIds.</summary>
        public ulong SubjectId { get; set; }

        /// <summary>Order of the lesson within the plan. Unique per plan.</summary>
        public ushort Sequence { get; set; }

        /// <summary>
        /// Required. First day of the learning part (concept, definitions, examples), local to LearningTimezone. Not before
        /// the plan's StartDate.
        /// </summary>
        public DateOnly? LearningAssignedDate { get; set; }

        /// <summary>Required. Last day to finish the learning part (inclusive). Not before LearningAssignedDate, not after the plan's end date.</summary>
        public DateOnly? LearningDueDate { get; set; }

        /// <summary>Required. IANA time zone, e.g. America/Puerto_Rico.</summary>
        public string LearningTimezone { get; set; } = null!;

        /// <summary>Study session dates. No duplicates; each must fall inside the learning window.</summary>
        public List<DateOnly> AssignedDates { get; set; } = new();

        /// <summary>
        /// Scheduled exams (Athenas quizzes of the lesson with IsExam = 1). The same exam can be scheduled on several
        /// days, but (ExamId, AssignedDate) must be unique.
        /// </summary>
        public List<CustomPlanExamRequestModel> Exams { get; set; } = new();

        /// <summary>The practices of the lesson (the ones the lesson has in Athenas), each with its own schedule. Practice ids unique per lesson.</summary>
        public List<CustomPlanPracticeRequestModel> Practices { get; set; } = new();

        /// <summary>Pre-test and post-test of the lesson (optional). (TestType, TestId) must be unique.</summary>
        public List<CustomPlanTestRequestModel> Tests { get; set; } = new();
    }

    /// <summary>
    /// One scheduled practice. Opens on or after the lesson's learning start and inside the plan dates; the deadline
    /// (read in <see cref="Timezone"/>) is not before AssignedDate nor after the plan's end date. No duration.
    /// </summary>
    public class CustomPlanPracticeRequestModel
    {
        /// <summary>Athenas quiz id of the practice (a quiz of the lesson with IsExam = 0). Required (not 0).</summary>
        public ulong PracticeId { get; set; }

        /// <summary>Order within the lesson (1, 2, 3...). Not 0; unique per lesson.</summary>
        public byte Sequence { get; set; }

        /// <summary>Day the practice opens, local to Timezone.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>Required. Deadline with time; send it with an offset or as UTC (no offset is read as UTC). Stored in UTC, truncated to seconds.</summary>
        public DateTime? DueDateTime { get; set; }

        /// <summary>Required. IANA time zone.</summary>
        public string Timezone { get; set; } = null!;
    }

    /// <summary>
    /// A scheduled pre-test or post-test. Inside the plan dates; the deadline (read in <see cref="Timezone"/>) is not
    /// before AssignedDate. Unlike practices and exams, it may open before the learning window.
    /// </summary>
    public class CustomPlanTestRequestModel
    {
        /// <summary>pre_test or post_test.</summary>
        public PlanTestType TestType { get; set; }

        /// <summary>Athenas quiz id of the pre-test (IsExam = 2) or post-test (IsExam = 3). Must be a quiz of the lesson of that kind.</summary>
        public ulong TestId { get; set; }

        /// <summary>Day the test opens, local to Timezone.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>Required. Deadline with time; send it with an offset or as UTC (no offset is read as UTC). Stored in UTC, truncated to seconds.</summary>
        public DateTime? DueDateTime { get; set; }

        /// <summary>Required. IANA time zone.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Time allowed to take the test, 1 to 480 minutes.</summary>
        public ushort DurationMinutes { get; set; }
    }

    /// <summary>
    /// Practices, exams and pre/post tests to add to one lesson of a plan that already started (for example, a practice
    /// Athenas added to the lesson after the plan was created). Same fields and rules as in the plan structure. At least
    /// one item is required; they cannot repeat what the lesson already has. Not allowed on completed or cancelled plans.
    /// Adding a practice or exam to a completed lesson moves it back to in_progress.
    /// </summary>
    public class CustomPlanLessonActivitiesRequestModel
    {
        /// <summary>Practices to add. Practice ids and sequences must not repeat the lesson's existing ones.</summary>
        public List<CustomPlanPracticeRequestModel> Practices { get; set; } = new();

        /// <summary>Exams to add. (ExamId, AssignedDate) must not repeat the lesson's existing ones.</summary>
        public List<CustomPlanExamRequestModel> Exams { get; set; } = new();

        /// <summary>Pre/post tests to add. (TestType, TestId) must not repeat the lesson's existing ones.</summary>
        public List<CustomPlanTestRequestModel> Tests { get; set; } = new();
    }

    /// <summary>Changes the status of one practice, exam or test of an active plan.</summary>
    public class CustomPlanActivityStatusRequestModel
    {
        /// <summary>not_started, in_progress or completed. Completed sets CompletedAt (UTC); any other status clears it.</summary>
        public PlanActivityStatus Status { get; set; }
    }

    /// <summary>
    /// Score of a practice attempt. Practice results are not stored in usqui_user_quiz_results, so the plan keeps them.
    /// Each call counts one more attempt, keeps the last and best score and completes the practice (plan must be active).
    /// </summary>
    public class CustomPlanPracticeResultRequestModel
    {
        /// <summary>0-100. Stored rounded to 2 decimals.</summary>
        public decimal Score { get; set; }

        /// <summary>Correct answers, if known. Not greater than TotalQuestions when both are sent.</summary>
        public ushort? TotalCorrect { get; set; }

        /// <summary>Number of questions, if known.</summary>
        public ushort? TotalQuestions { get; set; }
    }

    /// <summary>
    /// One scheduled exam. Opens on or after the lesson's learning start and inside the plan dates; the deadline (read in
    /// <see cref="Timezone"/>) is not before AssignedDate nor after the plan's end date.
    /// </summary>
    public class CustomPlanExamRequestModel
    {
        /// <summary>Athenas quiz id (a quiz of the lesson with IsExam = 1). Required (not 0).</summary>
        public ulong ExamId { get; set; }

        /// <summary>Day the exam opens, local to Timezone.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>
        /// Required. Deadline with time. Send it with an offset or as UTC (like community events; no offset is read as
        /// UTC); it is stored in UTC, truncated to seconds.
        /// </summary>
        public DateTime? DueDateTime { get; set; }

        /// <summary>Required. IANA time zone of the exam.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Time allowed to take the exam, 1 to 480 minutes.</summary>
        public ushort DurationMinutes { get; set; }
    }

    /// <summary>Changes the status of a plan (only the plan's client).</summary>
    public class ChangeCustomPlanStatusRequestModel
    {
        /// <summary>
        /// New status. Allowed: draft -> active (needs at least one lesson) | cancelled; active -> paused | completed |
        /// cancelled; paused -> active | cancelled. Completed and cancelled are final. Sending the current status is a no-op.
        /// </summary>
        public CustomPlanStatus Status { get; set; }
    }

    /// <summary>Progress of one plan lesson (only while the plan is active). Null fields are left unchanged.</summary>
    public class CustomPlanLessonProgressRequestModel
    {
        /// <summary>
        /// New lesson status. Completed is accepted only when every practice and exam the lesson has in Athenas is
        /// scheduled and completed, and every scheduled one is completed (409 otherwise; 503 when Athenas is down).
        /// Completed sets CompletedAt; any other status clears it.
        /// </summary>
        public PlanLessonStatus? LessonStatus { get; set; }

        /// <summary>Concept done. With definitions and examples, sets LearningCompletedAt.</summary>
        public bool? ConceptCompleted { get; set; }

        /// <summary>Definitions done.</summary>
        public bool? DefinitionsCompleted { get; set; }

        /// <summary>Examples done.</summary>
        public bool? ExamplesCompleted { get; set; }

        /// <summary>Pre-test done. Recalculated from the scheduled pre-test whenever its status changes.</summary>
        public bool? PreCompleted { get; set; }

        /// <summary>Only for lessons without scheduled practices (rejected otherwise); with practices it follows their status.</summary>
        public bool? PracticesCompleted { get; set; }

        /// <summary>Exams done. Recalculated from the scheduled exams whenever one of them changes.</summary>
        public bool? TestCompleted { get; set; }

        /// <summary>Post-test done. Recalculated from the scheduled post-test whenever its status changes.</summary>
        public bool? PostCompleted { get; set; }
    }

    /// <summary>Links an exam or a pre/post test to the attempt in usqui_user_quiz_results and completes it (plan must be active).</summary>
    public class LinkExamResultRequestModel
    {
        /// <summary>
        /// usqui_user_quiz_results.id of the exam or test the student took. Must be the plan student's attempt at the
        /// same quiz and of the same kind (is_exam 1 for exams, also null on older rows; 2 pre-test; 3 post-test).
        /// </summary>
        public int GwpUserResultId { get; set; }
    }
}
