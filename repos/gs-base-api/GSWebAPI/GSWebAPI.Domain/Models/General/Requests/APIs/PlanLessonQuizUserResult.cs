using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs
{
    /// <summary>
    /// Model to insert the quiz results in teacher's api.
    /// </summary>
    public class PlanLessonQuizUserResult
    {
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// Gets or sets the quiz id.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the user quiz result id.
        /// </summary>
        public string UsquiUserResultId { get; set; }
    }
}