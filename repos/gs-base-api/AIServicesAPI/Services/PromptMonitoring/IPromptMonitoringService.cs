using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.PromptMonitoring.Request;
using GSAIServicesAPI.Models.PromptMonitoring.Response;

namespace GSAIServicesAPI.Services.PromptMonitoring
{
    /// <summary>
    /// Reads the stored prompt history. Reusable across the teacher view (a student's history) and
    /// the admin view (any user's history) — both call the same method, only the identifiers differ.
    /// </summary>
    public interface IPromptMonitoringService
    {
        /// <summary>
        /// Stores one prompt as an audit record. Shared by the student tutor and the teacher assistant.
        /// </summary>
        /// <remarks>
        /// Written whether or not it could be classified — an unclassified prompt is recoverable, one
        /// that was never stored is not. Without a classification, <c>classifiedAtUtc</c> stays null,
        /// which is what separates "reviewed and clean" from "never reviewed".
        /// </remarks>
        Task CaptureAsync(
            PromptMonitoringCaptureRequestModel capture,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Captures one <c>api/generate</c> exchange as an audit record, resolving the prompt text from
        /// whichever flow produced it (tutor conversation or teacher single prompt) and mapping the
        /// session and request onto the stored log.
        /// </summary>
        /// <remarks>
        /// Never throws: a failure is logged and swallowed, so auditing cannot break an answer the user
        /// already received. Owns a bounded timeout, so neither a closed tab nor a stalled database can
        /// hold the request open. Intended to be awaited after the response has been fully streamed.
        /// </remarks>
        /// <param name="userId">Who sent the prompt, from the validated session.</param>
        /// <param name="role">Role from the session; decides classification (student only).</param>
        /// <param name="request">The generation payload the prompt text and subject are read from.</param>
        /// <param name="isTutorFlow">Whether the request came through the tutor flow, which changes where the prompt text lives.</param>
        /// <param name="answer">The AI answer, accumulated as it was streamed to the client.</param>
        /// <param name="ipAddress">Origin IP, resolved by the caller from the HTTP context.</param>
        /// <param name="classificationJson">
        /// The raw arguments of the classification tool call, when the model produced one (students
        /// only), or null. Malformed or empty leaves the record unclassified rather than failing.
        /// </param>
        Task CaptureGenerateExchangeAsync(
            string userId,
            string? role,
            GenerateRequest request,
            bool isTutorFlow,
            string answer,
            string? ipAddress,
            string? classificationJson);

        /// <summary>
        /// Returns one page of the prompts sent by <paramref name="userId"/>, newest first. The date
        /// range is optional and is expected to come from the client (the academic-period filter is
        /// applied on the frontend): when <paramref name="from"/> and/or <paramref name="to"/> are
        /// provided the results are limited to that window; when both are null no date filter is applied.
        /// </summary>
        /// <param name="viewerId">
        /// Who is reading the history (teacher/director/admin from the session). Opening the history
        /// marks this student's alerts reviewed for this viewer, and decides which prompts come back as
        /// <c>isNew</c>. Best-effort: a failed mark never blocks the read.
        /// </param>
        /// <param name="userId">The user whose history is being read — the shard key.</param>
        /// <param name="from">Inclusive lower bound (UTC) on <c>createdAtUtc</c>, or null.</param>
        /// <param name="to">Inclusive upper bound (UTC) on <c>createdAtUtc</c>, or null.</param>
        /// <param name="page">1-based page number.</param>
        /// <param name="pageSize">Page size; clamped to a safe maximum to protect the database.</param>
        Task<PromptMonitoringUserHistoryResponseModel> GetHistoryAsync(
            string viewerId,
            string userId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Summarizes the prompt activity of several users at once — how many prompts each one sent,
        /// how many were flagged, and when they last wrote. Used by the teacher's student list to show
        /// the alert indicator without loading any prompt text.
        /// </summary>
        /// <param name="viewerId">
        /// Who is asking (teacher/director/admin from the session). Their per-student review watermarks
        /// decide each row's <c>unreadAlertCount</c> — the flagged prompts classified since this viewer
        /// last opened that student. "Read" is per viewer, so two adults sharing a student keep separate
        /// counts.
        /// </param>
        /// <param name="userIds">
        /// The users to summarize, by id. userId is the shard key, so the query reaches only those
        /// users' partitions instead of scanning every one. Users with no prompts in range are simply
        /// absent from the result.
        /// </param>
        /// <param name="from">Inclusive lower bound (UTC) on <c>createdAtUtc</c>, or null for no bound.</param>
        /// <param name="to">Inclusive upper bound (UTC) on <c>createdAtUtc</c>, or null for no bound.</param>
        Task<IReadOnlyList<PromptMonitoringUserSummaryResponseModel>> GetUsersSummaryAsync(
            string viewerId,
            IReadOnlyCollection<string> userIds,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the alerts a viewer has not reviewed yet, in one number for a global "unread" badge —
        /// the alerts classified after this viewer last opened each student. The scope follows the role:
        /// a platform-wide administrator counts unread alerts across every student; any other role counts
        /// only those of the students it is related to, whose ids are resolved from the users API. No
        /// paging and no student list from the caller — the backend derives the scope from role and token.
        /// </summary>
        /// <param name="viewerId">Who is asking (from the session). Unread is measured against this viewer's own review watermarks.</param>
        /// <param name="viewerRole">The viewer's role name (already resolved from the session), which selects the scope.</param>
        /// <param name="viewerToken">The viewer's session token, used to resolve their students when the role is bounded.</param>
        /// <param name="from">Inclusive lower bound (UTC) on <c>createdAtUtc</c>, or null for no bound.</param>
        /// <param name="to">Inclusive upper bound (UTC) on <c>createdAtUtc</c>, or null for no bound.</param>
        Task<long> CountUnreadAlertsForViewerAsync(
            string viewerId,
            string? viewerRole,
            string viewerToken,
            DateTime? from,
            DateTime? to,
            CancellationToken cancellationToken = default);
    }
}
