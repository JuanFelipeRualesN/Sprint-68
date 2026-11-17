namespace TeachersAPI.Domain.Models.Lessons
{
    public class QuizQuestionRubricCriterionValueModel
    {
        public string Id { get; set; }
        public string QuizQuestionRubricCriterionId { get; set; }
        public string Desc { get; set; }
        public string Value { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}