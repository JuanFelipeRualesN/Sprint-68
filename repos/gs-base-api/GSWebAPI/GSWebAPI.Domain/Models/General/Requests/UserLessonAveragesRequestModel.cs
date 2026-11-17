using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To handle UserLessonAveragesRequests.
    /// </summary>
    public class UserLessonAveragesRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets user lesson average model list.
        /// </summary>
        public List<UserLessonAverageModel> UserLessonAverageModelList { get; set; }
    }
}