using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// o handle the available dates for the other subjects integrations
    /// </summary>
    public class TeacherPlanOtherSubjectIntegrationDate
    {
        /// <summary>
        /// Unique identifier of the teacher other subjects integrations date.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The other subject integration teacher’s plan id
        /// </summary>
        public int TeacherPlanOtherSubjectIntegrationId { get; set; }

        /// <summary>
        /// The weekday is available the other subject integration.
        /// </summary>
        public DateTime ImplementationDate { get; set; }

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

        public virtual TeacherPlanOtherSubjectIntegration TeacherPlanOtherSubjectIntegration { get; set; }
    }
}
