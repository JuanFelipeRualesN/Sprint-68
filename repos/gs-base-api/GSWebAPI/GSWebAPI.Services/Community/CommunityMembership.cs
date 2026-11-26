using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.Community
{
    /// <summary>Membership checks shared by the community services.</summary>
    internal static class CommunityMembership
    {
        /// <summary>The active membership of the user in the group, or null. Soft-deleted groups/members are filtered out.</summary>
        public static Task<CommunityMember?> GetActiveAsync(GSWebDbContext db, ulong groupId, ulong userId, CancellationToken cancellationToken)
        {
            return db.CommunityMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    m => m.CommunityGroupId == groupId && m.UserId == userId && m.Status == CommunityMemberStatus.Active,
                    cancellationToken);
        }

        /// <summary>
        /// Loads what the user can do with community content (events, forums, resources) that may or may not belong to a
        /// group: one query for the user's active memberships.
        /// </summary>
        public static async Task<CommunityViewer> GetViewerAsync(GSWebDbContext db, RequestUser user, CancellationToken cancellationToken)
        {
            var memberships = await db.CommunityMembers
                .AsNoTracking()
                .Where(m => m.UserId == user.UserId && m.Status == CommunityMemberStatus.Active)
                .ToListAsync(cancellationToken);
            return new CommunityViewer(user, memberships.ToDictionary(m => m.CommunityGroupId));
        }

        public static bool IsOwnerOrModerator(CommunityMember? member)
        {
            return member is { Status: CommunityMemberStatus.Active }
                && (member.Role == CommunityMemberRole.Owner || member.Role == CommunityMemberRole.Moderator);
        }

        public static bool IsOwner(CommunityMember? member)
        {
            return member is { Status: CommunityMemberStatus.Active, Role: CommunityMemberRole.Owner };
        }

        public static CommunityMemberResponseModel Map(CommunityMember member, IReadOnlyDictionary<ulong, string>? names = null)
        {
            return new CommunityMemberResponseModel
            {
                Id = member.Id,
                CommunityGroupId = member.CommunityGroupId,
                UserId = member.UserId,
                UserName = names?.GetValueOrDefault(member.UserId),
                Role = member.Role,
                Status = member.Status,
                JoinedAt = member.JoinedAt,
            };
        }
    }

    /// <summary>
    /// The user in session and their active memberships. Content with a group is for that group's active members, and its
    /// owners/moderators moderate it. Content without a group (platform-wide) is for everyone, and staff moderate it.
    /// </summary>
    internal sealed class CommunityViewer
    {
        private readonly IReadOnlyDictionary<ulong, CommunityMember> _memberships;

        public CommunityViewer(RequestUser user, IReadOnlyDictionary<ulong, CommunityMember> memberships)
        {
            User = user;
            _memberships = memberships;
        }

        public RequestUser User { get; }

        public ulong UserId => User.UserId;

        /// <summary>Ids of the groups where the user is an active member (for list filters).</summary>
        public IReadOnlyCollection<ulong> GroupIds => _memberships.Keys.ToList();

        /// <summary>Ids of the groups where the user is an active owner or moderator (for list filters).</summary>
        public IReadOnlyCollection<ulong> ModeratedGroupIds =>
            _memberships.Values.Where(CommunityMembership.IsOwnerOrModerator).Select(m => m.CommunityGroupId).ToList();

        /// <summary>True for platform-wide content, or when the user is an active member of the group.</summary>
        public bool CanSee(ulong? groupId) => groupId is not { } id || _memberships.ContainsKey(id);

        /// <summary>Staff for platform-wide content; active owners/moderators for group content.</summary>
        public bool IsModerator(ulong? groupId) =>
            groupId is { } id ? CommunityMembership.IsOwnerOrModerator(_memberships.GetValueOrDefault(id)) : User.IsStaff;
    }
}
