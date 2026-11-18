using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the files attached by a student for a quiz/homework created by a teacher.
    /// </summary>
    public class UserQuizFile
    {
        public int Id { get; set; }

        /// <summary>
        /// The user&apos;s id (the student&apos;s id), who uploaded the file. This comes from gs_users_db_prod user&apos;s account table. 
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The quiz&apos;s id (the quiz created by the teacher).
        /// </summary>
        public int? QuizId { get; set; }

        /// <summary>
        /// The file&apos;s directory.
        /// </summary>
        public string FileUrl { get; set; }

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

        public virtual Quiz Quiz { get; set; }
    }
}
