using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSAIServicesAPI.Models.Quotas
{
    /// <summary>
    /// Append-only audit trail of every movement of a user's quota balance.
    /// Rows are never updated or deleted, so the history survives configuration changes and period resets. 
    /// Maps to the quota_usage_log table
    /// </summary>
    [Table("quota_usage_log")]
    public class QuotaUsageLog
    {
        /// <summary>
        /// Primary key.
        /// Maps to the id column
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// User the movement belongs to.
        /// Maps to the user_id column
        /// </summary>
        [Column("user_id")]
        [MaxLength(10)]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Resource consumed, referencing QuotaResourceType.Code
        /// Maps to the resource_type_code column
        /// </summary>
        [Column("resource_type_code")]
        [MaxLength(50)]
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Balance row the movement applied to, or null when the request was rejected before any period existed.
        /// Maps to the usage_period_id column
        /// </summary>
        [Column("usage_period_id")]
        public long? UsagePeriodId { get; set; }

        /// <summary>
        /// Correlation id shared by the reserve and its matching commit or release, so a single request can be followed end to end.
        /// Maps to the reservation_id column
        /// </summary>
        [Column("reservation_id")]
        [MaxLength(36)]
        public string ReservationId { get; set; } = string.Empty;

        /// <summary>
        /// What happened, stored as the name of QuotaUsageAction
        /// Maps to the action column
        /// </summary>
        [Column("action")]
        [MaxLength(20)]
        public QuotaUsageAction Action { get; set; }

        /// <summary>
        /// Signed change applied to the consumed amount, positive when balance is taken, negative when it is given back. 
        /// Maps to the amount column
        /// </summary>
        [Column("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Where the consumption came from (module/endpoint), used to break usage down by feature. 
        /// Maps to the reference column
        /// </summary>
        [Column("reference")]
        [MaxLength(100)]
        public string? Reference { get; set; }

        /// <summary>
        /// Creation timestamp. 
        /// Maps to the created_at column
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last-update timestamp.
        /// Maps to the updated_at column.
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
