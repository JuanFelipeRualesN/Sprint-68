namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// The results are grouped by the date they were taken.
    /// Intended for the reading lessons results chart
    /// </summary>
    public class ReadingLessonsSeriesDto
    {
        /// <summary>
        /// Day the evaluations were taken
        /// </summary>
        public DateOnly? Date { get; set; }

        public int Month { get; set; }

        /// <summary>
        /// Average of total_percent for the evaluations taken on that date.
        /// </summary>
        public decimal AvgPercent { get; set; }

        /// <summary>
        /// How many evaluations the average was calculated from
        /// </summary>
        public int TotalEvaluations { get; set; }
    }
}
