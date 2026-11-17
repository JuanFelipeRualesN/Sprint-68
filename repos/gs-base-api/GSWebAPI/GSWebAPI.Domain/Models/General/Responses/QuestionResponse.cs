using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{

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