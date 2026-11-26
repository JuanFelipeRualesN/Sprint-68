using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and praxis reflection completed reasons.
    /// </summary>
    public class TeacherPlanPraxisReflectionCompletedReason
    {
        /// <summary>
        /// Unique identifier of the teacher plan praxis reflection completed reason.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanPraxisReflectionId { get; set; }

        /// <summary>
        /// The praxis reflection completed reason&apos;s id.
        /// </summary>
        public int PraxisReflectionCompletedReasonId { get; set; }

        /// <summary>
        /// The user who modified the praxis reflection completed reason.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the praxis reflection completed reason.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the praxis reflection completed reason was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual PraxisReflectionCompletedReason PraxisReflectionCompletedReason { get; set; }

        public virtual TeacherPlanPraxisReflection TeacherPlanPraxisReflection { get; set; }
    }
}
