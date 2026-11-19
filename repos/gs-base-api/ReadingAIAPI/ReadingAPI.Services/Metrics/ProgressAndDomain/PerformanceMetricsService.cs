using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using ReadingAPI.Domain.DTOs;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Domain.Entities.Reading;
using ReadingAPI.ReadingAPI.Infrastructure;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Metrics;

namespace ReadingAPI.ReadingAPI.Services.Metrics.ProgressAndDomain
{
    /// <summary>
    /// Provides reading performance metrics scoped to the requesting user's role.
    /// Uses UserStoryResultsEntity to aggregate average scores per reading attempt
    /// accuracy, fluency, completeness, pronunciation, and words per minute and
    /// units completion percentage based on user quiz results.
    /// </summary>
    public class PerformanceMetricsService : MetricsBaseService<UserStoryResultsEntity>
    {
        private static readonly string[] ReadingLevelCodes = { "k", "1", "2", "3", "4", "5" };

        private readonly ReadingLessonsInfrastructure _readingLessons;

        private readonly ILogger<PerformanceMetricsService> _logger;

        public PerformanceMetricsService(ReadingAIDbContext dbContext, ILogger<PerformanceMetricsService> logger, ReadingLessonsInfrastructure readingLessons)
            : base(dbContext)
        {
            _logger = logger;
            _readingLessons = readingLessons;
        }

        // Set the entity for the query
        protected override IQueryable<UserStoryResultsEntity> GetBaseQuery()
            => _dbContext.UserStoryResults.AsQueryable();

        // Apply the filters for the query
        protected override IQueryable<UserStoryResultsEntity> ApplyFilters(IQueryable<UserStoryResultsEntity> query, UserSessionMetricsRequestDto request, int? regionId,
            int? schoolId, List<int>? studentIds,  bool applyDateFilters)
        {
            if (regionId.HasValue)
                query = query.Where(r => r.RegionId == regionId.Value);

            if (schoolId.HasValue)
                query = query.Where(r => r.SchoolId == schoolId.Value);

            if (studentIds != null)
                query = query.Where(r => studentIds.Contains(r.UserId));

            if (request.UserIds?.Count > 0)
                query = query.Where(r => request.UserIds.Contains(r.UserId));

            if (applyDateFilters)
            {
                // The dates are picked in the configured zone
                if (request.DateFrom.HasValue)
                {
                    var fromUtc = ConvertLocalDateToUtc(request.DateFrom.Value.ToDateTime(TimeOnly.MinValue));
                    query = query.Where(r => r.CreatedAt >= fromUtc);
                }

                if (request.DateTo.HasValue)
                {
                    var toUtc = ConvertLocalDateToUtc(request.DateTo.Value.AddDays(1).ToDateTime(TimeOnly.MinValue));
                    query = query.Where(r => r.CreatedAt < toUtc);
                }
            }

            return query;
        }

        private static IQueryable<UserStoryResultsEntity> ApplyDateRangeFilter(IQueryable<UserStoryResultsEntity> query, DateOnly from, DateOnly to)
        {
            var fromUtc = ConvertLocalDateToUtc(from.ToDateTime(TimeOnly.MinValue));
            var toUtc = ConvertLocalDateToUtc(to.AddDays(1).ToDateTime(TimeOnly.MinValue));

            return query.Where(r => r.CreatedAt >= fromUtc && r.CreatedAt < toUtc);
        }

