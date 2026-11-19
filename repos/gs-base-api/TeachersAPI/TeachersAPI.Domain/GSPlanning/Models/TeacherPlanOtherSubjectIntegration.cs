using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the other subject integrations attached to a teacher plan
    /// </summary>
    public class TeacherPlanOtherSubjectIntegration
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan id
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; } = 1;

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description
        {
            get
            {
                if (TeacherPlanOtherSubjectInegrationSubjects.Any())
                {
                    return string.Join(", ",
                        TeacherPlanOtherSubjectInegrationSubjects.Select(x => x.SubjectName).ToArray());
                }

                return "";
            }
        }

        public virtual TeacherPlan TeacherPlan { get; set; }
        
        public virtual ICollection<TeacherPlanOtherSubjectIntegrationSubject> TeacherPlanOtherSubjectInegrationSubjects
        {
            get;
            set;
        } = new List<TeacherPlanOtherSubjectIntegrationSubject>();

        public virtual ICollection<TeacherPlanOtherSubjectIntegrationDate> TeacherPlanOtherSubjectIntegrationDates
        {
            get;
            set;
        } = new List<TeacherPlanOtherSubjectIntegrationDate>();
    }
}
