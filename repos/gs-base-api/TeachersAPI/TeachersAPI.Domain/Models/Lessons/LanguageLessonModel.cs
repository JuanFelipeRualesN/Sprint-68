namespace TeachersAPI.Domain.Models.Lessons
{
    public class LanguageLessonModel
    {
        public string Id { get; set; }
        public string LanguageCode { get; set; }
        public string Language { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}