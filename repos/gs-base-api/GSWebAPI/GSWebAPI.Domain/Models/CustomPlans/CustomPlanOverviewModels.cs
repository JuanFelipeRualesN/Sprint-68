using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;

namespace GSWebAPI.GSWebAPI.Domain.Models.CustomPlans
{
    /// <summary>
    /// Filters of <c>GET gswebapi/custom-plans/overview</c>. All optional.
    /// </summary>
    public class CustomPlanOverviewFilterModel
    {
        /// <summary>
        /// Children (students) to include. Empty = every child of the client in session. Required for staff. For a client,
        /// ids of children they have no plan for are left out.
        /// </summary>
        public List<ulong> StudentIds { get; set; } = new();

        /// <summary>
        /// One student, the same as sending it in <see cref="StudentIds"/> (<c>?studentId=44</c>). Both can be sent;
        /// the ids are combined.
        /// </summary>
        public ulong? StudentId { get; set; }

        /// <summary>
        /// Calendar window (inclusive). A lesson is returned when any of its areas (learning, any practice, exam or
        /// pre/post test, session dates) overlaps the window. Either end can be left open.
        /// </summary>
        public DateOnly? DateFrom { get; set; }

        /// <summary>End of the calendar window (inclusive). Not before <see cref="DateFrom"/>.</summary>
        public DateOnly? DateTo { get; set; }

        /// <summary>
        /// Calendar month, <c>yyyy-MM</c> (e.g. <c>2026-11</c>): the same as <see cref="DateFrom"/> = its first day and
        /// <see cref="DateTo"/> = its last day, for a month calendar. Only the lessons with an area (learning, practice,
        /// exam, pre/post test or session date) in that month are loaded and returned, from any plan, old ones included.
        /// Cannot be combined with <see cref="DateFrom"/>/<see cref="DateTo"/>.
        /// </summary>
        public string? Month { get; set; }

        /// <summary>
        /// Plan window (inclusive), on the plan's own dates: a plan is returned when its StartDate..EndDate overlaps the
        /// window (a plan without EndDate is open-ended). Use it to leave out old (or future) plans. Either end can be left
        /// open. Unlike <see cref="DateFrom"/>/<see cref="DateTo"/>, it does not filter the lessons of the plans returned.
        /// </summary>
        public DateOnly? PlanDateFrom { get; set; }

        /// <summary>End of the plan window (inclusive). Not before <see cref="PlanDateFrom"/>.</summary>
        public DateOnly? PlanDateTo { get; set; }

        /// <summary>Lesson score range, 0-100 (inclusive). Lessons without a score are left out when either is set.</summary>
        public double? MinScore { get; set; }

        /// <summary>Upper bound of the lesson score, 0-100 (inclusive). Not below <see cref="MinScore"/>.</summary>
        public double? MaxScore { get; set; }

        /// <summary>Plan statuses to include. Empty = all.</summary>
        public List<CustomPlanStatus> Statuses { get; set; } = new();
    }

    /// <summary>One child with their plans.</summary>
    public class CustomPlanStudentOverviewModel
    {
        /// <summary>ul_user_accounts.id of the student.</summary>
        public ulong StudentId { get; set; }

        /// <summary>Full name from the Users API (null when it cannot be read).</summary>
        public string? StudentName { get; set; }

        /// <summary>Plans of the student, newest start date first. Empty when no plan matches the filters.</summary>
        public List<CustomPlanOverviewModel> Plans { get; set; } = new();
    }

    /// <summary>One plan in the overview, with its subjects and lessons.</summary>
    public class CustomPlanOverviewModel
    {
        /// <summary>custom_plan_client.id</summary>
        public ulong Id { get; set; }

        /// <summary>custom_plan_types.code.</summary>
        public string? CustomPlanTypeCode { get; set; }

        /// <summary>draft, active, paused, completed or cancelled.</summary>
        public CustomPlanStatus Status { get; set; }

        /// <summary>Minutes the student has studied in the plan so far (all its lessons, not only those shown by the filters).</summary>
        public uint TotalStudyMinutes { get; set; }

        /// <summary>First day of the plan.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Last day of the plan (inclusive).</summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>Plan length in weeks.</summary>
        public ushort PlanDurationWeeks { get; set; }

        /// <summary>Athenas level of the plan.</summary>
        public CustomPlanCatalogItemModel Level { get; set; } = new();

        /// <summary>True when the plan is aligned to a curriculum framework.</summary>
        public bool UsesFramework { get; set; }

        /// <summary>The curriculum framework, or null when the plan has none.</summary>
        public CustomPlanFrameworkModel? Framework { get; set; }

