namespace GSWebAPI.GSWebAPI.Domain.Models.UserQuiz
{
    public class UserQuizFileModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string QuizId { get; set; }
        public string FileUrl { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}