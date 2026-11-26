using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Dapper;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.Utils;
using MySqlConnector;

namespace DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper
{
    /// <summary>
    /// Read-only queries of the usage insights (gs_data_analysis). Rules that apply to every query:
    /// the session is opened in READ ONLY mode with time_zone UTC (the columns are UTC TIMESTAMPs); every value travels as a parameter;
    /// the caller scope is always applied inside the SQL (school.ore_id / school.school_id), never on rows already cut;
    /// emails, usernames and student data are never returned (only aggregates and the teacher name).
    /// Definitions: a connection is one connected_date row (one user in one day); teacher = user.role_id 5; student = role_id 3;
    /// a subject area is a subject; tests are a cohort by assignment date and are completed when they have a quiz_taken_date;
    /// the day is read in the local time given by the context offset (Puerto Rico, UTC-4, by default).
    /// </summary>
    public static class UsageInsights
    {
        private const int ClientRoleId = 1;
        private const int StudentRoleId = 3;
        private const int TeacherRoleId = 5;

        // ── Infrastructure ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Opens a connection to the reader database in READ ONLY mode and UTC time zone. It refuses to work if the session is not read only.
        /// </summary>
        /// <returns>Returns the open connection.</returns>
        public static MySqlConnection OpenReadOnlyConnection()
        {
            var connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                connection.Execute("SET time_zone = '+00:00'");
                connection.Execute("SET SESSION TRANSACTION READ ONLY");
                if (connection.ExecuteScalar<int>("SELECT @@session.transaction_read_only") != 1)
                {
                    throw new InvalidOperationException("The MySQL session is not read only; the query was cancelled for safety.");
                }
            }
            catch
            {
                connection.Dispose();
                throw;
            }

            return connection;
        }

        /// <summary>
        /// Runs a query and maps every row. When MySQL fails, the start of the SQL (fixed text with parameter names, never values) travels in the
        /// exception data so the caller can log the cause.
        /// </summary>
        private static List<T> Fetch<T>(UsageInsightsQueryContext context, string sql, DynamicParameters parameters, Func<IDictionary<string, object?>, T> map,
            int? commandTimeoutSeconds = null)
        {
            try
            {
                using var connection = OpenReadOnlyConnection();
                var rows = connection.Query(sql, parameters, commandTimeout: commandTimeoutSeconds ?? context.CommandTimeoutSeconds);
                return rows.Select(row => map((IDictionary<string, object?>)row)).ToList();
            }
            catch (MySqlException exception)
            {
                exception.Data["sql_start"] = sql;
                throw;
            }
        }

