using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the lesson quiz user results.
    /// </summary>
    public class PlanLessonQuizUserResult
    {
        public int Id { get; set; }

        /// <summary>
        /// The plan lesson&apos;s id.
        /// </summary>
        public int? PlanLessonId { get; set; }

        /// <summary>
        /// 1 if the teacher manually corrected the quiz. 0 if not.
        /// </summary>
        public int? ManuallyCorrectedByTeacher { get; set; }

        /// <summary>
        /// The user (student&apos;s) id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The quiz&apos;s id from Athenas.
        /// </summary>
        public int? QuizId { get; set; }

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
