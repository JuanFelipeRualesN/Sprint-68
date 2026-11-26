using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// custom_plan_type_translations: the name of a custom plan type in one language (e.g. <c>es</c> Refuerzos, <c>en</c>
    /// Reinforcements). One row per type and language.
    /// </summary>
    public class CustomPlanTypeTranslation : IAuditableEntity
    {
        /// <summary>FK to custom_plan_types (CASCADE). Part of the primary key.</summary>
        public byte CustomPlanTypeId { get; set; }

        /// <summary>Primary language tag, lower case (max 5), e.g. <c>en</c> or <c>es</c>. Part of the primary key.</summary>
        public string LangCode { get; set; } = null!;

        /// <summary>Type name in this language (max 60).</summary>
        public string Name { get; set; } = null!;

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>The translated plan type.</summary>
        public CustomPlanType CustomPlanType { get; set; } = null!;
    }
}
