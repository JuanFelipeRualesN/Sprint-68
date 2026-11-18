using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSAIServicesAPI.Models.Quotas
{
    /// <summary>
    /// A user's live balance for one resource during one period, under one specific limit.
    /// This is the row that every request reads and updates.
    /// Maps to the quota_usage_periods table
    /// <br/><br/>
    /// Rows are created lazily: the first request of a new period inserts the row and,
    /// when rollover is configured, carries the previous period's leftover into RolloverCarriedIn.
    /// <br/><br/>
    /// The remaining balance is intentionally not stored: it is always
    /// AllocatedAmount minus ConsumedAmount, which keeps the two values from ever drifting apart under concurrent updates.
    /// <br/><br/>
    /// A balance belongs to the limit that governed it. 
    /// Because limits may use different period cadences, the same user and resource can hold several balances at
    /// once, one per limit, only the one whose limit currently governs the user is enforced against.
    /// </summary>
    [Table("quota_usage_periods")]
    public class QuotaUsagePeriod
    {
        /// <summary>
        /// Primary key.
        /// Maps to the id column
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// Id of the user the balance belongs to.
        /// Maps to the user_id column
        /// </summary>
        [Column("user_id")]
        [MaxLength(10)]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Resource being metered, referencing QuotaResourceType.Code
        /// Maps to the resource_type_code column
        /// </summary>
        [Column("resource_type_code")]
        [MaxLength(50)]
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Id of the QuotaResourceLimit this balance was opened under, so a balance is always tied
        /// to the limit that produced it.
        /// <br/><br/>
        /// Consumption does not transfer between limits. 
        /// When the limit governing a user changes, whether an override is added, removed or re-targeted, the request opens a fresh
        /// balance for the new limit and it starts at zero.
        /// Maps to the limit_id column
        /// </summary>
        [Column("limit_id")]
        public int LimitId { get; set; }

        /// <summary>
        /// Start of the period.
        /// Together with the user, resource and limit this identifies the row uniquely.
        /// Maps to the period_start column
        /// </summary>
        [Column("period_start")]
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// End of the period, the first request at or after this instant opens the next period. 
        /// Maps to the period_end column
        /// </summary>
        [Column("period_end")]
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// Total the user may spend this period: the configured maximum plus whatever rolled over.
        /// Re-derived from the current limit on each request, so raising or lowering the maximum takes effect in the running period.
        /// The rolled-over part is kept, as it was settled when the period opened.
        /// Maps to the allocated_amount column
        /// </summary>
        [Column("allocated_amount")]
        public long AllocatedAmount { get; set; }

        /// <summary>
        /// Amount spent so far this period.
        /// Maps to the consumed_amount column
        /// </summary>
        [Column("consumed_amount")]
        public long ConsumedAmount { get; set; }

        /// <summary>
        /// Portion of AllocatedAmount that came from the previous period's leftover, after applying the rollover cap. 
        /// Kept separate for auditing.
        /// Maps to the rollover_carried_in column
        /// </summary>
        [Column("rollover_carried_in")]
        public long RolloverCarriedIn { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// Maps to the created_at column
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last-update timestamp.
        /// Maps to the updated_at column
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Balance still available this period
        /// </summary>
        [NotMapped]
        public long RemainingAmount => AllocatedAmount - ConsumedAmount;
    }
}
