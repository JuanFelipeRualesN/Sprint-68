namespace TeachersAPI.Domain.Models.Lessons
{
    public class LessonAdditionalMaterialModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string AdditionalMaterialId { get; set; }
        public string Page { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}