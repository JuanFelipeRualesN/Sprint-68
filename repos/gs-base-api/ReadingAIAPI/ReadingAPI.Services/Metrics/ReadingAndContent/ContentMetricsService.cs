using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Domain.Entities.Reading;
using ReadingAPI.ReadingAPI.Infrastructure;
using ReadingAPI.ReadingAPI.Services.Metrics;

namespace ReadingAPI.ReadingAPI.Services.Metrics.ReadingAndContent
{
    /// <summary>
    /// Provides reading content metrics scoped to the requesting user's role.
    /// Uses UserStoryResultsEntity to aggregate data across categories,
    /// reading time, pages read, total completed texts, and completion rate.
    /// </summary>
    public class ContentMetricsService : MetricsBaseService<UserStoryResultsEntity>
    {
        private readonly ILogger<ContentMetricsService> _logger;

        public ContentMetricsService(ReadingAIDbContext dbContext, ILogger<ContentMetricsService> logger)
            : base(dbContext)
        {
            _logger = logger;
        }

        // Set the entity for the query
        protected override IQueryable<UserStoryResultsEntity> GetBaseQuery()
            => _dbContext.UserStoryResults.AsQueryable();

        // Apply the filters for the query
        protected override IQueryable<UserStoryResultsEntity> ApplyFilters(IQueryable<UserStoryResultsEntity> query, UserSessionMetricsRequestDto request, int? regionId,
            int? schoolId, List<int>? studentIds, bool applyDateFilters)
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
                // The dates are picked in the configured zone, so the range is moved to UTC
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

            return query.Where(row => row.CreatedAt >= fromUtc && row.CreatedAt < toUtc);
        }

