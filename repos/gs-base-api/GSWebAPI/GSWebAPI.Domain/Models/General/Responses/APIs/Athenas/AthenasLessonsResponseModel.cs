using GSWebAPI.GSWebAPI.Domain.Models.Athenas;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
{
    public class AthenasLessonsResponseModel
    {
        public List<AthenasLessonModifierRequestModel> Lessons { get; set; }
        public int TotalResults { get; set; }
    }
}
