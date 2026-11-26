using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>Catalog: custom_plan_types. Seeded: 1 homeschool, 2 reinforcements, 3 specific_task, 4 customized.</summary>
    public class CustomPlanType : IAuditableEntity
    {
        /// <summary>Primary key (TINYINT UNSIGNED).</summary>
        public byte Id { get; set; }

        /// <summary>Unique snake_case code (max 30), e.g. homeschool.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Default display name (max 60); used when there is no translation for the requested language nor English.</summary>
        public string Name { get; set; } = null!;

        /// <summary>The name in each language (custom_plan_type_translations).</summary>
        public List<CustomPlanTypeTranslation> Translations { get; set; } = new();

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
