namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class ReadingAvgTimeResponseDto
    {
        public DateOnly? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int AvgTimeSeconds { get; set; }
    }
}
