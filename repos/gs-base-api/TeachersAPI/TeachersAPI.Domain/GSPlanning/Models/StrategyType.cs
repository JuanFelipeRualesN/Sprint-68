using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the strategies types.
    /// </summary>
    public class StrategyType
    {
        public const int IsCustom = 1;
        public const int IsDefault = 0;
        
        /// <summary>
        /// Unique identifier of the strategy type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the strategy type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the strategy type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The strategy category&apos;s id.
        /// </summary>
        public int StrategyCategoryId { get; set; }

        /// <summary>
        /// Descriminator used to identify the strategy type: default (0), custom (1).
        /// </summary>
        public int Type { get; set; }

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

        public virtual StrategyCategory StrategyCategory { get; set; }

        public virtual ICollection<TeacherPlanStrategyType> TeacherPlanStrategyTypes { get; set; } = new List<TeacherPlanStrategyType>();
    }
}
