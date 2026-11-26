using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the initiative or project innovation types
    /// </summary>
    public class InitiativeProjectInnovationType
    {
        public const int Default = 0;
        public const int Custom = 1;
        public int Id { get; set; }

        /// <summary>
        /// The name of the initiative or project innovation type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Descriminator used to identify the accommodation type: default (0), custom (1).
        /// </summary>
        public int Type { get; set; }

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

        public virtual ICollection<TeacherPlanInitiativeProjectInnovationType> TeacherPlanInitiativeProjectInnovationTypes { get; set; } = new List<TeacherPlanInitiativeProjectInnovationType>();
    }
}
