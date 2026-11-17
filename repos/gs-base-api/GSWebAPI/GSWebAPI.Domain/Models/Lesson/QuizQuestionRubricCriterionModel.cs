namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class QuizQuestionRubricCriterionModel
    {
        public string Id { get; set; }
        public string QuizQuestionRubricId { get; set; }
        public string Criterion { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}