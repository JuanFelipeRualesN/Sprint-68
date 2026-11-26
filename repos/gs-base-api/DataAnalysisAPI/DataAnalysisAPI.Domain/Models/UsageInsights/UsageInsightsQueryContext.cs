namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Everything a usage insights query needs besides its own parameters: the caller scope and the configured rules.
    /// </summary>
    /// <param name="Scope">Data the caller may see; always applied inside the SQL.</param>
    /// <param name="Exclusion">Test account exclusion rules.</param>
    /// <param name="UtcOffsetMinutes">Offset of the local day used to read dates (for example -240 for Puerto Rico).</param>
    /// <param name="CommandTimeoutSeconds">Command timeout of each query.</param>
    /// <param name="SubscriptionStatusColumn">Column of subscription_profile_history that holds the status.</param>
    /// <param name="SubscriptionCancellationColumn">Column of subscription_profile_history that holds the cancellation date.</param>
    public sealed record UsageInsightsQueryContext(DataScope Scope, TestAccountExclusion Exclusion, int UtcOffsetMinutes, int CommandTimeoutSeconds,
        string SubscriptionStatusColumn, string SubscriptionCancellationColumn);
}