        /// <summary>Average of the lesson scores returned (null if none has a score).</summary>
        public double? AverageScore { get; set; }

        /// <summary>Subjects of the plan with their lessons. With a date or score filter, subjects without matching lessons are left out.</summary>
        public List<CustomPlanSubjectOverviewModel> Subjects { get; set; } = new();

        /// <summary>Evidences and projects of the whole plan (not attached to a lesson), newest first.</summary>
        public List<CustomPlanEvidenceResponseModel> Evidences { get; set; } = new();
    }

    /// <summary>An Athenas catalog entry (subject, level). Code and name are null when Athenas cannot be reached.</summary>
    public class CustomPlanCatalogItemModel
    {
        /// <summary>Athenas id.</summary>
        public ulong Id { get; set; }

        /// <summary>Athenas code.</summary>
        public string? Code { get; set; }

        /// <summary>Athenas name, in the user's language.</summary>
        public string? Name { get; set; }
    }

    /// <summary>A curriculum framework (curriculum_frameworks).</summary>
    public class CustomPlanFrameworkModel
    {
        /// <summary>curriculum_frameworks.id</summary>
        public ushort Id { get; set; }

        /// <summary>Unique code, e.g. DEPR.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Display name.</summary>
        public string Name { get; set; } = null!;

        /// <summary>ISO 3166-1 alpha-2.</summary>
        public string CountryCode { get; set; } = null!;

        /// <summary>State or region, if any.</summary>
        public string? Region { get; set; }
    }

    /// <summary>One subject of a plan with its lessons.</summary>
    public class CustomPlanSubjectOverviewModel
    {
        /// <summary>Athenas subject.</summary>
        public CustomPlanCatalogItemModel Subject { get; set; } = new();

        /// <summary>Average of the lesson scores of this subject (null if none has a score).</summary>
        public double? AverageScore { get; set; }

        /// <summary>Lessons of the subject that match the filters, by sequence.</summary>
        public List<CustomPlanLessonOverviewModel> Lessons { get; set; } = new();
    }

    /// <summary>One lesson of a plan with every area (learning, practices, exams, pre/post tests), scores and evidences.</summary>
    public class CustomPlanLessonOverviewModel
    {
        /// <summary>custom_plan_client_lessons.id</summary>
        public ulong Id { get; set; }

        /// <summary>Athenas lesson id.</summary>
        public ulong LessonId { get; set; }

        /// <summary>From Athenas (null when Athenas cannot be reached).</summary>
        public string? Title { get; set; }

        /// <summary>Lesson number from Athenas (null when Athenas cannot be reached).</summary>
        public string? LessonNo { get; set; }

        /// <summary>Subject code from Athenas (null when Athenas cannot be reached).</summary>
        public string? SubjectCode { get; set; }

        /// <summary>Level code from Athenas (null when Athenas cannot be reached).</summary>
        public string? LevelCode { get; set; }

        /// <summary>The aligned standard, or null when the lesson has none.</summary>
        public CustomPlanStandardModel? Standard { get; set; }

        /// <summary>Order of the lesson within the plan.</summary>
        public ushort Sequence { get; set; }

        /// <summary>not_started, in_progress, completed or skipped.</summary>
        public PlanLessonStatus LessonStatus { get; set; }

        /// <summary>UTC. When the lesson was marked completed; null otherwise.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Minutes the student has studied this lesson so far.</summary>
        public uint TotalStudyMinutes { get; set; }

        /// <summary>Average of the exam results linked to the lesson (0-100). Null until an exam is taken. Pre/post tests and practices do not count.</summary>
        public double? Score { get; set; }

        /// <summary>Learning area: concept, definitions and examples.</summary>
        public CustomPlanLearningAreaModel Learning { get; set; } = new();

        /// <summary>All the practices of the lesson.</summary>
        public CustomPlanPracticesSummaryModel Practices { get; set; } = new();

        /// <summary>Scheduled exams, by deadline.</summary>
        public List<CustomPlanExamOverviewModel> Exams { get; set; } = new();

        /// <summary>Pre-test and post-test (optional).</summary>
        public List<CustomPlanTestOverviewModel> Tests { get; set; } = new();

        /// <summary>
        /// Practices the lesson has in Athenas that are not in the plan (e.g. added after the plan was created). The lesson
        /// cannot be completed until they are added (POST .../lessons/{id}/activities) and completed. Empty when Athenas
        /// cannot be reached.
        /// </summary>
        public List<ulong> MissingPracticeIds { get; set; } = new();

