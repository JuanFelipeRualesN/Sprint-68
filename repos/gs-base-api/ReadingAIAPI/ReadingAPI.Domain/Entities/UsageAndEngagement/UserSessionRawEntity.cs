namespace ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement
{
    public class UserSessionRawEntity
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int IsSchoolHours { get; set; }
        public string DeviceType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
