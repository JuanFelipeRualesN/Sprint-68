namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Completed tests of one subject area in two periods.
    /// </summary>
    /// <param name="Name">Subject name.</param>
    /// <param name="CompletedTestsA">Completed tests in period A.</param>
    /// <param name="CompletedTestsB">Completed tests in period B.</param>
    /// <param name="Difference">Period B minus period A.</param>
    /// <param name="VariationPercentage">Difference as a percentage of period A; null when period A is zero.</param>
    public record SubjectAreaComparisonRow(string Name, int CompletedTestsA, int CompletedTestsB, int Difference, double? VariationPercentage);
}
