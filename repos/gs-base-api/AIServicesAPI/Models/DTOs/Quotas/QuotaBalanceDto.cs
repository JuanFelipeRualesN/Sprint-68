namespace GSAIServicesAPI.Models.DTOs.Quotas
{
    /// <summary>
    /// A user's remaining allowance for one resource, as exposed to the frontend. 
    /// The frontend never decides whether a request is allowed.
    /// </summary>
    public class QuotaBalanceDto
    {
        /// <summary>
        /// Resource this balance refers to
        /// </summary>
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Resource name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Unit the amounts are expressed in
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// False when no limit applies to this user for this resource, in which case the
        /// amount fields are meaningless and consumption is unrestricted
        /// </summary>
        public bool Enforced { get; set; }

        /// <summary>
        /// Total allowed this period, including any rolled-over balance
        /// </summary>
        public long AllocatedAmount { get; set; }

        /// <summary>
        /// Amount already spent this period
        /// </summary>
        public long ConsumedAmount { get; set; }

        /// <summary>
        /// Amount still available, never reported below zero
        /// </summary>
        public long RemainingAmount { get; set; }

        /// <summary>
        /// Portion of the allocation carried over from the previous period
        /// </summary>
        public long RolloverCarriedIn { get; set; }

        /// <summary>
        /// Start of the current period.
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// End of the current period, i.e. when the quota resets
        /// </summary>
        public DateTime PeriodEnd { get; set; }
    }
}