        private static int ToInt(object? value) => value is null ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);

        private static double ToDouble(object? value) => Convert.ToDouble(value, CultureInfo.InvariantCulture);

        private static string? ToText(object? value) => value is null ? null : Convert.ToString(value, CultureInfo.InvariantCulture);

        private static DateOnly? ToLocalDay(object? value, UsageInsightsQueryContext context) =>
            value is DateTime instant ? DateOnly.FromDateTime(instant.AddMinutes(context.UtcOffsetMinutes)) : null;

        /// <summary>The local day "day" starts at this UTC instant (day 00:00 local = day 00:00 UTC minus the offset).</summary>
        private static DateTime StartOf(DateOnly day, UsageInsightsQueryContext context) =>
            day.ToDateTime(TimeOnly.MinValue).AddMinutes(-context.UtcOffsetMinutes);

        private static DateTime EndExclusiveOf(DateOnly day, UsageInsightsQueryContext context) =>
            day.AddDays(1).ToDateTime(TimeOnly.MinValue).AddMinutes(-context.UtcOffsetMinutes);

        private static string Direction(bool descending) => descending ? "DESC" : "ASC";

        private static DynamicParameters NewParameters(UsageInsightsQueryContext context)
        {
            var parameters = new DynamicParameters();
            if (context.Scope.OreId is { } oreId)
            {
                parameters.Add("@scope_ore", oreId);
            }

            if (context.Scope.SchoolId is { } schoolId)
            {
                parameters.Add("@scope_school", schoolId);
            }

            var exclusion = context.Exclusion;
            if (exclusion.IsActive)
            {
                for (var i = 0; i < exclusion.EmailDomains.Length; i++)
                {
                    parameters.Add($"@excl_domain{i}", "%@" + exclusion.EmailDomains[i]);
                }

                for (var i = 0; i < exclusion.Ores.Length; i++)
                {
                    parameters.Add($"@excl_ore{i}", "%" + exclusion.Ores[i] + "%");
                }

                for (var i = 0; i < exclusion.Schools.Length; i++)
                {
                    parameters.Add($"@excl_school{i}", exclusion.Schools[i]);
                }
            }

            return parameters;
        }

        /// <summary>
        /// Scope filter on the school (alias <paramref name="school"/>), always in SQL. With a scope, a user without school stays out
        /// (the comparison with NULL is never true). The ORE is matched by school.ore_id = OreId of UsersAPI.
        /// </summary>
        private static string ScopeFilter(UsageInsightsQueryContext context, string school)
        {
            if (context.Scope.IsAll)
            {
                return string.Empty;
            }

            var sql = new StringBuilder();
            if (context.Scope.OreId is not null)
            {
                sql.Append($" AND {school}.ore_id = @scope_ore");
            }

            if (context.Scope.SchoolId is not null)
            {
                sql.Append($" AND {school}.school_id = @scope_school");
            }

            return sql.ToString();
        }

        /// <summary>Exclusion by email domain of the user (alias <paramref name="user"/>).</summary>
        private static string DomainFilter(UsageInsightsQueryContext context, string user)
        {
            if (!context.Exclusion.IsActive)
            {
                return string.Empty;
            }

            var sql = new StringBuilder();
            for (var i = 0; i < context.Exclusion.EmailDomains.Length; i++)
            {
                sql.Append($" AND COALESCE({user}.email, '') NOT LIKE @excl_domain{i}");
            }

            return sql.ToString();
        }

        /// <summary>Scope plus exclusion by ORE and school name (users without school stay out, as in the Power BI views).</summary>
        private static string SchoolFilter(UsageInsightsQueryContext context, string school)
        {
            var sql = new StringBuilder(ScopeFilter(context, school));
            if (!context.Exclusion.IsActive)
            {
                return sql.ToString();
            }

            // Same semantics as the Power BI views: NOT (ore LIKE '%X%'); when the school or its ORE is NULL, the row stays out.
            for (var i = 0; i < context.Exclusion.Ores.Length; i++)
            {
                sql.Append($" AND {school}.ore NOT LIKE @excl_ore{i}");
            }

            for (var i = 0; i < context.Exclusion.Schools.Length; i++)
            {
                sql.Append($" AND COALESCE({school}.school_name, '') <> @excl_school{i}");
            }

            return sql.ToString();
        }

        private static string UserFilter(UsageInsightsQueryContext context, string user, string school) =>
            DomainFilter(context, user) + SchoolFilter(context, school);

        // ── Metadata quoted in every response ───────────────────────────────────────────

        /// <summary>
        /// Gets the last day with connection data, over the data that is queried (without the excluded test accounts).
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <returns>Returns the last local day, or null when there is no data.</returns>
        public static DateOnly? GetConnectionsDataUntil(UsageInsightsQueryContext context)
        {
            var sql = $@"SELECT MAX(dt.connected_date) AS last_instant
                    FROM connected_date dt JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {ClientRoleId}
                    LEFT JOIN school sc ON sc.school_id = us.school_id
                    WHERE 1 = 1 {UserFilter(context, "us", "sc")}";
            return Fetch(context, sql, NewParameters(context), row => ToLocalDay(row["last_instant"], context)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the last day with test data (taken or assigned), over the data that is queried.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <returns>Returns the last local day, or null when there is no data.</returns>
        public static DateOnly? GetTestsDataUntil(UsageInsightsQueryContext context)
        {
            var sql = $@"SELECT GREATEST(MAX(r.quiz_taken_date), MAX(r.quiz_assigned_to_plan_date)) AS last_instant
                    FROM result_generic r JOIN user us ON us.user_id = r.user_id AND us.role_id = @student_role
                    LEFT JOIN school sc ON sc.school_id = us.school_id
                    WHERE 1 = 1 {UserFilter(context, "us", "sc")}";
            var parameters = NewParameters(context);
            parameters.Add("@student_role", StudentRoleId);
            return Fetch(context, sql, parameters, row => ToLocalDay(row["last_instant"], context)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the name of the school or ORE that the scope covers, or null when the scope is not restricted.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <returns>Returns the school name, the ORE name, or a numeric fallback; null for the all-schools scope.</returns>
        public static (bool IsSchool, string Name)? GetScopeName(UsageInsightsQueryContext context)
        {
            if (context.Scope.IsAll)
            {
                return null;
            }

            if (context.Scope.SchoolId is { } schoolId)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@school", schoolId);
                var name = Fetch(context, "SELECT school_name AS name FROM school WHERE school_id = @school", parameters, row => ToText(row["name"])).FirstOrDefault();
                return (true, name ?? $"#{schoolId}");
            }

            var oreParameters = new DynamicParameters();
            oreParameters.Add("@ore", context.Scope.OreId!.Value);
            var oreName = Fetch(context, "SELECT MIN(ore) AS name FROM school WHERE ore_id = @ore", oreParameters, row => ToText(row["name"])).FirstOrDefault();
            return (false, oreName ?? $"#{context.Scope.OreId}");
        }

        /// <summary>
        /// Gets the school of a user (gs_data_analysis.user.school_id); it gives the scope of a school director.
        /// </summary>
        /// <param name="context">The query context (its scope is not used).</param>
        /// <param name="userId">The user id of the platform session.</param>
        /// <returns>Returns the school id, or null when it is not found.</returns>
        public static int? GetUserSchoolId(UsageInsightsQueryContext context, string userId)
        {
            if (!int.TryParse(userId, NumberStyles.None, CultureInfo.InvariantCulture, out var id))
            {
                return null;
            }

            var parameters = new DynamicParameters();
            parameters.Add("@user", id);
            return Fetch(context, "SELECT school_id AS school FROM user WHERE user_id = @user AND school_id IS NOT NULL LIMIT 1", parameters,
                row => (int?)ToInt(row["school"])).FirstOrDefault();
        }

        // ── School catalog ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Checks whether a school with that exact name exists inside the scope. A school out of scope behaves as if it did not exist.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="schoolName">The exact school name.</param>
        /// <returns>Returns true when the school exists and the caller may see it.</returns>
        public static bool SchoolExists(UsageInsightsQueryContext context, string schoolName)
        {
            var parameters = NewParameters(context);
            parameters.Add("@name", schoolName);
            return Fetch(context, $"SELECT 1 AS found FROM school sc WHERE sc.school_name = @name {SchoolFilter(context, "sc")} LIMIT 1", parameters,
                row => 1).Count > 0;
        }

        /// <summary>
        /// Searches the schools whose name contains a text (case and accent insensitive by the column collation).
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="text">The text to look for.</param>
        /// <param name="limit">Maximum number of schools.</param>
        /// <returns>Returns the schools ordered by name.</returns>
        public static List<SchoolCatalogRow> SearchSchools(UsageInsightsQueryContext context, string text, int limit)
        {
            var pattern = "%" + text.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_") + "%";
            var parameters = NewParameters(context);
            parameters.Add("@pattern", pattern);
            parameters.Add("@limit", limit);
            return Fetch(context,
                $"SELECT sc.school_name AS name FROM school sc WHERE sc.school_name LIKE @pattern {SchoolFilter(context, "sc")} ORDER BY sc.school_name LIMIT @limit",
                parameters, row => new SchoolCatalogRow(ToText(row["name"])!));
        }

        // ── Connections ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Gets the connections of every school (or the limited top) in a period.
        /// A school is considered when it has at least one student or one connection (real schools of the program).
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="descending">True for the most connections first.</param>
        /// <param name="limit">Maximum number of rows.</param>
        /// <returns>Returns the schools with their connections.</returns>
        public static List<SchoolConnectionsRow> GetConnectionsBySchool(UsageInsightsQueryContext context, DateOnly from, DateOnly to, bool descending, int limit)
        {
            var sql = $@"SELECT sh.school_name AS name, COALESCE(x.n, 0) AS connections, COALESCE(x.u, 0) AS users, x.last_instant
                FROM school sh
                LEFT JOIN (SELECT us.school_id, COUNT(*) AS n, COUNT(DISTINCT dt.user_id) AS u, MAX(dt.connected_date) AS last_instant
                           FROM connected_date dt JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {ClientRoleId}
                           WHERE dt.connected_date >= @from AND dt.connected_date < @to {DomainFilter(context, "us")}
                           GROUP BY us.school_id) x ON x.school_id = sh.school_id
                WHERE (EXISTS (SELECT 1 FROM user u2 WHERE u2.school_id = sh.school_id AND u2.role_id = {StudentRoleId})
                       OR EXISTS (SELECT 1 FROM connected_date d2 JOIN user u3 ON u3.user_id = d2.user_id AND u3.role_id <> {ClientRoleId}
                                  WHERE u3.school_id = sh.school_id)) {SchoolFilter(context, "sh")}
                ORDER BY connections {Direction(descending)}, sh.school_name {Direction(descending)}
                LIMIT @limit";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@limit", limit);
            return Fetch(context, sql, parameters, row => new SchoolConnectionsRow(ToText(row["name"])!, ToInt(row["connections"]), ToInt(row["users"]),
                LastConnection: ToLocalDay(row["last_instant"], context)));
        }

        /// <summary>
        /// Gets the connections of every teacher in a period. Only users with the teacher role and a school.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="descending">True for the most connections first.</param>
        /// <param name="limit">Maximum number of rows.</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns the teachers with their connections.</returns>
        public static List<TeacherConnectionsRow> GetConnectionsByTeacher(UsageInsightsQueryContext context, DateOnly from, DateOnly to, bool descending,
            int limit, string? schoolName)
        {
            var sql = $@"SELECT TRIM(CONCAT_WS(' ', us.first_name, us.last_name)) AS teacher,
                                COALESCE(sh.school_name, '(no school)') AS school,
                                COALESCE(x.n, 0) AS connections, x.last_instant
                FROM user us
                LEFT JOIN school sh ON sh.school_id = us.school_id
                LEFT JOIN (SELECT dt.user_id, COUNT(*) AS n, MAX(dt.connected_date) AS last_instant FROM connected_date dt
                           WHERE dt.connected_date >= @from AND dt.connected_date < @to
                           GROUP BY dt.user_id) x ON x.user_id = us.user_id
                WHERE us.role_id = @teacher_role AND us.school_id IS NOT NULL AND (@school IS NULL OR sh.school_name = @school) {UserFilter(context, "us", "sh")}
                ORDER BY connections {Direction(descending)}, teacher {Direction(descending)}, us.user_id
                LIMIT @limit";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@teacher_role", TeacherRoleId);
            parameters.Add("@limit", limit);
            parameters.Add("@school", schoolName);
            return Fetch(context, sql, parameters, row => new TeacherConnectionsRow(ToText(row["teacher"])!, ToText(row["school"])!, ToInt(row["connections"]),
                LastConnection: ToLocalDay(row["last_instant"], context)));
        }

        /// <summary>
        /// Gets the connections per local day (users with a school, role other than client). The service groups the days into periods.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns one entry per day with connections.</returns>
        public static List<(DateOnly Day, int Connections)> GetConnectionsTrend(UsageInsightsQueryContext context, DateOnly from, DateOnly to, string? schoolName)
        {
            var sql = $@"SELECT DATE(DATE_ADD(dt.connected_date, INTERVAL {context.UtcOffsetMinutes} MINUTE)) AS day, COUNT(*) AS n
                FROM connected_date dt
                JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {ClientRoleId}
                LEFT JOIN school sh ON sh.school_id = us.school_id
                WHERE dt.connected_date >= @from AND dt.connected_date < @to AND us.school_id IS NOT NULL AND (@school IS NULL OR sh.school_name = @school) {UserFilter(context, "us", "sh")}
                GROUP BY day";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@school", schoolName);
            return Fetch(context, sql, parameters, row => (DateOnly.FromDateTime(Convert.ToDateTime(row["day"], CultureInfo.InvariantCulture)), ToInt(row["n"])));
        }

        /// <summary>
        /// Gets, day by day, the connections and the new users (first connection of the range). The service groups and accumulates them.
        /// Same universe as <see cref="GetConnectionsTrend"/>.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns one row per day with connections.</returns>
        public static List<DailyConnectionsRow> GetDailyConnectionsAndNewUsers(UsageInsightsQueryContext context, DateOnly from, DateOnly to, string? schoolName)
        {
            var sql = $@"SELECT x.day, SUM(x.n) AS connections, SUM(x.is_first) AS new_users
                FROM (SELECT d.day, d.user_id, d.n, d.day = MIN(d.day) OVER (PARTITION BY d.user_id) AS is_first
                      FROM (SELECT DATE(DATE_ADD(dt.connected_date, INTERVAL {context.UtcOffsetMinutes} MINUTE)) AS day, dt.user_id, COUNT(*) AS n
                            FROM connected_date dt
                            JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {ClientRoleId}
                            LEFT JOIN school sh ON sh.school_id = us.school_id
                            WHERE dt.connected_date >= @from AND dt.connected_date < @to AND us.school_id IS NOT NULL
                              AND (@school IS NULL OR sh.school_name = @school) {UserFilter(context, "us", "sh")}
                            GROUP BY day, dt.user_id) d) x
                GROUP BY x.day ORDER BY x.day";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@school", schoolName);
            return Fetch(context, sql, parameters, row => new DailyConnectionsRow(
                DateOnly.FromDateTime(Convert.ToDateTime(row["day"], CultureInfo.InvariantCulture)), ToInt(row["connections"]), ToInt(row["new_users"])));
        }

        /// <summary>
        /// Gets the users with an active subscription (status 'active', without looking at the cancellation date, role other than client)
        /// and how many of them ever connected. It mirrors vw_total_users_with_connection_details: only the scope, the ORE exclusion and the
        /// domain exclusion apply (the Power BI card has no school filter).
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <returns>Returns the counts.</returns>
        public static ActiveUsersCount GetActiveUsersCount(UsageInsightsQueryContext context)
        {
            var schoolFilter = new StringBuilder(ScopeFilter(context, "sh"));
            if (context.Exclusion.IsActive)
            {
                for (var i = 0; i < context.Exclusion.Ores.Length; i++)
                {
                    schoolFilter.Append($" AND sh.ore NOT LIKE @excl_ore{i}");
                }
            }

            var statusColumn = SafeIdentifier(context.SubscriptionStatusColumn);
            var filter = $"sph.{statusColumn} = 'active' AND us.role_id <> {ClientRoleId} {schoolFilter} {DomainFilter(context, "us")}";
            var sql = $@"SELECT
                (SELECT COUNT(DISTINCT us.user_id) FROM user us LEFT JOIN school sh ON sh.school_id = us.school_id
                        JOIN subscription_profile_history sph ON sph.user_id = us.user_id WHERE {filter}) AS active_users,
                (SELECT COUNT(DISTINCT us.user_id) FROM user us LEFT JOIN school sh ON sh.school_id = us.school_id
                        JOIN subscription_profile_history sph ON sph.user_id = us.user_id
                        WHERE {filter} AND EXISTS (SELECT 1 FROM connected_date dt WHERE dt.user_id = us.user_id)) AS ever_connected";
            return Fetch(context, sql, NewParameters(context), row => new ActiveUsersCount(ToInt(row["active_users"]), ToInt(row["ever_connected"]))).First();
        }

        private static readonly string[] EnglishWeekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        /// <summary>
        /// Gets the connections grouped by weekday (always seven rows, Monday to Sunday, with English names) or by role (without the client role).
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="groupBy">"weekday" or "role".</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns the groups.</returns>
        public static List<ConnectionPatternRow> GetConnectionPatterns(UsageInsightsQueryContext context, DateOnly from, DateOnly to, string groupBy, string? schoolName)
        {
            var key = groupBy == "role" ? "COALESCE(us.role, '(no role)')" : $"WEEKDAY(DATE_ADD(dt.connected_date, INTERVAL {context.UtcOffsetMinutes} MINUTE))";
            var sql = $@"SELECT {key} AS group_key, COUNT(*) AS connections, COUNT(DISTINCT dt.user_id) AS users
                FROM connected_date dt
                JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {ClientRoleId}
                LEFT JOIN school sh ON sh.school_id = us.school_id
                WHERE dt.connected_date >= @from AND dt.connected_date < @to AND us.school_id IS NOT NULL
                  AND (@school IS NULL OR sh.school_name = @school) {UserFilter(context, "us", "sh")}
                GROUP BY group_key";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@school", schoolName);
            var rows = Fetch(context, sql, parameters, row => (Key: ToText(row["group_key"])!, Connections: ToInt(row["connections"]), Users: ToInt(row["users"])));
            if (groupBy == "role")
            {
                return rows.OrderByDescending(row => row.Connections).ThenBy(row => row.Key, StringComparer.Ordinal)
                    .Select(row => new ConnectionPatternRow(row.Key, row.Connections, row.Users)).ToList();
            }

            return EnglishWeekdays.Select((weekday, index) =>
            {
                var match = rows.FirstOrDefault(row => row.Key == index.ToString(CultureInfo.InvariantCulture));
                return new ConnectionPatternRow(weekday, match.Connections, match.Users);
            }).ToList();
        }

        /// <summary>
        /// Gets the participation of every school with active users or connections in the period. Active = subscription 'active'
        /// (same definition as <see cref="GetActiveUsersCount"/>); connected = distinct users with a connection in the period, with or without an active subscription.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <returns>Returns the schools ordered by name.</returns>
        public static List<SchoolParticipationRow> GetSchoolParticipation(UsageInsightsQueryContext context, DateOnly from, DateOnly to)
        {
            var statusColumn = SafeIdentifier(context.SubscriptionStatusColumn);
            var sql = $@"SELECT sh.school_name AS name,
                (SELECT COUNT(DISTINCT us.user_id) FROM user us JOIN subscription_profile_history sph ON sph.user_id = us.user_id
                  WHERE us.school_id = sh.school_id AND sph.{statusColumn} = 'active' AND us.role_id <> {ClientRoleId} {DomainFilter(context, "us")}) AS active_users,
                (SELECT COUNT(DISTINCT dt.user_id) FROM connected_date dt JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {ClientRoleId}
                  WHERE us.school_id = sh.school_id AND dt.connected_date >= @from AND dt.connected_date < @to {DomainFilter(context, "us")}) AS connected_users
                FROM school sh WHERE 1 = 1 {SchoolFilter(context, "sh")}
                HAVING active_users > 0 OR connected_users > 0
                ORDER BY sh.school_name";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            return Fetch(context, sql, parameters, row =>
            {
                int active = ToInt(row["active_users"]), connected = ToInt(row["connected_users"]);
                return new SchoolParticipationRow(ToText(row["name"])!, active, connected, active == 0 ? null : Math.Round(100.0 * connected / active, 1));
            });
        }

        // ── Tests by subject area ───────────────────────────────────────────────────────

        // Universe: subjects with at least one test ever assigned (the catalog has about 70 subjects, almost all empty).
        // Do NOT use `r.user_id IN (SELECT ... LEFT JOIN ...)` inside the ON of a LEFT JOIN: MySQL 8 returned wrong results
        // (it counted teacher tests as student tests). A derived table with explicit JOINs is used instead.
        private static string FromSubjectAreas(UsageInsightsQueryContext context) => $@"
            FROM subject s
            LEFT JOIN (SELECT rr.subject_code, rr.quiz_assigned_to_plan_date, rr.quiz_taken_date
                       FROM result_generic rr
                       JOIN user us ON us.user_id = rr.user_id AND us.role_id = @student_role
                       LEFT JOIN school sc ON sc.school_id = us.school_id
                       WHERE 1 = 1 {UserFilter(context, "us", "sc")}) r ON r.subject_code = s.subject_code
            WHERE EXISTS (SELECT 1 FROM result_generic x WHERE x.subject_code = s.subject_code)
            GROUP BY s.subject_code, s.subject_name";

        /// <summary>
        /// Gets the tests assigned and completed per subject area in a period. One date base (cohort by assignment date),
        /// so completed is never above assigned and the percentage is coherent.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="descending">True for the most completed tests first.</param>
        /// <param name="limit">Maximum number of rows.</param>
        /// <returns>Returns the subject areas.</returns>
        public static List<SubjectAreaUsageRow> GetSubjectAreaUsage(UsageInsightsQueryContext context, DateOnly from, DateOnly to, bool descending, int limit)
        {
            var sql = $@"SELECT s.subject_name AS name,
                COALESCE(SUM(r.quiz_assigned_to_plan_date >= @from AND r.quiz_assigned_to_plan_date < @to), 0) AS assigned,
                COALESCE(SUM(r.quiz_assigned_to_plan_date >= @from AND r.quiz_assigned_to_plan_date < @to
                             AND r.quiz_taken_date IS NOT NULL), 0) AS completed
                {FromSubjectAreas(context)}
                ORDER BY completed {Direction(descending)}, s.subject_name {Direction(descending)}
                LIMIT @limit";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@student_role", StudentRoleId);
            parameters.Add("@limit", limit);
            return Fetch(context, sql, parameters, row =>
            {
                int assigned = ToInt(row["assigned"]), completed = ToInt(row["completed"]);
                return new SubjectAreaUsageRow(ToText(row["name"])!, assigned, completed, assigned == 0 ? null : Math.Round(100.0 * completed / assigned, 1));
            });
        }

        /// <summary>
        /// Compares the completed tests of every subject area in two periods.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="fromA">First local day of period A.</param>
        /// <param name="toA">Last local day of period A.</param>
        /// <param name="fromB">First local day of period B.</param>
        /// <param name="toB">Last local day of period B.</param>
        /// <returns>Returns the subject areas ordered by name.</returns>
        public static List<SubjectAreaComparisonRow> CompareSubjectAreas(UsageInsightsQueryContext context, DateOnly fromA, DateOnly toA, DateOnly fromB, DateOnly toB)
        {
            var sql = $@"SELECT s.subject_name AS name,
                COALESCE(SUM(r.quiz_assigned_to_plan_date >= @from_a AND r.quiz_assigned_to_plan_date < @to_a AND r.quiz_taken_date IS NOT NULL), 0) AS value_a,
                COALESCE(SUM(r.quiz_assigned_to_plan_date >= @from_b AND r.quiz_assigned_to_plan_date < @to_b AND r.quiz_taken_date IS NOT NULL), 0) AS value_b
                {FromSubjectAreas(context)}
                ORDER BY s.subject_name";
            var parameters = NewParameters(context);
            parameters.Add("@from_a", StartOf(fromA, context));
            parameters.Add("@to_a", EndExclusiveOf(toA, context));
            parameters.Add("@from_b", StartOf(fromB, context));
            parameters.Add("@to_b", EndExclusiveOf(toB, context));
            parameters.Add("@student_role", StudentRoleId);
            return Fetch(context, sql, parameters, row =>
            {
                int valueA = ToInt(row["value_a"]), valueB = ToInt(row["value_b"]), difference = valueB - valueA;
                return new SubjectAreaComparisonRow(ToText(row["name"])!, valueA, valueB, difference,
                    valueA == 0 ? null : Math.Round(100.0 * difference / valueA, 1));
            });
        }

        /// <summary>
        /// Gets the Full Quiz average (result_generic.total_percent_full_quiz, same scale as Power BI). Only student tests with a school.
        /// Every test counts in the period by its assignment date.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="from">First local day.</param>
        /// <param name="to">Last local day.</param>
        /// <param name="groupBy">"school", "subject" or "school_subject".</param>
        /// <param name="descending">True for the highest average first.</param>
        /// <param name="limit">Maximum number of rows.</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns the groups.</returns>
        public static List<FullQuizAverageRow> GetFullQuizAverage(UsageInsightsQueryContext context, DateOnly from, DateOnly to, string groupBy, bool descending,
            int limit, string? schoolName)
        {
            bool bySchool = groupBy is "school" or "school_subject", bySubject = groupBy is "subject" or "school_subject";
            var groupColumns = new List<string>();
            if (bySchool)
            {
                groupColumns.Add("sc.school_name");
            }

            if (bySubject)
            {
                groupColumns.Add("COALESCE(s.subject_name, rr.subject_code)");
            }

            var groupBySql = string.Join(", ", groupColumns);
            var sql = $@"SELECT {(bySchool ? "sc.school_name" : "NULL")} AS school, {(bySubject ? "COALESCE(s.subject_name, rr.subject_code)" : "NULL")} AS subject,
                COUNT(*) AS assigned,
                COALESCE(SUM(rr.quiz_taken_date IS NOT NULL), 0) AS completed,
                AVG(CASE WHEN rr.quiz_taken_date IS NOT NULL THEN rr.total_percent_full_quiz END) AS average_completed,
                AVG(COALESCE(rr.total_percent_full_quiz, 0)) AS average_all
                FROM result_generic rr
                JOIN user us ON us.user_id = rr.user_id AND us.role_id = @student_role
                JOIN school sc ON sc.school_id = us.school_id
                LEFT JOIN subject s ON s.subject_code = rr.subject_code
                WHERE rr.quiz_assigned_to_plan_date >= @from AND rr.quiz_assigned_to_plan_date < @to
                  AND (@school IS NULL OR sc.school_name = @school) {UserFilter(context, "us", "sc")}
                GROUP BY {groupBySql}
                ORDER BY average_completed IS NULL, average_completed {Direction(descending)}, {groupBySql}
                LIMIT @limit";
            var parameters = NewParameters(context);
            parameters.Add("@from", StartOf(from, context));
            parameters.Add("@to", EndExclusiveOf(to, context));
            parameters.Add("@student_role", StudentRoleId);
            parameters.Add("@limit", limit);
            parameters.Add("@school", schoolName);
            return Fetch(context, sql, parameters, row => new FullQuizAverageRow(
                ToText(row["school"]), ToText(row["subject"]), ToInt(row["assigned"]), ToInt(row["completed"]),
                row["average_completed"] is null ? null : Math.Round(ToDouble(row["average_completed"]), 2),
                Math.Round(ToDouble(row["average_all"]), 2)));
        }

        // ── Teacher activity (vw_teacher_activities) ────────────────────────────────────
        // The view (real definition, 18-nov-2026, different from the wiki) has ONE ROW PER TEACHER AND ACADEMIC PERIOD of their school, with
        // the indicators computed inside that period. Power BI picks a period and counts DISTINCT teachers.
        // It reads production databases of other APIs through the view: data is as of the query time (Power BI shows a daily snapshot).

        private static string TeacherActivityFilter(UsageInsightsQueryContext context)
        {
            var sql = new StringBuilder();

            // The view only has NAMES (school and ORE), not ids. A school of the scope with the SAME school name and the SAME ORE name must exist
            // (restrictive: it is never filtered by only one of them). ASSUMPTION to verify: ORE name <-> ore_id is 1:1.
            if (!context.Scope.IsAll)
            {
                sql.Append($" AND EXISTS (SELECT 1 FROM school sa WHERE sa.school_name = t.school AND sa.ore = t.ore {ScopeFilter(context, "sa")})");
            }

            if (!context.Exclusion.IsActive)
            {
                return sql.ToString();
            }

            for (var i = 0; i < context.Exclusion.Ores.Length; i++)
            {
                sql.Append($" AND t.ore NOT LIKE @excl_ore{i}");
            }

            for (var i = 0; i < context.Exclusion.Schools.Length; i++)
            {
                sql.Append($" AND COALESCE(t.school, '') <> @excl_school{i}");
            }

            return sql.ToString();
        }

        /// <summary>
        /// Gets the teacher activity of every school, in the academic period that contains <paramref name="date"/> or the one that starts on
        /// <paramref name="periodFrom"/>. It returns the rows per school and period, and a total row (distinct teachers over the whole result).
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="date">A date inside the academic period (used when <paramref name="periodFrom"/> is null).</param>
        /// <param name="periodFrom">Start of the academic period.</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns the rows and the total.</returns>
        public static (List<TeacherActivityRow> Rows, TeacherActivityRow Total) GetTeacherActivity(UsageInsightsQueryContext context, DateOnly? date,
            DateOnly? periodFrom, string? schoolName)
        {
            const string indicators = @"COUNT(DISTINCT t.sie) AS teachers,
                COUNT(DISTINCT CASE WHEN t.has_connections_current_period = 'SI' THEN t.sie END) AS with_connections,
                COUNT(DISTINCT CASE WHEN t.has_created_instructional_plans_current_period = 'SI' THEN t.sie END) AS with_plans,
                COUNT(DISTINCT CASE WHEN t.plans_have_data_current_period = 'SI' THEN t.sie END) AS plans_with_data,
                COUNT(DISTINCT CASE WHEN t.has_created_groups_current_year = 'SI' THEN t.sie END) AS with_groups,
                COUNT(DISTINCT CASE WHEN t.has_created_assignments_overall = 'SI' THEN t.sie END) AS with_assignments";
            var source = $@"FROM vw_teacher_activities t
                WHERE (CASE WHEN @period_from IS NULL THEN @date BETWEEN DATE(t.from_period) AND DATE(t.to_period) ELSE DATE(t.from_period) = @period_from END)
                  AND (@school IS NULL OR t.school = @school) {TeacherActivityFilter(context)}";

            DynamicParameters BuildParameters()
            {
                var parameters = NewParameters(context);
                parameters.Add("@date", date is { } d ? d.ToDateTime(TimeOnly.MinValue) : null);
                parameters.Add("@period_from", periodFrom is { } p ? p.ToDateTime(TimeOnly.MinValue) : null);
                parameters.Add("@school", schoolName);
                return parameters;
            }

            static string FormatDay(object? value) => value is DateTime day ? DateOnly.FromDateTime(day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;

            var rows = Fetch(context, $@"SELECT t.school AS school, DATE(t.from_period) AS period_from, DATE(t.to_period) AS period_to, {indicators} {source}
                GROUP BY t.school, DATE(t.from_period), DATE(t.to_period)
                ORDER BY COUNT(DISTINCT t.sie) DESC, t.school, DATE(t.from_period)", BuildParameters(),
                row => new TeacherActivityRow(ToText(row["school"]) ?? "(no school)", FormatDay(row["period_from"]), FormatDay(row["period_to"]),
                    ToInt(row["teachers"]), ToInt(row["with_connections"]), ToInt(row["with_plans"]), ToInt(row["plans_with_data"]),
                    ToInt(row["with_groups"]), ToInt(row["with_assignments"])));
            var total = Fetch(context, $"SELECT {indicators} {source}", BuildParameters(),
                row => new TeacherActivityRow("TOTAL", string.Empty, string.Empty, ToInt(row["teachers"]), ToInt(row["with_connections"]),
                    ToInt(row["with_plans"]), ToInt(row["plans_with_data"]), ToInt(row["with_groups"]), ToInt(row["with_assignments"]))).First();
            return (rows, total);
        }

        // ── Users by subscription status ────────────────────────────────────────────────

        // Set once when the cancellation column does not exist in the database, so it is not probed again.
        private static volatile bool _cancellationColumnMissing;

        /// <summary>
        /// Gets the users by subscription status. The universe is the one of the "Active" card: role other than client, outside the test accounts and
        /// inside the scope. Every user falls in ONE category:
        /// active (some row with status 'active', without looking at the cancellation date: definition A of the Power BI dashboard),
        /// canceled (no active row and some status containing "cancel"), inactive (rows but none active or canceled), none (no rows).
        /// The real status values are returned in the total so nothing depends on assumptions.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <param name="groupBy">"total", "school" or "role".</param>
        /// <param name="schoolName">Optional exact school name.</param>
        /// <returns>Returns the rows per group and the total.</returns>
        public static (List<SubscriptionStatusRow> Rows, SubscriptionStatusCount Total) GetUsersBySubscriptionStatus(UsageInsightsQueryContext context,
            string groupBy, string? schoolName)
        {
            var statusColumn = SafeIdentifier(context.SubscriptionStatusColumn);
            var cancellationColumn = SafeIdentifier(context.SubscriptionCancellationColumn);
            var withCancellationDate = !_cancellationColumnMissing;
            var group = groupBy switch
            {
                "school" => "COALESCE(c.school, '(no school)')",
                "role" => "COALESCE(c.role, '(no role)')",
                _ => "'total'",
            };

            string BuildSql(bool includeCancellationDate) => $@"SELECT {group} AS grp, SUM(c.category = 'active') AS active, SUM(c.category = 'inactive') AS inactive,
                    SUM(c.category = 'canceled') AS canceled, SUM(c.category = 'none') AS none, COUNT(*) AS total, SUM(c.expired) AS expired
                FROM (SELECT us.user_id, us.role AS role, sh.school_name AS school,
                             CASE WHEN s.user_id IS NULL THEN 'none' WHEN COALESCE(s.act, 0) = 1 THEN 'active' WHEN COALESCE(s.can, 0) = 1 THEN 'canceled' ELSE 'inactive' END AS category,
                             CASE WHEN COALESCE(s.act, 0) = 1 AND COALESCE(s.expired, 0) = 1 THEN 1 ELSE 0 END AS expired
                      FROM user us
                      LEFT JOIN school sh ON sh.school_id = us.school_id
                      LEFT JOIN (SELECT sph.user_id, MAX(sph.{statusColumn} = 'active') AS act, MAX(sph.{statusColumn} LIKE '%cancel%') AS can,
                                        {(includeCancellationDate ? $"MAX(sph.{statusColumn} = 'active' AND (sph.{cancellationColumn} IS NULL OR sph.{cancellationColumn} < NOW()))" : "0")} AS expired
                                 FROM subscription_profile_history sph GROUP BY sph.user_id) s ON s.user_id = us.user_id
                      WHERE us.role_id <> {ClientRoleId} AND (@school IS NULL OR sh.school_name = @school) {UserFilter(context, "us", "sh")}) c
                GROUP BY grp";

            DynamicParameters BuildParameters()
            {
                var parameters = NewParameters(context);
                parameters.Add("@school", schoolName);
                return parameters;
            }

            var expiredTotal = 0;
            SubscriptionStatusRow ToRow(IDictionary<string, object?> row) => new(ToText(row["grp"])!, ToInt(row["active"]), ToInt(row["inactive"]),
                ToInt(row["canceled"]), ToInt(row["none"]), ToInt(row["total"]));

            List<SubscriptionStatusRow> rows;
            try
            {
                rows = Fetch(context, BuildSql(withCancellationDate), BuildParameters(), row =>
                {
                    expiredTotal += ToInt(row["expired"]);
                    return ToRow(row);
                }, 60);
            }
            catch (MySqlException exception) when (withCancellationDate && exception.Number == 1054
                && exception.Message.Contains(cancellationColumn, StringComparison.OrdinalIgnoreCase))
            {
                // The cancellation column does not exist: the query goes on without it (ActiveWithExpiredCancellationDate = null).
                _cancellationColumnMissing = true;
                withCancellationDate = false;
                expiredTotal = 0;
                rows = Fetch(context, BuildSql(false), BuildParameters(), ToRow, 60);
            }

            // Real status values (users with some subscription in each one): informative; if it fails the query is not dropped.
            var originalStatuses = new Dictionary<string, int>();
            try
            {
                originalStatuses = Fetch(context, $@"SELECT COALESCE(sph.{statusColumn}, '(empty)') AS status_value, COUNT(DISTINCT sph.user_id) AS users
                    FROM subscription_profile_history sph
                    JOIN user us ON us.user_id = sph.user_id
                    LEFT JOIN school sh ON sh.school_id = us.school_id
                    WHERE us.role_id <> {ClientRoleId} AND (@school IS NULL OR sh.school_name = @school) {UserFilter(context, "us", "sh")}
                    GROUP BY sph.{statusColumn} ORDER BY 2 DESC, 1", BuildParameters(),
                    row => (Status: ToText(row["status_value"])!, Users: ToInt(row["users"])), 60).ToDictionary(item => item.Status, item => item.Users);
            }
            catch (MySqlException)
            {
                // Informational only.
            }

            var total = new SubscriptionStatusCount(rows.Sum(r => r.Active), rows.Sum(r => r.Inactive), rows.Sum(r => r.Canceled),
                rows.Sum(r => r.WithoutSubscription), withCancellationDate ? expiredTotal : null, originalStatuses);
            return (rows, total);
        }

        /// <summary>
        /// A column name from the configuration goes inside the SQL: only letters, digits and underscore are allowed.
        /// </summary>
        private static string SafeIdentifier(string name) =>
            System.Text.RegularExpressions.Regex.IsMatch(name, "^[A-Za-z_][A-Za-z0-9_]*$")
                ? name
                : throw new InvalidOperationException("Invalid column name in the configuration.");
    }
}