        /// <summary>Exams the lesson has in Athenas that are not in the plan. Same as <see cref="MissingPracticeIds"/>.</summary>
        public List<ulong> MissingExamIds { get; set; } = new();

        /// <summary>
        /// True when every practice and exam the lesson has in Athenas is in the plan and every scheduled practice and
        /// exam is completed (pre/post tests are optional). Null when Athenas cannot be reached.
        /// </summary>
        public bool? CanBeCompleted { get; set; }

        /// <summary>Pre-test part of the lesson done (lesson flag; follows the scheduled pre-test when there is one).</summary>
        public bool PreCompleted { get; set; }

        /// <summary>Exams part of the lesson done (lesson flag; follows the scheduled exams when there are any).</summary>
        public bool TestCompleted { get; set; }

        /// <summary>Post-test part of the lesson done (lesson flag; follows the scheduled post-test when there is one).</summary>
        public bool PostCompleted { get; set; }

        /// <summary>Study session dates.</summary>
        public List<DateOnly> SessionDates { get; set; } = new();

        /// <summary>Evidences and projects attached to this lesson, newest first.</summary>
        public List<CustomPlanEvidenceResponseModel> Evidences { get; set; } = new();
    }

    /// <summary>The standard a lesson is aligned to.</summary>
    public class CustomPlanStandardModel
    {
        /// <summary>Athenas standard id.</summary>
        public ulong Id { get; set; }

        /// <summary>From Athenas (null when it cannot be read).</summary>
        public string? Code { get; set; }

        /// <summary>Standard name from Athenas (null when it cannot be read).</summary>
        public string? Description { get; set; }
    }

    /// <summary>A scheduled area of a lesson: dates read in <see cref="Timezone"/>.</summary>
    public abstract class CustomPlanAreaModel
    {
        /// <summary>First day of the area, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>Last day of the area (inclusive), local to <see cref="Timezone"/>.</summary>
        public DateOnly DueDate { get; set; }

        /// <summary>IANA time zone.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>True when the area is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Not completed and the due date has passed in <see cref="Timezone"/>.</summary>
        public bool IsOverdue { get; set; }
    }

    /// <summary>Concept, definitions and examples. Completed when the three are done.</summary>
    public class CustomPlanLearningAreaModel : CustomPlanAreaModel
    {
        /// <summary>Concept done.</summary>
        public bool ConceptCompleted { get; set; }

        /// <summary>Definitions done.</summary>
        public bool DefinitionsCompleted { get; set; }

        /// <summary>Examples done.</summary>
        public bool ExamplesCompleted { get; set; }
    }

    /// <summary>All the practices of a lesson: totals plus one item per practice.</summary>
    public class CustomPlanPracticesSummaryModel
    {
        /// <summary>Number of scheduled practices.</summary>
        public int Total { get; set; }

        /// <summary>Number of completed practices.</summary>
        public int Completed { get; set; }

        /// <summary>True when the lesson has practices and all are completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>UTC. When the last practice was completed (only when all are).</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Practices not completed whose deadline has passed.</summary>
        public int Overdue { get; set; }

        /// <summary>Average of the practices' last scores (0-100). Null until one has a score.</summary>
        public double? AverageScore { get; set; }

        /// <summary>One item per practice, by sequence.</summary>
        public List<CustomPlanPracticeOverviewModel> Items { get; set; } = new();
    }

    /// <summary>Fields shared by practices, exams and pre/post tests.</summary>
    public abstract class CustomPlanActivityOverviewModel
    {
        /// <summary>Row id in the plan (custom_plan_client_lesson_practices, _exam_dates or _tests).</summary>
        public ulong Id { get; set; }

        /// <summary>From Athenas (null when it cannot be read).</summary>
        public string? Title { get; set; }

        /// <summary>Day it opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>not_started, in_progress or completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>True when <see cref="Status"/> is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Not completed and the deadline has passed.</summary>
        public bool IsOverdue { get; set; }
    }

    /// <summary>One scheduled practice (an Athenas quiz with IsExam = 0). Its score is kept by the plan.</summary>
    public class CustomPlanPracticeOverviewModel : CustomPlanActivityOverviewModel
    {
        /// <summary>Athenas quiz id.</summary>
        public ulong PracticeId { get; set; }

        /// <summary>Depth of knowledge (DOK) from Athenas.</summary>
        public string? LevelOfKnowledge { get; set; }

        /// <summary>Order within the lesson.</summary>
        public byte Sequence { get; set; }

        /// <summary>Last attempt, 0-100.</summary>
        public double? Score { get; set; }

        /// <summary>Best attempt, 0-100.</summary>
        public double? BestScore { get; set; }