        /// <summary>
        /// Returns average performance scores grouped by period and split by story language,
        /// scoped to the requesting user's role. The english and spanish variants of a story are
        /// practiced and scored independently, so each language is aggregated on its own.
        /// Groups by month for year view, by day for week and month views.
        /// Defaults to the current calendar week when no DateFrom/DateTo is provided.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin) with optional filters</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<PerformanceMetricsByLanguageResponseDto> GetPerformanceSummary(UserSessionMetricsRequestDto request, MetricsPeriod period, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            try
            {
                // Build role-scoped query for the requesting user
                var query = await BuildScopedQuery(request, userId, isStaff, isDirector, oreId);

                if (query == null)
                    return new PerformanceMetricsByLanguageResponseDto();

                // Apply default date range if no explicit dates were provided
                if (!request.DateFrom.HasValue && !request.DateTo.HasValue)
                {
                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    var (from, to) = GetDefaultDateRange(period, today);
                    query = ApplyDateRangeFilter(query, from, to);
                }

                // "created_at" is stored in UTC, so it is shifted before grouping, otherwise a
                // reading done late in the evening lands on the next day
                var offsetMinutes = GetReportsUtcOffsetMinutes();

                // Year view, group by language and month
                if (period == MetricsPeriod.Year)
                {
                    var monthlyRows = await query
                        .GroupBy(r => new { r.Language, r.CreatedAt.AddMinutes(offsetMinutes).Year, r.CreatedAt.AddMinutes(offsetMinutes).Month })
                        .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                        .Select(g => new
                        {
                            g.Key.Language,
                            g.Key.Year,
                            g.Key.Month,
                            AvgAccuracyScore = Math.Round(g.Average(row => row.AccuracyScore), 1),
                            AvgFluencyScore = Math.Round(g.Average(row => row.FluencyScore), 1),
                            AvgCompletenessScore = Math.Round(g.Average(row => row.CompletenessScore), 1),
                            AvgPronunciationScore = Math.Round(g.Average(row => row.PronunciationScore), 1),
                            AvgWordsPerMinute = (int)Math.Round(g.Average(row => row.WordsPerMinute)),
                        })
                        .ToListAsync();

                    var (englishMonthly, spanishMonthly) = SplitByLanguage(monthlyRows.Select(row => (row.Language, new PerformanceMetricsResponseDto
                    {
                        Year = row.Year,
                        Month = row.Month,
                        AvgAccuracyScore = row.AvgAccuracyScore,
                        AvgFluencyScore = row.AvgFluencyScore,
                        AvgCompletenessScore = row.AvgCompletenessScore,
                        AvgPronunciationScore = row.AvgPronunciationScore,
                        AvgWordsPerMinute = row.AvgWordsPerMinute,
                    })));

                    return new PerformanceMetricsByLanguageResponseDto { English = englishMonthly, Spanish = spanishMonthly };
                }

                // Week and month views, group by language and day
                var dailyRows = await query
                    .GroupBy(r => new { r.Language, r.CreatedAt.AddMinutes(offsetMinutes).Year, r.CreatedAt.AddMinutes(offsetMinutes).Month, r.CreatedAt.AddMinutes(offsetMinutes).Day })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month).ThenBy(g => g.Key.Day)
                    .Select(g => new
                    {
                        g.Key.Language,
                        g.Key.Year,
                        g.Key.Month,
                        g.Key.Day,
                        AvgAccuracyScore = Math.Round(g.Average(r => r.AccuracyScore), 1),
                        AvgFluencyScore = Math.Round(g.Average(r => r.FluencyScore), 1),
                        AvgCompletenessScore = Math.Round(g.Average(r => r.CompletenessScore), 1),
                        AvgPronunciationScore = Math.Round(g.Average(r => r.PronunciationScore), 1),
                        AvgWordsPerMinute = (int)Math.Round(g.Average(r => r.WordsPerMinute)),
                    })
                    .ToListAsync();

                var (englishDaily, spanishDaily) = SplitByLanguage(dailyRows.Select(row => (row.Language, new PerformanceMetricsResponseDto
                {
                    Date = new DateOnly(row.Year, row.Month, row.Day),
                    AvgAccuracyScore = row.AvgAccuracyScore,
                    AvgFluencyScore = row.AvgFluencyScore,
                    AvgCompletenessScore = row.AvgCompletenessScore,
                    AvgPronunciationScore = row.AvgPronunciationScore,
                    AvgWordsPerMinute = row.AvgWordsPerMinute,
                })));

