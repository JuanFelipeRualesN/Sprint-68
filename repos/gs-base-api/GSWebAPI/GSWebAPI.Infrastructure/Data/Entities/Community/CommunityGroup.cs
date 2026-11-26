using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>community_groups: a community group. Members, forums, events and group resources hang off a group. Soft-deleted.</summary>
    public class CommunityGroup : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>Name of the group (max 120).</summary>
        public string CommunityName { get; set; } = null!;

        /// <summary>City/region (max 255); null for online-only groups.</summary>
        public string? CommunityLocation { get; set; }

        /// <summary>Optional description (TEXT).</summary>
        public string? CommunityDescription { get; set; }

        /// <summary>Creator. ul_user_accounts.id (logical reference to the users DB). Becomes the owner member.</summary>
        public ulong CreatedByUserId { get; set; }

        /// <summary>Public or private (DB default <c>public</c>). Joining a private group needs approval.</summary>
        public CommunityGroupVisibility Visibility { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Memberships of the group.</summary>
        public List<CommunityMember> Members { get; set; } = new();

        /// <summary>Forums of the group.</summary>
        public List<CommunityForum> Forums { get; set; } = new();

        /// <summary>Events of the group.</summary>
        public List<CommunityEvent> Events { get; set; } = new();

        /// <summary>Resources shared in the group.</summary>
        public List<CommunityResource> Resources { get; set; } = new();
    }
}
