using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and strategy dates.
    /// </summary>
    public class TeacherPlanStrategyDate
    {
        /// <summary>
        /// Unique identifier of the teacher plan strategy date.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan strategy id.
        /// </summary>
        public int TeacherPlanStrategyId { get; set; }

        /// <summary>
        /// Date on which the plan will be taught.
        /// </summary>
        public DateTime ImplementationDate { get; set; }

        /// <summary>
        /// The user who modified the strategy date.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the strategy date.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the strategy date was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlanStrategy TeacherPlanStrategy { get; set; }
    }
}
