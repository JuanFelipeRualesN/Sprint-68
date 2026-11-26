using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.External;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// custom_plan_client_lesson_tests: a pre-test or post-test (Athenas quiz with IsExam = 2 or 3) scheduled for a plan
    /// lesson. Scheduled like an exam, and its result lives in usqui_user_quiz_results like the exams'.
    /// Unique per lesson, test type and test id.
    /// </summary>
    public class CustomPlanClientLessonTest : IAuditableEntity, IPlanActivity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <inheritdoc/>
        public ulong CustomPlanClientLessonId { get; set; }

        /// <summary>Pre-test or post-test (<c>pre_test</c> / <c>post_test</c>).</summary>
        public PlanTestType TestType { get; set; }

        /// <summary>Athenas quiz id (logical reference). Must be a pre-test or post-test of the lesson in Athenas.</summary>
        public ulong TestId { get; set; }

        /// <summary>Day the test opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC deadline. Shown to the student in <see cref="Timezone"/>.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone of the test.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Minutes the student has to complete the test once started (CHECK 1–480).</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>Test status (DB default <c>not_started</c>). Linking a result sets it to completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>
        /// usqui_user_quiz_results.id of the attempt (FK, RESTRICT; is_exam = 2 or 3, matching <see cref="TestType"/>).
        /// Signed INT to match the existing table.
        /// </summary>
        public int? GwpUserResultId { get; set; }

        /// <summary>UTC. Set when the status becomes completed; cleared when it moves away.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Lesson the test is scheduled for.</summary>
        public CustomPlanClientLesson Lesson { get; set; } = null!;

        /// <summary>Linked result in usqui_user_quiz_results, if any.</summary>
        public UserQuizResultRef? GwpUserResult { get; set; }
    }
}
