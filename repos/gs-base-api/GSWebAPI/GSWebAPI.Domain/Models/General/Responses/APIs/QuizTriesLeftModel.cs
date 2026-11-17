using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{
    /// <summary>
    /// To handle the quiz tries for user response when checking the tries left.
    /// </summary>
    public class QuizTriesLeftModel
    {
        /// <summary>
        /// Gets or sets the quiz's id.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets how many tries are left.
        /// </summary>
        public int TriesLeft { get; set; }

        /// <summary>
        /// Gets or sets how many tries total.
        /// </summary>
        public int TriesTotal { get; set; }

        /// <summary>
        /// Gets or sets how many tries used.
        /// </summary>
        public int TriesUsed { get; set; }
    }
}