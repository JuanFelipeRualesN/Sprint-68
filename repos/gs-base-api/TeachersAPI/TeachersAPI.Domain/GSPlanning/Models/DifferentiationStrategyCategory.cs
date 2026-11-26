using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Models;

namespace GSPlanning.Models
{
    public class DifferentiationStrategyCategory
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

        public virtual ICollection<DifferentiationStrategyType> DifferentiationStrategyTypes { get; set; } = new List<DifferentiationStrategyType>();

        public virtual ICollection<TeacherPlanDifferentiationStrategyCategory> TeacherPlanDifferentiationStrategyCategories { get; set; } = new List<TeacherPlanDifferentiationStrategyCategory>();
    }
}
