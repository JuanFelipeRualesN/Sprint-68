using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>Catalog: curriculum_frameworks (official curriculum frameworks of any country or state). Seeded with DEPR.</summary>
    public class CurriculumFramework : IAuditableEntity
    {
        /// <summary>Code of the seeded Departamento de Educación de Puerto Rico framework (id 1), the only one that accepts standards for now.</summary>
        public const string DeprCode = "DEPR";

        // Nullable backing field so EF can tell "not set" (use the DB default 1) from an explicit false.
        private bool? _isActive;

        /// <summary>Primary key (SMALLINT UNSIGNED).</summary>
        public ushort Id { get; set; }

        /// <summary>Short unique code (max 30), e.g. DEPR.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Display name (max 120).</summary>
        public string Name { get; set; } = null!;

        /// <summary>ISO 3166-1 alpha-2 (CHAR(2)).</summary>
        public string CountryCode { get; set; } = null!;

        /// <summary>State or region within the country (max 80); null when the framework covers the whole country.</summary>
        public string? Region { get; set; }

        /// <summary>Only active frameworks can be assigned to a plan. DB default true.</summary>
        public bool IsActive
        {
            get => _isActive ?? true;
            set => _isActive = value;
        }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
