namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    public class PerformanceMetricsResponseDto
    {
        public DateOnly? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public double AvgAccuracyScore { get; set; }
        public double AvgFluencyScore { get; set; }
        public double AvgCompletenessScore { get; set; }
        public double AvgPronunciationScore { get; set; }
        public int AvgWordsPerMinute { get; set; }
    }
}
