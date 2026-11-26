using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSAIServicesAPI.Models.Quotas
{
    /// <summary>
    /// Catalog of the AI resource kinds that can be metered (text tokens, generated images, and future ones such as audio or transcriptions).
    /// <br/><br/>
    /// This table is the extensibility point of the whole quota system: adding a new
    /// meterable resource is a matter of inserting a row here. 
    /// Maps to the quota_resource_types table
    /// </summary>
    [Table("quota_resource_types")]
    public class QuotaResourceType
    {
        /// <summary>
        /// Code used as the primary key and referenced by QuotaResourceLimit.ResourceTypeCode and the consumption tables
        /// Maps to the code column
        /// </summary>
        [Key]
        [Column("code")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Name shown in the frontend. 
        /// Maps to the name column
        /// </summary>
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unit the resource is measured in (tokens, images, etc), used only for display. 
        /// Maps to the unit column
        /// </summary>
        [Column("unit")]
        [MaxLength(30)]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// When false the resource type is hidden from configuration and ignored by validation, without deleting historical data. 
        /// Maps to the is_active column
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

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
        /// Code of the text-token resource
        /// </summary>
        public const string TextTokens = "TEXT_TOKENS";

        /// <summary>
        /// Code of the image-count resource
        /// </summary>
        public const string ImageCount = "IMAGE_COUNT";
    }
}