        /// <summary>
        /// Returns reading content metrics for the selected period, split by story language and
        /// scoped to the requesting user's role. The english and spanish variants of a story are read
        /// and tracked independently, so every metric is aggregated per language.
        /// Includes readings per category, average reading time, pages/slides read, total texts completed, and completion rate.
        /// Defaults to the current calendar week when no DateFrom/DateTo is provided.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin) with optional filters</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<ContentMetricsByLanguageResponseDto> GetContentSummary(UserSessionMetricsRequestDto request, MetricsPeriod period, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            try
            {
                // Build role-scoped query without date filters so scopedUserIds can be reused
                // for UserStoryProgress queries that need the full user scope regardless of date
                var scopedNoDate = await BuildScopedQuery(request, userId, isStaff, isDirector, oreId, applyDateFilters: false);

                if (scopedNoDate == null)
                {
                    return new ContentMetricsByLanguageResponseDto();
                }

                // Get the effective date range, use request values or fall back to the period default
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var (defaultDateFrom, defaultDateTo) = GetDefaultDateRange(period, today);
                var dateFrom = request.DateFrom ?? defaultDateFrom;
                var dateTo = request.DateTo ?? defaultDateTo;

                // Apply date filters on the scoped query to get the period results
                IQueryable<UserStoryResultsEntity> scopedResults = scopedNoDate;
                if (request.DateFrom.HasValue)
                {
                    var requestedFromUtc = ConvertLocalDateToUtc(request.DateFrom.Value.ToDateTime(TimeOnly.MinValue));
                    scopedResults = scopedResults.Where(row => row.CreatedAt >= requestedFromUtc);
                }

                if (request.DateTo.HasValue)
                {
                    var requestedToUtc = ConvertLocalDateToUtc(request.DateTo.Value.AddDays(1).ToDateTime(TimeOnly.MinValue));
                    scopedResults = scopedResults.Where(row => row.CreatedAt < requestedToUtc);
                }

                // Apply default values
                if (!request.DateFrom.HasValue && !request.DateTo.HasValue)
                    scopedResults = ApplyDateRangeFilter(scopedResults, dateFrom, dateTo);

                var scopedUserIds = scopedNoDate.Select(row => row.UserId).Distinct();

                // Both the range and the grouping run in the configured zone, so the chart shows the day the student saw instead of UTC
                var offsetMinutes = GetReportsUtcOffsetMinutes();
                var fromDate = ConvertLocalDateToUtc(dateFrom.ToDateTime(TimeOnly.MinValue));
                var toDate = ConvertLocalDateToUtc(dateTo.AddDays(1).ToDateTime(TimeOnly.MinValue));

                // One reading is a story read by a student, no matter how many slides it has
                // or how many times each slide was attempted
                var storyReadings = (
                    from result in scopedResults
                    join story in _dbContext.Stories on result.StoryId equals story.Id
                    join category in _dbContext.StoryCategories on story.CategoryId equals category.Id
                    select new { result.Language, result.UserId, StoryId = story.Id, CategoryId = category.Id, CategoryName = category.Name })
                    .Distinct();

                // Total readings per category and language for the time period
                var categoryRows = await storyReadings
                    .GroupBy(x => new { x.Language, x.CategoryId, x.CategoryName })
                    .OrderBy(g => g.Key.CategoryName)
                    .Select(g => new
                    {
                        g.Key.Language,
                        g.Key.CategoryId,
                        g.Key.CategoryName,
                        TotalReadings = g.Count(),
                    })
                    .ToListAsync();

                var (englishCategories, spanishCategories) = SplitByLanguage(categoryRows.Select(row => (row.Language, new ReadingCategoryResponseDto
                {
                    CategoryId = row.CategoryId,
                    CategoryName = row.CategoryName,
                    TotalReadings = row.TotalReadings,
                })));

                // Keep only the latest result for duplicated slides attempts,
                // the same slide read in both languages counts once per language
                var latestSlideIds = scopedResults
                    .GroupBy(row => new { row.UserId, row.StoryId, row.SlideId, row.Language })
                    .Select(g => g.Max(row => row.Id));

                // Filter results where the story is completed in the language it was read
                var completedStoryRows = await (
                    from result in scopedResults.Where(r => latestSlideIds.Contains(r.Id) && r.WordsPerMinute > 0)
                    join progress in _dbContext.UserStoryProgress
                        on new { result.UserId, result.StoryId, result.Language } equals new { progress.UserId, progress.StoryId, progress.Language }
                    where progress.StatusId == StoryProgressStatus.Completed
                    group result by new { result.UserId, result.StoryId, result.Language } into g
                    select new
                    {
                        g.Key.Language,
                        // Latest slide CreatedAt value used as the story completion date for period grouping
                        CompletionDate = g.Max(row => row.CreatedAt),
                        // Estimated reading time in seconds summed across all slides
                        TotalTime = g.Sum(row => (double)(row.EvaluatedText.Length - row.EvaluatedText.Replace(" ", "").Length + 1) / row.WordsPerMinute * 60)
                    })
                    .ToListAsync();

                var (englishCompleted, spanishCompleted) = SplitByLanguage(
                    completedStoryRows.Select(row => (row.Language, new CompletedStory(row.CompletionDate, row.TotalTime))));

                // Pages read per month (year view) or per day (week/month view)
                // Each row represents one slide attempt
                List<ReadingPagesResponseDto> englishPages;
                List<ReadingPagesResponseDto> spanishPages;

                if (period == MetricsPeriod.Year)
                {
                    var monthlyPageRows = await scopedResults
                        .GroupBy(row => new { row.Language, row.CreatedAt.AddMinutes(offsetMinutes).Year, row.CreatedAt.AddMinutes(offsetMinutes).Month })
                        .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                        .Select(g => new
                        {
                            g.Key.Language,
                            g.Key.Year,
                            g.Key.Month,
                            TotalPages = g.Count(),
                        })
                        .ToListAsync();

                    (englishPages, spanishPages) = SplitByLanguage(monthlyPageRows.Select(row => (row.Language, new ReadingPagesResponseDto
                    {
                        Year = row.Year,
                        Month = row.Month,
                        TotalPages = row.TotalPages,
                    })));
                }
                else
                {
                    var dailyPageRows = await scopedResults
                        .GroupBy(row => new { row.Language, row.CreatedAt.AddMinutes(offsetMinutes).Year, row.CreatedAt.AddMinutes(offsetMinutes).Month, row.CreatedAt.AddMinutes(offsetMinutes).Day })
                        .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month).ThenBy(g => g.Key.Day)
                        .Select(g => new
                        {
                            g.Key.Language,
                            g.Key.Year,
                            g.Key.Month,
                            g.Key.Day,
                            TotalPages = g.Count(),
                        })
                        .ToListAsync();

                    (englishPages, spanishPages) = SplitByLanguage(dailyPageRows.Select(row => (row.Language, new ReadingPagesResponseDto
                    {
                        Date = new DateOnly(row.Year, row.Month, row.Day),
                        TotalPages = row.TotalPages,
                    })));
                }

