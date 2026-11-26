namespace GSAIServicesAPI.Models.DTOs.Quotas
{
    /// <summary>
    /// Quota configuration exchanged with the frontend. 
    /// Combines the global master switch with the per-resource limits.
    /// </summary>
    public class QuotaConfigDto
    {
        /// <summary>
        /// On/off switch for the entire quota system
        /// </summary>
        public bool QuotasEnabled { get; set; }

        /// <summary>
        /// Master kill switch for AI generation.
        /// When false the guarded AI endpoints are unavailable regardless of quota.
        /// </summary>
        public bool AiEnabled { get; set; } = true;

        /// <summary>
        /// Scope the quotas apply at
        /// </summary>
        public string DefaultScope { get; set; } = "Global";

        /// <summary>
        /// Per-resource quota definitions (one entry per resource type)
        /// </summary>
        public List<QuotaResourceConfigDto> Resources { get; set; } = new();
    }

    /// <summary>
    /// Quota limit for one resource type
    /// </summary>
    public class QuotaResourceConfigDto
    {
        /// <summary>
        /// Resource code this limit applies to
        /// </summary>
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the resource
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Unit the resource is measured in
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Whether this resource's quota is enforced
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
        /// Optional cap on accumulated rollover balance. null = uncapped.
        /// </summary>
        public long? RolloverMaxAmount { get; set; }
    }
}
