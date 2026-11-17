using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the profile reports of averages.
    /// </summary>
    public class ProfileReportAverageModel
    {
        /// <summary>
        /// Gets or sets the lesson average complete info list.
        /// </summary>
        public List<LessonsAverageCompleteInfoResponseModel> LessonAverageCompleteInfoResponseList { get; set; }

        /// <summary>
        /// Gets or sets the average with lesson result details.
        /// </summary>
        public AverageWithLessonResultDetailsResponseModel AverageWithLessonResultDetails { get; set; }
    }
}