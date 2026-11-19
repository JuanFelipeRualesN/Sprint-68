using System;
using System.Collections.Generic;
using System.Linq;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the initiatives or projects innovations for a teacher plan
    /// </summary>
    public class TeacherPlanInitiativeProjectInnovation
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan id
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual string Description
        {
            get
            {
                if (TeacherPlanInitiativeProjectInnovationTypes.Any())
                {
                    return string.Join(", ",
                        TeacherPlanInitiativeProjectInnovationTypes.Select(x =>
                            x.InitiativeProjectInnovationType?.Description));
                }

                return string.Empty;
            }
        }
        
        public virtual TeacherPlan TeacherPlan { get; set; }
        
        public virtual ICollection<TeacherPlanInitiativeProjectInnovationDate> TeacherPlanInitiativeProjectInnovationDates { get; set; } = new List<TeacherPlanInitiativeProjectInnovationDate>();

        public virtual ICollection<TeacherPlanInitiativeProjectInnovationType> TeacherPlanInitiativeProjectInnovationTypes { get; set; } = new List<TeacherPlanInitiativeProjectInnovationType>();
    }
}
