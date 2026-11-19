using GSWebAPI.GSWebAPI.Domain.Models.Athenas;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
{
    public class AthenasLessonResponseByLessonModel : AthenasGeneralResponseModel
    {

        /// <summary>
        /// Lessons list with details
        /// </summary>
        public AthenasLessonsResponseModel LessonsDetails { get; set; }

        /// <summary>
        /// Gets or sets the Lesson Model List (if details are not needed)
        /// </summary>
        public List<LessonModel> LessonModelList { get; set; }
    }
}
