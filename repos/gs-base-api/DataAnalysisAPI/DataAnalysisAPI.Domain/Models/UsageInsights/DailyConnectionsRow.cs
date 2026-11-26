namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Connections and new users of one day.
    /// </summary>
    /// <param name="Day">Local day.</param>
    /// <param name="Connections">Connections that day.</param>
    /// <param name="NewUsers">Users whose first connection of the range is that day.</param>
    public record DailyConnectionsRow(DateOnly Day, int Connections, int NewUsers);
}
