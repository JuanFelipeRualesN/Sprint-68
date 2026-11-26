namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class ReadingPagesResponseDto
    {
        public DateOnly? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int TotalPages { get; set; }
    }
}
