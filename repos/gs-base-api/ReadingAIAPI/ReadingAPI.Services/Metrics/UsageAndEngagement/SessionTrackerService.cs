using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingAPI.ReadingAPI.Domain.DTOs.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement
{
    /// <summary>
    /// Service to manage user session lifecycles and metrics.
    /// </summary>
    public class SessionTrackerService
    {
        private const string EventTypeStart = "start";
        private const string EventTypeEnd = "end";
        private const string EventTypeHeartbeat = "heartbeat";
        private const string EventTypePause = "pause";
        private const string EventTypeResume = "resume";

        /// <summary>
        /// PR time (UTC-4), used when no time zone is configured.
        /// </summary>
        private const int DefaultUtcOffsetMinutes = -240;

        private static readonly HashSet<string> AllowedEventTypes = new()
        {
            EventTypeStart,
            EventTypeEnd,
            EventTypeHeartbeat,
            EventTypePause,
            EventTypeResume
        };

        private readonly ReadingAIDbContext _dbContext;
        private readonly ILogger<SessionTrackerService> _logger;

        public SessionTrackerService(ReadingAIDbContext dbContext, ILogger<SessionTrackerService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Inserts a new raw user session record and its initial start event
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="deviceType">The device type being used</param>
        /// <param name="isSchoolHours">Param to indicate if the app is being used in school hours</param>
        /// <returns>The ID of the created session.</returns>
        public async Task<long> InsertUserSessionRaw(string userId, string deviceType, int isSchoolHours)
        {
            // Transaction spans both inserts so a session is never persisted without its "start" event
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var session = new UserSessionRawEntity
                {
                    UserId = int.Parse(userId),
                    DeviceType = deviceType,
                    IsSchoolHours = isSchoolHours
                };

                _dbContext.UserSessionRaw.Add(session);
                await _dbContext.SaveChangesAsync();

                _dbContext.UserSessionEvents.Add(new UserSessionEventEntity
                {
                    SessionId = session.Id,
                    EventType = EventTypeStart
                });
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return session.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error inserting session for userId: {UserId}", userId);
                return 0L;
            }
        }

        /// <summary>
        /// Records a session event in the DB
        /// </summary>
        /// <param name="sessionId">The ID of the session to update.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="eventType">The event type (heartbeat, pause, resume, end).</param>
        /// <returns>The status</returns>
        public async Task<SessionStatus> RecordSessionEvent(long sessionId, string userId, string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventType) || !AllowedEventTypes.Contains(eventType))
            {
                _logger.LogWarning("Invalid event type received for sessionId {SessionId}: {EventType}", sessionId, eventType);
                return SessionStatus.InvalidEventType;
            }

            try
            {
                var session = await _dbContext.UserSessionRaw.FindAsync(sessionId);

                if (session == null)
                {
                    return SessionStatus.NotFound;
                }

                if (session.UserId != int.Parse(userId))
                {
                    return SessionStatus.Unauthorized;
                }

                // Heartbeat is only used as a keep-alive signal
                if (eventType == EventTypeHeartbeat)
                {
                    return SessionStatus.Active;
                }

                var lastEventType = await _dbContext.UserSessionEvents
                    .Where(e => e.SessionId == sessionId)
                    .OrderByDescending(e => e.Id)
                    .Select(e => e.EventType)
                    .FirstOrDefaultAsync();

                if (lastEventType == EventTypeEnd)
                {
                    return SessionStatus.Closed;
                }

                _dbContext.UserSessionEvents.Add(new UserSessionEventEntity
                {
                    SessionId = sessionId,
                    EventType = eventType
                });
                await _dbContext.SaveChangesAsync();

                return SessionStatus.Active;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording session event for sessionId: {SessionId}", sessionId);
                return SessionStatus.Error;
            }
        }

        /// <summary>
        /// Determines if the current time falls within weekday school hours.
        /// </summary>
        /// <returns>1 if within school hours, 0 otherwise.</returns>
        public int CalculateIsSchoolHours()
        {
            DateTime localTime = DateTime.UtcNow.AddMinutes(GetSchoolHoursUtcOffsetMinutes());

            DayOfWeek day = localTime.DayOfWeek;
            bool isWeekday = day != DayOfWeek.Saturday && day != DayOfWeek.Sunday;

            TimeSpan startTime = new TimeSpan(7, 0, 0);
            TimeSpan endTime = new TimeSpan(14, 30, 0);
            TimeSpan currentTime = localTime.TimeOfDay;

            bool isWithinSchoolHours = currentTime >= startTime && currentTime < endTime;

            return (isWeekday && isWithinSchoolHours) ? 1 : 0;
        }

        /// <summary>
        /// Minutes between the configured time zone and UTC. Falls back to PR time when the zone is missing
        /// </summary>
        private int GetSchoolHoursUtcOffsetMinutes()
        {
            var zoneId = AppConfiguration.Configuration["AppSettings:default_time_zone_for_exams"];

            if (string.IsNullOrWhiteSpace(zoneId))
            {
                _logger.LogWarning("No time zone configured, the school hours check falls back to Puerto Rico time.");
                return DefaultUtcOffsetMinutes;
            }

            try
            {
                var zone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
                return (int)zone.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Time zone {ZoneId} could not be resolved, the school hours check falls back to Puerto Rico time.", zoneId);
                return DefaultUtcOffsetMinutes;
            }
        }
    }
}
