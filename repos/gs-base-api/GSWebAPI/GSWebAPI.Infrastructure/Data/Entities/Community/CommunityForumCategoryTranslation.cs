using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>
    /// community_forum_category_translations: the name of a forum category in one language (e.g. <c>es</c> Matemáticas,
    /// <c>en</c> Math). One row per category and language. Seeded in English and Spanish.
    /// </summary>
    public class CommunityForumCategoryTranslation : IAuditableEntity
    {
        /// <summary>English, also the fallback when the requested language has no translation.</summary>
        public const string English = "en";

        /// <summary>Spanish.</summary>
        public const string Spanish = "es";

        /// <summary>FK to community_forum_categories (CASCADE). Part of the primary key.</summary>
        public byte CommunityForumCategoryId { get; set; }

        /// <summary>Primary language tag, lower case (max 5), e.g. <c>en</c> or <c>es</c>. Part of the primary key.</summary>
        public string LangCode { get; set; } = null!;

        /// <summary>Category name in this language (max 60).</summary>
        public string Name { get; set; } = null!;

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>The translated category.</summary>
        public CommunityForumCategory Category { get; set; } = null!;
    }
}
