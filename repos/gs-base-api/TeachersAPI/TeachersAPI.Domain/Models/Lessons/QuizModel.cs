namespace TeachersAPI.Domain.Models.Lessons
{
    public class QuizModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string LevelOfKnowledge { get; set; }
        public string IsExam { get; set; }
        public string QuizTypeId { get; set; }
        public string CustomValue { get; set; }
        public string ReadyToPublish { get; set; }
        public string Available { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}