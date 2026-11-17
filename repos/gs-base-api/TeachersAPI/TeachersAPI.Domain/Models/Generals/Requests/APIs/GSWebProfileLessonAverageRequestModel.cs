using TeachersAPI.Domain.Models.UserQuizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To get the profile lesson verage request model.
    /// </summary>
    public class GSWebProfileLessonAverageRequestModel
    {
        /// <summary>
        /// Gets or sets the user lesson average model list.
        /// </summary>
        public List<UserLessonAverageModel> UserLessonAverageModelList { get; set; }


    }
}