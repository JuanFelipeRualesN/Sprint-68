namespace GSAIServicesAPI.Models.DTOs.Quotas
{
    /// <summary>
    /// A quota limit that overrides the global default for one role or user.
    /// A row exists only for a target that needs something different from the global limit; 
    /// deleting it makes that target inherit from the level above again (User falls back to Role, Role to Global).
    /// </summary>
    public class QuotaScopedLimitDto
    {
        /// <summary>
        /// Leave it zero when creating a limit;
        /// the API fills it in on every limit it returns, and DELETE uses it to pick the row to remove.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Resource this limit governs (e.g. TEXT_TOKENS)
        /// </summary>
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Name of the resource
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Unit the resource is measured in
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Level this limit applies at, Role or User
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Id of the target the limit applies to: the role id when
        /// Scope is Role, the user id when it is User
        /// </summary>
        public string ScopeId { get; set; } = string.Empty;

        /// <summary>
        /// Whether this override is enforced
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Maximum amount allowed per period
        /// </summary>
        public long MaxAmount { get; set; }

        /// <summary>
        /// Period cadence: Daily, Weekly, Monthly, or Custom
        /// </summary>
        public string PeriodType { get; set; } = "Monthly";

        /// <summary>
        /// Number of days per period when PeriodType is Custom
        /// </summary>
        public int? CustomPeriodDays { get; set; }

        /// <summary>
        /// Whether unused balance rolls over into the next period
        /// </summary>
        public bool RolloverEnabled { get; set; }

        /// <summary>
        /// Optional cap on accumulated rollover balance (null = uncapped)
        /// </summary>
        public long? RolloverMaxAmount { get; set; }

        /// <summary>
        /// Amount the target has spent in the period running right now
        /// <br/><br/>
        /// Only filled for User-scoped overrides: consumption is metered per user, so a Role or
        /// School override has no single counter to report and this stays null
        /// </summary>
        public long? ConsumedAmount { get; set; }

        /// <summary>
        /// Start of the period ConsumedAmount covers
        /// </summary>
        public DateTime? PeriodStart { get; set; }
    }
}
