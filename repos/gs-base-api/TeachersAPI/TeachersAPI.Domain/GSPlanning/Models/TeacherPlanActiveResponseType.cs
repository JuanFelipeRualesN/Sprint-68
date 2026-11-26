using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Models;

namespace GSPlanning.Models
{
    /// <summary>
    /// Represents the link between a teacher plan active response
    /// and the specific active response types assigned to it.
    /// </summary>
    public class TeacherPlanActiveResponseType
    {
        /// <summary>
        /// <summary>
        /// Primary key identifier of the relation record.
        /// Maps to column: id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the teacher plan active response.
        /// Maps to column: teacher_plan_active_response_id
        /// </summary>
        public int TeacherPlanActiveResponseId { get; set; }

        /// <summary>
        /// Identifier of the active response type.
        /// Maps to column: active_response_types_id
        /// </summary>
        public int ActiveResponseTypesId { get; set; }

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


        public virtual ActiveResponseType ActiveResponseType { get; set; }

        public virtual TeacherPlanActiveResponse TeacherPlanActiveResponse { get; set; }
    }
}
