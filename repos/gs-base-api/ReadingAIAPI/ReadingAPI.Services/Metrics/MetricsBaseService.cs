using Dapper;
using MySqlConnector;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Metrics
{
    /// <summary>
    /// Abstract base class for all metrics services.
    /// Provides role-based query scoping so each user role only sees data they are authorized to access
    /// admin-ore is scoped to their region, admin can query across all schools and regions,
    /// director is scoped to their school, teachers and client accounts (homeschooling)
    /// are scoped to the students they are directly responsible for.
    /// Also provides shared date range utilities for week, month, and year periods.
    /// </summary>
    public abstract class MetricsBaseService<TEntity> where TEntity : class
    {
        protected const string SpanishLanguageCode = "es";

        /// <summary>
        /// PR time (UTC-4), used when no time zone is configured.
        /// </summary>
        private const int DefaultUtcOffsetMinutes = -240;

        protected readonly ReadingAIDbContext _dbContext;

        protected MetricsBaseService(ReadingAIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the base query for the entity this service is being used
        /// </summary>
        protected abstract IQueryable<TEntity> GetBaseQuery();

        /// <summary>
        /// Applies role-based scope and optional filters to the query.
        /// </summary>
        /// <param name="query">The base query to filter</param>
        /// <param name="request">The user request containing optional filters (UserIds, DateFrom, DateTo, SchoolId, RegionId)</param>
        /// <param name="regionId">Region scope from the user's role</param>
        /// <param name="schoolId">School scope from the user's role</param>
        /// <param name="studentIds">Student scope from the teacher's connections</param>
        /// <param name="applyDateFilters">Date filters for the results</param>
        protected abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, UserSessionMetricsRequestDto request, int? regionId, int? schoolId,
            List<int>? studentIds, bool applyDateFilters);

        /// <summary>
        /// Builds the base session query scoped to the requesting user's role
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin) with optional filters</param>
        /// <param name="isDirector">Scoped to the director's school.</param>
        /// <param name="oreId">The region Id</param>
        protected async Task<IQueryable<TEntity>?> BuildScopedQuery(UserSessionMetricsRequestDto request, string userId, bool isStaff, bool isDirector, string? oreId, bool applyDateFilters = true)
        {
            var scope = await GetQueryScope(request, userId, isStaff, isDirector, oreId);
            if (scope == null)
                return null;

            return ApplyFilters(GetBaseQuery(), request, scope.Value.RegionId, scope.Value.SchoolId, scope.Value.StudentIds, applyDateFilters);
        }

        /// <summary>
        /// Resolves the requesting user's role into a scope tuple, can be used for EF and Dapper queries.
        /// </summary>
        protected async Task<(int? RegionId, int? SchoolId, List<int>? StudentIds)?> GetQueryScope(UserSessionMetricsRequestDto request, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            // Validate oreId first for admin-ore users, since they are also staff.
            if (!string.IsNullOrWhiteSpace(oreId))
            {
                if (!int.TryParse(oreId, out int oreRegionId))
                    return null;
                return (oreRegionId, request.SchoolId, null);
            }

            // Admin users can filter data across all schools or regions
            if (isStaff)
            {
                if (request.UserIds?.Count > 0)
                    return (null, null, request.UserIds);
                return (request.RegionId, request.SchoolId, null);
            }

            // Director users can only filter within their school
            if (isDirector)
            {
                var schoolId = await GetDirectorSchoolId(int.Parse(userId));
                return (null, schoolId, null);
            }

            // For teachers and client accounts (homeschooling) get only the sessions
            // of the students they are directly responsible for
            var studentIds = await GetConnectedStudentIds(int.Parse(userId));
            if (studentIds.Count == 0)
                return null;

            return (null, null, studentIds);
        }

        /// <summary>
        /// Builds the SQL condition that limits "user_id" to the students the requesting role is
        /// allowed to see, and registers the parameters it needs.
        /// </summary>
        /// <param name="scope">The scope resolved by <see cref="GetQueryScope"/></param>
        /// <param name="request">The user request</param>
        /// <param name="parameters">The Dapper parameters the condition is added to</param>
        /// <param name="userColumn">Column holding the student id, qualified when the query joins other tables</param>
        protected static string? BuildUserScopeCondition((int? RegionId, int? SchoolId, List<int>? StudentIds) scope, UserSessionMetricsRequestDto request, DynamicParameters parameters, string userColumn = "user_id")
        {
            var conditions = new List<string>();

            // The scope granted by the role always applies.
            if (scope.StudentIds != null)
            {
                parameters.Add("ScopeStudentIds", scope.StudentIds);
                conditions.Add($"{userColumn} IN @ScopeStudentIds");
            }
            else if (scope.SchoolId.HasValue)
            {
                parameters.Add("SchoolId", scope.SchoolId.Value);
                conditions.Add($"{userColumn} IN (SELECT user_id FROM gs_users_db_prod.upd_user_schools WHERE school_id = @SchoolId)");
            }
            else if (scope.RegionId.HasValue)
            {
                parameters.Add("OreId", scope.RegionId.Value);
                conditions.Add($@"{userColumn} IN (
                    SELECT u.user_id FROM gs_users_db_prod.upd_user_schools u
                    INNER JOIN gs_schools_api_db_prod.sch_entities e ON e.id = u.school_id
                    INNER JOIN gs_schools_api_db_prod.sch_addresses ad ON ad.entity_id = e.id
                    WHERE ad.region_id = @OreId)");
            }

            /// Both conditions are joined with AND. The role scope decides which
            /// students the user is allowed to see, the requested ids only pick among those.
            /// Asking for a student outside the scope returns nothing.`
            if (request.UserIds?.Count > 0)
            {
                parameters.Add("RequestedUserIds", request.UserIds);
                conditions.Add($"{userColumn} IN @RequestedUserIds");
            }

            return conditions.Count == 0 ? null : string.Join(" AND ", conditions);
        }

        /// <summary>
        /// Returns whether the given language code belongs to the spanish variant of a story.
        /// </summary>
        protected static bool IsSpanish(string? language)
            => string.Equals(language, SpanishLanguageCode, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Splits already aggregated rows into their english and spanish lists, preserving the original order.
        /// The english and spanish variants of a story are read and scored independently, so every
        /// metric built from story results is reported per language.
        /// </summary>
        protected static (List<TItem> English, List<TItem> Spanish) SplitByLanguage<TItem>(IEnumerable<(string Language, TItem Item)> rows)
        {
            var english = new List<TItem>();
            var spanish = new List<TItem>();

            foreach (var (language, item) in rows)
            {
                if (IsSpanish(language))
                    spanish.Add(item);
                else
                    english.Add(item);
            }

            return (english, spanish);
        }

        /// <summary>
        /// Returns the default date range based on the selected period
        /// </summary>
        protected static (DateOnly From, DateOnly To) GetDefaultDateRange(MetricsPeriod period, DateOnly today)
        {
            return period switch
            {
                MetricsPeriod.Month => (
                    new DateOnly(today.Year, today.Month, 1),
                    new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month))
                ),
                MetricsPeriod.Year => (
                    new DateOnly(today.Year, 1, 1),
                    new DateOnly(today.Year, 12, 31)
                ),
                _ => GetCurrentWeekRange(today),
            };
        }

        /// <summary>
        /// Minutes between the configured time zone and UTC, so the metrics are grouped
        /// by the day the school sees. Falls back to Puerto Rico time when the zone is missing
        /// </summary>
        protected static int GetReportsUtcOffsetMinutes()
        {
            var zoneId = AppConfiguration.Configuration["AppSettings:default_time_zone_for_exams"];

            if (string.IsNullOrWhiteSpace(zoneId))
                return DefaultUtcOffsetMinutes;

            try
            {
                var zone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
                return (int)zone.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
            }
            catch (Exception)
            {
                return DefaultUtcOffsetMinutes;
            }
        }

        /// <summary>
        /// Converts an instant expressed in the configured zone into UTC, so the range the
        /// user picked is compared against the UTC values stored in "created_at"
        /// </summary>
        /// <param name="localDateTime">The instant as the configured zone sees it</param>
        protected static DateTime ConvertLocalDateToUtc(DateTime localDateTime)
        {
            var offsetMinutes = GetReportsUtcOffsetMinutes();

            if (offsetMinutes == 0)
                return localDateTime;

            // The offset is negative west of UTC, so reaching UTC means adding it back
            return localDateTime.AddMinutes(-offsetMinutes);
        }

        /// <summary>
        /// Returns the current week date range (Monday to Sunday)
        /// </summary>
        private static (DateOnly From, DateOnly To) GetCurrentWeekRange(DateOnly today)
        {
            int daysToMonday = today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1;
            var monday = today.AddDays(-daysToMonday);
            return (monday, monday.AddDays(6));
        }

        /// <summary>
        /// Returns the school Id associated with the given director.
        /// </summary>
        private static async Task<int> GetDirectorSchoolId(int directorId)
        {
            var connectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"];
            using var connection = new MySqlConnection(connectionString);
            var schoolId = await connection.QueryFirstOrDefaultAsync<int?>(
                @"SELECT school_id
                  FROM gs_users_db_prod.upd_user_schools
                  WHERE user_id = @DirectorId
                  LIMIT 1",
                new { DirectorId = directorId });

            if (schoolId == null)
                throw new InvalidOperationException($"User {directorId} is not associated with any school.");

            return schoolId.Value;
        }

        /// <summary>
        /// Returns the Ids of the students the given user is directly responsible for.
        /// Teachers reach their students through "urel_profile_connections", while client
        /// accounts (used for homeschooling) own their children through
        /// "urel_associated_accounts", so both sources are combined here.
        /// </summary>
        /// <param name="userId">The teacher or client account Id</param>
        private static async Task<List<int>> GetConnectedStudentIds(int userId)
        {
            var connectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"];
            using var connection = new MySqlConnection(connectionString);
            var ids = await connection.QueryAsync<int>(
                @"SELECT pro.user_id_b
                  FROM gs_users_db_prod.urel_profile_connections pro
                  INNER JOIN gs_users_db_prod.ur_user_roles ro ON ro.user_id = pro.user_id_b
                  WHERE pro.user_id_a = @UserId
                    AND pro.user_a_alpha = 1
                    AND ro.role_id = 3
                  UNION
                  SELECT acc.profile_user_account
                  FROM gs_users_db_prod.urel_associated_accounts acc
                  INNER JOIN gs_users_db_prod.ur_user_roles ro ON ro.user_id = acc.profile_user_account
                  WHERE acc.main_user_account = @UserId
                    AND acc.available = 1
                    AND ro.role_id = 3",
                new { UserId = userId });

            return ids.ToList();
        }
    }
}
