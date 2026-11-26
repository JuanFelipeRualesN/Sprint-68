namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// Aggregated all-time reading metrics for a single student.
    /// Unlike the role-scoped teacher metrics, these values have no period granularity
    /// averages and totals are calculated across every record available for the student.
    /// </summary>
    public class StudentMetricsResponseDto
    {
        /// <summary>
        /// Current consecutive-day streak
        /// </summary>
        public int Streak { get; set; }

        /// <summary>
        /// Average module usage time in seconds across all the student's daily session records
        /// </summary>
        public int AvgTimeSeconds { get; set; }

        /// <summary>
        /// Total number of stories the student has completed
        /// </summary>
        public int TotalTextsRead { get; set; }

        /// <summary>
        /// Average slides read per session.
        /// </summary>
        public double AvgPagesPerSession { get; set; }

        /// <summary>
        /// Module usage time per day for the current calendar week
        /// </summary>
        public List<WeeklyUsageDayDto> CurrentWeekUsage { get; set; } = new();

        /// <summary>
        /// Total module usage time in seconds for the current calendar year
        /// </summary>
        public int CurrentYearTimeSeconds { get; set; }

        /// <summary>
        /// Average percentage of usage happening at school across all the student's daily records
        /// </summary>
        public int AvgOnSchoolPercent { get; set; }

        /// <summary>
        /// Average percentage of usage happening off school across all the student's daily records
        /// </summary>
        public int AvgOffSchoolPercent { get; set; }

        /// <summary>
        /// All-time average assessment scores across every reading attempt
        /// </summary>
        public StudentAverageScoresDto AverageScores { get; set; } = new();

        /// <summary>
        /// Reading counts per story category
        /// </summary>
        public List<ReadingCategoryResponseDto> Categories { get; set; } = new();
    }
}
