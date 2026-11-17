namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class LessonModel
    {
        public string Id { get; set; }
        public string LessonNo { get; set; }
        public string LevelCode { get; set; }
        public string SubjectCode { get; set; }
        public string LessonTitle { get; set; }
        public string HasAudioRecorder { get; set; }
        public string IsDynamic { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}