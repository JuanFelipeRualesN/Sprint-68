using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSPlanning.Models
{
    /// <summary>
    /// Represents the link between a teacher plan differentiation strategy
    /// and the specific differentiation strategy types assigned to it.
    /// </summary>
    public class TeacherPlanDifferentiationStrategyType
    {
        /// <summary>
        /// <summary>
        /// Primary key identifier of the relation record.
        /// Maps to column: id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the teacher plan differentiation strategy.
        /// Maps to column: teacher_plan_differentiation_strategy_id
        /// </summary>
        public int TeacherPlanDifferentiationStrategyId { get; set; }

        /// <summary>
        /// Identifier of the differentiation strategy type.
        /// Maps to column: differentiation_strategy_types_id
        /// </summary>
        public int DifferentiationStrategyTypesId { get; set; }

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

        
        public virtual DifferentiationStrategyType DifferentiationStrategyType { get; set; }

        public virtual TeacherPlanDifferentiationStrategy TeacherPlanDifferentiationStrategy { get; set; }
    }
}
