using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To get the profile lesson verage request model.
    /// </summary>
    public class ProfileLessonAverageRequestModel
    {
        /// <summary>
        /// Gets or sets the user lesson average model list.
        /// </summary>
        public List<UserLessonAverageModel> UserLessonAverageModelList { get; set; }
    }
}