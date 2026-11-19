using Dapper;
using MySqlConnector;
using System;
using System.Linq;
using System.Collections.Generic;
using TeachersAPI.Domain.DTOs.Events;
using TeachersAPI.Domain.Models.Events.Admin;
using TeachersAPI.Domain.Models.Generals.Responses.Admin;
using EventAudienceRoles = TeachersAPI.Domain.Models.Events.EventAudienceRoles;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle events related to events using Dapper.
    /// </summary>
    public static class EventsDapperHandler
    {
        private const string EventDateFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Reads lesson, lesson-quiz, plan-quiz (homework) and calendar events (role / school /
        /// region / specific-user scoped, combined with AND per axis - see the axis-clause
        /// comment below) for a teacher within a given month/year and returns them as event
        /// responses.
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <param name="userId">The teacher's user id.</param>
        /// <param name="roleId">The teacher's role id, used to resolve role-scoped events.</param>
        /// <param name="schoolIds">The school ids the teacher belongs to. Optional.</param>
        /// <param name="regionIds">The region ids the teacher belongs to. Optional.</param>
        /// <returns>Returns the event response models for the given month and year.</returns>
        public static List<EventResponseModel> GetEventsByMonthAndYear(
            int month,
            int year,
            int userId,
            string roleId,
            List<int> schoolIds = null,
            List<string> regionIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            // Half-open range so the due_date / start_date indexes stay usable.
            var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var to = from.AddMonths(1);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@userId", userId);
            dynamicParameters.Add("@from", from);
            dynamicParameters.Add("@to", to);

            // Three independent branches off teacher_plans, kept separate on purpose
            // so the unrelated LEFT joins don't cross-multiply into duplicate rows.
            string sql = @"
/* --- lessons (plan -> lesson) --- */
SELECT
    pl.id          AS plan_id,
    pl.name        AS plan_name,
    pll.lesson_id  AS lesson_id,
    pll.due_date   AS due_date
FROM gs_teachers_web_db_prod.teacher_plans pl
INNER JOIN gs_teachers_web_db_prod.teacher_plan_lessons pll
    ON pll.plan_id = pl.id AND pll.available = 1
WHERE pl.available = 1
  AND pl.opened = 1
  AND pl.user_id = @userId
  AND pll.due_date >= @from
  AND pll.due_date <  @to;

/* --- lesson quizzes (plan -> lesson -> lesson_quiz) --- */
SELECT
    pl.id                   AS plan_id,
    pl.name                 AS plan_name,
    pll.lesson_id           AS lesson_id,
    pllq.quiz_id            AS quiz_id,
    null                    AS title,
    pllq.due_date           AS due_date,
    pllq.due_date_timezone  AS due_date_timezone,
    pllq.duration           AS duration
FROM gs_teachers_web_db_prod.teacher_plans pl
INNER JOIN gs_teachers_web_db_prod.teacher_plan_lessons pll
    ON pll.plan_id = pl.id AND pll.available = 1
INNER JOIN gs_teachers_web_db_prod.teacher_plan_lesson_quizs pllq
    ON pllq.plan_lesson_id = pll.id
WHERE pl.available = 1
  AND pl.opened = 1
  AND pl.user_id = @userId
  AND pllq.due_date >= @from
  AND pllq.due_date <  @to;

/* --- homework / plan quizzes (plan -> plan_quiz) --- */
SELECT
    pl.id          AS plan_id,
    pl.name        AS plan_name,
    plq.quiz_id    AS quiz_id,
    qui.title      AS title,
    plq.due_date   AS due_date
FROM gs_teachers_web_db_prod.teacher_plans pl
INNER JOIN gs_teachers_web_db_prod.teacher_plan_quizs plq
    ON plq.plan_id = pl.id AND plq.available = 1
LEFT JOIN gs_teachers_web_db_prod.quizs qui
    ON qui.id = plq.quiz_id AND qui.available = 1
WHERE pl.available = 1
  AND pl.opened = 1
  AND pl.user_id = @userId
  AND plq.due_date >= @from
  AND plq.due_date <  @to;
";

            var events = new List<EventResponseModel>();

            var connectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"];

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, dynamicParameters))
                {
                    var lessons = multi.Read<LessonEventRow>().ToList();
                    var lessonQuizzes = multi.Read<LessonQuizEventRow>().ToList();
                    var homeworks = multi.Read<HomeworkEventRow>().ToList();

                    foreach (var r in lessons)
                    {
                        events.Add(BuildEvent(
                            eventType: EventType.Lesson,
                            id: r.LessonId,
                            title: r.PlanName,
                            start: r.DueDate,
                            end: r.DueDate,
                            timeZoneCode: null));
                    }

                    foreach (var r in lessonQuizzes)
                    {
                        DateTime? end = (r.DueDate.HasValue && r.Duration.HasValue)
                            ? r.DueDate.Value.AddMinutes(r.Duration.Value)
                            : r.DueDate;

                        events.Add(BuildEvent(
                            eventType: EventType.LessonQuiz,
                            id: r.QuizId,
                            title: r.Title ?? r.PlanName,
                            start: r.DueDate,
                            end: end,
                            timeZoneCode: r.DueDateTimezone));
                    }

                    foreach (var r in homeworks)
                    {
                        events.Add(BuildEvent(
                            eventType: EventType.Homework,
                            id: r.QuizId,
                            title: r.Title ?? r.PlanName,
                            start: r.DueDate,
                            end: r.DueDate,   // no duration on plan quizzes
                            timeZoneCode: null));
                    }
                }
            }

            // Calendar events (role / school / region / specific-user scoped) are read and
            // shaped by GetCalendarEventsOnly, which the student calendar also calls directly
            // (students have their own dedicated endpoints for lessons/homework, so they only
            // need this slice, not the teacher_plans queries above).
            events.AddRange(GetCalendarEventsOnly(month, year, roleId, userId, schoolIds, regionIds));

            // StartDate is a string, but the "yyyy-MM-dd HH:mm:ss" format sorts correctly
            // lexicographically, and nulls sort first.
            return events
                .OrderBy(e => e.EventModel.StartDate)
                .ToList();
        }

        /// <summary>
        /// Reads calendar events (events module) scoped to the given role / school / region /
        /// specific-user audience within a month/year, along with their attachments. Called by
        /// <see cref="GetEventsByMonthAndYear"/> for the teacher calendar, and directly by the
        /// student calendar.
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <param name="roleId">The viewer's role id, used to resolve role-scoped events.</param>
        /// <param name="userId">The viewer's user id, used to resolve user-scoped events.</param>
        /// <param name="schoolIds">The school ids the viewer belongs to. Optional.</param>
        /// <param name="regionIds">The region ids the viewer belongs to. Optional.</param>
        /// <returns>Returns the calendar-event response models for the given month and year.</returns>
        public static List<EventResponseModel> GetCalendarEventsOnly(
            int month,
            int year,
            string roleId,
            int userId,
            List<int> schoolIds = null,
            List<string> regionIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var to = from.AddMonths(1);

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@userId", userId);
            dynamicParameters.Add("@from", from);
            dynamicParameters.Add("@to", to);
            dynamicParameters.Add("@roleId", roleId);

            var hasSchools = schoolIds != null && schoolIds.Count > 0;
            var hasRegions = regionIds != null && regionIds.Count > 0;
            var isAdmin = roleId == EventAudienceRoles.Admin;
            var isAdminOre = roleId == EventAudienceRoles.AdminOre;

            var axisClauses = new List<string>();

            if (isAdmin)
            {
                // Central Admin sees every available event in the requested date range,
                // regardless of its configured audience.
                axisClauses.Add("1 = 1");
            }
            else if (isAdminOre)
            {
                // Admin ORE sees every event explicitly assigned to their region, regardless of
                // the event's role, school or specific-user audience axes.
                if (hasRegions)
                {
                    axisClauses.Add(
                        @"EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_regions reg
                                   WHERE reg.event_id = ev.id AND reg.region_id IN @regionIds)");
                    dynamicParameters.Add("@regionIds", regionIds);
                }
                else
                {
                    // An Admin ORE session without a region must never fall back to global access.
                    axisClauses.Add("1 = 0");
                }
            }
            else
            {
                // Audience is AND'd per axis (role / school / region / specific-user): each axis
                // only restricts when the event actually has rows in its scope table.
                axisClauses.Add(
                    @"(NOT EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_roles ro WHERE ro.event_id = ev.id)
                       OR EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_roles ro
                                   WHERE ro.event_id = ev.id AND ro.role_id = @roleId))");
                // eu.user_id is varchar; MySQL compares it against @userId (int) using the same
                // implicit numeric coercion already relied on for sch.user_school_record below.
                axisClauses.Add(
                    @"(NOT EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_users eu WHERE eu.event_id = ev.id)
                       OR EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_users eu
                                   WHERE eu.event_id = ev.id AND eu.user_id = @userId))");

                if (hasSchools)
                {
                    axisClauses.Add(
                        @"(NOT EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_schools sch WHERE sch.event_id = ev.id)
                           OR EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_schools sch
                                       WHERE sch.event_id = ev.id AND sch.user_school_record IN @schoolIds))");
                    // Dapper expands a list into (?,?,?) - no parentheses in the SQL.
                    dynamicParameters.Add("@schoolIds", schoolIds);
                }
                else
                {
                    // A viewer with no school can still receive role/user/global events, but not
                    // events explicitly restricted to a school.
                    axisClauses.Add(
                        @"NOT EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_schools sch WHERE sch.event_id = ev.id)");
                }

                if (hasRegions)
                {
                    axisClauses.Add(
                        @"(NOT EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_regions reg WHERE reg.event_id = ev.id)
                           OR EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_regions reg
                                       WHERE reg.event_id = ev.id AND reg.region_id IN @regionIds))");
                    dynamicParameters.Add("@regionIds", regionIds);
                }
                else
                {
                    // A viewer with no region can still receive role/user/global events, but not
                    // events explicitly restricted to a region.
                    axisClauses.Add(
                        @"NOT EXISTS (SELECT 1 FROM gs_teachers_web_db_prod.event_regions reg WHERE reg.event_id = ev.id)");
                }
            }

            // Reused verbatim by the attachments statement below so the two can't drift apart.
            // end_date is nullable (open-ended events), so treat a null end as "still running" instead of letting the comparison drop it.
            var eventPredicate = $@"
      ev.available = '1'
  AND ev.start_date <  @to
  AND (ev.end_date IS NULL OR ev.end_date >= @from)
  AND ({string.Join(" AND ", axisClauses)})";

            string sql = $@"
