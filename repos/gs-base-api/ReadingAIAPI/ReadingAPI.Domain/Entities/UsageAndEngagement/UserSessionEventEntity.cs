namespace ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement
{
    public class UserSessionEventEntity
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public string EventType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
