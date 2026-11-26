using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Models;

namespace GSPlanning.Models
{
    public class TeacherPlanActiveResponse
    {
        /// <summary>
        /// Primary key identifier of the teacher plan active response.
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
                if (TeacherPlanActiveResponseTypes.Any())
                {
                    return string.Join(", ",
                        TeacherPlanActiveResponseTypes.Select(x => x.ActiveResponseType?.Description).ToArray());
                }

                return string.Empty;
            }
        }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanActiveResponseDate> TeacherPlanActiveResponseDates { get; set; } = new List<TeacherPlanActiveResponseDate>();

        public virtual ICollection<TeacherPlanActiveResponseType> TeacherPlanActiveResponseTypes { get; set; } = new List<TeacherPlanActiveResponseType>();

        public virtual ICollection<TeacherPlanActiveResponseInclude> TeacherPlanActiveResponseIncludes { get; set; } = new List<TeacherPlanActiveResponseInclude>();
    }
}
