using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the praxis reflection completed reasons.
    /// </summary>
    public class PraxisReflectionCompletedReason
    {
        /// <summary>
        /// Unique identifier of the praxis reflection completed reason.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the praxis reflection completed reason.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the praxis reflection completed reason.
        /// </summary>
        public string Description { get; set; }

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

        public virtual ICollection<TeacherPlanPraxisReflectionCompletedReason> TeacherPlanPraxisReflectionCompletedReasons { get; set; } = new List<TeacherPlanPraxisReflectionCompletedReason>();
    }
}
