namespace GSWebAPI.GSWebAPI.Domain.Models.UserQuiz
{
    public class UserQuizAuthorizationCodeModel : IResponse
    {
        public UserQuizAuthorizationCodeModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AuthorizationToken { get; set; }
        public string CustomQuizId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}