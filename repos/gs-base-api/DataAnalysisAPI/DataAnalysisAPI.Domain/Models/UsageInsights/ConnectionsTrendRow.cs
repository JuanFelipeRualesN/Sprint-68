namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Connections of one period (day, month, quarter or year) of a trend.
    /// </summary>
    /// <param name="Period">Period key, for example 2026-03, 2026-T1 or 2026.</param>
    /// <param name="Connections">Connections in the period.</param>
    /// <param name="DifferenceWithPreviousPeriod">Difference with the previous period; filled by the service layer.</param>
    /// <param name="VariationPercentage">Variation against the previous period; filled by the service layer.</param>
    public record ConnectionsTrendRow(string Period, int Connections, int? DifferenceWithPreviousPeriod = null, double? VariationPercentage = null);
}
