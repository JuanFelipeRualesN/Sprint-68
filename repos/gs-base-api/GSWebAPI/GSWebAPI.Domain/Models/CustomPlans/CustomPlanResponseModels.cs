using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;

namespace GSWebAPI.GSWebAPI.Domain.Models.CustomPlans
{
    /// <summary>Header of a plan (custom_plan_client), as listed by the plans endpoint.</summary>
    public class CustomPlanStudyTimeResponseModel
    {
        /// <summary>Custom plan id.</summary>
        public ulong PlanId { get; set; }

        /// <summary>Plan lesson id.</summary>
        public ulong PlanLessonId { get; set; }

        /// <summary>Minutes added by this report.</summary>
        public ushort AddedMinutes { get; set; }

        /// <summary>The lesson's total after this report.</summary>
        public uint LessonTotalStudyMinutes { get; set; }

        /// <summary>The plan's total after this report.</summary>
        public uint PlanTotalStudyMinutes { get; set; }
    }

    /// <summary>A custom plan type (catalog), with its name in the session's language.</summary>
    public class CustomPlanTypeResponseModel
    {
        /// <summary>custom_plan_types.id (the value for CustomPlanTypeId when creating a plan).</summary>
        public byte Id { get; set; }

        /// <summary>Stable code, e.g. homeschool. It does not change with the language.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Name in the session's language (Accept-Language); English when the language has no translation.</summary>
        public string Name { get; set; } = null!;
    }

    /// <summary>A plan without its structure (lists).</summary>
    public class CustomPlanSummaryModel
    {
        /// <summary>custom_plan_client.id</summary>
        public ulong Id { get; set; }

        /// <summary>custom_plan_types.id</summary>
        public byte CustomPlanTypeId { get; set; }

        /// <summary>custom_plan_types.code.</summary>
        public string? CustomPlanTypeCode { get; set; }

        /// <summary>ul_user_accounts.id of the client (parent) who owns the plan.</summary>
        public ulong ClientId { get; set; }

        /// <summary>Client's full name from the Users API (null when it cannot be read).</summary>
        public string? ClientName { get; set; }

        /// <summary>ul_user_accounts.id of the student.</summary>
        public ulong StudentId { get; set; }

        /// <summary>Student's full name from the Users API (null when it cannot be read).</summary>
        public string? StudentName { get; set; }

        /// <summary>Athenas level id.</summary>
        public ulong LevelId { get; set; }

        /// <summary>curriculum_frameworks.id. Null = no official standards alignment.</summary>
        public ushort? CurriculumFrameworkId { get; set; }

        /// <summary>curriculum_frameworks.code (e.g. DEPR). Null when the plan has no framework.</summary>
        public string? CurriculumFrameworkCode { get; set; }

        /// <summary>Plan length in weeks.</summary>
        public ushort PlanDurationWeeks { get; set; }

        /// <summary>First day of the plan.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Last day of the plan (inclusive).</summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>draft, active, paused, completed or cancelled.</summary>
        public CustomPlanStatus Status { get; set; }

        /// <summary>Minutes the student has studied in the plan so far (sum of its lessons).</summary>
        public uint TotalStudyMinutes { get; set; }

        /// <summary>UTC.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC.</summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>A plan with its full structure: subjects, weekly days and lessons with their activities.</summary>
    public class CustomPlanResponseModel : CustomPlanSummaryModel
    {
        /// <summary>Athenas subject ids of the plan, ascending.</summary>
        public List<ulong> SubjectIds { get; set; } = new();

        /// <summary>Weekly study days, by day of week.</summary>
        public List<CustomPlanDayResponseModel> Days { get; set; } = new();

        /// <summary>Lessons, by sequence.</summary>
        public List<CustomPlanLessonResponseModel> Lessons { get; set; } = new();
    }

    /// <summary>A weekly study day of the plan.</summary>
    public class CustomPlanDayResponseModel
    {
        /// <summary>custom_plan_client_days.id</summary>
        public ulong Id { get; set; }

        /// <summary>ISO 8601: 1 = Monday ... 7 = Sunday.</summary>
        public byte DayOfWeek { get; set; }

        /// <summary>Time slots of the day, by start time.</summary>
        public List<CustomPlanDayTimeResponseModel> Times { get; set; } = new();
    }

    /// <summary>A study time slot of a weekly day.</summary>
    public class CustomPlanDayTimeResponseModel
    {
        /// <summary>custom_plan_client_day_times.id</summary>
        public ulong Id { get; set; }

        /// <summary>Local time in <see cref="Timezone"/>.</summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>Length of the slot in minutes.</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>IANA time zone.</summary>
        public string Timezone { get; set; } = null!;
    }

    /// <summary>A lesson of the plan with its schedule, progress flags and activities.</summary>
    public class CustomPlanLessonResponseModel
    {
        /// <summary>custom_plan_client_lessons.id (the planLessonId of the routes).</summary>
        public ulong Id { get; set; }

        /// <summary>Athenas lesson id.</summary>
        public ulong LessonId { get; set; }

        /// <summary>Athenas standard id. Null when the plan has no framework or the lesson has no standard.</summary>
        public ulong? StandardId { get; set; }

