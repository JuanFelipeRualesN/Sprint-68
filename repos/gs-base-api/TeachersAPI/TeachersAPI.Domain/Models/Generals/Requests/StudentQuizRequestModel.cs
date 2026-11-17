using TeachersAPI.Domain.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// Handle student quiz request to correct or insert.
    /// </summary>
    public class StudentQuizRequestModel
    {
        /// <summary>
        /// Gets or sets the user quiz full detail model (with responses)
        /// </summary>
        public UserQuizFullDetailModel UserQuizFullDetailModel { get; set; }

        /// <summary>
        /// Gets or sets the authorization code to access to the quiz.
        /// </summary>
        public string AuthorizationCode { get; set; }
    }
}