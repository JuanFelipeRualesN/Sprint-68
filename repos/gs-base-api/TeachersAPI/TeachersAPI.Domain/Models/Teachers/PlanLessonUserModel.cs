using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class PlanLessonUserModel : IResponse
    {
        public PlanLessonUserModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string PlanId { get; set; }
        public string PlanLessonId { get; set; }
        public string LessonId { get; set; }
        public string UsquiUserResultId { get; set; }
        public string QuizId { get; set; }
        public string UserId { get; set; }
        public string OverallScore { get; set; }
        public string Completed { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}