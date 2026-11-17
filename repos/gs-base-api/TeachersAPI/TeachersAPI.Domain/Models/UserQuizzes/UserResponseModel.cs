namespace TeachersAPI.Domain.Models.UserQuizzes
{
    public class UserResponseModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string UserQuizResultId { get; set; }
        public string QuestionId { get; set; }
        public string ResponseId { get; set; }
        public string Correct { get; set; }
        public string ResponseExtra { get; set; }
        public string ResponseTime { get; set; }
        public string AssignedValue { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}