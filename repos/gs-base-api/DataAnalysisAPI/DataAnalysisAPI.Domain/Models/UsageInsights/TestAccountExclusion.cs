namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Internal and test accounts that distort rankings. They are excluded by default and every response states it.
    /// </summary>
    /// <param name="IsActive">True when the accounts are excluded.</param>
    /// <param name="Ores">ORE names (substring match) to exclude.</param>
    /// <param name="EmailDomains">User email domains to exclude.</param>
    /// <param name="Schools">Exact school names to exclude.</param>
    public record TestAccountExclusion(bool IsActive, string[] Ores, string[] EmailDomains, string[] Schools);
}
