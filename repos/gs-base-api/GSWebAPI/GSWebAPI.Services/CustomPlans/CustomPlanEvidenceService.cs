using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans
{
    /// <summary>Evidences and projects of a custom plan.</summary>
    public interface ICustomPlanEvidenceService
    {
        /// <summary>
        /// Adds an evidence to the plan. Only the plan's client; not on cancelled plans.
        /// <list type="bullet">
        /// <item>Title required (max 150), description optional (max 5,000).</item>
        /// <item>Either a file (uploaded to Blob Storage; the URL in the request is then ignored) or an http/https link (max 1,000).</item>
        /// <item>The lesson, when given, must belong to the same plan.</item>
        /// </list>
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">Type, title, description, external URL and optional plan lesson id.</param>
        /// <param name="file">The uploaded file, or null for an external link.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The created evidence (uploaded files with a fresh read link). Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>
        /// (missing or not the caller's plan), <see cref="ServiceErrorCodes.EvidencePlanClosed"/> (cancelled plan), the
        /// <c>custom_plan.evidence_*</c> validation codes (title, description, file size or type, missing source, invalid URL,
        /// lesson not in plan) and <see cref="ServiceErrorCodes.StorageUnavailable"/> when the upload fails.
        /// </returns>
        Task<ServiceResult<CustomPlanEvidenceResponseModel>> CreateAsync(RequestUser user, ulong planId, CustomPlanEvidenceRequestModel request, UploadedFile? file, CancellationToken cancellationToken = default);

        /// <summary>Lists the evidences of the plan, newest first. The plan's client or staff.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The evidences (uploaded files with a fresh 1-hour read link), or <see cref="ServiceErrorCodes.PlanNotFound"/>.</returns>
        Task<ServiceResult<List<CustomPlanEvidenceResponseModel>>> ListAsync(RequestUser user, ulong planId, CancellationToken cancellationToken = default);

        /// <summary>Deletes (soft delete) an evidence of the plan. Only the plan's client; the blob is kept.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="evidenceId">Evidence id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when deleted, or <see cref="ServiceErrorCodes.EvidenceNotFound"/> (missing, other plan, or not the caller's plan).</returns>
        Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong planId, ulong evidenceId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>The plan's client adds and deletes evidences; the client and staff read them.</item>
    /// <item>Each evidence is either an uploaded file (stored in Blob Storage) or an external http/https link.</item>
    /// <item>Files: up to <see cref="MaxFileBytes"/> bytes, only the document, image, audio and video types in
    /// <see cref="AllowedTypes"/>. The content type comes from the extension, not from the client.</item>
    /// <item>Optionally attached to a lesson of the same plan. Not allowed on cancelled plans.</item>
    /// <item>Delete is a soft delete; the blob is kept (same as the rest of the soft-deleted data).</item>
    /// </list>
    /// </summary>
    public sealed class CustomPlanEvidenceService : ICustomPlanEvidenceService
    {
        /// <summary>Maximum size of an uploaded file: 25 MB.</summary>
        public const long MaxFileBytes = 25L * 1024 * 1024;
        private const int TitleMaxLength = 150;
        private const int DescriptionMaxLength = 5_000;
        private const int UrlMaxLength = 1_000;

        /// <summary>Allowed extensions and the content type stored with the blob.</summary>
        public static readonly IReadOnlyDictionary<string, string> AllowedTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = "application/pdf",
            [".png"] = "image/png",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".gif"] = "image/gif",
            [".webp"] = "image/webp",
            [".heic"] = "image/heic",
            [".doc"] = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".xls"] = "application/vnd.ms-excel",
            [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            [".ppt"] = "application/vnd.ms-powerpoint",
            [".pptx"] = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            [".txt"] = "text/plain",
            [".mp3"] = "audio/mpeg",
            [".m4a"] = "audio/mp4",
            [".mp4"] = "video/mp4",
            [".mov"] = "video/quicktime",
        };

        private readonly GSWebDbContext _db;
        private readonly IBlobFileStorage _storage;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWeb database context.</param>
        /// <param name="storage">Blob storage for uploaded files and read links.</param>
        public CustomPlanEvidenceService(GSWebDbContext db, IBlobFileStorage storage)
        {
            _db = db;
            _storage = storage;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanEvidenceResponseModel>> CreateAsync(RequestUser user, ulong planId, CustomPlanEvidenceRequestModel request, UploadedFile? file, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<CustomPlanEvidenceResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            if (plan.Status == CustomPlanStatus.Cancelled)
            {
                return ServiceResult<CustomPlanEvidenceResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.EvidencePlanClosed);
            }

            var errors = new List<string>();
            var title = request.Title?.Trim();
            if (string.IsNullOrEmpty(title) || title.Length > TitleMaxLength)
            {
                errors.Add(ServiceErrorCodes.EvidenceTitleRequired);
            }

            var description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            if (description?.Length > DescriptionMaxLength)
            {
                errors.Add(ServiceErrorCodes.EvidenceDescriptionTooLong);
            }

            string? contentType = null;
            string? extension = null;
            var externalUrl = string.IsNullOrWhiteSpace(request.Url) ? null : request.Url.Trim();
            if (file != null)
            {
                extension = Path.GetExtension(file.FileName);
                if (file.Length <= 0 || file.Length > MaxFileBytes)
                {
                    errors.Add(ServiceErrorCodes.EvidenceFileTooLarge);
                }

                if (string.IsNullOrEmpty(extension) || !AllowedTypes.TryGetValue(extension, out contentType))
                {
                    errors.Add(ServiceErrorCodes.EvidenceFileTypeNotAllowed);
                }
            }
            else if (externalUrl == null)
            {
                errors.Add(ServiceErrorCodes.EvidenceSourceRequired);
            }
            else if (externalUrl.Length > UrlMaxLength
                || !Uri.TryCreate(externalUrl, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
            {
                errors.Add(ServiceErrorCodes.EvidenceUrlInvalid);
            }

            if (request.PlanLessonId is { } lessonId
                && !await _db.CustomPlanClientLessons.AnyAsync(l => l.Id == lessonId && l.CustomPlanClientId == planId, cancellationToken))
            {
                errors.Add(ServiceErrorCodes.EvidenceLessonNotInPlan);
            }

            if (errors.Count > 0)
            {
                return ServiceResult<CustomPlanEvidenceResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            var evidence = new CustomPlanEvidence
            {
                CustomPlanClientId = planId,
                CustomPlanClientLessonId = request.PlanLessonId,
                EvidenceType = request.EvidenceType,
                Title = title!,
                Description = description,
                UploadedByUserId = user.UserId,
            };

            if (file != null)
            {
                var blobName = $"custom-plans/{planId}/evidences/{Guid.NewGuid():N}{extension!.ToLowerInvariant()}";
                var url = await _storage.UploadAsync(blobName, file.Content, contentType!, cancellationToken);
                if (url == null)
                {
                    return ServiceResult<CustomPlanEvidenceResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.StorageUnavailable);
                }

                evidence.Url = url;
                evidence.BlobName = blobName;
                evidence.FileName = Path.GetFileName(file.FileName) is { Length: > 255 } longName ? longName[..255] : Path.GetFileName(file.FileName);
                evidence.ContentType = contentType;
                evidence.SizeBytes = (ulong)file.Length;
            }
            else
            {
                evidence.Url = externalUrl!;
            }

            _db.CustomPlanEvidences.Add(evidence);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanEvidenceResponseModel>.Ok(Map(evidence, _storage));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<CustomPlanEvidenceResponseModel>>> ListAsync(RequestUser user, ulong planId, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || (plan.ClientId != user.UserId && !user.IsStaff))
            {
                return ServiceResult<List<CustomPlanEvidenceResponseModel>>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            var evidences = await _db.CustomPlanEvidences.AsNoTracking()
                .Where(e => e.CustomPlanClientId == planId)
                .OrderByDescending(e => e.CreatedAt).ThenByDescending(e => e.Id)
                .ToListAsync(cancellationToken);
            return ServiceResult<List<CustomPlanEvidenceResponseModel>>.Ok(evidences.Select(e => Map(e, _storage)).ToList());
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong planId, ulong evidenceId, CancellationToken cancellationToken = default)
        {
            var evidence = await _db.CustomPlanEvidences
                .Include(e => e.CustomPlanClient)
                .FirstOrDefaultAsync(e => e.Id == evidenceId && e.CustomPlanClientId == planId, cancellationToken);
            if (evidence == null || evidence.CustomPlanClient.ClientId != user.UserId)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.EvidenceNotFound);
            }

            _db.CustomPlanEvidences.Remove(evidence);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <summary>Uploaded files get a fresh read-only link; external links are returned as they are.</summary>
        /// <param name="evidence">The evidence row.</param>
        /// <param name="storage">Storage used to sign the read link of uploaded files.</param>
        /// <returns>The response model.</returns>
        internal static CustomPlanEvidenceResponseModel Map(CustomPlanEvidence evidence, IBlobFileStorage storage)
        {
            return new CustomPlanEvidenceResponseModel
            {
                Id = evidence.Id,
                CustomPlanClientId = evidence.CustomPlanClientId,
                PlanLessonId = evidence.CustomPlanClientLessonId,
                EvidenceType = evidence.EvidenceType,
                Title = evidence.Title,
                Description = evidence.Description,
                Url = evidence.BlobName != null ? storage.GetReadUrl(evidence.BlobName) : evidence.Url,
                IsUploadedFile = evidence.BlobName != null,
                FileName = evidence.FileName,
                ContentType = evidence.ContentType,
                SizeBytes = evidence.SizeBytes,
                UploadedByUserId = evidence.UploadedByUserId,
                CreatedAt = evidence.CreatedAt,
            };
        }
    }
}
