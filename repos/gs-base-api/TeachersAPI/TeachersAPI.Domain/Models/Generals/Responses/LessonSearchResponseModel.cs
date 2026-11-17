using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Domain.Models.Lessons.LessonSearchModels;
using TeachersAPI.Domain.Models.Lessons.Response;


namespace TeachersAPI.Domain.Models.Generals.Responses
{
	public class LessonSearchResponseModel : GeneralResponseModel
	{
        /// <summary>
        /// Lessons list with details
        /// </summary>
        public LessonsResponseModel LessonsDetails { get; set; }

        /// <summary>
        /// Gets or sets the Lesson Model List (if details are not needed)
        /// </summary>
        public List<LessonSearchModel> LessonModelList { get; set; }
    }
}