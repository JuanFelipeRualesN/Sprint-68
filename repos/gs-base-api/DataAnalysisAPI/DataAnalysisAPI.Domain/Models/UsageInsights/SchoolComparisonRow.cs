namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Connections of one school in two periods.
    /// </summary>
    /// <param name="Name">School name.</param>
    /// <param name="ConnectionsA">Connections in period A.</param>
    /// <param name="ConnectionsB">Connections in period B.</param>
    /// <param name="Difference">Period B minus period A.</param>
    /// <param name="VariationPercentage">Difference as a percentage of period A; null when period A is zero.</param>
    public record SchoolComparisonRow(string Name, int ConnectionsA, int ConnectionsB, int Difference, double? VariationPercentage);
}
