using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;

namespace GSPlanning.Models
{
    /// <summary>
    /// Represents the differentiation strategies associated with a teacher plan.
    /// </summary>
    public class TeacherPlanDifferentiationStrategy
    {
        /// <summary>
        /// Primary key identifier of the teacher plan differentiation strategy.
        /// Maps to column: id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the teacher plan.
        /// Maps to column: teacher_plan_id
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// Identifier of the user who last modified the record.
        /// Maps to column: modified_by
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime when the record was created.
        /// Maps to column: created_at
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the record was last updated.
        /// Maps to column: updated_at
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual string Description
        {
            get
            {
                if (TeacherPlanDifferentiationStrategyTypes.Any())
                {
                    return string.Join(", ",
                        TeacherPlanDifferentiationStrategyTypes.Select(x => x.DifferentiationStrategyType?.Description).ToArray());
                }

                return string.Empty;
            }
        }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanDifferentiationStrategyDate> TeacherPlanDifferentiationStrategyDates { get; set; } = new List<TeacherPlanDifferentiationStrategyDate>();

        public virtual ICollection<TeacherPlanDifferentiationStrategyType> TeacherPlanDifferentiationStrategyTypes { get; set; } = new List<TeacherPlanDifferentiationStrategyType>();

        public virtual ICollection<TeacherPlanDifferentiationStrategyCategory> TeacherPlanDifferentiationStrategyCategories { get; set; } = new List<TeacherPlanDifferentiationStrategyCategory>();
    }
}
