using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the lesson quiz user results from pre and post-tests.
    /// </summary>
    public class PlanPrePostUserResult
    {
        public int Id { get; set; }

        /// <summary>
        /// The plan lesson&apos;s id.
        /// </summary>
        public int? PlanLessonId { get; set; }

        /// <summary>
        /// The user (student&apos;s) id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The quiz&apos;s id from Athenas.
        /// </summary>
        public int? QuizId { get; set; }

        /// <summary>
        /// 0 for pre, 1 for post
        /// </summary>
        public int? PrePost { get; set; }

        /// <summary>
        /// The total of questions that the student got the response right.
        /// </summary>
        public int? TotalCorrect { get; set; }

        /// <summary>
        /// The total questions included in the quiz.
        /// </summary>
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// The score.
        /// </summary>
        public float? Score { get; set; }

        /// <summary>
        /// The user quiz result id from GS Web DB/API. Do not confuse this with usqui_user_results from this API.
        /// </summary>
        public long? UsquiUserResultId { get; set; }

        /// <summary>
        /// The user who modified the row.
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

        public virtual TeacherPlanLesson PlanLesson { get; set; }
    }
}
