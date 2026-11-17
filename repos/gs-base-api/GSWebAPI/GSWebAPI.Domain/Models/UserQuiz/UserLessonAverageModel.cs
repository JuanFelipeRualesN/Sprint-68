namespace GSWebAPI.GSWebAPI.Domain.Models.UserQuiz
{
    public class UserLessonAverageModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LessonId { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public string Average { get; set; }
        public string Completed { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Extended property
        public string LessonTitle { get; set; }
        public string SubjectName { get; set; }

        public bool CanSeeResults { get; set; } = true;

    }
}