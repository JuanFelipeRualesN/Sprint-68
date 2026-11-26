using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;

namespace GSPlanning.Models
{
    public class TeacherPlanDifferentiationStrategyCategory
    {
        /// <summary>
        /// The teacher plan&apos;s id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanDifferentiationStrategyId { get; set; }

        /// <summary>
        /// The strategy category&apos;s id.
        /// </summary>
        public int DifferentiationStrategyCategoryId { get; set; }

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
        public virtual DifferentiationStrategyCategory DifferentiationStrategyCategory { get; set; }

        public virtual TeacherPlanDifferentiationStrategy TeacherPlanDifferentiationStrategy { get; set; }
    }
}
