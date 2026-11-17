namespace TeachersAPI.Domain.Models.Lessons
{
    public class LessonTemplateModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string TemplateId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}