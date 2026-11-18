namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class UserSessionMetricsRequestDto
    {
        public int? SchoolId { get; set; }
        public int? RegionId { get; set; }
        public List<int>? UserIds { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
    }
}
