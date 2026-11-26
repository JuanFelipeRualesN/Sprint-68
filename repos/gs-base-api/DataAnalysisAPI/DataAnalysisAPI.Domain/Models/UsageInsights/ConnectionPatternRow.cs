namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Connections grouped by a pattern (weekday or role).
    /// </summary>
    /// <param name="Name">Weekday name or role name.</param>
    /// <param name="Connections">Connections in the group.</param>
    /// <param name="ConnectedUsers">Distinct connected users in the group.</param>
    /// <param name="PercentageOfTotal">Share of all connections; filled by the service layer.</param>
    public record ConnectionPatternRow(string Name, int Connections, int ConnectedUsers, double? PercentageOfTotal = null);
}
