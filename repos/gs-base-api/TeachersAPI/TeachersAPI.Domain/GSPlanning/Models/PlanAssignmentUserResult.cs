using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To stablish a relationship between the curricular plan and the user results.
    /// </summary>
    public class PlanAssignmentUserResult
    {
        /// <summary>
        /// An identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The curricular plan quiz id (from tm_teacher_plan_quiz).
        /// </summary>
        public int? PlanQuizId { get; set; }

        /// <summary>
        /// The user (student) id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 1 if the teacher manually corrected the quiz. 0 if not.
        /// </summary>
        public int? ManuallyCorrectedByTeacher { get; set; }

        /// <summary>
        /// The score of the user.
        /// </summary>
        public float? Score { get; set; }

        /// <summary>
        /// The total percentage after calculating the score of the student vs. the max score. 
        /// </summary>
        public float? TotalPercent { get; set; }

        /// <summary>
        /// The assignment result from usqui_user_quiz_results from Teachers DB (Do not confuse this with GS Web DB).
        /// </summary>
        public int? UsquiUserResultId { get; set; }

        /// <summary>
        /// The user that modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The date when the row was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The date when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlanQuiz PlanQuiz { get; set; }

        public virtual UserQuizResult UsquiUserResult { get; set; }
    }
}
