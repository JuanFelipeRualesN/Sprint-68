namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    public class EmailAttachmentModel
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string AttachmentUrl { get; set; }
        public string AttachmentName { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}