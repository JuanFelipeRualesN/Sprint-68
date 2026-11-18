using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSAIServicesAPI.Models.Quotas
{
    /// <summary>
    /// Global master switch for the quota system. 
    /// There is only ever one row, identified by SingletonId. 
    /// Maps to the quota_settings table
    /// </summary>
    [Table("quota_settings")]
    public class QuotaSetting
    {
        /// <summary>
        /// Primary key of the single configuration row.
        /// The row is seeded with the fixed SingletonId value
        /// Maps to the id column.
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Master on/off switch.
        /// When false the API performs no quota validation at all and every request is allowed through. 
        /// Maps to the quotas_enabled column.
        /// </summary>
        [Column("quotas_enabled")]
        public bool QuotasEnabled { get; set; }

        /// <summary>
        /// Master kill switch for AI generation. this turns the AI endpoints off entirely, 
        /// whatever quota a user has.
        /// Maps to the ai_enabled column
        /// </summary>
        [Column("ai_enabled")]
        public bool AiEnabled { get; set; } = true;

        /// <summary>
        /// Scope recorded as the system's default. Stored and shown in the frontend, but
        /// the enforcement path ignores it: limits are resolved from the most specific matching row, whatever this says. 
        /// Maps to the default_scope column.
        /// </summary>
        [Column("default_scope")]
        [MaxLength(30)]
        public string DefaultScope { get; set; } = QuotaScope.Global.ToString();

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
        /// The fixed PK value of the one configuration row
        /// </summary>
        public const int SingletonId = 1;
    }
}
