namespace GSAIServicesAPI.Models.Quotas
{
    /// <summary>
    /// Length of a quota period. Determines how often a user's balance is reset
    /// (and, when rollover is enabled, when the remaining balance is carried over).
    /// Stored in the DB as its string name so new period types stay readable.
    /// </summary>
    public enum PeriodType
    {
        /// <summary>
        /// Resets every calendar day
        /// </summary>
        Daily,

        /// <summary>
        /// Resets every 7 days
        /// </summary>
        Weekly,

        /// <summary>
        /// Resets every calendar month
        /// </summary>
        Monthly,

        /// <summary>
        /// Resets every CustomPeriodDays days
        /// </summary>
        Custom
    }

    /// <summary>
    /// Level at which a quota configuration applies. All four scopes are in use.
    /// </summary>
    public enum QuotaScope
    {
        /// <summary>
        /// Applies to every user
        /// </summary>
        Global,

        /// <summary>
        /// Applies to a specific school.
        /// </summary>
        School,

        /// <summary>
        /// Applies to a specific role
        /// </summary>
        Role,

        /// <summary>
        /// Applies to a specific user
        /// </summary>
        User
    }

    /// <summary>
    /// What a row in the audit log records. 
    /// A single request normally produces a Reserve followed by a Commit (or a Release if the request failed), 
    /// so the log explains every  movement of a user's balance.
    /// </summary>
    public enum QuotaUsageAction
    {
        /// <summary>
        /// Balance was held up-front, before the AI call ran
        /// </summary>
        Reserve,

        /// <summary>
        /// The reservation was settled against the real consumption
        /// </summary>
        Commit,

        /// <summary>
        /// The reservation was returned because the operation did not complete
        /// </summary>
        Release,

        /// <summary>
        /// The request was denied because the quota was exhausted
        /// </summary>
        Reject,

        /// <summary>
        /// Consumption recorded for auditing when no quota governed the request (quotas
        /// off, or no limit for the user). No balance moves; the row exists only so usage
        /// stays measurable with enforcement disabled.
        /// </summary>
        Record
    }
}