        /// <summary>Correct answers of the last attempt, if known.</summary>
        public int? TotalCorrect { get; set; }

        /// <summary>Questions of the last attempt, if known.</summary>
        public int? TotalQuestions { get; set; }

        /// <summary>Number of recorded attempts.</summary>
        public int Attempts { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? LastAttemptAt { get; set; }
    }

    /// <summary>A pre-test or post-test. The result comes from usqui_user_quiz_results.</summary>
    public class CustomPlanTestOverviewModel : CustomPlanActivityOverviewModel
    {
        /// <summary>pre_test or post_test.</summary>
        public PlanTestType TestType { get; set; }

        /// <summary>Athenas quiz id.</summary>
        public ulong TestId { get; set; }

        /// <summary>Time allowed, in minutes.</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>The linked attempt, or null when none is linked.</summary>
        public CustomPlanExamResultModel? Result { get; set; }
    }

    /// <summary>A scheduled exam. The result comes from usqui_user_quiz_results.</summary>
    public class CustomPlanExamOverviewModel : CustomPlanActivityOverviewModel
    {
        /// <summary>Athenas quiz id.</summary>
        public ulong ExamId { get; set; }

        /// <summary>Time allowed, in minutes.</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>The linked attempt, or null when none is linked.</summary>
        public CustomPlanExamResultModel? Result { get; set; }
    }

    /// <summary>The linked usqui_user_quiz_results row (exam or pre/post test).</summary>
    public class CustomPlanExamResultModel
    {
        /// <summary>usqui_user_quiz_results.id</summary>
        public int ResultId { get; set; }

        /// <summary>Attempt number.</summary>
        public int? TryNo { get; set; }

        /// <summary>Correct answers.</summary>
        public int? TotalCorrect { get; set; }

        /// <summary>Number of questions.</summary>
        public int? TotalQuestions { get; set; }

        /// <summary>total_percent, 0-100, rounded to 2 decimals.</summary>
        public double? Score { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? TakenAt { get; set; }
    }

    /// <summary>
    /// Body of <c>POST gswebapi/custom-plans/{planId}/evidences</c> (multipart/form-data). Send either a file
    /// (uploaded to Blob Storage) or an external Url. Only the plan's client; not allowed on cancelled plans.
    /// </summary>
    public class CustomPlanEvidenceRequestModel
    {
        /// <summary>Required, up to 150 characters (trimmed).</summary>
        public string Title { get; set; } = null!;

        /// <summary>Optional, up to 5000 characters (trimmed; blank = null).</summary>
        public string? Description { get; set; }

        /// <summary>evidence or project.</summary>
        public CustomPlanEvidenceType EvidenceType { get; set; }

        /// <summary>custom_plan_client_lessons.id of a lesson of the same plan, or null for the whole plan.</summary>
        public ulong? PlanLessonId { get; set; }

        /// <summary>External link (absolute http/https, up to 1000 characters). Required when no file is sent; ignored when a file is sent.</summary>
        public string? Url { get; set; }
    }

    /// <summary>An evidence or project of a plan.</summary>
    public class CustomPlanEvidenceResponseModel
    {
        /// <summary>custom_plan_evidences.id</summary>
        public ulong Id { get; set; }

        /// <summary>custom_plan_client.id of the plan.</summary>
        public ulong CustomPlanClientId { get; set; }

        /// <summary>custom_plan_client_lessons.id, or null when it belongs to the whole plan.</summary>
        public ulong? PlanLessonId { get; set; }

        /// <summary>evidence or project.</summary>
        public CustomPlanEvidenceType EvidenceType { get; set; }

        /// <summary>Title.</summary>
        public string Title { get; set; } = null!;

        /// <summary>Optional description.</summary>
        public string? Description { get; set; }

        /// <summary>
        /// For uploaded files, a fresh read-only SAS link valid for 1 hour (request it again when it expires). For external
        /// evidences, the link as it was sent.
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>True when the evidence is a file stored in Blob Storage; false for an external link.</summary>
        public bool IsUploadedFile { get; set; }

        /// <summary>Original file name (up to 255 characters). Null for external links.</summary>
        public string? FileName { get; set; }

        /// <summary>Content type derived from the file extension. Null for external links.</summary>
        public string? ContentType { get; set; }

        /// <summary>File size in bytes. Null for external links.</summary>
        public ulong? SizeBytes { get; set; }

        /// <summary>ul_user_accounts.id of the user who added it.</summary>
        public ulong UploadedByUserId { get; set; }

        /// <summary>UTC.</summary>
        public DateTime CreatedAt { get; set; }
    }
}
