using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.Community
{
    /// <summary>Community group forums and their posts (community_forums / community_forum_posts).</summary>
    public interface ICommunityForumService
    {
        /// <summary>
        /// Lists the forum categories (seeded catalog) ordered by id, with the name in the session's language
        /// (Accept-Language: <c>es</c>, <c>es-PR</c>... use the Spanish name). A language with no translation gets the English
        /// name, and a category with no translation at all its stored name. It needs no group membership.
        /// </summary>
        /// <param name="langCode">Language of the session (Accept-Language); null = English.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The categories. It does not fail.</returns>
        Task<ServiceResult<List<CommunityForumCategoryResponseModel>>> ListCategoriesAsync(string? langCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists forums: pinned first, then by latest activity (last post, or creation when there are no posts). With a group:
        /// that group's forums, for its active members only. Without (optional filter): every forum the user can see,
        /// platform-wide ones plus those of the groups where the user is an active member.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Optional group filter.</param>
        /// <param name="categoryId">If set, only forums of this category; null = all categories.</param>
        /// <param name="page">Zero-based page; negative values become 0.</param>
        /// <param name="pageSize">Page size, clamped to 1-100.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The page of forums with the total items and pages. Errors (only with a group): NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>.</returns>
        Task<ServiceResult<PagedResponseModel<CommunityForumResponseModel>>> ListAsync(RequestUser user, ulong? groupId, byte? categoryId, int page, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>Reads one forum with its post counters. Only active members of the forum's group may call it; anyone for a platform-wide forum.</summary>
        /// <param name="user">User in session.</param>
        /// <param name="forumId">Forum id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The forum. Errors: NotFound <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>.</returns>
        Task<ServiceResult<CommunityForumResponseModel>> GetAsync(RequestUser user, ulong forumId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a forum; the caller is its author. In a group, any active member of the group may call it; without a group
        /// (platform-wide) any user may. Moderators-only categories (announcements) need a group owner or moderator, or staff
        /// for a platform-wide forum. Title and description pass the profanity filter.
        /// </summary>
        /// <param name="user">User in session (the author).</param>
        /// <param name="groupId">Group id; null = platform-wide.</param>
        /// <param name="request">Title, description and category (null = general).</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The created forum. Errors: NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/> or <see cref="ServiceErrorCodes.ForumCategoryModeratorsOnly"/>, Validation
        /// <see cref="ServiceErrorCodes.ForumTitleRequired"/>, <see cref="ServiceErrorCodes.ForumDescriptionTooLong"/>,
        /// <see cref="ServiceErrorCodes.ForumContentNotAllowed"/>, <see cref="ServiceErrorCodes.ForumCategoryInvalid"/>.
        /// </returns>
        Task<ServiceResult<CommunityForumResponseModel>> CreateAsync(RequestUser user, ulong? groupId, CommunityForumRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the title, description and category of a forum. The author and the group owners/moderators may call it
        /// (active members only); on a locked forum only owners/moderators. Same validation as <see cref="CreateAsync"/>.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="forumId">Forum id.</param>
        /// <param name="request">New values; a null category keeps the current one.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated forum. Errors: NotFound <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/>, <see cref="ServiceErrorCodes.ForumNotAuthor"/> or
        /// <see cref="ServiceErrorCodes.ForumCategoryModeratorsOnly"/>, Conflict <see cref="ServiceErrorCodes.ForumLocked"/>, and
        /// the validation errors of <see cref="CreateAsync"/>.
        /// </returns>
        Task<ServiceResult<CommunityForumResponseModel>> UpdateAsync(RequestUser user, ulong forumId, CommunityForumRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>Pins/unpins and locks/unlocks a forum. Only active owners and moderators of the group may call it.</summary>
        /// <param name="user">User in session.</param>
        /// <param name="forumId">Forum id.</param>
        /// <param name="request">New flags; null fields are left unchanged.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated forum. Errors: NotFound <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/> or <see cref="ServiceErrorCodes.NotModerator"/>.
        /// </returns>
        Task<ServiceResult<CommunityForumResponseModel>> ModerateAsync(RequestUser user, ulong forumId, CommunityForumModerationRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes a forum with all its posts, in one transaction. The author and the group owners/moderators may call it
        /// (active members only); the lock does not apply.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="forumId">Forum id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// True when deleted. Errors: NotFound <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/> or <see cref="ServiceErrorCodes.ForumNotAuthor"/>.
        /// </returns>
        Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong forumId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the posts of a forum as a flat list ordered by creation (replies point to their parent through
        /// <see cref="CommunityForumPostResponseModel.ParentPostId"/>). Deleted posts are left out, except those that still have
        /// replies, which are returned as placeholders with no author or text. Only active members of the group may call it.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="forumId">Forum id.</param>
        /// <param name="page">Zero-based page; negative values become 0.</param>
        /// <param name="pageSize">Page size, clamped to 1-100.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The page of posts with the total items and pages. Errors: NotFound <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>.</returns>
        Task<ServiceResult<PagedResponseModel<CommunityForumPostResponseModel>>> ListPostsAsync(RequestUser user, ulong forumId, int page, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a post, or a reply to a post of the same forum that is not deleted. Any active member of the group may call it;
        /// on a locked forum only owners/moderators. The text passes the profanity filter.
        /// </summary>
        /// <param name="user">User in session (the author).</param>
        /// <param name="forumId">Forum id.</param>
        /// <param name="request">Text and optional parent post.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The created post. Errors: NotFound <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/>, Conflict <see cref="ServiceErrorCodes.ForumLocked"/>, Validation
        /// <see cref="ServiceErrorCodes.PostTextRequired"/> (blank or over 10,000 characters),
        /// <see cref="ServiceErrorCodes.ForumContentNotAllowed"/>, <see cref="ServiceErrorCodes.PostParentInvalid"/>.
        /// </returns>
        Task<ServiceResult<CommunityForumPostResponseModel>> CreatePostAsync(RequestUser user, ulong forumId, CommunityForumPostRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edits the text of a post (the parent does not change). Only the author may call it, while an active member; on a
        /// locked forum only if the author is an owner/moderator. edited_at is set only when the text really changes.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="postId">Post id.</param>
        /// <param name="request">New text; <see cref="CommunityForumPostRequestModel.ParentPostId"/> is ignored.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The post. Errors: NotFound <see cref="ServiceErrorCodes.PostNotFound"/> (also deleted posts) or
        /// <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/> or
        /// <see cref="ServiceErrorCodes.PostNotAuthor"/>, Conflict <see cref="ServiceErrorCodes.ForumLocked"/>, Validation
        /// <see cref="ServiceErrorCodes.PostTextRequired"/> or <see cref="ServiceErrorCodes.ForumContentNotAllowed"/>.
        /// </returns>
        Task<ServiceResult<CommunityForumPostResponseModel>> UpdatePostAsync(RequestUser user, ulong postId, CommunityForumPostRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes a post; its replies are kept and, while it has replies, it is listed as a placeholder. The author and the
        /// group owners/moderators may call it (active members only); the lock does not apply.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="postId">Post id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// True when deleted. Errors: NotFound <see cref="ServiceErrorCodes.PostNotFound"/> (also already deleted posts) or
        /// <see cref="ServiceErrorCodes.ForumNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/> or
        /// <see cref="ServiceErrorCodes.PostNotAuthor"/>.
        /// </returns>
        Task<ServiceResult<bool>> DeletePostAsync(RequestUser user, ulong postId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>Only active members of the group see its forums and posts. The group is optional: a platform-wide forum (no
    /// group) is created and used by any user, and staff take the owner/moderator role (pin, lock, moderate, announcements).</item>
    /// <item>Any active member creates forums; categories marked moderators_only (announcements) need an owner or
    /// moderator.</item>
    /// <item>The author or a group owner/moderator edits and deletes a forum. Only owners/moderators pin or lock it.</item>
    /// <item>A locked forum accepts no new posts and no edits, except from owners/moderators.</item>
    /// <item>A reply must point to a post of the same forum that is not deleted.</item>
    /// <item>Only the author edits a post (edited_at is set). The author or an owner/moderator deletes it.</item>
    /// <item>Deleting a post keeps its replies; while it has replies, the post is listed as deleted with no text.
    /// Deleting a forum soft-deletes its posts.</item>
    /// <item>Titles, descriptions and posts cannot contain foul language in English or Spanish (<see cref="IProfanityFilter"/>).</item>
    /// </list>
    /// </summary>
    public sealed class CommunityForumService : ICommunityForumService
    {
        private const int TitleMaxLength = 150;
        private const int TextMaxLength = 10_000;

        private readonly GSWebDbContext _db;
        private readonly IUserDirectoryClient _users;
        private readonly TimeProvider _timeProvider;
        private readonly IProfanityFilter _profanity;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWebAPI database context.</param>
        /// <param name="users">Reads user full names from the Users API.</param>
        /// <param name="timeProvider">Clock for edited_at (UTC, whole seconds).</param>
        /// <param name="profanity">Rejects foul language (English and Spanish) in forum titles, descriptions and posts.</param>
        public CommunityForumService(GSWebDbContext db, IUserDirectoryClient users, TimeProvider timeProvider, IProfanityFilter profanity)
        {
            _db = db;
            _users = users;
            _timeProvider = timeProvider;
            _profanity = profanity;
        }

        // ---------- Categories ----------

        /// <inheritdoc/>
        public async Task<ServiceResult<List<CommunityForumCategoryResponseModel>>> ListCategoriesAsync(string? langCode, CancellationToken cancellationToken = default)
        {
            var lang = RequestLanguage.Primary(langCode);
            var categories = await _db.CommunityForumCategories.AsNoTracking()
                .OrderBy(c => c.Id)
                .Select(c => new CommunityForumCategoryResponseModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Translations.Where(t => t.LangCode == lang).Select(t => t.Name).FirstOrDefault()
                        ?? c.Translations.Where(t => t.LangCode == CommunityForumCategoryTranslation.English).Select(t => t.Name).FirstOrDefault()
                        ?? c.Name,
                    ModeratorsOnly = c.ModeratorsOnly,
                })
                .ToListAsync(cancellationToken);
            return ServiceResult<List<CommunityForumCategoryResponseModel>>.Ok(categories);
        }

        /// <summary>Category id to its name in the session's language (same rules as <see cref="ListCategoriesAsync"/>).</summary>
        private async Task<Dictionary<byte, string>> CategoryNamesAsync(string? langCode, CancellationToken cancellationToken)
        {
            var categories = await ListCategoriesAsync(langCode, cancellationToken);
            return categories.Data!.ToDictionary(c => c.Id, c => c.Name);
        }

        // ---------- Forums ----------

        /// <inheritdoc/>
        public async Task<ServiceResult<PagedResponseModel<CommunityForumResponseModel>>> ListAsync(RequestUser user, ulong? groupId, byte? categoryId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var viewer = await CommunityMembership.GetViewerAsync(_db, user, cancellationToken);
            var query = _db.CommunityForums.AsNoTracking();
            if (groupId is { } id)
            {
                if (!viewer.CanSee(id))
                {
                    return await _db.CommunityGroups.AnyAsync(g => g.Id == id, cancellationToken)
                        ? ServiceResult<PagedResponseModel<CommunityForumResponseModel>>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember)
                        : ServiceResult<PagedResponseModel<CommunityForumResponseModel>>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
                }

                query = query.Where(f => f.CommunityGroupId == id);
            }
            else
            {
                // No filter: platform-wide forums plus those of the user's groups.
                var myGroups = viewer.GroupIds;
                query = query.Where(f => f.CommunityGroupId == null || myGroups.Contains(f.CommunityGroupId.Value));
            }

            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 0);

            if (categoryId.HasValue)
            {
                query = query.Where(f => f.CommunityForumCategoryId == categoryId.Value);
            }

            var total = await query.CountAsync(cancellationToken);

            // Pinned first, then by latest activity (last post, or creation when there are no posts).
            var rows = await Project(query)
                .OrderByDescending(r => r.Forum.IsPinned)
                .ThenByDescending(r => r.LastPostAt ?? r.Forum.CreatedAt)
                .ThenByDescending(r => r.Forum.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var names = await _users.GetFullNamesAsync(user, rows.Select(r => r.Forum.UserId), cancellationToken);
            var categoryNames = await CategoryNamesAsync(user.LangCode, cancellationToken);
            return ServiceResult<PagedResponseModel<CommunityForumResponseModel>>.Ok(
                PagedResponseModel<CommunityForumResponseModel>.Create(rows.Select(r => Map(r, viewer, names, categoryNames)).ToList(), page, pageSize, total));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityForumResponseModel>> GetAsync(RequestUser user, ulong forumId, CancellationToken cancellationToken = default)
        {
            var access = await LoadForumAsync(user, forumId, track: false, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<CommunityForumResponseModel>();
            }

            var row = await Project(_db.CommunityForums.AsNoTracking().Where(f => f.Id == forumId)).FirstAsync(cancellationToken);
            var names = await _users.GetFullNamesAsync(user, new[] { row.Forum.UserId }, cancellationToken);
            var categoryNames = await CategoryNamesAsync(user.LangCode, cancellationToken);
            return ServiceResult<CommunityForumResponseModel>.Ok(Map(row, access.Viewer!, names, categoryNames));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityForumResponseModel>> CreateAsync(RequestUser user, ulong? groupId, CommunityForumRequestModel request, CancellationToken cancellationToken = default)
        {
            if (groupId is { } id && !await _db.CommunityGroups.AnyAsync(g => g.Id == id, cancellationToken))
            {
                return ServiceResult<CommunityForumResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
            }

            var viewer = await CommunityMembership.GetViewerAsync(_db, user, cancellationToken);
            if (!viewer.CanSee(groupId))
            {
                return ServiceResult<CommunityForumResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember);
            }

            var categoryId = request.CategoryId ?? CommunityForumCategory.GeneralId;
            var failure = Validate(request) ?? await CheckCategoryAsync(categoryId, viewer.IsModerator(groupId), cancellationToken);
            if (failure != null)
            {
                return failure.As<CommunityForumResponseModel>();
            }

            var forum = new CommunityForum
            {
                CommunityGroupId = groupId,
                CommunityForumCategoryId = categoryId,
                UserId = user.UserId,
                Title = request.Title.Trim(),
                Description = Normalize(request.Description),
            };
            _db.CommunityForums.Add(forum);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, forum.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityForumResponseModel>> UpdateAsync(RequestUser user, ulong forumId, CommunityForumRequestModel request, CancellationToken cancellationToken = default)
        {
            var access = await LoadForumAsync(user, forumId, track: true, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<CommunityForumResponseModel>();
            }

            var forum = access.Forum!;
            var isModerator = access.IsModerator;
            if (forum.UserId != user.UserId && !isModerator)
            {
                return ServiceResult<CommunityForumResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.ForumNotAuthor);
            }

            if (forum.IsLocked && !isModerator)
            {
                return ServiceResult<CommunityForumResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.ForumLocked);
            }

            var categoryId = request.CategoryId ?? forum.CommunityForumCategoryId;
            var failure = Validate(request) ?? await CheckCategoryAsync(categoryId, isModerator, cancellationToken);
            if (failure != null)
            {
                return failure.As<CommunityForumResponseModel>();
            }

            forum.Title = request.Title.Trim();
            forum.Description = Normalize(request.Description);
            forum.CommunityForumCategoryId = categoryId;
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, forumId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityForumResponseModel>> ModerateAsync(RequestUser user, ulong forumId, CommunityForumModerationRequestModel request, CancellationToken cancellationToken = default)
        {
            var access = await LoadForumAsync(user, forumId, track: true, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<CommunityForumResponseModel>();
            }

            if (!access.IsModerator)
            {
                return ServiceResult<CommunityForumResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotModerator);
            }

            access.Forum!.IsPinned = request.IsPinned ?? access.Forum.IsPinned;
            access.Forum.IsLocked = request.IsLocked ?? access.Forum.IsLocked;
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, forumId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong forumId, CancellationToken cancellationToken = default)
        {
            var access = await LoadForumAsync(user, forumId, track: true, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<bool>();
            }

            if (access.Forum!.UserId != user.UserId && !access.IsModerator)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.ForumNotAuthor);
            }

            // Forum and posts are soft-deleted in one SaveChanges (one transaction).
            _db.CommunityForumPosts.RemoveRange(await _db.CommunityForumPosts.Where(p => p.CommunityForumId == forumId).ToListAsync(cancellationToken));
            _db.CommunityForums.Remove(access.Forum);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        // ---------- Posts ----------

        /// <inheritdoc/>
        public async Task<ServiceResult<PagedResponseModel<CommunityForumPostResponseModel>>> ListPostsAsync(RequestUser user, ulong forumId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var access = await LoadForumAsync(user, forumId, track: false, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<PagedResponseModel<CommunityForumPostResponseModel>>();
            }

            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 0);

            // IgnoreQueryFilters to include deleted posts that still have replies; it turns the filters off for the
            // whole query, so the subqueries check deleted_at themselves.
            var posts = _db.CommunityForumPosts.IgnoreQueryFilters();
            var shown = posts.AsNoTracking()
                .Where(p => p.CommunityForumId == forumId
                    && (p.DeletedAt == null || posts.Any(r => r.ParentPostId == p.Id && r.DeletedAt == null)));
            var total = await shown.CountAsync(cancellationToken);
            var rows = await shown
                .OrderBy(p => p.CreatedAt)
                .ThenBy(p => p.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(p => new PostRow
                {
                    Post = p,
                    RepliesCount = posts.Count(r => r.ParentPostId == p.Id && r.DeletedAt == null),
                })
                .ToListAsync(cancellationToken);

            var names = await _users.GetFullNamesAsync(user, rows.Where(r => r.Post.DeletedAt == null).Select(r => r.Post.UserId), cancellationToken);
            return ServiceResult<PagedResponseModel<CommunityForumPostResponseModel>>.Ok(
                PagedResponseModel<CommunityForumPostResponseModel>.Create(rows.Select(r => Map(r, access.Forum!, access.Viewer!, names)).ToList(), page, pageSize, total));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityForumPostResponseModel>> CreatePostAsync(RequestUser user, ulong forumId, CommunityForumPostRequestModel request, CancellationToken cancellationToken = default)
        {
            var access = await LoadForumAsync(user, forumId, track: false, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<CommunityForumPostResponseModel>();
            }

            if (access.Forum!.IsLocked && !access.IsModerator)
            {
                return ServiceResult<CommunityForumPostResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.ForumLocked);
            }

            var failure = ValidateText(request.PostText);
            if (failure != null)
            {
                return failure.As<CommunityForumPostResponseModel>();
            }

            if (request.ParentPostId is { } parentId
                && !await _db.CommunityForumPosts.AnyAsync(p => p.Id == parentId && p.CommunityForumId == forumId, cancellationToken))
            {
                return ServiceResult<CommunityForumPostResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PostParentInvalid);
            }

            var post = new CommunityForumPost
            {
                CommunityForumId = forumId,
                ParentPostId = request.ParentPostId,
                UserId = user.UserId,
                PostText = request.PostText.Trim(),
            };
            _db.CommunityForumPosts.Add(post);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetPostAsync(user, post.Id, access.Forum, access.Viewer!, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityForumPostResponseModel>> UpdatePostAsync(RequestUser user, ulong postId, CommunityForumPostRequestModel request, CancellationToken cancellationToken = default)
        {
            var post = await _db.CommunityForumPosts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
            if (post == null)
            {
                return ServiceResult<CommunityForumPostResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PostNotFound);
            }

            var access = await LoadForumAsync(user, post.CommunityForumId, track: false, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<CommunityForumPostResponseModel>();
            }

            if (post.UserId != user.UserId)
            {
                return ServiceResult<CommunityForumPostResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.PostNotAuthor);
            }

            if (access.Forum!.IsLocked && !access.IsModerator)
            {
                return ServiceResult<CommunityForumPostResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.ForumLocked);
            }

            var failure = ValidateText(request.PostText);
            if (failure != null)
            {
                return failure.As<CommunityForumPostResponseModel>();
            }

            // The parent of a post does not change. edited_at only moves when the text really changes.
            var text = request.PostText.Trim();
            if (text != post.PostText)
            {
                post.PostText = text;
                post.EditedAt = _timeProvider.UtcNowSeconds();
                await _db.SaveChangesAsync(cancellationToken);
            }

            return await GetPostAsync(user, post.Id, access.Forum, access.Viewer!, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeletePostAsync(RequestUser user, ulong postId, CancellationToken cancellationToken = default)
        {
            var post = await _db.CommunityForumPosts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
            if (post == null)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PostNotFound);
            }

            var access = await LoadForumAsync(user, post.CommunityForumId, track: false, cancellationToken);
            if (access.Failure != null)
            {
                return access.Failure.As<bool>();
            }

            if (post.UserId != user.UserId && !access.IsModerator)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.PostNotAuthor);
            }

            // Replies are kept (see the ClientNoAction rule in CommunityForumPostConfiguration).
            _db.CommunityForumPosts.Remove(post);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        // ---------- Helpers ----------

        private sealed class ForumRow
        {
            public CommunityForum Forum { get; init; } = null!;

            public string CategoryCode { get; init; } = null!;

            public int PostsCount { get; init; }

            public DateTime? LastPostAt { get; init; }
        }

        private sealed class PostRow
        {
            public CommunityForumPost Post { get; init; } = null!;

            public int RepliesCount { get; init; }
        }

        private sealed record ForumAccess(CommunityForum? Forum, CommunityViewer? Viewer, ServiceResult<bool>? Failure)
        {
            /// <summary>Owner/moderator of the forum's group, or staff for a platform-wide forum.</summary>
            public bool IsModerator => Forum != null && Viewer != null && Viewer.IsModerator(Forum.CommunityGroupId);
        }

        /// <summary>The forum (not deleted) and what the user can do with it (active member of its group, or anyone without a group).</summary>
        private async Task<ForumAccess> LoadForumAsync(RequestUser user, ulong forumId, bool track, CancellationToken cancellationToken)
        {
            var forums = track ? _db.CommunityForums : _db.CommunityForums.AsNoTracking();
            var forum = await forums.FirstOrDefaultAsync(f => f.Id == forumId, cancellationToken);
            if (forum == null)
            {
                return new ForumAccess(null, null, ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.ForumNotFound));
            }

            var viewer = await CommunityMembership.GetViewerAsync(_db, user, cancellationToken);
            return viewer.CanSee(forum.CommunityGroupId)
                ? new ForumAccess(forum, viewer, null)
                : new ForumAccess(null, null, ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember));
        }

        /// <summary>Post counters use the posts query filter, so deleted posts are not counted.</summary>
        private static IQueryable<ForumRow> Project(IQueryable<CommunityForum> forums)
        {
            return forums.Select(f => new ForumRow
            {
                Forum = f,
                CategoryCode = f.Category.Code,
                PostsCount = f.Posts.Count(),
                LastPostAt = f.Posts.Max(p => (DateTime?)p.CreatedAt),
            });
        }

        private async Task<ServiceResult<CommunityForumPostResponseModel>> GetPostAsync(RequestUser user, ulong postId, CommunityForum forum, CommunityViewer viewer, CancellationToken cancellationToken)
        {
            var row = await _db.CommunityForumPosts.AsNoTracking()
                .Where(p => p.Id == postId)
                .Select(p => new PostRow { Post = p, RepliesCount = p.Replies.Count() })
                .FirstAsync(cancellationToken);
            var names = await _users.GetFullNamesAsync(user, new[] { row.Post.UserId }, cancellationToken);
            return ServiceResult<CommunityForumPostResponseModel>.Ok(Map(row, forum, viewer, names));
        }

        private async Task<ServiceResult<bool>?> CheckCategoryAsync(byte categoryId, bool isModerator, CancellationToken cancellationToken)
        {
            var category = await _db.CommunityForumCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
            if (category == null)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.ForumCategoryInvalid);
            }

            return category.ModeratorsOnly && !isModerator
                ? ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.ForumCategoryModeratorsOnly)
                : null;
        }

        private ServiceResult<bool>? Validate(CommunityForumRequestModel request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > TitleMaxLength)
            {
                errors.Add(ServiceErrorCodes.ForumTitleRequired);
            }

            if (request.Description != null && request.Description.Trim().Length > TextMaxLength)
            {
                errors.Add(ServiceErrorCodes.ForumDescriptionTooLong);
            }

            if (_profanity.ContainsProfanity(request.Title) || _profanity.ContainsProfanity(request.Description))
            {
                errors.Add(ServiceErrorCodes.ForumContentNotAllowed);
            }

            return errors.Count > 0 ? ServiceResult<bool>.Fail(ServiceErrorKind.Validation, errors) : null;
        }

        private ServiceResult<bool>? ValidateText(string? text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Trim().Length > TextMaxLength)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PostTextRequired);
            }

            return _profanity.ContainsProfanity(text)
                ? ServiceResult<bool>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.ForumContentNotAllowed)
                : null;
        }

        private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static CommunityForumResponseModel Map(ForumRow row, CommunityViewer viewer, IReadOnlyDictionary<ulong, string> names, IReadOnlyDictionary<byte, string> categoryNames)
        {
            var forum = row.Forum;
            return new CommunityForumResponseModel
            {
                Id = forum.Id,
                CommunityGroupId = forum.CommunityGroupId,
                CategoryId = forum.CommunityForumCategoryId,
                CategoryCode = row.CategoryCode,
                CategoryName = categoryNames.GetValueOrDefault(forum.CommunityForumCategoryId),
                AuthorId = forum.UserId,
                AuthorName = names.GetValueOrDefault(forum.UserId),
                Title = forum.Title,
                Description = forum.Description,
                IsPinned = forum.IsPinned,
                IsLocked = forum.IsLocked,
                PostsCount = row.PostsCount,
                LastPostAt = row.LastPostAt.HasValue ? UtcDateTimeConverter.ToUtc(row.LastPostAt.Value) : null,
                CanEdit = forum.UserId == viewer.UserId || viewer.IsModerator(forum.CommunityGroupId),
                CreatedAt = forum.CreatedAt,
                UpdatedAt = forum.UpdatedAt,
            };
        }

        private static CommunityForumPostResponseModel Map(PostRow row, CommunityForum forum, CommunityViewer viewer, IReadOnlyDictionary<ulong, string> names)
        {
            var post = row.Post;
            var isDeleted = post.DeletedAt != null;
            var isAuthor = post.UserId == viewer.UserId;
            var isModerator = viewer.IsModerator(forum.CommunityGroupId);
            return new CommunityForumPostResponseModel
            {
                Id = post.Id,
                CommunityForumId = post.CommunityForumId,
                ParentPostId = post.ParentPostId,
                AuthorId = isDeleted ? null : post.UserId,
                AuthorName = isDeleted ? null : names.GetValueOrDefault(post.UserId),
                PostText = isDeleted ? null : post.PostText,
                IsDeleted = isDeleted,
                RepliesCount = row.RepliesCount,
                CanEdit = !isDeleted && isAuthor && (!forum.IsLocked || isModerator),
                CanDelete = !isDeleted && (isAuthor || isModerator),
                CreatedAt = post.CreatedAt,
                EditedAt = post.EditedAt,
            };
        }
    }
}
