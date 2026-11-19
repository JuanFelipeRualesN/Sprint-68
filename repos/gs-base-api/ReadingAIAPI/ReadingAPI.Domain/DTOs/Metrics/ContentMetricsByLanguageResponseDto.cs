namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// Reading content metrics split by the language variant of the story that was read,
    /// since the english and spanish versions of a story are read and tracked independently.
    /// </summary>
    public class ContentMetricsByLanguageResponseDto
    {
        public ContentMetricsResponseDto English { get; set; } = new();
        public ContentMetricsResponseDto Spanish { get; set; } = new();
    }
}
