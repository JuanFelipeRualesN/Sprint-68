namespace TeachersAPI.Domain.Models.Events.Admin
{
    public class EventAttachmentModel
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string AttachmentUrl { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
