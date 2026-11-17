namespace TeachersAPI.Domain.Models.Lessons
{
    public class QuizRuleModel
    {
        public string Id { get; set; }
        public string QuizTypeId { get; set; }
        public string LessonAddressedToId { get; set; }
        public string QuestionMaxLength { get; set; }
        public string ResponseMaxLength { get; set; }
        public string ExtraInfoMaxLength { get; set; }
        public string InstructionMaxLength { get; set; }
        public string MaxQuestions { get; set; }
        public string MinQuestions { get; set; }
        public string FalseResponseRequired { get; set; }
        public string TitleMaxLength { get; set; }
        public string CriterionRequired { get; set; }
        public string ReferenceRequired { get; set; }
        public string DeleteQuestionAllow { get; set; }
        public string EditWarning { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }

}