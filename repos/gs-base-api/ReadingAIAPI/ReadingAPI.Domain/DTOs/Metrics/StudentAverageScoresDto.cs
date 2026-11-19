namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// All-time average assessment scores for a student
    /// </summary>
    public class StudentAverageScoresDto
    {
        public double AvgAccuracyScore { get; set; }
        public double AvgFluencyScore { get; set; }
        public double AvgCompletenessScore { get; set; }
        public double AvgPronunciationScore { get; set; }
    }
}
