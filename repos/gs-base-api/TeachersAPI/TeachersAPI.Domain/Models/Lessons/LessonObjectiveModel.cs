namespace TeachersAPI.Domain.Models.Lessons
{
    public class LessonObjectiveModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string Desc { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}