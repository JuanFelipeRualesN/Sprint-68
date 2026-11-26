using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the available dates for the teacher plan evaluations
    /// </summary>
    public class TeacherPlanEvaluationsDate
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan evaluation id
        /// </summary>
        public int TeacherPlanEvaluationsId { get; set; }

        /// <summary>
        /// The weekday is available for the evaluation
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

        public virtual TeacherPlanEvaluation TeacherPlanEvaluations { get; set; }
    }
}
