namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class ReadingCategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public int TotalReadings { get; set; }
    }
}
