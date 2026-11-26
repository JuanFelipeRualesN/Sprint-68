namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Connections of one school in a period.
    /// </summary>
    /// <param name="Name">School name.</param>
    /// <param name="Connections">Connections (one per user and day).</param>
    /// <param name="ConnectedUsers">Distinct users with at least one connection.</param>
    /// <param name="PercentageOfTotal">Share of all connections; filled by the service layer.</param>
    /// <param name="ConnectionsPerUser">Connections divided by connected users; filled by the service layer.</param>
    /// <param name="LastConnection">Last day with a connection inside the period (local query time); null when there was none.</param>
    public record SchoolConnectionsRow(string Name, int Connections, int ConnectedUsers, double? PercentageOfTotal = null, double? ConnectionsPerUser = null, DateOnly? LastConnection = null);
}
