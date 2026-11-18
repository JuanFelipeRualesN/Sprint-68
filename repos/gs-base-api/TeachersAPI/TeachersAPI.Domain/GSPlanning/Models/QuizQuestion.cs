using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the questions of a quiz created by a teacher.
    /// </summary>
    public class QuizQuestion
    {
        public int Id { get; set; }

        /// <summary>
        /// The quiz id created by the teacher.
        /// </summary>
        public int? QuizId { get; set; }

        public int? QuestionTypeId { get; set; }

        /// <summary>
        /// The question.
        /// </summary>
        public string Desc { get; set; }

        public float? CustomValue { get; set; }

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

        public virtual QuizQuestionType QuestionType { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<QuizQuestionResponse> QuizQuestionResponses { get; set; } = new List<QuizQuestionResponse>();

        public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
}
