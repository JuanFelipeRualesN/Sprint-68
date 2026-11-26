using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeachersAPI.Domain.Models.Events;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Infrastructure.Repositories.Events;
using TeachersAPI.Utils;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Sole orchestrator of the Microsoft Teams calendar sync for Events: resolves audience
    /// (EventAudienceResolver), calls Microsoft Graph (GraphEventsAPI), and persists
    /// TeamsEventId/TeamsSyncStatus. EventModifierManager delegates here after persisting the
    /// event; this class never touches event persistence beyond those two sync columns, and
    /// never throws — failures are logged and reflected in TeamsSyncStatus without rolling back
    /// the already-persisted event.
    /// </summary>
    public class EventTeamsManager
    {
        private readonly string _connectionString;

        // Self-contained: builds its own logger, so no DI/constructor changes are needed anywhere
        // else in the project (Controllers, other Managers) just for this ticket.
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("EventTeamsManager");

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        public EventTeamsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Resolves the audience and creates the event in Microsoft Graph.
        /// </summary>
        public async Task SyncEventCreation(EventModel eventModel, List<EventRoleModel> roles, List<EventSchoolModel> schools, List<EventRegionModel> regions, List<EventUserModel> users, EventOrganizerModel organizer)
        {
            List<UserResponse> attendees = await EventAudienceResolver.ResolveAudience(roles, schools, regions, users, organizer.Token);
            await SyncCreateOrUpdate(eventModel, attendees, organizer);
        }

        /// <summary>
        /// Resolves the audience and updates (or creates, if never synced before) the event in
        /// Microsoft Graph.
        /// </summary>
        public async Task SyncEventUpdate(EventModel eventModel, List<EventRoleModel> roles, List<EventSchoolModel> schools, List<EventRegionModel> regions, List<EventUserModel> users, EventOrganizerModel organizer)
        {
            List<UserResponse> attendees = await EventAudienceResolver.ResolveAudience(roles, schools, regions, users, organizer.Token);
            await SyncCreateOrUpdate(eventModel, attendees, organizer);
        }

        /// <summary>
        /// Deletes the event from Microsoft Graph, if it was ever synced. Never cancels the
        /// (already-applied) soft delete.
        /// </summary>
        public async Task SyncEventDeletion(EventModel eventModel, EventOrganizerModel organizer)
        {
            if (string.IsNullOrEmpty(eventModel.TeamsEventId))
            {
                return;
            }

            try
            {
                await ExecuteWithRetryAsync(() => GraphEventsAPI.DeleteEventAsync(eventModel.TeamsEventId, organizer.Email));
                LogSync(eventModel.Id, EventSyncLogActions.Delete, EventSyncLogStatuses.Success, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete event {EventId}'s Graph event {TeamsEventId}.", eventModel.Id, eventModel.TeamsEventId);
                LogSync(eventModel.Id, EventSyncLogActions.Delete, EventSyncLogStatuses.Error, ex.Message);
            }
        }

        /// <summary>
        /// Shared create-or-update path once the audience is already resolved: skips Graph
        /// entirely when there is no one to sync to, otherwise creates (no TeamsEventId yet) or
        /// updates (TeamsEventId present) and persists the resulting status.
        /// </summary>
        private async Task SyncCreateOrUpdate(EventModel eventModel, List<UserResponse> attendees, EventOrganizerModel organizer)
        {
            if (attendees.Count == 0)
            {
                _logger.LogInformation("Event {EventId} has no resolvable audience yet; skipping Graph sync.", eventModel.Id);
                new EventDAO().Update(_connectionString, new EventModel { TeamsSyncStatus = EventTeamsSyncStatuses.Pending }, new EventModel { Id = eventModel.Id });
                return;
            }

            string action = string.IsNullOrEmpty(eventModel.TeamsEventId) ? EventSyncLogActions.Create : EventSyncLogActions.Update;

            if (string.IsNullOrEmpty(eventModel.TeamsEventId))
            {
                try
                {
                    string teamsEventId = await ExecuteWithRetryAsync(() => GraphEventsAPI.CreateEventAsync(eventModel, attendees, organizer));

                    if (string.IsNullOrEmpty(teamsEventId))
                    {
                        _logger.LogWarning("Event {EventId} could not be synced: organizer email is not valid for the configured tenant.", eventModel.Id);
                        new EventDAO().Update(_connectionString, new EventModel { TeamsSyncStatus = EventTeamsSyncStatuses.Error }, new EventModel { Id = eventModel.Id });
                        LogSync(eventModel.Id, action, EventSyncLogStatuses.Error, "Organizer email is not valid for the configured tenant.");
                        return;
                    }

                    new EventDAO().Update(_connectionString, new EventModel { TeamsEventId = teamsEventId, TeamsSyncStatus = EventTeamsSyncStatuses.Synced, TeamsSyncedAt = Now() }, new EventModel { Id = eventModel.Id });
                    LogSync(eventModel.Id, action, EventSyncLogStatuses.Success, null);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create event {EventId} in Microsoft Graph.", eventModel.Id);
                    new EventDAO().Update(_connectionString, new EventModel { TeamsSyncStatus = EventTeamsSyncStatuses.Error }, new EventModel { Id = eventModel.Id });
                    LogSync(eventModel.Id, action, EventSyncLogStatuses.Error, ex.Message);
                }

                return;
            }

            try
            {
                bool updated = await ExecuteWithRetryAsync(() => GraphEventsAPI.UpdateEventAsync(eventModel, attendees, organizer));

                if (!updated)
                {
                    _logger.LogWarning("Event {EventId}'s Graph event {TeamsEventId} no longer exists (404); clearing stored id.", eventModel.Id, eventModel.TeamsEventId);
                    new EventDAO().ClearTeamsEventId(_connectionString, eventModel.Id, EventTeamsSyncStatuses.Error);
                    LogSync(eventModel.Id, action, EventSyncLogStatuses.Error, "Graph event no longer exists (404).");
                    return;
                }

                new EventDAO().Update(_connectionString, new EventModel { TeamsSyncStatus = EventTeamsSyncStatuses.Synced, TeamsSyncedAt = Now() }, new EventModel { Id = eventModel.Id });
                LogSync(eventModel.Id, action, EventSyncLogStatuses.Success, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update event {EventId} in Microsoft Graph.", eventModel.Id);
                new EventDAO().Update(_connectionString, new EventModel { TeamsSyncStatus = EventTeamsSyncStatuses.Error }, new EventModel { Id = eventModel.Id });
                LogSync(eventModel.Id, action, EventSyncLogStatuses.Error, ex.Message);
            }
        }

        /// <summary>
        /// Retries a Graph call up to 3 attempts total with exponential backoff (1s, 2s) between
        /// attempts. Each GraphEventsAPI call already re-acquires its own access token, so a retry
        /// here also covers auth failures caused by a stale/expired token.
        /// </summary>
        private static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
        {
            const int maxAttempts = 3;
            Exception lastException = null;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    if (attempt < maxAttempts)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)));
                    }
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Non-generic overload for Graph calls that return a plain Task (e.g. DeleteEventAsync).
        /// </summary>
        private static async Task ExecuteWithRetryAsync(Func<Task> action)
        {
            const int maxAttempts = 3;
            Exception lastException = null;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    if (attempt < maxAttempts)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)));
                    }
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Records the outcome of a single sync operation (create/update/delete) in
        /// event_sync_logs. Never throws — a logging failure must never affect the sync itself.
        /// </summary>
        private void LogSync(string eventId, string action, string status, string errorMessage)
        {
            try
            {
                new EventDAO().InsertSyncLog(_connectionString, new EventSyncLogModel
                {
                    EventId = eventId,
                    Action = action,
                    Status = status,
                    ErrorMessage = errorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record sync log for event {EventId}.", eventId);
            }
        }

        private static string Now()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
