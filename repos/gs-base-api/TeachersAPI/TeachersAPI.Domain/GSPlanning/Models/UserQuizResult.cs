using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the quiz/homework results of a quiz created by the teacher.
    /// </summary>
    public class UserQuizResult
    {
        public int Id { get; set; }

        /// <summary>
        /// The try number (how many times the user has taken the same quiz). Not used for this project as 03.17.21
        /// </summary>
        public int? TryNo { get; set; }

        /// <summary>
        /// The ID of the user who took the quiz. This comes from gs_users_db_prod user&apos;s account table. 
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The quiz id. This is the quiz id from a quiz created by the teacher. (homework)
        /// </summary>
        public int? QuizId { get; set; }

        /// <summary>
        /// The level of knowledge. It is not used for this project (03.17.2021).
        /// </summary>
        public int? Dok { get; set; }

        /// <summary>
        /// 1 if exam, 0 if practice. Not used for this project as 03.17.2021.
        /// </summary>
        public int? IsExam { get; set; }

        /// <summary>
        /// The Id of the lesson. Not used for this project as 2021.03.17.
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// The subject&apos;s code. Not used for this project. The subject and level are retrieve from the teacher&apos;s plan.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The level&apos;s code. Not used for this project. The subject and level are retrieve from the teacher&apos;s plan.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// How many questions the student got right.
        /// </summary>
        public int? TotalCorrect { get; set; }

        /// <summary>
        /// The total questions of the quiz.
        /// </summary>
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// The student&apos;s score. 
        /// </summary>
        public float? TotalPercent { get; set; }

        /// <summary>
        /// The feedback given by the teacher to the student.
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// The status. C for completed, P for pending, etc. 
        /// </summary>
        public string Status { get; set; }

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

        public virtual ICollection<PlanAssignmentUserResult> PlanAssignmentUserResults { get; set; } = new List<PlanAssignmentUserResult>();

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
}
