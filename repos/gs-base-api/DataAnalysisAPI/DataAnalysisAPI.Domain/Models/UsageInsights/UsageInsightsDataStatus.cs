namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Last day with data for each family of queries.
    /// </summary>
    /// <param name="TestsDataUntil">Last day with test data.</param>
    /// <param name="ConnectionsDataUntil">Last day with connection data.</param>
    /// <param name="Source">Data source name.</param>
    public record UsageInsightsDataStatus(DateOnly? TestsDataUntil, DateOnly? ConnectionsDataUntil, string Source);
}