/* --- calendar events (role / school / region / specific-user scoped, AND'd per axis) --- */
SELECT
    ev.id                 AS id,
    ev.title              AS title,
    ev.description        AS description,
    ev.location           AS location,
    ev.start_date         AS start_date,
    ev.end_date           AS end_date,
    ev.timezone_code      AS timezone_code,
    ev.teams_event_id     AS teams_event_id,
    ev.teams_sync_status  AS teams_sync_status,
    ev.available          AS available,
    ev.modified_by        AS modified_by,
    ev.created_at         AS created_at,
    ev.updated_at         AS updated_at
FROM gs_teachers_web_db_prod.events ev
WHERE {eventPredicate};

/* --- attachments for those same events --- */
/* Kept as its own result set rather than a LEFT JOIN above: joining would repeat every
   wide event row (description, location, teams fields) once per attachment. This way the
   fan-out only carries the narrow attachment columns, and it is still one round trip. */
SELECT
    att.id             AS id,
    att.event_id       AS event_id,
    att.attachment_url AS attachment_url
FROM gs_teachers_web_db_prod.event_attachments att
INNER JOIN gs_teachers_web_db_prod.events ev
    ON ev.id = att.event_id
WHERE {eventPredicate};
";

            var events = new List<EventResponseModel>();

            var connectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"];

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, dynamicParameters))
                {
                    var calendarRows = multi.Read<CalendarEventRow>().ToList();
                    var attachmentRows = multi.Read<EventAttachmentRow>().ToList();

                    // One pass over the attachments instead of a scan per event.
                    var attachmentsByEvent = attachmentRows
                        .Where(a => !string.IsNullOrEmpty(a.EventId))
                        .ToLookup(a => a.EventId);

                    foreach (var r in calendarRows)
                    {
                        events.Add(BuildCalendarEvent(r, attachmentsByEvent[r.Id]));
                    }
                }
            }

            return events;
        }

        /// <summary>Row shape for the calendar-events result set (one row per event).</summary>
        private sealed class CalendarEventRow
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string TimezoneCode { get; set; }
            public string TeamsEventId { get; set; }
            public string TeamsSyncStatus { get; set; }
            public string Available { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        /// <summary>Row shape for the event-attachments result set.</summary>
        private sealed class EventAttachmentRow
        {
            public string Id { get; set; }
            public string EventId { get; set; }
            public string AttachmentUrl { get; set; }
        }

        /// <summary>
        /// Builds an <see cref="EventResponseModel"/> for a row of the events table, including
        /// its attachments.
        /// </summary>
        /// <param name="row">The event row.</param>
        /// <param name="attachments">The attachment rows belonging to that event.</param>
        /// <returns>Returns the populated event response model.</returns>
        private static EventResponseModel BuildCalendarEvent(
            CalendarEventRow row, IEnumerable<EventAttachmentRow> attachments)
        {
            var response = new EventResponseModel
            {
                EventModel = new EventModel
                {
                    Valid = true,
                    Messages = new List<string>(),
                    EventType = EventType.CalendarEvent,
                    Id = row.Id,
                    Title = row.Title,
                    Description = row.Description,
                    Location = row.Location,
                    StartDate = row.StartDate?.ToString(EventDateFormat),
                    EndDate = row.EndDate?.ToString(EventDateFormat),
                    TimeZoneCode = row.TimezoneCode,
                    TeamsEventId = row.TeamsEventId,
                    TeamsSyncStatus = row.TeamsSyncStatus,
                    Available = row.Available,
                    ModifiedBy = row.ModifiedBy,
                    CreatedAt = row.CreatedAt?.ToString(EventDateFormat),
                    UpdatedAt = row.UpdatedAt?.ToString(EventDateFormat)
                }
            };

            foreach (var a in attachments)
            {
                response.EventAttachmentModelList.Add(new EventAttachmentModel
                {
                    Id = a.Id,
                    EventId = a.EventId,
                    AttachmentUrl = a.AttachmentUrl
                });
            }

            return response;
        }

        /// <summary>
        /// Builds an <see cref="EventResponseModel"/> from the given event fields.
        /// </summary>
        /// <param name="eventType">The type of the event (homework, lesson, or lesson quiz).</param>
        /// <param name="id">The event's identifier.</param>
        /// <param name="title">The event's title.</param>
        /// <param name="start">The event's start date, or null if none.</param>
        /// <param name="end">The event's end date, or null if none.</param>
        /// <param name="timeZoneCode">The event's time zone code, or null if none.</param>
        /// <returns>Returns the populated event response model.</returns>
        private static EventResponseModel BuildEvent(
            EventType eventType, string id, string title,
            DateTime? start, DateTime? end, string timeZoneCode)
        {
            return new EventResponseModel
            {
                EventModel = new EventModel
                {
                    Valid = true,
                    Messages = new List<string>(),
                    EventType = eventType,
                    Id = id,
                    Title = title,
                    Description = null,
                    Location = null,
                    StartDate = start?.ToString(EventDateFormat),
                    EndDate = end?.ToString(EventDateFormat),
                    TimeZoneCode = timeZoneCode,
                    TeamsEventId = null,
                    TeamsSyncStatus = null,
                    Available = "1",
                    ModifiedBy = null,
                    CreatedAt = null,
                    UpdatedAt = null
                }
                // EventAttachmentModelList is initialized empty by the constructor
            };
        }
    }
}
