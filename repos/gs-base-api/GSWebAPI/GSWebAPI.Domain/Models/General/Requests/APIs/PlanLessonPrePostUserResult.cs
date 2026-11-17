using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs
{
    /// <summary>
    /// To insert the pre post.
    /// </summary>
    public class PlanLessonPrePostUserResult : PlanLessonQuizUserResult
    {
        /// <summary>
        /// Gets or sets if pre or post. 0 for pre, 1 for post.
        /// </summary>
        public string PrePost { get; set; }

        /// <summary>
        /// Gets or sets the total correct.
        /// </summary>
        public string TotalCorrect { get; set; }

        /// <summary>
        /// Gets or sets the total questions.
        /// </summary>
        public string TotalQuestions { get; set; }
    }
}