using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>custom_plan_client_lesson_dates: a study-session date a lesson is scheduled on. Unique per lesson and date.</summary>
    public class CustomPlanClientLessonDate : IAuditableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_client_lessons (CASCADE).</summary>
        public ulong CustomPlanClientLessonId { get; set; }

        /// <summary>Session date; must fall inside the lesson's learning window (service rule).</summary>
        public DateOnly AssignedDate { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Lesson the date belongs to.</summary>
        public CustomPlanClientLesson Lesson { get; set; } = null!;
    }
}
