using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Catalogs
{
    /// <summary>
    /// Catalog: meeting_formats. Generic on purpose (shared with future modules). Seeded: 1 online, 2 in_person.
    /// </summary>
    public class MeetingFormat : IAuditableEntity
    {
        /// <summary>Id of the seeded <c>online</c> format.</summary>
        public const byte OnlineId = 1;

        /// <summary>Id of the seeded <c>in_person</c> format.</summary>
        public const byte InPersonId = 2;

        /// <summary>Primary key (TINYINT UNSIGNED).</summary>
        public byte Id { get; set; }

        /// <summary>Unique snake_case code (max 30), e.g. in_person.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Display name (max 60).</summary>
        public string Name { get; set; } = null!;

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
