namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Tests assigned and completed for one subject area in a period (cohort by assignment date).
    /// </summary>
    /// <param name="Name">Subject name.</param>
    /// <param name="AssignedTests">Tests assigned in the period.</param>
    /// <param name="CompletedTests">Of the assigned tests, those already taken.</param>
    /// <param name="CompletedPercentage">Completed tests divided by assigned tests; null when nothing was assigned.</param>
    /// <param name="PercentageOfTotalCompleted">Share of all completed tests; filled by the service layer.</param>
    public record SubjectAreaUsageRow(string Name, int AssignedTests, int CompletedTests, double? CompletedPercentage, double? PercentageOfTotalCompleted = null);
}
