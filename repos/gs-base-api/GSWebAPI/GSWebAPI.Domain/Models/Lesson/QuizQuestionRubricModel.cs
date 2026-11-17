namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class QuizQuestionRubricModel
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string Rubric { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}