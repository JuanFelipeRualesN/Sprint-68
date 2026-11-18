using System;

namespace GSPlanning.Domain
{
    public class TeacherPlanOtherSubjectIntegrationSubject
    {
        public int Id { get; set; }

        /// <summary>
        /// The other subject integration teacher’s plan id's
        /// </summary>
        public int TeacherPlanOtherSubjectIntegrationId { get; set; }
        
        /// <summary>
        /// The subject id. It comes from Athenas.
        /// </summary>
        public int SubjectId { get; set; }
        
        /// <summary>
        /// The subject name. It comes from Athenas.
        /// </summary>
        public virtual string SubjectName { get; set; }
        
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