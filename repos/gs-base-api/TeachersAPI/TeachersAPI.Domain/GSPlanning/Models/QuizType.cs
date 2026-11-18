using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the quiz types (select, match, etc).
    /// </summary>
    public class QuizType
    {
        public int Id { get; set; }

        /// <summary>
        /// The quiz type&apos;s name (if select, true or false, etc).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the quiz type is auto correct or not (match, selections, etc). 
        /// </summary>
        public int? AutoCorrect { get; set; }

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

        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
