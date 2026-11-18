using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSPlanning.Models
{
    public class TeacherPlanActiveResponseDate
    {
        /// <summary>
        /// Primary key identifier of the record.
        /// Maps to column: id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the teacher plan active response.
        /// Maps to column: teacher_plan_active_response_id
        /// </summary>
        public int TeacherPlanActiveResponseId { get; set; }

        /// <summary>
        /// Date when the active response will be implemented.
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
