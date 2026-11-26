using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the attachments for a quiz created by the teacher.
    /// </summary>
    public class QuizAttachment
    {
        public int Id { get; set; }

        /// <summary>
        /// The quiz id from the quiz created by the teacher.
        /// </summary>
        public int? QuizId { get; set; }

        /// <summary>
        /// The quizzes&apos; attachment url. 
        /// </summary>
        public string AttachmentUrl { get; set; }

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
