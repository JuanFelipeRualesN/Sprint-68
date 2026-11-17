namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class LessonDefinitionModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Frames { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}