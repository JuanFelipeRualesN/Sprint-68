namespace TeachersAPI.Domain.Models.Events
{
    public class EventUserModel
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string UserId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
