using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Domain.Models.Athenas
{
    public class LessonModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string LessonNo { get; set; }
        public string LevelCode { get; set; }
        public string SubjectCode { get; set; }
        public string LessonTitle { get; set; }
        public string LessonImg { get; set; }
        public string HasAudioRecorder { get; set; }
        public string IsDynamic { get; set; }
        public string IsGapClosing { get; set; }
        public string Blueprint { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}
