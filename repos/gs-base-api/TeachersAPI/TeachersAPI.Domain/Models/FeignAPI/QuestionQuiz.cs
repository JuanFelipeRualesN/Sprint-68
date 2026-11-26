using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.FeignAPI
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

    /// <summary>
    /// To handle the questions responses.
    /// </summary>
    public class QuestionResponse
    {
        /// <summary>
        /// Gets or sets the response id.
        /// </summary>
        public string ResponseId { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the response was selected by user or not.
        /// </summary>
        public bool SelectedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user response id.
        /// </summary>
        public string ResponseUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the response was correct or not..
        /// </summary>
        public bool Correct { get; set; }
    }
}
