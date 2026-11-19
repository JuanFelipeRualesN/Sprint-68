using System;
using System.Collections.Generic;
using System.Linq;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and strategies.
    /// </summary>
    public class TeacherPlanStrategy
    {
        /// <summary>
        /// Unique identifier of the teacher plan strategy.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The user who modified the strategy.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the strategy.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the strategy was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual string Description
        {
            get
            {
                if (TeacherPlanStrategyCategories.Any())
                {
                    return string.Join(", ",
                        TeacherPlanStrategyCategories.Select(x => x.StrategyCategory?.Name).ToArray());
                }

                return string.Empty;
            }
        }

        public virtual string Strategies
        {
            get
            {
                if (TeacherPlanStrategyTypes.Any())
                {
                    return string.Join(", ",
                        TeacherPlanStrategyTypes.Select(x => x.StrategyType?.Name).ToArray());
                }

                return string.Empty;
            }
        }

        public virtual ICollection<TeacherPlanStrategyCategory> TeacherPlanStrategyCategories { get; set; } = new List<TeacherPlanStrategyCategory>();

        public virtual ICollection<TeacherPlanStrategyDate> TeacherPlanStrategyDates { get; set; } = new List<TeacherPlanStrategyDate>();

        public virtual ICollection<TeacherPlanStrategyType> TeacherPlanStrategyTypes { get; set; } = new List<TeacherPlanStrategyType>();
    }
}
