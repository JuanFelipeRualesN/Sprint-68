using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// JSON to ask lesson model specifics.
    /// </summary>
    public class LessonModelSpecificsRequestModel
    {
        /// <summary>
        /// Gets or sets the lesson models. 
        /// </summary>
        public List<LessonModel> LessonModelList { get; set; }
    }
}