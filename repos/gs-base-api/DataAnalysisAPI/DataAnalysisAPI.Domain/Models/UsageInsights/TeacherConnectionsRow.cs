namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Connections of one teacher in a period.
    /// </summary>
    /// <param name="Name">Teacher full name.</param>
    /// <param name="School">School name.</param>
    /// <param name="Connections">Connections (one per day).</param>
    /// <param name="PercentageOfTotal">Share of all teacher connections; filled by the service layer.</param>
    /// <param name="LastConnection">Last day with a connection inside the period; null when there was none.</param>
    public record TeacherConnectionsRow(string Name, string School, int Connections, double? PercentageOfTotal = null, DateOnly? LastConnection = null);
}
