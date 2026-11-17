using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To handle lessons readers.
    /// </summary>
    public class LessonsResponseModel
    {
        public LessonsResponseModel()
        {
            LessonModelList = new List<LessonModel>();
        }
        /// <summary>
        /// Gets or sets the Lesson Model List (if details are not needed).
        /// </summary>
        public List<LessonModel> LessonModelList { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

    }

}