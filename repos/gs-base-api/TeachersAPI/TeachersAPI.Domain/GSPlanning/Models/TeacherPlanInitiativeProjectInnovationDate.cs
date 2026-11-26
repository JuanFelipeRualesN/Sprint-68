using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the available dates for the initiative or project innovations
    /// </summary>
    public class TeacherPlanInitiativeProjectInnovationDate
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan initiative or project innovations id
        /// </summary>
        public int TeacherPlanInitiativeProjectInnovationId { get; set; }

        /// <summary>
        /// The weekday is available  the custom lesson
        /// </summary>
        public DateTime ImplementationDate { get; set; }

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

        public virtual TeacherPlanInitiativeProjectInnovation TeacherPlanInitiativeProjectInnovation { get; set; }
    }
}
