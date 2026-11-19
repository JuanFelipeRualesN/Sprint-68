namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class ContentMetricsResponseDto
    {
        public List<ReadingAvgTimeResponseDto> AvgTimeByPeriod { get; set; } = new();
        public List<ReadingCategoryResponseDto> Categories { get; set; } = new();
        public List<ReadingPagesResponseDto> PagesByPeriod { get; set; } = new();
        public int TotalTextsRead { get; set; }
        public double CompletionRate { get; set; }
    }
}
