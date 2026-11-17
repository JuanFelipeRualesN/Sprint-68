using System;

namespace SchoolsAPI.SchoolsAPI.Services.Schools.CustomClasses
{
    /// <summary>
    /// To handle date convertions.
    /// </summary>
    public static class DateConverter
    {
        /// <summary>
        /// Converts the datetime to MySQL Format
        /// </summary>
        /// <param name="dateTime">Datetime string to convert.</param>
        /// <returns>Returns a string with the date format.</returns>
        public static string ConvertDateTimeToMySQLFormat(string dateTime)
        {
            DateTime dateTimeToInsert = DateTime.Parse(dateTime);
            return dateTimeToInsert.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
