using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the initiatives or projects innovations types for a teacher plan
    /// </summary>
    public class TeacherPlanInitiativeProjectInnovationType
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan initiative or project innovations id
        /// </summary>
        public int TeacherPlanInitiativeProjectInnovationId { get; set; }

        /// <summary>
        /// The initiative or project innovation type id
        /// </summary>
        public int InitiativeProjectInnovationTypeId { get; set; }

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

        public virtual InitiativeProjectInnovationType InitiativeProjectInnovationType { get; set; }

        public virtual TeacherPlanInitiativeProjectInnovation TeacherPlanInitiativeProjectInnovation { get; set; }
    }
}
