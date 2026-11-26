namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// A school returned by the school search.
    /// </summary>
    /// <param name="Name">Exact school name to use as the school filter.</param>
    public record SchoolCatalogRow(string Name);
}
