using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and praxis reflection.
    /// </summary>
    public class TeacherPlanPraxisReflection
    {
        /// <summary>
        /// Unique identifier of the teacher plan praxis reflection.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// Indicates whether the praxis reflection was completed or not.
        /// </summary>
        public int? IsCompleted { get; set; }

        /// <summary>
        /// More details about the praxis reflection.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// The user who modified the praxis reflection.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the praxis reflection.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the praxis reflection was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanPraxisReflectionCompletedReason> TeacherPlanPraxisReflectionCompletedReasons { get; set; } = new List<TeacherPlanPraxisReflectionCompletedReason>();

        public virtual ICollection<TeacherPlanPraxisReflectionDate> TeacherPlanPraxisReflectionDates { get; set; } = new List<TeacherPlanPraxisReflectionDate>();
    }
}
