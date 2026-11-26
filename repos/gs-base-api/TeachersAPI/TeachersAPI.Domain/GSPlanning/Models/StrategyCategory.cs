using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the strategies categories.
    /// </summary>
    public class StrategyCategory
    {
        /// <summary>
        /// Unique identifier of the strategy category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the strategy category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the strategy category.
        /// </summary>
        public string Description { get; set; }

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

        public virtual ICollection<StrategyType> StrategyTypes { get; set; } = new List<StrategyType>();

        public virtual ICollection<TeacherPlanStrategyCategory> TeacherPlanStrategyCategories { get; set; } = new List<TeacherPlanStrategyCategory>();
    }
}
