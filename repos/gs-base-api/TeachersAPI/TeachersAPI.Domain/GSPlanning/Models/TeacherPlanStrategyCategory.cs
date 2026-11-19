using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and strategy category.
    /// </summary>
    public class TeacherPlanStrategyCategory
    {
        /// <summary>
        /// The teacher plan&apos;s id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanStrategyId { get; set; }

        /// <summary>
        /// The strategy category&apos;s id.
        /// </summary>
        public int StrategyCategoryId { get; set; }

        /// <summary>
        /// The user who modified the strategy category.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the strategy category.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the strategy category was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        public virtual StrategyCategory StrategyCategory { get; set; }

        public virtual TeacherPlanStrategy TeacherPlanStrategy { get; set; }
    }
}
