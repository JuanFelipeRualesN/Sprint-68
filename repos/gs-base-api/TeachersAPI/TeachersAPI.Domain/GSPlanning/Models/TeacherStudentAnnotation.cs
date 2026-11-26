using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the annotations created by a teacher about a student in a teacher plan.
    /// </summary>
    public class TeacherStudentAnnotation
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s id (user&apos;s id).
        /// </summary>
        public int? TeacherId { get; set; }

        /// <summary>
        /// The student&apos;s id (student&apos;s id).
        /// </summary>
        public int? StudentId { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int? TeacherPlanId { get; set; }

        /// <summary>
        /// Annotations created by teacher about the student.
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlan TeacherPlan { get; set; }
    }
}
