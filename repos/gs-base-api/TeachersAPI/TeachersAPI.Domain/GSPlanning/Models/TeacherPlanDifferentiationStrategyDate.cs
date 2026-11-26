using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;

namespace GSPlanning.Models
{
    /// <summary>
    /// Represents the implementation dates for differentiation strategies
    /// in a teacher plan.
    /// </summary>
    public class TeacherPlanDifferentiationStrategyDate
    {
        /// <summary>
        /// Primary key identifier of the record.
        /// Maps to column: id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the teacher plan differentiation strategy.
        /// Maps to column: teacher_plan_differentiation_strategy_id
        /// </summary>
        public int TeacherPlanDifferentiationStrategyId { get; set; }

        /// <summary>
        /// Date when the differentiation strategy will be implemented.
        /// Maps to column: implementation_date
        /// </summary>
        public DateTime ImplementationDate { get; set; }

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
    }
}
