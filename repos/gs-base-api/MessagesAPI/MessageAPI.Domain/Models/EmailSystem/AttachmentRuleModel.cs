namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    public class AttachmentRuleModel
    {
        public string Id { get; set; }
        public string FileType { get; set; }
        public string MaxSize { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}