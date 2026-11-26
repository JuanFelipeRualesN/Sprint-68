namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// With total grouping: one row per subscription status.
    /// </summary>
    /// <param name="Name">Status label.</param>
    /// <param name="Users">Users in the status.</param>
    /// <param name="PercentageOfTotal">Share of all users; filled by the service layer.</param>
    public record SubscriptionStatusTotalRow(string Name, int Users, double? PercentageOfTotal = null);
}
