using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.External;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// custom_plan_client_lesson_exam_dates: an exam scheduled for a plan lesson. Unique per lesson, exam and assigned date.
    /// The score is read from the linked usqui_user_quiz_results row.
    /// </summary>
    public class CustomPlanClientLessonExamDate : IAuditableEntity, IPlanActivity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <inheritdoc/>
        public ulong CustomPlanClientLessonId { get; set; }

        /// <summary>Athenas quiz (exam) id (logical reference). Must be an exam of the lesson in Athenas.</summary>
        public ulong ExamId { get; set; }

        /// <summary>Day the exam opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC deadline to take the exam. Shown to the student in <see cref="Timezone"/>.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone of the exam (assigned date and display of the deadline).</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Minutes the student has to complete the exam once started (CHECK 1–480).</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>
        /// usqui_user_quiz_results.id once the student takes the exam (FK, RESTRICT). Signed INT (not BIGINT UNSIGNED) because
        /// the FK must match the PK type of the existing table exactly. Must be the student's attempt at this exam.
        /// </summary>
        public int? GwpUserResultId { get; set; }

        /// <summary>Exam status (DB default <c>not_started</c>). Linking a result sets it to completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>UTC. Set when the status becomes completed (linking the result completes it); cleared when it moves away.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Lesson the exam is scheduled for.</summary>
        public CustomPlanClientLesson Lesson { get; set; } = null!;

        /// <summary>Linked result in usqui_user_quiz_results, if any.</summary>
        public UserQuizResultRef? GwpUserResult { get; set; }
    }
}
