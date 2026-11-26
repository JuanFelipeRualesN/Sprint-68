using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To store the user&apos;s score according to a lesson (and according to a instructional plan that the lesson is part of).
    /// </summary>
    public class PlanLessonUser
    {
        /// <summary>
        /// An identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int? PlanId { get; set; }

        /// <summary>
        /// The teacher plan lesson id.
        /// </summary>
        public int? PlanLessonId { get; set; }

        /// <summary>
        /// The lesson&apos;s id from Athenas.
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// The user (student) id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The overall score (average) of the lesson.
        /// </summary>
        public float? OverallScore { get; set; }

        /// <summary>
        /// If the lesson was completed or not.
        /// </summary>
        public int? Completed { get; set; }

        /// <summary>
        /// The user that modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The date when the row was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The date when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlan Plan { get; set; }

        public virtual TeacherPlanLesson PlanLesson { get; set; }
    }
}
