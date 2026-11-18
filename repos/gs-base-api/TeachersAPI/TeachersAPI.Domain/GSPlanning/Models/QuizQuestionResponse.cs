using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the questions&apos; responses of a quiz created by a teacher.
    /// </summary>
    public class QuizQuestionResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// The quiz question id of the quiz created by the teacher.
        /// </summary>
        public int? QuizQuestionId { get; set; }

        /// <summary>
        /// The question&apos;s response created by the teacher.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// If the response is correct (1) or not (0).
        /// </summary>
        public int? Correct { get; set; }

        /// <summary>
        /// The value assigned by teacher or auto assigned per response.
        /// </summary>
        public decimal? Value { get; set; }

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

        public virtual QuizQuestion QuizQuestion { get; set; }

        public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
}
