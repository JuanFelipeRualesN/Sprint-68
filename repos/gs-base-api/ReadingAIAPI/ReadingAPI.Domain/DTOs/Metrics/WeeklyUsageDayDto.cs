namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// Module usage time for a single day of the current calendar week.
    /// </summary>
    public class WeeklyUsageDayDto
    {
        /// <summary>
        /// The calendar date this day represents
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Day of the week
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Total module usage time in seconds for this day
        /// </summary>
        public int TimeSeconds { get; set; }
    }
}
