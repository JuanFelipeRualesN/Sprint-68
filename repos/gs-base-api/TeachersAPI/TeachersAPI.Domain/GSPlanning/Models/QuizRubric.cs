using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the rubrics of a quiz created by a teacher.
    /// </summary>
    public class QuizRubric
    {
        public int Id { get; set; }

        /// <summary>
        /// The quiz&apos;s  id.
        /// </summary>
        public int? QuizId { get; set; }

        /// <summary>
        /// The rubric&apos;s description.
        /// </summary>
        public string Desc { get; set; }

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
    }
}
