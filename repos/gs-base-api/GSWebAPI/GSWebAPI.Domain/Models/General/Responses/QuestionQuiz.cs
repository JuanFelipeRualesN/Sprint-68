using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{

    /// <summary>
    /// To handle the questions quizzes.
    /// </summary>
    public class QuestionQuiz
    {
        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the question responses.
        /// </summary>
        public List<QuestionResponse> QuestionResponses { get; set; }
    }
}