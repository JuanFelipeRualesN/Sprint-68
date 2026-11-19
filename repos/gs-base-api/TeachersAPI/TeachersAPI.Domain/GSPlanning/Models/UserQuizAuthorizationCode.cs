using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// Authorization codes for the student for taking an assignment.
    /// </summary>
    public class UserQuizAuthorizationCode
    {
        public int Id { get; set; }

        /// <summary>
        /// The user&apos;s id (student&apos;s id). This comes from gs_users_db_prod user&apos;s account table. 
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The authorization&apos;s code so the student can access to the quiz.
        /// </summary>
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// The custom quiz&apos;s id (quiz created by the teacher).
        /// </summary>
        public int? CustomQuizId { get; set; }

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

        public virtual Quiz CustomQuiz { get; set; }
    }
}
