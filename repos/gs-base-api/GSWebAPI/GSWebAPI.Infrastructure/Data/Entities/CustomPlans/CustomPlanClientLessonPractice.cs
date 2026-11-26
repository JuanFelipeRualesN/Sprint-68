using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// custom_plan_client_lesson_practices: one practice (an Athenas quiz with IsExam = 0) scheduled for a plan lesson.
    /// Scheduled like an exam (assigned date, due date-time, time zone, status), but with no duration, and the score is
    /// kept here because practice results are not stored in usqui_user_quiz_results. Unique per lesson and practice.
    /// </summary>
    public class CustomPlanClientLessonPractice : IAuditableEntity, IPlanActivity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <inheritdoc/>
        public ulong CustomPlanClientLessonId { get; set; }

        /// <summary>Athenas quiz id of the practice (logical reference). Must be a practice of the lesson in Athenas.</summary>
        public ulong PracticeId { get; set; }

        /// <summary>Order of the practice within the lesson (1, 2, 3...), unique per lesson (service rule).</summary>
        public byte Sequence { get; set; }

        /// <summary>Day the practice opens, local to <see cref="Timezone"/>.</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC deadline. Shown to the student in <see cref="Timezone"/>.</summary>
        public DateTime DueDateTime { get; set; }

        /// <summary>IANA time zone of the practice.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Practice status (DB default <c>not_started</c>). Recording an attempt sets it to completed.</summary>
        public PlanActivityStatus Status { get; set; }

        /// <summary>Score of the last attempt, 0–100 (DECIMAL(5,2), CHECK). Null until the first attempt.</summary>
        public decimal? Score { get; set; }

        /// <summary>Best score of all attempts, 0–100 (DECIMAL(5,2), CHECK). Null until the first attempt.</summary>
        public decimal? BestScore { get; set; }

        /// <summary>Correct answers of the last attempt; null if unknown.</summary>
        public ushort? TotalCorrect { get; set; }

        /// <summary>Questions of the last attempt; null if unknown.</summary>
        public ushort? TotalQuestions { get; set; }

        /// <summary>Number of recorded attempts (DB default 0). No per-attempt history is kept.</summary>
        public ushort Attempts { get; set; }

        /// <summary>UTC time of the last recorded attempt; null if none.</summary>
        public DateTime? LastAttemptAt { get; set; }

        /// <summary>UTC. Set when the status becomes completed; cleared when it moves away.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Lesson the practice is scheduled for.</summary>
        public CustomPlanClientLesson Lesson { get; set; } = null!;
    }
}
