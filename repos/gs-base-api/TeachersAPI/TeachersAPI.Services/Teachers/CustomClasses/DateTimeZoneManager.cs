using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses
{
    /// <summary>
    /// To handle the date time zone details.
    /// </summary>
    public static class DateTimeZoneManager
    {
        /// <summary>
        /// To convert time zone to UTC.
        /// </summary>
        /// <param name="dateTime">The date time to convert.</param>
        /// <param name="timeZoneString">The timezone that we want to convert to UTC.</param>
        /// <returns>Returns the converted datetime to UTC.</returns>

        public static DateTime ConvertTimeZoneAndDateToUniversalDateTime(DateTime dateTime, string timeZoneString)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneString));
        }

        /// <summary>
        /// To convert the datetime to UTC.
        /// </summary>
        /// <param name="dateTime">The datetime to convert.</param>
        /// <returns>The date time converted as UTC.</returns>
        public static DateTime ConvertToUniversalDateTime(DateTime dateTime)
        {
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// To convert a datetime to a mysql date string.
        /// </summary>
        /// <param name="dateTime">The date time to convert.</param>
        /// <returns>Returns the converted date.</returns>
        public static string ConvertToMySQLDateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