                return new PerformanceMetricsByLanguageResponseDto { English = englishDaily, Spanish = spanishDaily };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching performance summary for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Returns the reading lesson completion rate per level, scoped to the requesting user's role.
        /// Queries usqui_user_quiz_results table and references each lesson_id against the reading catalog lessons,
        /// keeping only results where the quiz level matches the catalog LevelCode.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin) with optional filters</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<List<UnitMetricsResponseDto>> GetUnitsCompletion(UserSessionMetricsRequestDto request, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            try
            {
                var scope = await GetQueryScope(request, userId, isStaff, isDirector, oreId);

                if (scope == null)
                {
                    return ReadingLevelCodes.Select(level => new UnitMetricsResponseDto { LevelCode = level }).ToList();
                }

                // Get Reading Lessons from Athenas API
                var readingLessons = await Task.Run(() => AthenasFeignAPI.GetReadingLessons());

                if (readingLessons.Count == 0)
                {
                    return ReadingLevelCodes.Select(level => new UnitMetricsResponseDto { LevelCode = level }).ToList();
                }

                var lessonLookup = readingLessons
                    .GroupBy(lesson => lesson.LessonId)
                    .ToDictionary(g => g.Key, g => g.First().LevelCode);

                // Total lessons per level from reading catalog
                var totalLessonsByLevel = readingLessons
                    .GroupBy(lesson => lesson.LevelCode)
                    .ToDictionary(g => g.Key, g => g.Select(lesson => lesson.LessonId).Distinct().Count());

                var readingLessonIds = readingLessons.Select(lesson => lesson.LessonId).Distinct().ToList();

                var quizParams = new DynamicParameters();
                quizParams.Add("LessonIds", readingLessonIds);

                // Apply filters for Dapper query
                var userScopeCondition = BuildUserScopeCondition(scope.Value, request, quizParams);

                var webAPIConnectionString = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"];
                using var webAPIConnection = new MySqlConnection(webAPIConnectionString);

                var whereCondition = "lesson_id IN @LessonIds";

                if (userScopeCondition != null)
                {
                    whereCondition += $" AND {userScopeCondition}";
                }

                var results = await webAPIConnection.QueryAsync(
                    $@"SELECT DISTINCT
                            user_id AS UserId,
                            lesson_id AS LessonId,
                            level AS LevelCode
                       FROM usqui_user_quiz_results
                       WHERE {whereCondition}",
                    quizParams);

                var quizResults = results.Select(result => (UserId: (int)result.UserId, LessonId: (int)result.LessonId, LevelCode: (string)result.LevelCode)).ToList();

                if (quizResults.Count == 0)
                {
                    return ReadingLevelCodes.Select(level => new UnitMetricsResponseDto { LevelCode = level }).ToList();
                }

                // Keep only rows where quiz level matches the catalog LevelCode for that lesson_id
                var validResults = quizResults
                    .Where(result => lessonLookup.TryGetValue(result.LessonId, out var catalogLevel) && catalogLevel == result.LevelCode)
                    .Select(result => new { result.UserId, result.LevelCode, result.LessonId })
                    .ToList();

                if (validResults.Count == 0)
                {
                    return ReadingLevelCodes.Select(level => new UnitMetricsResponseDto { LevelCode = level }).ToList();
                }

                var studentsIds = validResults.Select(result => result.UserId).Distinct().ToList();
                var isSingleStudent = studentsIds.Count == 1;

                var studentCompletions = validResults
                    .GroupBy(result => result.UserId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.GroupBy(result => result.LevelCode)
                              .ToDictionary(levelGroup => levelGroup.Key, levelGroup => levelGroup.Select(result => result.LessonId).Distinct().Count())
                    );


                // Single student, return individual completion rate per level
                if (isSingleStudent)
                {
                    var studentData = studentCompletions.GetValueOrDefault(studentsIds[0]) ?? new Dictionary<string, int>();

                    return ReadingLevelCodes.Select(level => 
                    {
                        totalLessonsByLevel.TryGetValue(level, out var total);
                        studentData.TryGetValue(level, out var completed);

                        var completionRate = total > 0 ? Math.Round((double)completed / total * 100, 1) : 0;

                        return new UnitMetricsResponseDto
                        {
                            LevelCode = level,
                            CompletionRate = completionRate
                        };
                    }).ToList();
                }

                // Multiple students, average completion rate per level
                return ReadingLevelCodes.Select(level =>
                {
                    if (!totalLessonsByLevel.TryGetValue(level, out var total) || total == 0)
                    {
                        return new UnitMetricsResponseDto
                        {
                            LevelCode = level
                        };
                    }

                    var avgRate = studentsIds
                        .Select(studentId =>
                        {
                            if (!studentCompletions.TryGetValue(studentId, out var levelMap))
                            {
                                return 0.0;
                            }

                            levelMap.TryGetValue(level, out var completed);

                            return (double)completed / total * 100;
                        })
                        .Average();

                    return new UnitMetricsResponseDto
                    {
                        LevelCode = level,
                        CompletionRate = Math.Round(avgRate, 1)
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching units completion");
                throw;
            }
        }

        /// <summary>
        /// Returns the reading lessons evaluations taken by the students the requesting user
        /// is allowed to see.
        /// The results come from "usqui_user_quiz_results" filtered by the Athenas lessons that
        /// carry the "R" or "LLI" standards from kindergarten to fifth grade.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="fromPage">Zero based page index</param>
        /// <param name="limit">Results per page</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin)</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<PaginationModel<ReadingLessonsResultDto>> GetReadingLessonsResults(ReadingLessonsResultsRequestDto request, MetricsPeriod period, int fromPage, int limit, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            var pageIndex = Math.Max(0, fromPage);
            var pageSize = Math.Max(1, limit);

            try
            {
                var scope = await GetQueryScope(request, userId, isStaff, isDirector, oreId);

                if (scope == null)
                    return new PaginationModel<ReadingLessonsResultDto>(0, pageSize, new List<ReadingLessonsResultDto>());

                var lessonIds = await _readingLessons.GetReadingLessonIds();

                if (lessonIds.Count == 0)
                    return new PaginationModel<ReadingLessonsResultDto>(0, pageSize, new List<ReadingLessonsResultDto>());

                var parameters = new DynamicParameters();
                parameters.Add("LessonIds", lessonIds);
                parameters.Add("Subjects", ReadingLessonsInfrastructure.ReadingSubjectCodes);
                parameters.Add("Limit", pageSize);
                parameters.Add("Offset", pageIndex * pageSize);

                var conditions = new List<string>
                {
                    "res.lesson_id IN @LessonIds",
                    "LOWER(res.subject) IN @Subjects",
                };

                var userScopeCondition = BuildUserScopeCondition(scope.Value, request, parameters, "res.user_id");

                if (userScopeCondition != null)
                    conditions.Add(userScopeCondition);

                AddDateRangeConditions(request, period, conditions, parameters);

                AddStudentNameConditions(request, conditions, parameters);

                var whereClause = string.Join(" AND ", conditions);

                var sql = $@"
                    SELECT COUNT(*)
                    FROM usqui_user_quiz_results res
                    LEFT JOIN gs_users_db_prod.upd_user_full_names nam ON nam.user_id = res.user_id
                    LEFT JOIN gs_users_db_prod.ul_user_accounts acc ON acc.id = res.user_id
                    WHERE {whereClause};

                    SELECT res.id                                                AS UserQuizResultId,
                           res.user_id                                           AS UserId,
                           nam.given_name                                        AS GivenName,
                           nam.surname                                           AS Surname,
                           acc.username                                          AS Username,
                           res.created_at                                        AS TakenAt,
                           CAST(res.total_percent AS DECIMAL(6,2))               AS TotalPercent,
                           res.lesson_id                                         AS LessonId,
                           res.subject                                           AS Subject,
                           res.level                                             AS LevelCode,
                           res.is_exam                                           AS ExamType
                    FROM usqui_user_quiz_results res
                    LEFT JOIN gs_users_db_prod.upd_user_full_names nam ON nam.user_id = res.user_id
                    LEFT JOIN gs_users_db_prod.ul_user_accounts acc ON acc.id = res.user_id
                    WHERE {whereClause}
                    ORDER BY res.created_at DESC, res.id DESC
                    LIMIT @Limit OFFSET @Offset;";

                var webAPIConnectionString = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"];
                using var webAPIConnection = new MySqlConnection(webAPIConnectionString);

                using var reader = await webAPIConnection.QueryMultipleAsync(sql, parameters);

                var total = await reader.ReadSingleAsync<int>();
                var items = (await reader.ReadAsync<ReadingLessonsResultDto>()).ToList();

                var lessonTitles = await _readingLessons.GetReadingLessonTitles();

                foreach (var item in items)
                {
                    if (lessonTitles.TryGetValue(item.LessonId, out var title))
                        item.LessonTitle = title;
                }

                return new PaginationModel<ReadingLessonsResultDto>(total, pageSize, items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lessons results for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Returns the same reading lessons results already grouped by the date they were taken,
        /// intended for the reading comprehension chart
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin)</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<List<ReadingLessonsSeriesDto>> GetReadingLessonsSeries(UserSessionMetricsRequestDto request, MetricsPeriod period, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            try
            {
                var scope = await GetQueryScope(request, userId, isStaff, isDirector, oreId);

                if (scope == null)
                    return new List<ReadingLessonsSeriesDto>();

                var lessonIds = await _readingLessons.GetReadingLessonIds();

                if (lessonIds.Count == 0)
                    return new List<ReadingLessonsSeriesDto>();

                var parameters = new DynamicParameters();

                parameters.Add("LessonIds", lessonIds);
                parameters.Add("Subjects", ReadingLessonsInfrastructure.ReadingSubjectCodes);

                var conditions = new List<string>
                {
                    "res.lesson_id IN @LessonIds",
                    "LOWER(res.subject) IN @Subjects",
                };

                var userScopeCondition = BuildUserScopeCondition(scope.Value, request, parameters, "res.user_id");

                if (userScopeCondition != null)
                    conditions.Add(userScopeCondition);

                AddDateRangeConditions(request, period, conditions, parameters);

                var whereClause = string.Join(" AND ", conditions);

                var groupByMonth = period == MetricsPeriod.Year;

                var localCreatedAt = BuildCreatedAtInLocalTimeSql();

                var dayColumn = groupByMonth ? "1" : $"DAY({localCreatedAt})";

                var sql = $@"
                    SELECT YEAR({localCreatedAt})                       AS Year,
                           MONTH({localCreatedAt})                      AS Month,
                           {dayColumn}                                  AS Day,
                           CAST(AVG(res.total_percent) AS DECIMAL(6,2)) AS AvgPercent,
                           COUNT(*)                                     AS TotalEvaluations
                    FROM usqui_user_quiz_results res
                    WHERE {whereClause}
                    GROUP BY YEAR({localCreatedAt}), MONTH({localCreatedAt}){(groupByMonth ? "" : $", DAY({localCreatedAt})")}
                    ORDER BY Year, Month, Day;";

                var webAPIConnectionString = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"];
                using var webAPIConnection = new MySqlConnection(webAPIConnectionString);

                var rows = await webAPIConnection.QueryAsync<ReadingLessonsSeriesRow>(sql, parameters);

                return rows.Select(row => new ReadingLessonsSeriesDto
                {
                    Date = groupByMonth ? null : new DateOnly(row.Year, row.Month, row.Day),
                    Month = row.Month,
                    AvgPercent = row.AvgPercent,
                    TotalEvaluations = row.TotalEvaluations,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lessons results series for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Builds the expression that reads "created_at" in the configured time zone.
        /// </summary>
        private string BuildCreatedAtInLocalTimeSql()
        {
            var totalMinutes = GetReportsUtcOffsetMinutes();

            if (totalMinutes == 0)
                return "res.created_at";

            var sign = totalMinutes < 0 ? "-" : "+";
            var absolute = Math.Abs(totalMinutes);

            var offset = $"{sign}{absolute / 60:D2}:{absolute % 60:D2}";

            return $"CONVERT_TZ(res.created_at, '+00:00', '{offset}')";
        }

        /// <summary>
        /// Adds the optional student name filters to the reading lessons results query
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="conditions">The conditions the filters are added to</param>
        /// <param name="parameters">The parameters the filters are added to</param>
        private static void AddStudentNameConditions(ReadingLessonsResultsRequestDto request, List<string> conditions, DynamicParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(request.GivenName))
            {
                conditions.Add("nam.given_name LIKE @GivenName");
                parameters.Add("GivenName", $"%{request.GivenName.Trim()}%");
            }

            if (!string.IsNullOrWhiteSpace(request.Surname))
            {
                conditions.Add("nam.surname LIKE @Surname");
                parameters.Add("Surname", $"%{request.Surname.Trim()}%");
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                conditions.Add("acc.username LIKE @Username");
                parameters.Add("Username", $"%{request.Username.Trim()}%");
            }
        }

        /// <summary>
        /// Adds the date range conditions to the reading lessons queries
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="conditions">The conditions the range is added to</param>
        /// <param name="parameters">The parameters the range is added to</param>
        private void AddDateRangeConditions(UserSessionMetricsRequestDto request, MetricsPeriod period, List<string> conditions, DynamicParameters parameters)
        {
            var (defaultFrom, defaultTo) = GetDefaultDateRange(period, DateOnly.FromDateTime(DateTime.UtcNow));

            var from = request.DateFrom ?? defaultFrom;
            var to = request.DateTo ?? defaultTo;

            var fromUtc = ConvertLocalDateToUtc(from.ToDateTime(TimeOnly.MinValue));
            var toUtc = ConvertLocalDateToUtc(to.AddDays(1).ToDateTime(TimeOnly.MinValue));

            conditions.Add("res.created_at >= @DateFrom");
            parameters.Add("DateFrom", fromUtc);

            conditions.Add("res.created_at < @DateTo");
            parameters.Add("DateTo", toUtc);
        }

        /// <summary>
        /// Row returned by the reading lessons series query
        /// </summary>
        private sealed class ReadingLessonsSeriesRow
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
            public decimal AvgPercent { get; set; }
            public int TotalEvaluations { get; set; }
        }
    }
}
