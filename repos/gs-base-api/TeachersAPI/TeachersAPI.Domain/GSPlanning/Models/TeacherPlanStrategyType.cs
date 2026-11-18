using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and strategy types.
    /// </summary>
    public class TeacherPlanStrategyType
    {
        /// <summary>
        /// Unique identifier of the teacher plan strategy type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan strategy id.
        /// </summary>
        public int TeacherPlanStrategyId { get; set; }

        /// <summary>
        /// The strategy type&apos;s id.
        /// </summary>
        public int StrategyTypeId { get; set; }

        /// <summary>
        /// The user who modified the strategy type.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the strategy type.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the strategy type was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual StrategyType StrategyType { get; set; }

        public virtual TeacherPlanStrategy TeacherPlanStrategy { get; set; }
    }
}
