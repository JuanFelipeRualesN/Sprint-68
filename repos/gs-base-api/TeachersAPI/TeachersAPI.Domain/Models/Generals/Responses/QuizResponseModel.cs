using TeachersAPI.Domain.Models.Lessons.Request;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class QuizResponseModel
    {
        public QuizResponseModel()
        {
            TransactionValid = true;
            SessionValid = true;
        }
        /// <summary>
        /// To main object for creating or updating a quiz
        /// </summary>
        public QuizRequestModel QuizRequestModel { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<string> Messages { get; set; }

        public bool TransactionValid { get; set; } // Remove after refactor the quiz edit area in frontend.

        public bool SessionValid { get; set; } // Remove after refactor the quiz edit area in frontend.
    }
}