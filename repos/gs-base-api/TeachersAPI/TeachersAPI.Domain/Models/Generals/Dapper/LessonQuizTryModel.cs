using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Models
{
    /// <summary>
    /// To handle the tries for lesson-quiz.
    /// </summary>
    public class LessonQuizTryModel
    {
        /// <summary>
        /// Gets or sets the lesson's id.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the tries quantities.
        /// </summary>
        public int TriesQuantity { get; set; }

        /// <summary>
        /// Gets or sets the quiz's id.
        /// </summary>
        public string QuizId { get; set; }
    }
}