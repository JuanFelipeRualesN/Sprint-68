namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class SessionTotalsResponseDto
    {
        public int TotalStudents { get; set; }
        public int TotalSessions { get; set; }
        public int TotalActiveDays { get; set; }
        public int AvgTimeSeconds { get; set; }
        public int AvgOffSchoolPercent { get; set; }
        public int AvgOnSchoolPercent { get; set; }
        public int AvgDesktopPercent { get; set; }
        public int AvgMobilePercent { get; set; }
        public int AvgTabletPercent { get; set; }
    }
}
