using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.Community
{
    /// <summary>Community groups and their members.</summary>
    public interface ICommunityGroupService
    {
        /// <summary>
        /// Creates a group. Any user may call it; the caller becomes its active owner member, saved in the same
        /// transaction as the group.
        /// </summary>
        /// <param name="user">User in session (the creator).</param>
        /// <param name="request">Name, location, description and visibility.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The created group (as <see cref="GetAsync"/> returns it). Validation error
        /// <see cref="ServiceErrorCodes.GroupNameRequired"/> if the name is blank or longer than 120 characters, or the
        /// location is longer than 255.
        /// </returns>
        Task<ServiceResult<CommunityGroupResponseModel>> CreateAsync(RequestUser user, CommunityGroupRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>Reads one group with its active member count and the membership of the user in session. Any user may call it.</summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The group, or NotFound <see cref="ServiceErrorCodes.GroupNotFound"/> (also for soft-deleted groups).</returns>
        Task<ServiceResult<CommunityGroupResponseModel>> GetAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists groups (public and private), newest first (by creation date), one page at a time. Any user may call it.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="onlyMine">True to return only groups where the user is active or pending (not banned).</param>
        /// <param name="page">Zero-based page; negative values become 0.</param>
        /// <param name="pageSize">Page size, clamped to 1-100.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The page of groups (may be empty) with the total items and pages. It does not fail.</returns>
        Task<ServiceResult<PagedResponseModel<CommunityGroupResponseModel>>> ListAsync(RequestUser user, bool onlyMine, int page, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>Replaces the name, location, description and visibility of a group. Only active owners may call it.</summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="request">New values.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated group. Errors: NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotOwner"/>, Validation <see cref="ServiceErrorCodes.GroupNameRequired"/>.
        /// </returns>
        Task<ServiceResult<CommunityGroupResponseModel>> UpdateAsync(RequestUser user, ulong groupId, CommunityGroupRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes a group with its members, forums, forum posts, events, event registrations and group resources,
        /// in one transaction. Only active owners may call it.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>True when deleted. Errors: NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotOwner"/>.</returns>
        Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Joins the user in session to a group as a member: active right away in a public group, pending (until an
        /// owner or moderator approves) in a private group. A membership the user left before is restored.
        /// </summary>
        /// <param name="user">User in session (the one joining).</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The new membership. Errors: NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.MemberBanned"/>, Conflict <see cref="ServiceErrorCodes.AlreadyPending"/> or
        /// <see cref="ServiceErrorCodes.AlreadyMember"/> (also when two join requests race).
        /// </returns>
        Task<ServiceResult<CommunityMemberResponseModel>> JoinAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes (soft-deletes) the membership of the user in session; a pending request is withdrawn the same way.
        /// Banned users cannot leave (the ban is kept) and the last active owner cannot leave.
        /// </summary>
        /// <param name="user">User in session (the one leaving).</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// True when left. Errors: NotFound <see cref="ServiceErrorCodes.MemberNotFound"/> (also if the group does not
        /// exist), Forbidden <see cref="ServiceErrorCodes.MemberBanned"/>, Conflict <see cref="ServiceErrorCodes.LastOwner"/>.
        /// </returns>
        Task<ServiceResult<bool>> LeaveAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the members of a group, ordered by membership id, with their names from the Users API. Only active
        /// members may call it; plain members see only active members, owners and moderators also see pending and banned ones.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The members. Errors: NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/>.
        /// </returns>
        Task<ServiceResult<List<CommunityMemberResponseModel>>> ListMembersAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default);

        /// <summary>Approves a pending join request (status active, joined_at set). Only active owners and moderators may call it.</summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the pending member.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated membership (without the name). Errors: Forbidden <see cref="ServiceErrorCodes.NotModerator"/>
        /// (also if the group does not exist), NotFound <see cref="ServiceErrorCodes.MemberNotFound"/>, Conflict
        /// <see cref="ServiceErrorCodes.MemberNotPending"/>.
        /// </returns>
        Task<ServiceResult<CommunityMemberResponseModel>> ApproveMemberAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken = default);

        /// <summary>Rejects a pending join request (soft-deletes it, so the user can ask again). Only active owners and moderators may call it.</summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the pending member.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// True when rejected. Errors: Forbidden <see cref="ServiceErrorCodes.NotModerator"/> (also if the group does
        /// not exist), NotFound <see cref="ServiceErrorCodes.MemberNotFound"/>, Conflict <see cref="ServiceErrorCodes.MemberNotPending"/>.
        /// </returns>
        Task<ServiceResult<bool>> RejectMemberAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Bans a member or pending user (status banned, role reset to member); the row is kept so the user cannot join
        /// again. Only active owners and moderators may call it; owners and the caller themselves cannot be banned.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the member to ban.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated membership (without the name). Errors: Forbidden <see cref="ServiceErrorCodes.NotModerator"/>
        /// (also if the group does not exist) or <see cref="ServiceErrorCodes.CannotTargetOwner"/>, NotFound
        /// <see cref="ServiceErrorCodes.MemberNotFound"/>.
        /// </returns>
        Task<ServiceResult<CommunityMemberResponseModel>> BanMemberAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Changes the role of an active member (including promoting to owner). Only active owners may call it; the
        /// last active owner cannot be demoted.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the active member.</param>
        /// <param name="role">New role.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated membership (without the name). Errors: Forbidden <see cref="ServiceErrorCodes.NotOwner"/> (also if
        /// the group does not exist), NotFound <see cref="ServiceErrorCodes.MemberNotFound"/>, Conflict
        /// <see cref="ServiceErrorCodes.LastOwner"/>.
        /// </returns>
        Task<ServiceResult<CommunityMemberResponseModel>> ChangeMemberRoleAsync(RequestUser user, ulong groupId, ulong memberUserId, CommunityMemberRole role, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>The creator becomes the owner member, saved in the same transaction (one SaveChanges) as the group.</item>
    /// <item>Joining a public group: active + joined_at. Joining a private group: pending, until an owner or moderator
    /// approves it (active + joined_at).</item>
    /// <item>Re-joining after leaving restores the soft-deleted membership row (the unique key covers it).</item>
    /// <item>Banned users cannot join again or leave (the ban row is kept). The last owner cannot leave or be demoted.</item>
    /// <item>Deleting a group (owner) soft-deletes the group with its members, forums, forum posts, events, event
    /// registrations and group resources.</item>
    /// </list>
    /// </summary>
    public sealed class CommunityGroupService : ICommunityGroupService
    {
        private readonly GSWebDbContext _db;
        private readonly IUserDirectoryClient _users;
        private readonly TimeProvider _timeProvider;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWebAPI database context.</param>
        /// <param name="users">Reads user full names from the Users API.</param>
        /// <param name="timeProvider">Clock for joined_at (UTC, whole seconds).</param>
        public CommunityGroupService(GSWebDbContext db, IUserDirectoryClient users, TimeProvider timeProvider)
        {
            _db = db;
            _users = users;
            _timeProvider = timeProvider;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityGroupResponseModel>> CreateAsync(RequestUser user, CommunityGroupRequestModel request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(request);
            if (errors.Count > 0)
            {
                return ServiceResult<CommunityGroupResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            var group = new CommunityGroup
            {
                CommunityName = request.CommunityName.Trim(),
                CommunityLocation = Normalize(request.CommunityLocation),
                CommunityDescription = Normalize(request.CommunityDescription),
                CreatedByUserId = user.UserId,
                Visibility = request.Visibility,
            };
            group.Members.Add(new CommunityMember
            {
                UserId = user.UserId,
                Role = CommunityMemberRole.Owner,
                Status = CommunityMemberStatus.Active,
                JoinedAt = _timeProvider.UtcNowSeconds(),
            });

            // One SaveChanges = one transaction: the group and its owner are inserted together or not at all.
            _db.CommunityGroups.Add(group);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, group.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityGroupResponseModel>> GetAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default)
        {
            var groups = await ProjectGroups(_db.CommunityGroups.Where(g => g.Id == groupId), user).ToListAsync(cancellationToken);
            if (groups.Count == 0)
            {
                return ServiceResult<CommunityGroupResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
            }

            await AddNamesAsync(user, groups, cancellationToken);
            return ServiceResult<CommunityGroupResponseModel>.Ok(groups[0]);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<PagedResponseModel<CommunityGroupResponseModel>>> ListAsync(RequestUser user, bool onlyMine, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 0);

            var query = _db.CommunityGroups.AsQueryable();
            if (onlyMine)
            {
                query = query.Where(g => g.Members.Any(m => m.UserId == user.UserId && m.Status != CommunityMemberStatus.Banned));
            }

            var total = await query.CountAsync(cancellationToken);
            var groups = await ProjectGroups(query.OrderByDescending(g => g.CreatedAt).ThenByDescending(g => g.Id).Skip(page * pageSize).Take(pageSize), user)
                .ToListAsync(cancellationToken);
            await AddNamesAsync(user, groups, cancellationToken);
            return ServiceResult<PagedResponseModel<CommunityGroupResponseModel>>.Ok(PagedResponseModel<CommunityGroupResponseModel>.Create(groups, page, pageSize, total));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityGroupResponseModel>> UpdateAsync(RequestUser user, ulong groupId, CommunityGroupRequestModel request, CancellationToken cancellationToken = default)
        {
            var group = await _db.CommunityGroups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
            if (group == null)
            {
                return ServiceResult<CommunityGroupResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
            }

            if (!CommunityMembership.IsOwner(await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken)))
            {
                return ServiceResult<CommunityGroupResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotOwner);
            }

            var errors = Validate(request);
            if (errors.Count > 0)
            {
                return ServiceResult<CommunityGroupResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            group.CommunityName = request.CommunityName.Trim();
            group.CommunityLocation = Normalize(request.CommunityLocation);
            group.CommunityDescription = Normalize(request.CommunityDescription);
            group.Visibility = request.Visibility;
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, groupId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default)
        {
            if (!await _db.CommunityGroups.AnyAsync(g => g.Id == groupId, cancellationToken))
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
            }

            if (!CommunityMembership.IsOwner(await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken)))
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotOwner);
            }

            // Soft-delete the group and its children in one SaveChanges (one transaction).
            var group = await _db.CommunityGroups
                .Include(g => g.Members)
                .Include(g => g.Forums).ThenInclude(f => f.Posts)
                .Include(g => g.Events).ThenInclude(e => e.Interests)
                .Include(g => g.Resources)
                .AsSplitQuery()
                .FirstAsync(g => g.Id == groupId, cancellationToken);
            _db.CommunityEventInterests.RemoveRange(group.Events.SelectMany(e => e.Interests));
            _db.CommunityEvents.RemoveRange(group.Events);
            _db.CommunityMembers.RemoveRange(group.Members);
            _db.CommunityForumPosts.RemoveRange(group.Forums.SelectMany(f => f.Posts));
            _db.CommunityForums.RemoveRange(group.Forums);
            _db.CommunityResources.RemoveRange(group.Resources);
            _db.CommunityGroups.Remove(group);
            await _db.SaveChangesAsync(cancellationToken);

            return ServiceResult<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityMemberResponseModel>> JoinAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default)
        {
            var group = await _db.CommunityGroups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
            if (group == null)
            {
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
            }

            var now = _timeProvider.UtcNowSeconds();
            var isPublic = group.Visibility == CommunityGroupVisibility.Public;

            // IgnoreQueryFilters: a soft-deleted row (the user left before) is restored instead of inserting a duplicate.
            var member = await _db.CommunityMembers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.CommunityGroupId == groupId && m.UserId == user.UserId, cancellationToken);
            if (member is { DeletedAt: null })
            {
                return member.Status switch
                {
                    CommunityMemberStatus.Banned => ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.MemberBanned),
                    CommunityMemberStatus.Pending => ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.AlreadyPending),
                    _ => ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.AlreadyMember),
                };
            }

            if (member == null)
            {
                member = new CommunityMember { CommunityGroupId = groupId, UserId = user.UserId };
                _db.CommunityMembers.Add(member);
            }

            member.DeletedAt = null;
            member.Role = CommunityMemberRole.Member;
            member.Status = isPublic ? CommunityMemberStatus.Active : CommunityMemberStatus.Pending;
            member.JoinedAt = isPublic ? now : null;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (ex.IsUniqueViolation())
            {
                // Two simultaneous join requests for the same user.
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.AlreadyMember);
            }

            return ServiceResult<CommunityMemberResponseModel>.Ok(await MapWithNameAsync(user, member, cancellationToken));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> LeaveAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default)
        {
            var member = await _db.CommunityMembers.FirstOrDefaultAsync(m => m.CommunityGroupId == groupId && m.UserId == user.UserId, cancellationToken);
            if (member == null)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.MemberNotFound);
            }

            if (member.Status == CommunityMemberStatus.Banned)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.MemberBanned);
            }

            if (member.Role == CommunityMemberRole.Owner && await IsLastOwnerAsync(groupId, member.UserId, cancellationToken))
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.LastOwner);
            }

            _db.CommunityMembers.Remove(member);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<CommunityMemberResponseModel>>> ListMembersAsync(RequestUser user, ulong groupId, CancellationToken cancellationToken = default)
        {
            var me = await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken);
            if (me == null)
            {
                return await _db.CommunityGroups.AnyAsync(g => g.Id == groupId, cancellationToken)
                    ? ServiceResult<List<CommunityMemberResponseModel>>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember)
                    : ServiceResult<List<CommunityMemberResponseModel>>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
            }

            // Pending and banned members are only visible to owners and moderators.
            var query = _db.CommunityMembers.AsNoTracking().Where(m => m.CommunityGroupId == groupId);
            if (!CommunityMembership.IsOwnerOrModerator(me))
            {
                query = query.Where(m => m.Status == CommunityMemberStatus.Active);
            }

            var members = await query.OrderBy(m => m.Id).ToListAsync(cancellationToken);
            var names = await _users.GetFullNamesAsync(user, members.Select(m => m.UserId), cancellationToken);
            return ServiceResult<List<CommunityMemberResponseModel>>.Ok(members.Select(m => CommunityMembership.Map(m, names)).ToList());
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityMemberResponseModel>> ApproveMemberAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken = default)
        {
            var (target, error) = await LoadTargetAsModeratorAsync(user, groupId, memberUserId, cancellationToken);
            if (error != null)
            {
                return error.As<CommunityMemberResponseModel>();
            }

            if (target!.Status != CommunityMemberStatus.Pending)
            {
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.MemberNotPending);
            }

            target.Status = CommunityMemberStatus.Active;
            target.JoinedAt = _timeProvider.UtcNowSeconds();
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CommunityMemberResponseModel>.Ok(await MapWithNameAsync(user, target, cancellationToken));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> RejectMemberAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken = default)
        {
            var (target, error) = await LoadTargetAsModeratorAsync(user, groupId, memberUserId, cancellationToken);
            if (error != null)
            {
                return error.As<bool>();
            }

            if (target!.Status != CommunityMemberStatus.Pending)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.MemberNotPending);
            }

            _db.CommunityMembers.Remove(target);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityMemberResponseModel>> BanMemberAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken = default)
        {
            var (target, error) = await LoadTargetAsModeratorAsync(user, groupId, memberUserId, cancellationToken);
            if (error != null)
            {
                return error.As<CommunityMemberResponseModel>();
            }

            if (target!.Role == CommunityMemberRole.Owner || target.UserId == user.UserId)
            {
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.CannotTargetOwner);
            }

            target.Status = CommunityMemberStatus.Banned;
            target.Role = CommunityMemberRole.Member;
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CommunityMemberResponseModel>.Ok(await MapWithNameAsync(user, target, cancellationToken));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityMemberResponseModel>> ChangeMemberRoleAsync(RequestUser user, ulong groupId, ulong memberUserId, CommunityMemberRole role, CancellationToken cancellationToken = default)
        {
            var me = await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken);
            if (!CommunityMembership.IsOwner(me))
            {
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotOwner);
            }

            var target = await _db.CommunityMembers.FirstOrDefaultAsync(
                m => m.CommunityGroupId == groupId && m.UserId == memberUserId && m.Status == CommunityMemberStatus.Active,
                cancellationToken);
            if (target == null)
            {
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.MemberNotFound);
            }

            if (target.Role == CommunityMemberRole.Owner && role != CommunityMemberRole.Owner && await IsLastOwnerAsync(groupId, target.UserId, cancellationToken))
            {
                return ServiceResult<CommunityMemberResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.LastOwner);
            }

            target.Role = role;
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CommunityMemberResponseModel>.Ok(await MapWithNameAsync(user, target, cancellationToken));
        }

        private async Task<(CommunityMember? Target, ServiceResult<bool>? Error)> LoadTargetAsModeratorAsync(RequestUser user, ulong groupId, ulong memberUserId, CancellationToken cancellationToken)
        {
            var me = await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken);
            if (!CommunityMembership.IsOwnerOrModerator(me))
            {
                return (null, ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotModerator));
            }

            var target = await _db.CommunityMembers.FirstOrDefaultAsync(m => m.CommunityGroupId == groupId && m.UserId == memberUserId, cancellationToken);
            return target == null
                ? (null, ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.MemberNotFound))
                : (target, null);
        }

        private async Task<bool> IsLastOwnerAsync(ulong groupId, ulong ownerUserId, CancellationToken cancellationToken)
        {
            var otherOwnerExists = await _db.CommunityMembers.AnyAsync(
                m => m.CommunityGroupId == groupId && m.Role == CommunityMemberRole.Owner && m.Status == CommunityMemberStatus.Active && m.UserId != ownerUserId,
                cancellationToken);
            return !otherOwnerExists;
        }

        private static IQueryable<CommunityGroupResponseModel> ProjectGroups(IQueryable<CommunityGroup> query, RequestUser user)
        {
            return query.AsNoTracking().Select(g => new CommunityGroupResponseModel
            {
                Id = g.Id,
                CommunityName = g.CommunityName,
                CommunityLocation = g.CommunityLocation,
                CommunityDescription = g.CommunityDescription,
                CreatedByUserId = g.CreatedByUserId,
                Visibility = g.Visibility,
                ActiveMembers = g.Members.Count(m => m.Status == CommunityMemberStatus.Active),
                CreatedAt = g.CreatedAt,
                MyMembership = g.Members
                    .Where(m => m.UserId == user.UserId)
                    .Select(m => new CommunityMemberResponseModel
                    {
                        Id = m.Id,
                        CommunityGroupId = m.CommunityGroupId,
                        UserId = m.UserId,
                        Role = m.Role,
                        Status = m.Status,
                        JoinedAt = m.JoinedAt,
                    })
                    .FirstOrDefault(),
            });
        }

        /// <summary>Maps a membership with the member's full name (Users API <c>usersapi/users/fullnames</c>; null if not returned).</summary>
        private async Task<CommunityMemberResponseModel> MapWithNameAsync(RequestUser user, CommunityMember member, CancellationToken cancellationToken)
        {
            var names = await _users.GetFullNamesAsync(user, new[] { member.UserId }, cancellationToken);
            return CommunityMembership.Map(member, names);
        }

        /// <summary>
        /// Fills the creator's name and the user's own <see cref="CommunityGroupResponseModel.MyMembership"/> name, in one
        /// Users API call (<c>usersapi/users/fullnames</c>). Names the API does not return stay null.
        /// </summary>
        private async Task AddNamesAsync(RequestUser user, List<CommunityGroupResponseModel> groups, CancellationToken cancellationToken)
        {
            var userIds = groups.Select(g => g.CreatedByUserId);
            if (groups.Any(g => g.MyMembership != null))
            {
                userIds = userIds.Append(user.UserId);
            }

            var names = await _users.GetFullNamesAsync(user, userIds, cancellationToken);
            foreach (var group in groups)
            {
                group.CreatedByName = names.GetValueOrDefault(group.CreatedByUserId);
                if (group.MyMembership != null)
                {
                    group.MyMembership.UserName = names.GetValueOrDefault(group.MyMembership.UserId);
                }
            }
        }

        private static List<string> Validate(CommunityGroupRequestModel request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.CommunityName) || request.CommunityName.Trim().Length > 120
                || (request.CommunityLocation?.Trim().Length ?? 0) > 255)
            {
                errors.Add(ServiceErrorCodes.GroupNameRequired);
            }

            return errors;
        }

        private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
