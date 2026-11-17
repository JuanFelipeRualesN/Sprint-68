namespace TeachersAPI.Domain.Models.UserQuizzes
{
    public class UserQuizFileModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string QuizId { get; set; }
        public string FileUrl { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}