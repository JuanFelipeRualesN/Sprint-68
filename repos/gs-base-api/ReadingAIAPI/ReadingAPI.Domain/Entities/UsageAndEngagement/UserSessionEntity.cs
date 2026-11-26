namespace ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement
{
    public class UserSessionEntity
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public int RegionId { get; set; }
        public int TotalSessions { get; set; }
        public int TotalTimeSeconds { get; set; }
        public int OffSchoolPercent { get; set; }
        public int OnSchoolPercent { get; set; }
        public int Streak { get; set; }
        public int DesktopPercent { get; set; }
        public int MobilePercent { get; set; }
        public int TabletPercent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
