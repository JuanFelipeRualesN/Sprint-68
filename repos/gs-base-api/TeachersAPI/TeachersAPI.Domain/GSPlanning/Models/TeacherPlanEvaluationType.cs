using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the evaluation types related to the teacher plan
    /// </summary>
    public class TeacherPlanEvaluationType
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan initiative or project innovations id
        /// </summary>
        public int TeacherPlanEvaluationId { get; set; }

        /// <summary>
        /// The initiative or project innovation type id
        /// </summary>
        public int EvaluationTypeId { get; set; }

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

        public virtual EvaluationType EvaluationType { get; set; }

        public virtual TeacherPlanEvaluation TeacherPlanEvaluation { get; set; }
    }
}
