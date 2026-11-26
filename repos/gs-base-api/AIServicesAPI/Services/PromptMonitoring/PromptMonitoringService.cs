using GSAIServicesAPI.Infrastructure.Interfaces;
using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.PromptMonitoring;
using GSAIServicesAPI.Models.PromptMonitoring.Request;
using GSAIServicesAPI.Models.PromptMonitoring.Response;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace GSAIServicesAPI.Services.PromptMonitoring
{
    /// <summary>
    /// Default <see cref="IPromptMonitoringService"/> built on the generic
    /// <see cref="IMongoRepository{T}"/>. Every query pins <c>userId</c> so it hits the shard key, and
    /// is paginated or grouped inside the database so large collections never load whole into memory.
    /// </summary>
    public class PromptMonitoringService : IPromptMonitoringService
    {
        private readonly IMongoRepository<PromptLog> _repository;
        private readonly IMongoRepository<AlertReview> _alertReviewRepository;
        private readonly IViewerStudentsProvider _viewerStudentsProvider;
        private readonly IPromptClassifier _classifier;
        private readonly ILogger<PromptMonitoringService> _logger;

        /// <summary>Role name that turns on classification. A blank config leaves it off.</summary>
        private readonly string? _studentRoleName;

        /// <summary>
        /// Role name whose viewer sees every student on the platform. That role counts alerts across the
        /// whole collection; every other role is scoped to its own students. A blank config leaves the
        /// platform-wide branch off, so everyone is treated as scoped.
        /// </summary>
        private readonly string? _administratorRoleName;

        /// <summary>Upper bound on page size, to keep a single query bounded regardless of client input.</summary>
        private const int MaxPageSize = 100;

        /// <summary>
        /// How long the audit write may take. Bounded so a stalled database cannot hold open a request
        /// whose answer was already delivered.
        /// </summary>
        private static readonly TimeSpan CaptureTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Upper bound on how many users one summary request may ask for. Each user adds an id to the
        /// query, so this keeps a single request from fanning out without limit.
        /// </summary>
        private const int MaxUsersPerSummaryRequest = 200;

        /// <summary>The AI writes camelCase, so match properties without regard to casing.</summary>
        private static readonly JsonSerializerOptions ClassificationJsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public PromptMonitoringService(
            IMongoRepository<PromptLog> repository,
            IMongoRepository<AlertReview> alertReviewRepository,
            IViewerStudentsProvider viewerStudentsProvider,
            IPromptClassifier classifier,
            IConfiguration config,
            ILogger<PromptMonitoringService> logger)
        {
            _repository = repository;
            _alertReviewRepository = alertReviewRepository;
            _viewerStudentsProvider = viewerStudentsProvider;
            _classifier = classifier;
            _studentRoleName = config["AppSettings:student_role_id"];
            _administratorRoleName = config["AppSettings:admin_role_id"];
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task CaptureGenerateExchangeAsync(
            string userId,
            string? role,
            GenerateRequest request,
            bool isTutorFlow,
            string answer,
            string? ipAddress,
            string? classificationJson)
        {
            try
            {
                var prompt = ResolvePromptText(request, isTutorFlow);

                // Nothing was asked, so there is nothing to audit.
                if (string.IsNullOrWhiteSpace(prompt))
                {
                    return;
                }

                // A student's prompt must be classified. The inline path (which rides the answer
                // completion) can come back empty when the model refuses and skips the tool, which is
                // exactly the risky case. So when it is empty, fall back to a dedicated, forced call that
                // compels a verdict. Best-effort: the classifier bounds itself and never throws.
                var isStudent = !string.IsNullOrWhiteSpace(_studentRoleName)
                    && string.Equals(role, _studentRoleName, StringComparison.OrdinalIgnoreCase);
                if (isStudent && string.IsNullOrWhiteSpace(classificationJson))
                {
                    classificationJson = await _classifier.ClassifyAsync(prompt);
                }

                // A token of its own, not the request one: a user closing the tab must not cancel the
                // audit write. Bounded so a stalled database cannot hold the request open either.
                using var captureTimeout = new CancellationTokenSource(CaptureTimeout);

                await CaptureAsync(
                    new PromptMonitoringCaptureRequestModel
                    {
                        UserId = userId,
                        Role = role,
                        Prompt = prompt,
                        Response = answer,
                        Subject = request.LessonContext?.Subject,
                        IpAddress = ipAddress,
                        ClassificationJson = classificationJson
                    },
                    captureTimeout.Token);
            }
            catch (Exception ex)
            {
                // Auditing must not break an answer the user already received: log and swallow.
                _logger.LogError(ex, "Could not store the prompt log for user {UserId}.", userId);
            }
        }

        /// <inheritdoc />
        public async Task CaptureAsync(
            PromptMonitoringCaptureRequestModel capture,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(capture);
            ArgumentException.ThrowIfNullOrWhiteSpace(capture.UserId);

            var promptLog = new PromptLog
            {
                UserId = capture.UserId,
                Role = capture.Role,
                Prompt = capture.Prompt,
                Response = capture.Response,
                Subject = capture.Subject,
                IpAddress = capture.IpAddress
            };

            ApplyClassification(promptLog, capture.ClassificationJson);

            await _repository.InsertAsync(promptLog, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PromptMonitoringUserHistoryResponseModel> GetHistoryAsync(
            string viewerId,
            string userId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            // userId is the shard key, so a blank one cannot reach a partition. That is a caller bug,
            // not a request to match everything — fail loudly instead of scanning the collection.
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);
            ValidateDateRange(from, to);

            var filter = ApplyDateRange(BuildShardKeyFilter(userId), from, to);

            // Normalize paging into locals rather than reassigning the parameters, so the response can
            // report what was actually applied and one request cannot ask for the whole history.
            var appliedPage = Math.Max(page, 1);
            var appliedPageSize = Math.Clamp(pageSize, 1, MaxPageSize);
            var documentsToSkip = (appliedPage - 1) * appliedPageSize;

            var newestFirst = Builders<PromptLog>.Sort.Descending(log => log.CreatedAtUtc);

            // How far this viewer had reviewed this student before opening the history, read first so
            // isNew reflects the state as it was on arrival — not after the visit marks it reviewed.
            var priorReview = await ReadReviewWatermarkAsync(viewerId, userId, cancellationToken);

            // Total counts every match so the client can paginate; the second call brings back only
            // this page. Skip and limit both run inside MongoDB, so one page is what travels over the
            // wire — not the user's whole history.
            var total = await _repository.CountAsync(filter, cancellationToken);
            var promptLogs = await _repository.GetByFilterAsync(
                filter, appliedPageSize, documentsToSkip, newestFirst, cancellationToken);

            var response = new PromptMonitoringUserHistoryResponseModel
            {
                Prompts = promptLogs.Select(log => MapToPrompt(log, priorReview)).ToList(),
                Total = total,
                Page = appliedPage,
                PageSize = appliedPageSize
            };

            // Opening the history is the "read" event: move this viewer's watermark to now. Runs after
            // the page is built, and is best-effort — a failed mark must never fail the read.
            await MarkReviewedAsync(viewerId, userId, cancellationToken);

            return response;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<PromptMonitoringUserSummaryResponseModel>> GetUsersSummaryAsync(
            string viewerId,
            IReadOnlyCollection<string> userIds,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(userIds);
            ValidateDateRange(from, to);

            var requestedUserIds = NormalizeRequestedUserIds(userIds);

            if (requestedUserIds.Count == 0)
            {
                return [];
            }

            // userId is the shard key, so an $in over the requested ids routes straight to their
            // partitions instead of scanning the collection.
            var usersFilter = Builders<PromptLog>.Filter.In(log => log.UserId, requestedUserIds);

            // Collapse the matched prompts into one row per user. The counting runs inside MongoDB, so
            // the response size depends on how many users were asked for — not on how many prompts
            // they wrote.
            var pipeline = new EmptyPipelineDefinition<PromptLog>()
                .Match(ApplyDateRange(usersFilter, from, to))
                .Group(new BsonDocument
                {
                    // Group by userId, so each user yields exactly one row.
                    { "_id", "$userId" },
                    { "promptCount", new BsonDocument("$sum", 1) },
                    // isFlagged is a boolean, so turn it into 1/0 before summing.
                    { "flaggedCount", new BsonDocument("$sum",
                        new BsonDocument("$cond", new BsonArray { "$isFlagged", 1, 0 })) },
                    { "lastPromptAtUtc", new BsonDocument("$max", "$createdAtUtc") }
                });

            var summaryRows = await _repository.AggregateAsync(pipeline, cancellationToken);
            var summaries = summaryRows.Select(MapToUserSummary).ToList();

            // Layer on each viewer's "unread" count. Kept out of the aggregation above because the
            // watermark is per (viewer, student) and lives in another collection, so a cross-shard
            // join would be needed — this instead reads the viewer's marks (one partition) and counts
            // only the flagged prompts newer than them.
            var unreadByStudent = await CountUnreadAlertsByStudentAsync(
                viewerId, requestedUserIds, from, to, cancellationToken);

            if (unreadByStudent.Count > 0)
            {
                foreach (var summary in summaries)
                {
                    if (unreadByStudent.TryGetValue(summary.UserId, out var unread))
                    {
                        summary.UnreadAlertCount = unread;
                    }
                }
            }

            return summaries;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="viewerId"></param>
        /// <param name="viewerRole"></param>
        /// <param name="viewerToken"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CountUnreadAlertsForViewerAsync(
            string viewerId,
            string? viewerRole,
            string viewerToken,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken = default)
        {
            ValidateDateRange(from, to);

            // A platform-wide administrator sees every student, so their unread count spans every alert
            // in the collection — there is no student list to resolve.
            if (ViewerSeesEveryStudent(viewerRole))
            {
                return await CountPlatformWideUnreadAlertsAsync(viewerId, from, to, cancellationToken);
            }

            // Every other role is bounded to the students it is related to. Resolving those ids first
            // lets the count target only their partitions (userId is the shard key), so it stays cheap
            // instead of scanning the whole collection.
            var studentIds = await _viewerStudentsProvider.GetStudentIdsAsync(viewerToken, cancellationToken);

            if (studentIds.Count == 0)
            {
                return 0;
            }

            // Reuse the per-student unread count (flagged prompts newer than this viewer's watermark)
            // and add them into the single badge number.
            var unreadByStudent = await CountUnreadAlertsByStudentAsync(
                viewerId, RemoveBlankAndDuplicateIds(studentIds), from, to, cancellationToken);

            return unreadByStudent.Values.Sum();
        }

        /// <summary>
        /// True when <paramref name="role"/> is the platform-wide administrator, whose count spans every
        /// student rather than a related subset.
        /// </summary>
        private bool ViewerSeesEveryStudent(string? role) =>
            !string.IsNullOrWhiteSpace(_administratorRoleName)
            && string.Equals(role, _administratorRoleName, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Counts the alerts across every student that the administrator has not reviewed yet, as
        /// (all alerts) minus (alerts already read under this admin's watermarks). The admin has opened
        /// few students, so the "read" side stays a small query, and the whole-collection alert count is
        /// bounded by how many alerts exist, not by how many students there are. Cross-partition, but
        /// reserved for the administrator role — few such viewers.
        /// </summary>
        private async Task<long> CountPlatformWideUnreadAlertsAsync(
            string viewerId, DateTime? from, DateTime? to, CancellationToken cancellationToken)
        {
            var allAlertsFilter = ApplyDateRange(FlaggedPromptFilter(), from, to);
            var allAlerts = await _repository.CountAsync(allAlertsFilter, cancellationToken);

            // No viewer or no alerts: nothing can have been read, so every alert is unread.
            if (string.IsNullOrWhiteSpace(viewerId) || allAlerts == 0)
            {
                return allAlerts;
            }

            // This admin's own watermarks, in one read against their partition (viewerId is the review
            // shard key). Only students they have opened appear.
            var reviews = await _alertReviewRepository.GetByFilterAsync(
                Builders<AlertReview>.Filter.Eq(review => review.ViewerId, viewerId), cancellationToken);

            if (reviews.Count == 0)
            {
                return allAlerts;
            }

            // Alerts already read: for each reviewed student, those classified at or before the mark.
            var filterBuilder = Builders<PromptLog>.Filter;
            var readClauses = reviews.Select(review =>
                filterBuilder.Eq(log => log.UserId, review.StudentId)
                & filterBuilder.Lte(log => log.ClassifiedAtUtc, review.LastReviewedAtUtc));

            var readAlerts = await _repository.CountAsync(
                allAlertsFilter & filterBuilder.Or(readClauses), cancellationToken);

            return allAlerts - readAlerts;
        }

        /// <summary>The filter that selects alerts: prompts the classification marked as flagged.</summary>
        private static FilterDefinition<PromptLog> FlaggedPromptFilter() =>
            Builders<PromptLog>.Filter.Eq(log => log.IsFlagged, true);

        /// <summary>
        /// Drops blank ids (they cannot target a partition) and removes duplicates so a repeated id adds
        /// no redundant work. Shared cleanup for any query that takes a list of user ids.
        /// </summary>
        private static List<string> RemoveBlankAndDuplicateIds(IEnumerable<string> userIds) =>
            userIds
                .Where(userId => !string.IsNullOrWhiteSpace(userId))
                .Distinct()
                .ToList();

        /// <summary>
        /// Cleans a list of user ids and then caps how many one summary request may fan out to, so a
        /// single request cannot expand without limit.
        /// </summary>
        private static List<string> NormalizeRequestedUserIds(IReadOnlyCollection<string> userIds) =>
            RemoveBlankAndDuplicateIds(userIds)
                .Take(MaxUsersPerSummaryRequest)
                .ToList();

        /// <summary>
        /// Counts, per student, the flagged prompts <paramref name="viewerId"/> has not reviewed yet:
        /// those classified after this viewer last opened that student. Reads the viewer's watermarks
        /// (a single partition, since <c>viewerId</c> is the review collection's shard key), then counts
        /// only flagged prompts inside MongoDB. A student with no watermark has all their flagged
        /// prompts counted as unread. Returns an empty map when there is no viewer to attribute to.
        /// </summary>
        private async Task<IReadOnlyDictionary<string, long>> CountUnreadAlertsByStudentAsync(
            string viewerId,
            IReadOnlyCollection<string> studentIds,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken)
        {
            // No viewer means nothing to attribute a "read" state to, so no count can be made.
            if (string.IsNullOrWhiteSpace(viewerId) || studentIds.Count == 0)
            {
                return new Dictionary<string, long>();
            }

            // All of this viewer's marks for the roster, in one read against their partition.
            var reviewFilter =
                Builders<AlertReview>.Filter.Eq(review => review.ViewerId, viewerId)
                & Builders<AlertReview>.Filter.In(review => review.StudentId, studentIds);
            var reviews = await _alertReviewRepository.GetByFilterAsync(reviewFilter, cancellationToken);
            var watermarkByStudent = reviews.ToDictionary(
                review => review.StudentId, review => review.LastReviewedAtUtc);

            // One clause per student: newer-than-watermark when they have one, all flagged when they do
            // not. Grouped under a single isFlagged match so only alerts are ever scanned.
            var filterBuilder = Builders<PromptLog>.Filter;
            var studentClauses = studentIds.Select(studentId =>
            {
                var clause = filterBuilder.Eq(log => log.UserId, studentId);
                return watermarkByStudent.TryGetValue(studentId, out var watermark)
                    ? clause & filterBuilder.Gt(log => log.ClassifiedAtUtc, watermark)
                    : clause;
            });

            var unreadFilter = ApplyDateRange(filterBuilder.Eq(log => log.IsFlagged, true), from, to)
                & filterBuilder.Or(studentClauses);

            var pipeline = new EmptyPipelineDefinition<PromptLog>()
                .Match(unreadFilter)
                .Group(new BsonDocument
                {
                    { "_id", "$userId" },
                    { "unreadCount", new BsonDocument("$sum", 1) }
                });

            var unreadRows = await _repository.AggregateAsync(pipeline, cancellationToken);

            return unreadRows.ToDictionary(
                row => row["_id"].AsString, row => row["unreadCount"].ToInt64());
        }

        /// <summary>
        /// Reads how far <paramref name="viewerId"/> had reviewed <paramref name="studentId"/>, or null
        /// when they never have (so every flagged prompt reads as new). Returns null for a blank viewer.
        /// </summary>
        private async Task<DateTime?> ReadReviewWatermarkAsync(
            string viewerId, string studentId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(viewerId))
            {
                return null;
            }

            var filter =
                Builders<AlertReview>.Filter.Eq(review => review.ViewerId, viewerId)
                & Builders<AlertReview>.Filter.Eq(review => review.StudentId, studentId);
            var review = (await _alertReviewRepository.GetByFilterAsync(filter, cancellationToken))
                .FirstOrDefault();

            return review?.LastReviewedAtUtc;
        }

        /// <summary>
        /// Moves <paramref name="viewerId"/>'s watermark on <paramref name="studentId"/> to now, so the
        /// alerts they just saw stop counting as unread. Best-effort: a failure is logged and swallowed,
        /// because it must never turn a successful history read into an error.
        /// </summary>
        private async Task MarkReviewedAsync(
            string viewerId, string studentId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(viewerId))
            {
                return;
            }

            try
            {
                // viewerId and studentId are the natural key: they live in the filter, so on insert
                // MongoDB seeds them into the new document from these equalities. Only the watermark is
                // updated on every visit; the schema version is stamped once, on insert.
                var filter =
                    Builders<AlertReview>.Filter.Eq(review => review.ViewerId, viewerId)
                    & Builders<AlertReview>.Filter.Eq(review => review.StudentId, studentId);

                var update = Builders<AlertReview>.Update
                    .Set(review => review.LastReviewedAtUtc, DateTime.UtcNow)
                    .SetOnInsert(review => review.SchemaVersion, AlertReview.CurrentSchemaVersion);

                await _alertReviewRepository.UpsertAsync(filter, update, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Could not mark alerts reviewed for viewer {ViewerId}, student {StudentId}.",
                    viewerId,
                    studentId);
            }
        }

        /// <summary>
        /// Copies the AI verdict onto the log. A missing or malformed payload leaves
        /// <c>ClassifiedAtUtc</c> null, so the record reads as never reviewed rather than as clean.
        /// </summary>
        private static void ApplyClassification(PromptLog promptLog, string? classificationJson)
        {
            if (string.IsNullOrWhiteSpace(classificationJson))
            {
                return;
            }

            PromptMonitoringClassificationModel? classification;

            try
            {
                classification = JsonSerializer.Deserialize<PromptMonitoringClassificationModel>(
                    classificationJson, ClassificationJsonOptions);
            }
            catch (JsonException)
            {
                // The AI returned something unparseable. Keep the prompt, drop the verdict.
                return;
            }

            if (classification is null)
            {
                return;
            }

            // The model can be internally inconsistent: list a category but forget to set the flag, or
            // set the flag without naming a category. Reconcile the two so a stored alert is never
            // half-formed — a category implies flagged, and a flagged verdict always carries a category.
            var categories = classification.AlertCategories ?? [];
            var isFlagged = classification.IsFlagged || categories.Count > 0;
            if (isFlagged && categories.Count == 0)
            {
                categories.Add("otro");
            }

            promptLog.IsFlagged = isFlagged;
            promptLog.AlertCategories = categories;
            promptLog.AlertReason = classification.AlertReason;
            promptLog.ClassifiedAtUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Picks the text the user actually asked, which lives in a different place per flow: the
        /// tutor sends a conversation whose last user message is the new prompt, while the teacher
        /// assistant sends a single one.
        /// </summary>
        private static string? ResolvePromptText(GenerateRequest request, bool isTutorFlow)
        {
            if (!isTutorFlow)
            {
                return request.User;
            }

            return request.Messages?
                .LastOrDefault(message =>
                    !string.IsNullOrWhiteSpace(message.Content)
                    && !string.Equals(message.Role, "assistant", StringComparison.OrdinalIgnoreCase))?
                .Content;
        }

        /// <summary>
        /// Rejects a range that can never match, so the caller learns it sent something wrong instead
        /// of reading an empty list as "this user wrote nothing".
        /// </summary>
        private static void ValidateDateRange(DateTime? from, DateTime? to)
        {
            if (from.HasValue && to.HasValue && from.Value > to.Value)
            {
                throw new ArgumentException($"'{nameof(from)}' cannot be later than '{nameof(to)}'.", nameof(from));
            }
        }

        /// <summary>
        /// Pins the shard key. Fixing <c>userId</c> is what lets MongoDB go straight to that user's
        /// partition instead of scanning every one.
        /// </summary>
        private static FilterDefinition<PromptLog> BuildShardKeyFilter(string userId)
        {
            return Builders<PromptLog>.Filter.Eq(log => log.UserId, userId);
        }

        /// <summary>
        /// Narrows a filter to the requested window. Both bounds are optional and inclusive; the
        /// academic period is resolved by the client and arrives already as dates.
        /// </summary>
        private static FilterDefinition<PromptLog> ApplyDateRange(
            FilterDefinition<PromptLog> filter, DateTime? from, DateTime? to)
        {
            var filterBuilder = Builders<PromptLog>.Filter;

            if (from.HasValue)
            {
                filter &= filterBuilder.Gte(log => log.CreatedAtUtc, from.Value.ToUniversalTime());
            }

            if (to.HasValue)
            {
                filter &= filterBuilder.Lte(log => log.CreatedAtUtc, to.Value.ToUniversalTime());
            }

            return filter;
        }

        /// <summary>
        /// Turns one stored document into the client-facing model, leaving the storage-only fields
        /// (schema version, unmapped extra elements) inside the API. <paramref name="priorReview"/> is
        /// the viewer's watermark on this student before this visit: a flagged prompt classified after
        /// it reads as new. A null watermark means the viewer never reviewed this student, so every
        /// flagged prompt is new.
        /// </summary>
        private static PromptMonitoringPromptResponseModel MapToPrompt(PromptLog log, DateTime? priorReview) =>
            new()
            {
                Id = log.Id,
                Prompt = log.Prompt,
                Response = log.Response,
                Role = log.Role,
                IsFlagged = log.IsFlagged,
                AlertCategories = log.AlertCategories,
                AlertReason = log.AlertReason,
                IsNew = log.IsFlagged
                    && log.ClassifiedAtUtc.HasValue
                    && (priorReview is null || log.ClassifiedAtUtc.Value > priorReview.Value),
                CreatedAtUtc = log.CreatedAtUtc
            };

        /// <summary>
        /// Turns one raw aggregation row into the response model. The grouped <c>userId</c> lives under
        /// <c>_id</c> because that is where MongoDB's group stage puts it.
        /// </summary>
        private static PromptMonitoringUserSummaryResponseModel MapToUserSummary(BsonDocument row) =>
            new()
            {
                UserId = row["_id"].AsString,
                PromptCount = row["promptCount"].ToInt64(),
                FlaggedCount = row["flaggedCount"].ToInt64(),
                LastPromptAtUtc = row["lastPromptAtUtc"].ToUniversalTime()
            };
    }
}
