namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class SessionByPeriodResponseDto
    {
        public DateOnly? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int TotalSessions { get; set; }
        public int TotalTimeSeconds { get; set; }
    }
}
