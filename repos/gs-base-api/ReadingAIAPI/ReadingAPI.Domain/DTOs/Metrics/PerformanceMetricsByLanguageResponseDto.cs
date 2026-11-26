namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// Performance scores split by the language variant of the story that was read,
    /// since the english and spanish versions of a story are practiced and scored independently.
    /// </summary>
    public class PerformanceMetricsByLanguageResponseDto
    {
        public List<PerformanceMetricsResponseDto> English { get; set; } = new();
        public List<PerformanceMetricsResponseDto> Spanish { get; set; } = new();
    }
}
