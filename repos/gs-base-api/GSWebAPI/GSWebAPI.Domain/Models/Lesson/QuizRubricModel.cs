namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class QuizRubricModel
    {
        public string Id { get; set; }
        public string QuizId { get; set; }
        public string Desc { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}