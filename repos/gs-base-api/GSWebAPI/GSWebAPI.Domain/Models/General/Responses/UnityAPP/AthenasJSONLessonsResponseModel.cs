using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{
    /// <summary>
    /// Handles Athenas Lessons requests (response of Lesson Model). It uses the same object of the same library (LessonModel).
    /// </summary>
    public class AthenasJSONLessonsResponseModel
    {
        /// <summary>
        /// Gets or sets the lesson model.
        /// </summary>
        public List<LessonModel> LessonModelList { get; set; }
    }
}