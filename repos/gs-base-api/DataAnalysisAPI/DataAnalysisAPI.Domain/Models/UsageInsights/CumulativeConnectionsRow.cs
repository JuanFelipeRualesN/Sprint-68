namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Cumulative connections of one period.
    /// </summary>
    /// <param name="Period">Period key.</param>
    /// <param name="Connections">Connections in the period.</param>
    /// <param name="CumulativeConnections">Running sum of connections since the start of the requested range.</param>
    /// <param name="NewUsers">Users whose first connection of the range falls in this period.</param>
    /// <param name="CumulativeUsers">Running sum of new users.</param>
    public record CumulativeConnectionsRow(string Period, int Connections, int CumulativeConnections, int NewUsers, int CumulativeUsers);
}
