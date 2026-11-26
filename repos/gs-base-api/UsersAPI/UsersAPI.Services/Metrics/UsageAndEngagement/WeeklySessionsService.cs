using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.UsersAPI.Domain.Models.Metrics.UsageAndEngagement;
using UsersAPI.UsersAPI.Infrastructure.DapperHandler.Metrics.UsageAndEngagement;


namespace UsersAPI.UsersAPI.Services.Metrics.UsageAndEngagement
{
    public static class WeeklySessionsService
    {
        /// <summary>
        /// Get user weekly sessions.
        /// </summary>
        /// <returns>Returns the WeeklySessionsModel containing the session count.</returns>
        public static async Task<UsageMetricsModel> GetUserWeeklySessions(int requestUserId, DateTime startDate, DateTime endDate, int weekNumber)
        {
            List<int> filteredUserIds = new List<int> { requestUserId };

            var frequencyData = await SessionsRepository.GetWeeklySessionFrequencyAsync(
                    filteredUserIds,
                    startDate,
                    endDate);

            var model = new UsageMetricsModel
            {
                TotalSessions = frequencyData.Sum(d => d.SessionCount),

                FrequencyUsage = frequencyData,
            };

            return model;
        }

        /// <summary>
        /// Helper method to get the start and end date of a specific week
        /// </summary>
        public static(DateTime startDate, DateTime endDate, string weekNumberString, int weekNumberInt) GetWeekDateRange(int weekOffset)
        {
            var today = DateTime.Now;

            int daysToSubtract = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysToSubtract < 0)
                daysToSubtract += 7;

            var mondayThisWeek = today.AddDays(-daysToSubtract).Date;

            /// Get sessions based on week
            var startDate = mondayThisWeek.AddDays(weekOffset * 7);
            var endDate = startDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

            /// Calculate week number
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var weekNumInt = culture.Calendar.GetWeekOfYear(startDate,
                System.Globalization.CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);

            string weekNumberString = $"{startDate.Year}-W{weekNumInt:00}";

            return (startDate, endDate, weekNumberString, weekNumInt);
        }
    }
}