using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>
    /// Catalog: community_forum_categories. Seeded: 1 general, 2 math, 3 reading_writing, 4 science, 5 organization,
    /// 6 legal_depr, 7 first_steps (none moderators only). Names per language are in community_forum_category_translations.
    /// </summary>
    public class CommunityForumCategory : IAuditableEntity
    {
        /// <summary>Id of the seeded <c>general</c> category (the default for new forums).</summary>
        public const byte GeneralId = 1;

        /// <summary>Id of the seeded <c>math</c> category.</summary>
        public const byte MathId = 2;

        /// <summary>Id of the seeded <c>reading_writing</c> category.</summary>
        public const byte ReadingWritingId = 3;

        /// <summary>Id of the seeded <c>science</c> category.</summary>
        public const byte ScienceId = 4;

        /// <summary>Id of the seeded <c>organization</c> category.</summary>
        public const byte OrganizationId = 5;

        /// <summary>Id of the seeded <c>legal_depr</c> category.</summary>
        public const byte LegalDeprId = 6;

        /// <summary>Id of the seeded <c>first_steps</c> category.</summary>
        public const byte FirstStepsId = 7;

        /// <summary>Primary key (TINYINT UNSIGNED).</summary>
        public byte Id { get; set; }

        /// <summary>Unique snake_case code (max 30), e.g. announcements.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Default display name (max 60); used when there is no translation for the requested language nor English.</summary>
        public string Name { get; set; } = null!;

        /// <summary>The name in each language (community_forum_category_translations).</summary>
        public List<CommunityForumCategoryTranslation> Translations { get; set; } = new();

        /// <summary>Only group owners/moderators can create forums in this category (e.g. announcements). DB default false.</summary>
        public bool ModeratorsOnly { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
