using UsersAPI.UsersAPI.Domain.Models.Roles.Responses;

namespace UsersAPI.UsersAPI.Domain.Models.Emails
{
    public class EmailSettingModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string MailDomainCom { get; set; }
        public string EmailSender { get; set; }
        public string SenderUser { get; set; }
        public string SenderPassword { get; set; }
        public string EmailPort { get; set; }
        public string DefaultCredential { get; set; }
        public string EnableSsl { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}