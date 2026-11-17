namespace TeachersAPI.Domain.Models.UserQuizzes
{
    public class UserQuizResultModel : IResponse
    {
        public UserQuizResultModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string TryNo { get; set; }
        public string UserId { get; set; }
        public string QuizId { get; set; }
        public string Dok { get; set; }
        public string IsExam { get; set; }
        public string LessonId { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }
        public string TotalCorrect { get; set; }
        public string TotalQuestions { get; set; }
        public string TotalPercent { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public double TotalAccumulated { get; set; }

    }
}