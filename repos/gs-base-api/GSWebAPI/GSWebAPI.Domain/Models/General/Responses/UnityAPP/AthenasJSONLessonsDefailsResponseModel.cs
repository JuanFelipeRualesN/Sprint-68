using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{

    /// <summary>
    /// To handle lesson details.
    /// </summary>
    public class AthenasJSONLessonsDefailsResponseModel
    {
        /// <summary>
        /// Gets or sets the lesson detials.
        /// </summary>
        public List<LessonModel> LessonRequestModel { get; set; }
    }

}