using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement
{
    /// <summary>
    /// Provides session usage metrics scoped to the requesting user's role.
    /// Uses UserSessionEntity to aggregate session counts, active days,
    /// total time, device usage, and on/off school percentages.
    /// </summary>
    public class SessionMetricsService : MetricsBaseService<UserSessionEntity>
    {
        private readonly ILogger<SessionMetricsService> _logger;

        public SessionMetricsService(ReadingAIDbContext dbContext, ILogger<SessionMetricsService> logger)
            : base(dbContext)
        {
            _logger = logger;
        }

        // Set the entity for the query
        protected override IQueryable<UserSessionEntity> GetBaseQuery()
            => _dbContext.UserSessions.AsQueryable();

        // Apply the filters for the query
        protected override IQueryable<UserSessionEntity> ApplyFilters(IQueryable<UserSessionEntity> query, UserSessionMetricsRequestDto request, int? regionId, int? schoolId,
            List<int>? studentIds, bool applyDateFilters)
        {
            if (regionId.HasValue)
                query = query.Where(s => s.RegionId == regionId.Value);

            if (schoolId.HasValue)
                query = query.Where(s => s.SchoolId == schoolId.Value);

            if (studentIds != null)
                query = query.Where(s => studentIds.Contains(s.UserId));

            if (request.UserIds?.Count > 0)
                query = query.Where(s => request.UserIds.Contains(s.UserId));

            if (applyDateFilters)
            {
                if (request.DateFrom.HasValue)
                    query = query.Where(s => s.Date >= request.DateFrom.Value);

                if (request.DateTo.HasValue)
                    query = query.Where(s => s.Date <= request.DateTo.Value);
            }

            return query;
        }

        /// <summary>
        /// Get sessions grouped by week, month or year, scoped to the requesting user's role, intended for chart indicators
        /// Defaults to the current calendar week when no DateFrom/DateTo is provided
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin) with optional filters</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<List<SessionByPeriodResponseDto>> GetSessionSummary(UserSessionMetricsRequestDto request, MetricsPeriod period, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            try
            {
                var query = await BuildScopedQuery(request, userId, isStaff, isDirector, oreId);

                if (query == null)
                {
                    return new List<SessionByPeriodResponseDto>();
                }

                // Use the current week if no date range is provided
                if (!request.DateFrom.HasValue && !request.DateTo.HasValue)
                {
                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    var (from, to) = GetDefaultDateRange(period, today);
                    query = query.Where(session => session.Date >= from && session.Date <= to);
                }

                // Year uses month granularity, so it needs its own grouping
                if (period == MetricsPeriod.Year)
                {
                    var sessionsByMonth = query
                        .GroupBy(session => new { session.Date.Year, session.Date.Month })
                        .OrderBy(group => group.Key.Year).ThenBy(group => group.Key.Month)
                        .Select(group => new SessionByPeriodResponseDto
                        {
                            Year = group.Key.Year,
                            Month = group.Key.Month,
                            TotalSessions = group.Sum(session => session.TotalSessions),
                            TotalTimeSeconds = group.Sum(session => session.TotalTimeSeconds)
                        });

                    return await sessionsByMonth.ToListAsync();
                }

                // Week and Month share day granularity, only the date range changes
                var sessionsByDay = query
                .GroupBy(session => session.Date)
                    .OrderBy(group => group.Key)
                    .Select(group => new SessionByPeriodResponseDto
                    {
                        Date = group.Key,
                        TotalSessions = group.Sum(session => session.TotalSessions),
                        TotalTimeSeconds = group.Sum(session => session.TotalTimeSeconds)
                    });

                return await sessionsByDay.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching session summary for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Returns aggregated totals and averages for the given period, intended for dashboard indicators
        /// Defaults to the current calendar week when no DateFrom/DateTo is provided
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="period">The period to return the data</param>
        /// <param name="userId">The user Id</param>
        /// <param name="isStaff">Staff access (admin-ore / admin) with optional filters</param>
        /// <param name="isDirector">Scoped to the director's school</param>
        /// <param name="oreId">The region Id</param>
        public async Task<SessionTotalsResponseDto> GetSessionTotals(UserSessionMetricsRequestDto request, MetricsPeriod period, string userId, bool isStaff, bool isDirector, string? oreId)
        {
            try
            {
                var query = await BuildScopedQuery(request, userId, isStaff, isDirector, oreId);

                if (query == null)
                {
                    return new SessionTotalsResponseDto();
                }

                // Apply default date range if no date filters are provided
                if (!request.DateFrom.HasValue && !request.DateTo.HasValue)
                {
                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    var (from, to) = GetDefaultDateRange(period, today);
                    query = query.Where(session => session.Date >= from && session.Date <= to);
                }

                // Single student: AvgTimeSeconds = average of their daily usage records
                bool isSingleStudent = request.UserIds?.Count == 1;

                if (isSingleStudent)
                {
                    var studentResult = query
                        .GroupBy(_ => 1)
                        .Select(group => new SessionTotalsResponseDto
                        {
                            TotalStudents = 1,
                            TotalSessions = group.Sum(s => s.TotalSessions),
                            TotalActiveDays = group.Count(),
                            AvgTimeSeconds = (int)Math.Round(group.Average(session => session.TotalTimeSeconds)),
                            AvgOffSchoolPercent = (int)Math.Round(group.Average(session => session.OffSchoolPercent)),
                            AvgOnSchoolPercent = (int)Math.Round(group.Average(session => session.OnSchoolPercent)),
                            AvgDesktopPercent = (int)Math.Round(group.Average(session => session.DesktopPercent)),
                            AvgMobilePercent = (int)Math.Round(group.Average(session => session.MobilePercent)),
                            AvgTabletPercent = (int)Math.Round(group.Average(session => session.TabletPercent)),
                        });

                    return await studentResult.FirstOrDefaultAsync() ?? new SessionTotalsResponseDto();
                }

                // Multiple students/no filter: AvgTimeSeconds = total time / students
                var studentsResults = query
                    .GroupBy(_ => 1)
                    .Select(group => new SessionTotalsResponseDto
                    {
                        TotalStudents = group.Select(session => session.UserId).Distinct().Count(),
                        TotalSessions = group.Sum(session => session.TotalSessions),
                        TotalActiveDays = group.Count(),
                        AvgTimeSeconds = (int)Math.Round((double)group.Sum(session => session.TotalTimeSeconds) / group.Select(session => session.UserId).Distinct().Count()),
                        AvgOffSchoolPercent = (int)Math.Round(group.Average(session => session.OffSchoolPercent)),
                        AvgOnSchoolPercent = (int)Math.Round(group.Average(session => session.OnSchoolPercent)),
                        AvgDesktopPercent = (int)Math.Round(group.Average(session => session.DesktopPercent)),
                        AvgMobilePercent = (int)Math.Round(group.Average(session => session.MobilePercent)),
                        AvgTabletPercent = (int)Math.Round(group.Average(session => session.TabletPercent)),
                    });

                return await studentsResults.FirstOrDefaultAsync() ?? new SessionTotalsResponseDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching session totals for user {UserId}", userId);
                throw;
            }
        }
    }
}
