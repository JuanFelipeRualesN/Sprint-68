using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSPlanning.Models
{
    public class TeacherPlanActiveResponseInclude
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
        /// Indicates if the student active response is included.
        /// If it equals 1, it is included; if it equals 0, it is not included.
        /// </summary>
        public int IncludesStudentActiveResponse { get; set; }

        /// <summary>
        /// Indicates if the learning check activity is included.
        /// If it equals 1, it is included; if it equals 0, it is not included.
        /// </summary>
        public int IncludesLearningCheckActivity { get; set; }

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

        public virtual TeacherPlanActiveResponse TeacherPlanActiveResponse { get; set; }
    }
}
