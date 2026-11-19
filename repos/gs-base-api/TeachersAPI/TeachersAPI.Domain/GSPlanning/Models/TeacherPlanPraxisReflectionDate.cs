using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and praxis reflection dates.
    /// </summary>
    public class TeacherPlanPraxisReflectionDate
    {
        /// <summary>
        /// Unique identifier of the teacher plan praxis reflection date.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan praxis reflection id.
        /// </summary>
        public int TeacherPlanPraxisReflectionId { get; set; }

        /// <summary>
        /// Date on which the plan will be taught.
        /// </summary>
        public DateTime ImplementationDate { get; set; }

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

        public virtual TeacherPlanPraxisReflection TeacherPlanPraxisReflection { get; set; }
    }
}