        /// <summary>Athenas subject id (one of the plan's subjects).</summary>
        public ulong SubjectId { get; set; }

        /// <summary>Order of the lesson within the plan.</summary>
        public ushort Sequence { get; set; }

        /// <summary>First day of the learning part (concept, definitions, examples), local to <see cref="LearningTimezone"/>.</summary>
        public DateOnly LearningAssignedDate { get; set; }

        /// <summary>Last day to finish the learning part (inclusive), local to <see cref="LearningTimezone"/>.</summary>
        public DateOnly LearningDueDate { get; set; }

        /// <summary>IANA time zone of the learning dates.</summary>
        public string LearningTimezone { get; set; } = null!;

        /// <summary>UTC. Set when concept, definitions and examples are all completed; null otherwise.</summary>
        public DateTime? LearningCompletedAt { get; set; }

        /// <summary>not_started, in_progress, completed or skipped.</summary>
        public PlanLessonStatus LessonStatus { get; set; }

        /// <summary>Concept done.</summary>
        public bool ConceptCompleted { get; set; }

        /// <summary>Definitions done.</summary>
        public bool DefinitionsCompleted { get; set; }

        /// <summary>Examples done.</summary>
        public bool ExamplesCompleted { get; set; }

        /// <summary>Pre-test done (follows the scheduled pre-test when there is one).</summary>
        public bool PreCompleted { get; set; }

        /// <summary>Practices done (when the lesson has scheduled practices, true once all are completed).</summary>
        public bool PracticesCompleted { get; set; }

        /// <summary>Exams done (when the lesson has scheduled exams, true once all are completed).</summary>
        public bool TestCompleted { get; set; }

        /// <summary>Post-test done (follows the scheduled post-test when there is one).</summary>
        public bool PostCompleted { get; set; }

        /// <summary>UTC. Set when <see cref="LessonStatus"/> becomes completed; null otherwise.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Minutes the student has studied this lesson so far.</summary>
        public uint TotalStudyMinutes { get; set; }

        /// <summary>Study session dates, ascending.</summary>
        public List<DateOnly> AssignedDates { get; set; } = new();

        /// <summary>Scheduled exams, by assigned date.</summary>
        public List<CustomPlanExamResponseModel> Exams { get; set; } = new();

        /// <summary>Scheduled practices, by sequence.</summary>
        public List<CustomPlanPracticeResponseModel> Practices { get; set; } = new();

        /// <summary>Scheduled pre-test and post-test.</summary>
        public List<CustomPlanTestResponseModel> Tests { get; set; } = new();
    }

    /// <summary>A scheduled practice. Its score is stored on the plan (not in usqui_user_quiz_results).</summary>
    public class CustomPlanPracticeResponseModel
    {
        /// <summary>custom_plan_client_lesson_practices.id (the planPracticeId of the routes).</summary>
        public ulong Id { get; set; }

        /// <summary>Athenas quiz id.</summary>
        public ulong PracticeId { get; set; }

        /// <summary>Order within the lesson.</summary>
        public byte Sequence { get; set; }

        /// <summary>Day the practice opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone of the practice.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>not_started, in_progress or completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>Last attempt, 0-100. Null until an attempt is recorded.</summary>
        public decimal? Score { get; set; }

        /// <summary>Best attempt, 0-100. Null until an attempt is recorded.</summary>
        public decimal? BestScore { get; set; }

        /// <summary>Correct answers of the last attempt, if known.</summary>
        public ushort? TotalCorrect { get; set; }

        /// <summary>Questions of the last attempt, if known.</summary>
        public ushort? TotalQuestions { get; set; }

        /// <summary>Number of recorded attempts.</summary>
        public ushort Attempts { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? LastAttemptAt { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? CompletedAt { get; set; }
    }

    /// <summary>A scheduled pre-test or post-test.</summary>
    public class CustomPlanTestResponseModel
    {
        /// <summary>custom_plan_client_lesson_tests.id (the planTestId of the routes).</summary>
        public ulong Id { get; set; }

        /// <summary>pre_test or post_test.</summary>
        public PlanTestType TestType { get; set; }

        /// <summary>Athenas quiz id.</summary>
        public ulong TestId { get; set; }

        /// <summary>Day the test opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone of the test.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Time allowed, in minutes (1-480).</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>not_started, in_progress or completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>usqui_user_quiz_results.id; the score is read from there.</summary>
        public int? GwpUserResultId { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? CompletedAt { get; set; }
    }

    /// <summary>A scheduled exam.</summary>
    public class CustomPlanExamResponseModel
    {
        /// <summary>custom_plan_client_lesson_exam_dates.id (the examDateId of the routes).</summary>
        public ulong Id { get; set; }

        /// <summary>Athenas quiz id.</summary>
        public ulong ExamId { get; set; }

        /// <summary>Day the exam opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone of the exam.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Time allowed, in minutes (1-480).</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>usqui_user_quiz_results.id; the grade is read from there.</summary>
        public int? GwpUserResultId { get; set; }

        /// <summary>not_started, in_progress or completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>UTC.</summary>
        public DateTime? CompletedAt { get; set; }
    }
}
