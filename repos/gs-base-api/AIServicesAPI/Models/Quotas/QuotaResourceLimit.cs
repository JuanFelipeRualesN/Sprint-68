using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSAIServicesAPI.Models.Quotas
{
    /// <summary>
    /// A single quota limit, how much of one resource a user may consume within one
    /// period, and how the period behaves (reset cadence and rollover). 
    /// There is one row per resource type at a given scope. Maps to the quota_resource_limits table.
    /// </summary>
    [Table("quota_resource_limits")]
    public class QuotaResourceLimit
    {
        /// <summary>
        /// Primary Key. Maps to the id column
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Code of the resource this limit governs, referencing QuotaResourceType.Code. 
        /// Maps to the resource_type_code column
        /// </summary>
        [Column("resource_type_code")]
        [MaxLength(50)]
        public string ResourceTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Scope at which this limit applies, stored as the string name of QuotaScope
        /// </summary>
        [Column("scope")]
        [MaxLength(30)]
        public QuotaScope Scope { get; set; } = QuotaScope.Global;

        /// <summary>
        /// Identifier of the scoped entity (school id, role id, or user id) when Scope is not Global, empty string for global limits. 
        /// Maps to the scope_id column.
        /// <br/><br/>
        /// Deliberately never null: MySQL treats every NULL as distinct from every other,
        /// so a nullable column would silently defeat the unique index on
        /// (resource_type_code, scope, scope_id) and allow two contradictory limits for
        /// the same target
        /// </summary>
        [Column("scope_id")]
        [MaxLength(100)]
        public string ScopeId { get; set; } = string.Empty;

        /// <summary>
        /// Whether this particular limit is enforced. 
        /// Independent from the global QuotaSetting.QuotasEnabled switch. 
        /// Maps to the enabled column
        /// </summary>
        [Column("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Maximum amount of the resource allowed per period (tokens, images, etc).
        /// bigint so large token budgets fit. 
        /// Maps to the max_amount column
        /// </summary>
        [Column("max_amount")]
        public long MaxAmount { get; set; }

        /// <summary>
        /// Period cadence, stored as the string name of PeriodType.
        /// Maps to the period_type column
        /// </summary>
        [Column("period_type")]
        [MaxLength(20)]
        public PeriodType PeriodType { get; set; } = PeriodType.Monthly;

        /// <summary>
        /// Number of days in a period when PeriodType is Custom, null otherwise. 
        /// Maps to the custom_period_days column
        /// </summary>
        [Column("custom_period_days")]
        public int? CustomPeriodDays { get; set; }

        /// <summary>
        /// Whether unused balance is carried over to the next period. 
        /// Maps to the rollover_enabled column
        /// </summary>
        [Column("rollover_enabled")]
        public bool RolloverEnabled { get; set; }

        /// <summary>
        /// Optional cap on how much accumulated balance rollover may reach, null means uncapped. 
        /// Only relevant when RolloverEnabled is true. 
        /// Maps to the rollover_max_amount column
        /// </summary>
        [Column("rollover_max_amount")]
        public long? RolloverMaxAmount { get; set; }

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
    }
}
