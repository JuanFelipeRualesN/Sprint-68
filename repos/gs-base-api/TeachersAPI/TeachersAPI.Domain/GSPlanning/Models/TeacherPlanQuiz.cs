using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relation between the quizzes/homework created by the teacher and the teacher&apos;s instructional plan.
    /// </summary>
    public class TeacherPlanQuiz
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// The quiz&apos;s id (quiz created by the teacher).
        /// </summary>
        public int QuizId { get; set; }

        /// <summary>
        /// The quantity of tries that the student has to repeat the homework. As 2023.04.03 this is not used. 1 is set as default.
        /// </summary>
        public int? TriesQuantity { get; set; }

        /// <summary>
        /// If the quiz is available or not in the teacher&apos;s plan.
        /// </summary>
        public int? Available { get; set; }

        /// <summary>
        /// The date when the student is suppose to complete the quiz.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 1 if it is for grading, 0 if not.
        /// </summary>
        public int? ForGrading { get; set; }

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

        public virtual TeacherPlan Plan { get; set; }

        public virtual ICollection<PlanAssignmentUserResult> PlanAssignmentUserResults { get; set; } = new List<PlanAssignmentUserResult>();

        public virtual Quiz Quiz { get; set; }
    }
}
