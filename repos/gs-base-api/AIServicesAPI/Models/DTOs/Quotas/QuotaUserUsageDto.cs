namespace GSAIServicesAPI.Models.DTOs.Quotas
{
    /// <summary>
    /// What one user has spent on one resource during the period running now, together with the
    /// limit that governs them and the scope that limit came from.
    /// <br/><br/>
    /// Counterpart to the scoped-limit list: that one shows configured limits, this one shows who is
    /// spending against them. Read from recorded consumption, the only way to see usage under a School
    /// or Role limit since those keep no counter of their own.
    /// </summary>
    public class QuotaUserUsageDto
    {
        /// <summary>
        /// Id of the user the usage belongs to
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Resource being metered
        /// </summary>
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Name of the resource
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Unit the amounts are expressed in
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Amount spent in the period below.
        /// </summary>
        public long ConsumedAmount { get; set; }

        /// <summary>
        /// Total the user may spend in this period
        /// </summary>
        public long? AllocatedAmount { get; set; }

        /// <summary>
        /// Scope of the limit that governs this user for this resource
        /// </summary>
        public string? LimitScope { get; set; }

        /// <summary>
        /// Target id of the governing limit: the user, school or role id it was configured for.
        /// Null when no limit applies, and empty for the global default.
        /// </summary>
        public string? LimitScopeId { get; set; }

        /// <summary>
        /// Start of the period the amounts cover.
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// End of the period the amounts cover.
        /// </summary>
        public DateTime PeriodEnd { get; set; }
    }
}
