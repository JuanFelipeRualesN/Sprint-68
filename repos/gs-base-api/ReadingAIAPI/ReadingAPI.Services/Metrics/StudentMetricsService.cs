using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Domain.Entities.Reading;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Metrics
{
    /// <summary>
    /// Provides all-time reading metrics for a single student.
    /// </summary>
    public class StudentMetricsService
    {
        private readonly ReadingAIDbContext _dbContext;
        private readonly ILogger<StudentMetricsService> _logger;

        public StudentMetricsService(ReadingAIDbContext dbContext, ILogger<StudentMetricsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Returns the student's current streak, average daily usage time, total completed stories,
        /// and average pages slides per session, aggregated across all available records.
        /// </summary>
        /// <param name="studentId">The student's user Id</param>
        public async Task<StudentMetricsResponseDto> GetStudentSummary(int studentId)
        {
            try
            {
                var sessions = _dbContext.UserSessions.Where(session => session.UserId == studentId);

                // Average daily usage time, total sessions, and on/off school
                var sessionStats = await sessions
                    .GroupBy(_ => 1)
                    .Select(group => new
                    {
                        AvgTimeSeconds = (int)Math.Round(group.Average(session => (double)session.TotalTimeSeconds)),
                        TotalSessions = group.Sum(session => session.TotalSessions),
                        AvgOnSchoolPercent = (int)Math.Round(group.Average(session => session.OnSchoolPercent)),
                        AvgOffSchoolPercent = (int)Math.Round(group.Average(session => session.OffSchoolPercent)),
                    })
                    .FirstOrDefaultAsync();

                // Usage time per day for the current calendar week
                var (weekFrom, weekTo) = GetCurrentWeekRange(DateOnly.FromDateTime(DateTime.UtcNow));
                var weekTimeByDate = await sessions
                    .Where(session => session.Date >= weekFrom && session.Date <= weekTo)
                    .GroupBy(session => session.Date)
                    .Select(group => new { Date = group.Key, TimeSeconds = group.Sum(session => session.TotalTimeSeconds) })
                    .ToDictionaryAsync(day => day.Date, day => day.TimeSeconds);

                // Build the full week so days without activity are present as 0
                var currentWeekUsage = Enumerable.Range(0, 7)
                    .Select(offset =>
                    {
                        var date = weekFrom.AddDays(offset);
                        return new WeeklyUsageDayDto
                        {
                            Date = date,
                            DayOfWeek = offset + 1,
                            TimeSeconds = weekTimeByDate.TryGetValue(date, out var seconds) ? seconds : 0,
                        };
                    })
                    .ToList();

                // Total usage time for the current calendar year
                var currentYear = DateOnly.FromDateTime(DateTime.UtcNow).Year;
                var currentYearTimeSeconds = await sessions
                    .Where(session => session.Date.Year == currentYear)
                    .SumAsync(session => session.TotalTimeSeconds);

                // Current streak = streak of the most recent daily record
                var streak = await sessions
                    .OrderByDescending(session => session.Date)
                    .Select(session => session.Streak)
                    .FirstOrDefaultAsync();

                // Total texts read are stories marked completed
                var totalTextsRead = await _dbContext.UserStoryProgress
                    .CountAsync(progress => progress.UserId == studentId
                        && progress.StatusId == StoryProgressStatus.Completed);

                var studentResults = _dbContext.UserStoryResults.Where(result => result.UserId == studentId);

                // Total pages read are one row per slide attempt.
                var totalPages = await studentResults.CountAsync();

                var totalSessions = sessionStats?.TotalSessions ?? 0;
                var avgPagesPerSession = totalSessions > 0
                    ? Math.Round((double)totalPages / totalSessions, 1)
                    : 0.0;

                // Keep only the latest attempt per slide to avoid repeated readings
                var latestSlideIds = studentResults
                    .GroupBy(result => new { result.StoryId, result.SlideId })
                    .Select(group => group.Max(result => result.Id));

                // All-time average assessment scores, one score per slide (latest attempt)
                var averageScores = await studentResults
                    .Where(result => latestSlideIds.Contains(result.Id))
                    .GroupBy(_ => 1)
                    .Select(group => new StudentAverageScoresDto
                    {
                        AvgAccuracyScore = Math.Round(group.Average(result => result.AccuracyScore), 1),
                        AvgFluencyScore = Math.Round(group.Average(result => result.FluencyScore), 1),
                        AvgCompletenessScore = Math.Round(group.Average(result => result.CompletenessScore), 1),
                        AvgPronunciationScore = Math.Round(group.Average(result => result.PronunciationScore), 1),
                    })
                    .FirstOrDefaultAsync() ?? new StudentAverageScoresDto();

                // One reading is a story read by the student in one language, no matter how many
                // slides it has or how many times each slide was attempted
                var storyReadings = (
                    from result in studentResults
                    join story in _dbContext.Stories on result.StoryId equals story.Id
                    join category in _dbContext.StoryCategories on story.CategoryId equals category.Id
                    select new { result.Language, StoryId = story.Id, CategoryId = category.Id, CategoryName = category.Name })
                    .Distinct();

                // Reading counts per story category
                var categories = await storyReadings
                    .GroupBy(x => new { x.CategoryId, x.CategoryName })
                    .OrderBy(g => g.Key.CategoryName)
                    .Select(g => new ReadingCategoryResponseDto
                    {
                        CategoryId = g.Key.CategoryId,
                        CategoryName = g.Key.CategoryName,
                        TotalReadings = g.Count(),
                    })
                    .ToListAsync();

                return new StudentMetricsResponseDto
                {
                    Streak = streak,
                    AvgTimeSeconds = sessionStats?.AvgTimeSeconds ?? 0,
                    TotalTextsRead = totalTextsRead,
                    AvgPagesPerSession = avgPagesPerSession,
                    CurrentWeekUsage = currentWeekUsage,
                    CurrentYearTimeSeconds = currentYearTimeSeconds,
                    AvgOnSchoolPercent = sessionStats?.AvgOnSchoolPercent ?? 0,
                    AvgOffSchoolPercent = sessionStats?.AvgOffSchoolPercent ?? 0,
                    AverageScores = averageScores,
                    Categories = categories,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student summary for user {UserId}", studentId);
                throw;
            }
        }

        /// <summary>
        /// Returns the current week date range for the given day.
        /// </summary>
        private static (DateOnly From, DateOnly To) GetCurrentWeekRange(DateOnly today)
        {
            int daysToMonday = today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1;
            var monday = today.AddDays(-daysToMonday);
            return (monday, monday.AddDays(6));
        }
    }
}
