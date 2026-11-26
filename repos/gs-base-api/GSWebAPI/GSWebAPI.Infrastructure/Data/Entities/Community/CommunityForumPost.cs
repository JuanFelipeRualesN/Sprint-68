using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>community_forum_posts: a post of a community forum, or a reply to another post of the same forum. Soft-deleted.</summary>
    public class CommunityForumPost : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to community_forums (CASCADE).</summary>
        public ulong CommunityForumId { get; set; }

        /// <summary>The post this one replies to (same forum; self FK, no cascade). Null = top-level post.</summary>
        public ulong? ParentPostId { get; set; }

        /// <summary>Author. ul_user_accounts.id (logical reference to the users DB).</summary>
        public ulong UserId { get; set; }

        /// <summary>Text of the post (TEXT; max 10,000 chars, service rule). Checked by the foul-language filter.</summary>
        public string PostText { get; set; } = null!;

        /// <summary>UTC. Last time the author changed the text; null if never edited.</summary>
        public DateTime? EditedAt { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Forum the post belongs to.</summary>
        public CommunityForum Forum { get; set; } = null!;

        /// <summary>Post this one replies to, if any.</summary>
        public CommunityForumPost? ParentPost { get; set; }

        /// <summary>Direct replies to this post.</summary>
        public List<CommunityForumPost> Replies { get; set; } = new();
    }
}
