using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>
    /// community_members: a user's membership in a group. Unique per group and user, including soft-deleted rows,
    /// so re-joining restores the old row. Soft-deleted.
    /// </summary>
    public class CommunityMember : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to community_groups (CASCADE).</summary>
        public ulong CommunityGroupId { get; set; }

        /// <summary>ul_user_accounts.id (logical reference to the users DB).</summary>
        public ulong UserId { get; set; }

        /// <summary>Role in the group (DB default <c>member</c>).</summary>
        public CommunityMemberRole Role { get; set; }

        /// <summary>Membership status (DB default <c>active</c>).</summary>
        public CommunityMemberStatus Status { get; set; }

        /// <summary>UTC. Set when <see cref="Status"/> becomes active; null while pending.</summary>
        public DateTime? JoinedAt { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Group of the membership.</summary>
        public CommunityGroup Group { get; set; } = null!;
    }
}
