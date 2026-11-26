using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>
    /// community_forums: a discussion forum (topic) of a community group. Stored in GS Web, independent of the
    /// Teachers API forums. Soft-deleted.
    /// </summary>
    public class CommunityForum : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to community_groups (CASCADE).</summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>FK to community_forum_categories (RESTRICT).</summary>
        public byte CommunityForumCategoryId { get; set; }

        /// <summary>Author. ul_user_accounts.id (logical reference to the users DB).</summary>
        public ulong UserId { get; set; }

        /// <summary>Title (max 150). Checked by the foul-language filter.</summary>
        public string Title { get; set; } = null!;

        /// <summary>Optional description (TEXT). Checked by the foul-language filter.</summary>
        public string? Description { get; set; }

        /// <summary>Pinned forums are listed first. Set by group owners/moderators.</summary>
        public bool IsPinned { get; set; }

        /// <summary>Locked forums accept no new posts or edits, except from group owners/moderators.</summary>
        public bool IsLocked { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Group the forum belongs to.</summary>
        public CommunityGroup? Group { get; set; }

        /// <summary>Category of the forum.</summary>
        public CommunityForumCategory Category { get; set; } = null!;

        /// <summary>Posts and replies of the forum.</summary>
        public List<CommunityForumPost> Posts { get; set; } = new();
    }
}
