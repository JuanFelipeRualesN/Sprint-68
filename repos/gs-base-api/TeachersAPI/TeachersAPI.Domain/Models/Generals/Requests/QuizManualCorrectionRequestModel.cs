using TeachersAPI.Domain.Models.UserQuizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// Handles the manual corrections.
    /// </summary>
    public class QuizManualCorrectionRequestModel
    {
        /// <summary>
        /// Gets or sets the plan quiz id.
        /// </summary>
        public string PlanQuizId { get; set; }

        /// <summary>
        /// Gets or sets the uses quiz responses.
        /// </summary>
        public List<UserResponseModel> UserResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the user quiz results id. 
        /// </summary>
        public string UserQuizResultsId { get; set; }

        /// <summary>
        /// Gets or sets the teacher feedback.
        /// </summary>
        public string Feedback { get; set; }
    }
}