                // Single query for both total texts read and completion rate
                // load all stories with a status change in the time period
                var allProgress = await _dbContext.UserStoryProgress
                    .Where(storyProgress => scopedUserIds.Contains(storyProgress.UserId)
                             && storyProgress.UpdatedAt >= fromDate && storyProgress.UpdatedAt < toDate)
                    .ToListAsync();

                var (englishProgress, spanishProgress) = SplitByLanguage(
                    allProgress.Select(storyProgress => (storyProgress.Language, storyProgress)));

                return new ContentMetricsByLanguageResponseDto
                {
                    English = BuildContentSummary(englishCategories, englishCompleted, englishPages, englishProgress, period, offsetMinutes),
                    Spanish = BuildContentSummary(spanishCategories, spanishCompleted, spanishPages, spanishProgress, period, offsetMinutes),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching content summary for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// A story completed by a student in a given language, used to average reading time per period.
        /// </summary>
        /// <param name="CompletionDate">Date of the latest slide read for that story</param>
        /// <param name="TotalTime">Estimated reading time in seconds summed across all slides</param>
        private sealed record CompletedStory(DateTime CompletionDate, double TotalTime);

        /// <summary>
        /// Assembles the content metrics of a single language.
        /// </summary>
        private static ContentMetricsResponseDto BuildContentSummary(List<ReadingCategoryResponseDto> categories, List<CompletedStory> completedStories,
            List<ReadingPagesResponseDto> pagesByPeriod, List<UserStoryProgressEntity> storyProgress, MetricsPeriod period, int offsetMinutes)
        {
            // stories marked completed with UpdatedAt in the period
            var totalCompleted = storyProgress.Count(progress => progress.StatusId == StoryProgressStatus.Completed);

            // all stories with status activity this time period
            var totalActive = storyProgress.Count;

            return new ContentMetricsResponseDto
            {
                AvgTimeByPeriod = BuildAvgTimeByPeriod(completedStories, period, offsetMinutes),
                Categories = categories,
                PagesByPeriod = pagesByPeriod,
                TotalTextsRead = totalCompleted,
                CompletionRate = totalActive > 0
                    ? Math.Round((double)totalCompleted / totalActive * 100, 1)
                    : 0.0,
            };
        }

        /// <summary>
        /// Groups the average reading time by month (year view) or by day (week/month view).
        /// </summary>
        private static List<ReadingAvgTimeResponseDto> BuildAvgTimeByPeriod(List<CompletedStory> completedStories, MetricsPeriod period, int offsetMinutes)
        {
            if (period == MetricsPeriod.Year)
            {
                return completedStories
                    .GroupBy(story => new { story.CompletionDate.AddMinutes(offsetMinutes).Year, story.CompletionDate.AddMinutes(offsetMinutes).Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new ReadingAvgTimeResponseDto
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        AvgTimeSeconds = (int)Math.Round(g.Average(story => story.TotalTime)),
                    })
                    .ToList();
            }

            return completedStories
                .GroupBy(story => DateOnly.FromDateTime(story.CompletionDate.AddMinutes(offsetMinutes)))
                .OrderBy(g => g.Key)
                .Select(g => new ReadingAvgTimeResponseDto
                {
                    Date = g.Key,
                    AvgTimeSeconds = (int)Math.Round(g.Average(story => story.TotalTime)),
                })
                .ToList();
        }
    }
}
