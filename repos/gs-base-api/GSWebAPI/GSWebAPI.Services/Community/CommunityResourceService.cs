using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.Community
{
    /// <summary>Community resources (external links or uploaded files), per group or platform-wide.</summary>
    public interface ICommunityResourceService
    {
        /// <summary>
        /// Creates a resource; the caller is its author. Group resources need an active member of the group; platform-wide
        /// resources (no group) can be created by any user. It is published now or kept as a draft depending on
        /// <see cref="CommunityResourceRequestModel.Publish"/>. The resource is either an uploaded file (<paramref name="file"/>,
        /// stored in Blob Storage; the link and type in the request are then ignored) or an external http/https link.
        /// </summary>
        /// <param name="user">User in session (the author).</param>
        /// <param name="request">Group (null = platform-wide), title, description, type, link and publish flag.</param>
        /// <param name="file">The uploaded file (see <see cref="CommunityResourceFiles"/>), or null for an external link.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The created resource (an uploaded file comes with a signed read link). Errors: Validation
        /// <see cref="ServiceErrorCodes.ResourceTitleRequired"/> (blank or over 150 characters),
        /// <see cref="ServiceErrorCodes.ResourceLinkRequired"/> (no file and no link, or link over 500),
        /// <see cref="ServiceErrorCodes.ResourceLinkInvalid"/>, <see cref="ServiceErrorCodes.ResourceFileTooLarge"/>,
        /// <see cref="ServiceErrorCodes.ResourceFileTypeNotAllowed"/>; NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>,
        /// Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>; Unavailable <see cref="ServiceErrorCodes.ResourceStorageUnavailable"/>.
        /// </returns>
        Task<ServiceResult<CommunityResourceResponseModel>> CreateAsync(RequestUser user, CommunityResourceRequestModel request, UploadedFile? file, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the title, description, type, link and publish state of a resource (the group does not change). Only the
        /// author may call it (for a group resource, while an active member). Publishing again keeps the first published date;
        /// <c>Publish = false</c> turns it back into a draft. For an uploaded file, the file, its type and its link are kept
        /// (the link and type in the request are ignored).
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="resourceId">Resource id.</param>
        /// <param name="request">New values; <see cref="CommunityResourceRequestModel.CommunityGroupId"/> is ignored.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated resource. Errors: NotFound <see cref="ServiceErrorCodes.ResourceNotFound"/> (also when the user cannot
        /// see it), Forbidden <see cref="ServiceErrorCodes.ResourceNotAuthor"/>, and the validation errors of <see cref="CreateAsync"/>.
        /// </returns>
        Task<ServiceResult<CommunityResourceResponseModel>> UpdateAsync(RequestUser user, ulong resourceId, CommunityResourceRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes a resource. The author, the group owners/moderators (active) or, for platform-wide resources, staff may
        /// call it.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="resourceId">Resource id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// True when deleted. Errors: NotFound <see cref="ServiceErrorCodes.ResourceNotFound"/> (also when the user cannot see
        /// it), Forbidden <see cref="ServiceErrorCodes.ResourceNotAuthor"/>.
        /// </returns>
        Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong resourceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads one resource. Group resources are visible to active members of the group; platform-wide ones to everyone.
        /// Drafts are visible only to the author and the group owners/moderators (staff for platform-wide resources). An
        /// uploaded file comes with a read-only signed link valid for one hour.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="resourceId">Resource id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The resource, or NotFound <see cref="ServiceErrorCodes.ResourceNotFound"/> (also when the user cannot see it).</returns>
        Task<ServiceResult<CommunityResourceResponseModel>> GetAsync(RequestUser user, ulong resourceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Resources newest first (by published date, or creation date for drafts), one page at a time. With a group: that group's
        /// resources, for its active members only. Without (optional filter): every resource the user can see, platform-wide
        /// ones plus those of the groups where the user is an active member. Same visibility rules as <see cref="GetAsync"/>:
        /// drafts are listed only to their author and to owners/moderators (staff for platform-wide).
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Optional group filter.</param>
        /// <param name="page">Zero-based page; negative values become 0.</param>
        /// <param name="pageSize">Page size, clamped to 1-100.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The page of resources with the total items and pages. Errors (only with a group): NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden
        /// <see cref="ServiceErrorCodes.NotActiveMember"/>.
        /// </returns>
        Task<ServiceResult<PagedResponseModel<CommunityResourceResponseModel>>> ListAsync(RequestUser user, ulong? groupId, int page, int pageSize, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>Group resources: only active members create and see them. Platform-wide resources (no group): any user
    /// creates them; everyone sees them once published.</item>
    /// <item>A resource with no published_date is a draft, visible only to its author and to the group owners and
    /// moderators (staff for platform-wide resources).</item>
    /// <item>Only the author edits a resource. The author, group owners/moderators (or staff for platform-wide
    /// resources) delete it.</item>
    /// <item>A resource is an external http/https link or an uploaded file (<see cref="CommunityResourceFiles"/>: office
    /// documents, pdf, txt, mp4, mp3; up to 100 MB; content checked against the extension). The type of an uploaded file
    /// comes from the file. The database keeps the blob name; every read returns a new read-only signed link (SAS),
    /// valid for one hour. Deleting is a soft delete and keeps the blob, like the rest of the soft-deleted data.</item>
    /// </list>
    /// </summary>
    public sealed class CommunityResourceService : ICommunityResourceService
    {
        private readonly GSWebDbContext _db;
        private readonly IUserDirectoryClient _users;
        private readonly TimeProvider _timeProvider;
        private readonly IBlobFileStorage _storage;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWebAPI database context.</param>
        /// <param name="users">Reads user full names from the Users API.</param>
        /// <param name="timeProvider">Clock for published_date (UTC, whole seconds).</param>
        /// <param name="storage">Blob Storage for uploaded files and their signed read links.</param>
        public CommunityResourceService(GSWebDbContext db, IUserDirectoryClient users, TimeProvider timeProvider, IBlobFileStorage storage)
        {
            _db = db;
            _users = users;
            _timeProvider = timeProvider;
            _storage = storage;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityResourceResponseModel>> CreateAsync(RequestUser user, CommunityResourceRequestModel request, UploadedFile? file, CancellationToken cancellationToken = default)
        {
            var errors = Validate(request, linkRequired: file == null);
            StoredFile? stored = null;
            if (file != null)
            {
                var (fileErrors, checkedFile) = await CheckFileAsync(file, cancellationToken);
                errors.AddRange(fileErrors);
                stored = checkedFile;
            }

            if (errors.Count > 0)
            {
                return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            if (request.CommunityGroupId is { } groupId)
            {
                if (!await _db.CommunityGroups.AnyAsync(g => g.Id == groupId, cancellationToken))
                {
                    return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
                }

                if (await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken) == null)
                {
                    return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember);
                }
            }

            var resource = new CommunityResource
            {
                CommunityGroupId = request.CommunityGroupId,
                AuthorId = user.UserId,
            };
            Apply(resource, request, _timeProvider.UtcNowSeconds());

            // Uploaded last, once every rule passed, so a rejected request leaves no blob behind.
            if (stored != null)
            {
                var blobName = $"community/resources/{Guid.NewGuid():N}{stored.Extension}";
                var url = await _storage.UploadAsync(blobName, stored.Content, stored.ContentType, cancellationToken);
                if (url == null)
                {
                    return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.ResourceStorageUnavailable);
                }

                resource.Link = url;
                resource.BlobName = blobName;
                resource.FileName = stored.FileName;
                resource.ContentType = stored.ContentType;
                resource.SizeBytes = (ulong)stored.Length;
                resource.ResourceType = stored.ResourceType;
            }

            _db.CommunityResources.Add(resource);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, resource.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityResourceResponseModel>> UpdateAsync(RequestUser user, ulong resourceId, CommunityResourceRequestModel request, CancellationToken cancellationToken = default)
        {
            var resource = await _db.CommunityResources.FirstOrDefaultAsync(r => r.Id == resourceId, cancellationToken);
            if (resource == null || !await CanSeeAsync(user, resource, cancellationToken))
            {
                return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.ResourceNotFound);
            }

            if (resource.AuthorId != user.UserId)
            {
                return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.ResourceNotAuthor);
            }

            var errors = Validate(request, linkRequired: resource.BlobName == null);
            if (errors.Count > 0)
            {
                return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            // The group of a resource does not change, and neither does an uploaded file (Apply keeps it).
            Apply(resource, request, _timeProvider.UtcNowSeconds());
            await _db.SaveChangesAsync(cancellationToken);
            return await GetAsync(user, resource.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong resourceId, CancellationToken cancellationToken = default)
        {
            var resource = await _db.CommunityResources.FirstOrDefaultAsync(r => r.Id == resourceId, cancellationToken);
            if (resource == null || !await CanSeeAsync(user, resource, cancellationToken))
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.ResourceNotFound);
            }

            if (resource.AuthorId != user.UserId && !await IsCuratorAsync(user, resource.CommunityGroupId, cancellationToken))
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.ResourceNotAuthor);
            }

            _db.CommunityResources.Remove(resource);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityResourceResponseModel>> GetAsync(RequestUser user, ulong resourceId, CancellationToken cancellationToken = default)
        {
            var resource = await _db.CommunityResources.AsNoTracking().FirstOrDefaultAsync(r => r.Id == resourceId, cancellationToken);
            if (resource == null || !await CanSeeAsync(user, resource, cancellationToken))
            {
                return ServiceResult<CommunityResourceResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.ResourceNotFound);
            }

            var names = await _users.GetFullNamesAsync(user, new[] { resource.AuthorId }, cancellationToken);
            return ServiceResult<CommunityResourceResponseModel>.Ok(Map(resource, names, _storage));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<PagedResponseModel<CommunityResourceResponseModel>>> ListAsync(RequestUser user, ulong? groupId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var viewer = await CommunityMembership.GetViewerAsync(_db, user, cancellationToken);
            var query = _db.CommunityResources.AsNoTracking();
            var userId = user.UserId;
            if (groupId is { } id)
            {
                if (!viewer.CanSee(id))
                {
                    return await _db.CommunityGroups.AnyAsync(g => g.Id == id, cancellationToken)
                        ? ServiceResult<PagedResponseModel<CommunityResourceResponseModel>>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember)
                        : ServiceResult<PagedResponseModel<CommunityResourceResponseModel>>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
                }

                var seesDrafts = viewer.IsModerator(id);
                query = query.Where(r => r.CommunityGroupId == id && (seesDrafts || r.PublishedDate != null || r.AuthorId == userId));
            }
            else
            {
                // No filter: platform-wide resources plus those of the user's groups, with the same draft rule per group.
                var myGroups = viewer.GroupIds;
                var moderated = viewer.ModeratedGroupIds;
                var isStaff = user.IsStaff;
                query = query.Where(r =>
                    (r.CommunityGroupId == null && (isStaff || r.PublishedDate != null || r.AuthorId == userId))
                    || (r.CommunityGroupId != null && myGroups.Contains(r.CommunityGroupId.Value)
                        && (moderated.Contains(r.CommunityGroupId.Value) || r.PublishedDate != null || r.AuthorId == userId)));
            }

            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 0);
            var total = await query.CountAsync(cancellationToken);
            var resources = await query
                .OrderByDescending(r => r.PublishedDate ?? r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var names = await _users.GetFullNamesAsync(user, resources.Select(r => r.AuthorId), cancellationToken);
            return ServiceResult<PagedResponseModel<CommunityResourceResponseModel>>.Ok(
                PagedResponseModel<CommunityResourceResponseModel>.Create(resources.Select(r => Map(r, names, _storage)).ToList(), page, pageSize, total));
        }

        private async Task<bool> CanSeeAsync(RequestUser user, CommunityResource resource, CancellationToken cancellationToken)
        {
            if (resource.CommunityGroupId is { } groupId)
            {
                var me = await CommunityMembership.GetActiveAsync(_db, groupId, user.UserId, cancellationToken);
                return me != null && (resource.PublishedDate != null || resource.AuthorId == user.UserId || CommunityMembership.IsOwnerOrModerator(me));
            }

            return resource.PublishedDate != null || resource.AuthorId == user.UserId || user.IsStaff;
        }

        private async Task<bool> IsCuratorAsync(RequestUser user, ulong? groupId, CancellationToken cancellationToken)
        {
            return groupId.HasValue
                ? CommunityMembership.IsOwnerOrModerator(await CommunityMembership.GetActiveAsync(_db, groupId.Value, user.UserId, cancellationToken))
                : user.IsStaff;
        }

        /// <summary>Title always; the link only when the resource is (or stays) an external link.</summary>
        private static List<string> Validate(CommunityResourceRequestModel request, bool linkRequired)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 150)
            {
                errors.Add(ServiceErrorCodes.ResourceTitleRequired);
            }

            if (!linkRequired)
            {
                return errors;
            }

            var link = request.Link?.Trim();
            if (string.IsNullOrEmpty(link) || link.Length > 500)
            {
                errors.Add(ServiceErrorCodes.ResourceLinkRequired);
            }
            else if (!Uri.TryCreate(link, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
            {
                errors.Add(ServiceErrorCodes.ResourceLinkInvalid);
            }

            return errors;
        }

        /// <summary>A file that passed the checks, ready to upload.</summary>
        private sealed record StoredFile(Stream Content, string FileName, string Extension, string ContentType, CommunityResourceType ResourceType, long Length);

        /// <summary>
        /// Size, allowed extension and content check (<see cref="CommunityResourceFiles"/>). Reads the first bytes of the
        /// stream and rewinds it (a stream that cannot seek is copied to memory first; it is at most 100 MB).
        /// </summary>
        private static async Task<(List<string> Errors, StoredFile? File)> CheckFileAsync(UploadedFile file, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            if (file.Length <= 0 || file.Length > CommunityResourceFiles.MaxFileBytes)
            {
                errors.Add(ServiceErrorCodes.ResourceFileTooLarge);
            }

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (!CommunityResourceFiles.TryGetType(extension, out var contentType, out var resourceType))
            {
                errors.Add(ServiceErrorCodes.ResourceFileTypeNotAllowed);
            }

            if (errors.Count > 0)
            {
                return (errors, null);
            }

            var content = file.Content;
            if (!content.CanSeek)
            {
                var buffer = new MemoryStream();
                await content.CopyToAsync(buffer, cancellationToken);
                content = buffer;
            }

            content.Position = 0;
            var header = new byte[CommunityResourceFiles.HeaderLength];
            var read = await content.ReadAtLeastAsync(header, header.Length, throwOnEndOfStream: false, cancellationToken);
            content.Position = 0;
            if (!CommunityResourceFiles.MatchesContent(extension!, header.AsSpan(0, read)))
            {
                return (new List<string> { ServiceErrorCodes.ResourceFileTypeNotAllowed }, null);
            }

            var fileName = Path.GetFileName(file.FileName);
            if (fileName.Length > 255)
            {
                fileName = fileName[..255];
            }

            return (errors, new StoredFile(content, fileName, extension!, contentType, resourceType, file.Length));
        }

        private static void Apply(CommunityResource resource, CommunityResourceRequestModel request, DateTime utcNow)
        {
            resource.Title = request.Title.Trim();
            resource.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

            // An uploaded file keeps its link and type; only external links take them from the request.
            if (resource.BlobName == null && !string.IsNullOrWhiteSpace(request.Link))
            {
                resource.ResourceType = request.ResourceType;
                resource.Link = request.Link.Trim();
            }

            if (request.Publish)
            {
                resource.PublishedDate ??= utcNow;
            }
            else
            {
                resource.PublishedDate = null;
            }
        }

        /// <summary>Uploaded files get a new read-only signed link (1 hour); external links are returned as they are.</summary>
        private static CommunityResourceResponseModel Map(CommunityResource resource, IReadOnlyDictionary<ulong, string> names, IBlobFileStorage storage)
        {
            return new CommunityResourceResponseModel
            {
                Id = resource.Id,
                CommunityGroupId = resource.CommunityGroupId,
                AuthorId = resource.AuthorId,
                AuthorName = names.GetValueOrDefault(resource.AuthorId),
                Title = resource.Title,
                Description = resource.Description,
                ResourceType = resource.ResourceType,
                Link = resource.BlobName != null ? storage.GetReadUrl(resource.BlobName) : resource.Link,
                IsUploadedFile = resource.BlobName != null,
                FileName = resource.FileName,
                ContentType = resource.ContentType,
                SizeBytes = resource.SizeBytes,
                PublishedDate = resource.PublishedDate,
                CreatedAt = resource.CreatedAt,
            };
        }
    }
}
