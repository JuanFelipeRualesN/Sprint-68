using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TeachersAPI.Utils
{
    public static class DatesFormatter
    {
        // Quick check: already MySQL-friendly? just return it.
        private static readonly Regex MySqlRegex =
            new(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$", RegexOptions.Compiled);

        // Formats WITHOUT timezone/offset
        private static readonly string[] LocalFormats = new[]
        {
        "M/d/yyyy h:mm:ss tt",   // 5/29/2025 12:00:00 AM
        "M/d/yyyy hh:mm:ss tt",
        "M/d/yyyy HH:mm:ss",     // 10/03/2025 00:00:00  <-- your failing case
        "MM/dd/yyyy HH:mm:ss",
        "M/d/yyyy h:mm tt",
        "M/d/yyyy HH:mm",
        "MM/dd/yyyy",
        "M/d/yyyy",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss.fff",
    };

        // Formats WITH timezone/offset (Z or ±hh:mm)
        private static readonly string[] OffsetFormats = new[]
        {
        "yyyy-MM-ddTHH:mm:ssK",
        "yyyy-MM-ddTHH:mm:ss.fffK",
        "o" // 2025-10-03T04:00:00.0000000Z
    };

        /// <summary>
        /// Converts many common date strings into MySQL DATETIME string "yyyy-MM-dd HH:mm:ss".
        /// keepWallClock=true: keeps the same clock time even if input has a timezone/offset.
        /// keepWallClock=false: normalizes offset/Z inputs to UTC before formatting.
        /// </summary>
        public static string ConvertFromCSharpDateTimeToMySql(string dateStr, bool keepWallClock = true)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return dateStr;

            dateStr = dateStr.Trim();

            if (MySqlRegex.IsMatch(dateStr))
                return dateStr; // already MySQL friendly

            // 1) Offset-aware parse first, if input looks like it has Z or an offset
            if (dateStr.Contains('Z') || dateStr.Contains('+') || dateStr.Contains('-') && dateStr.Contains("T"))
            {
                if (DateTimeOffset.TryParseExact(
                        dateStr,
                        OffsetFormats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AllowWhiteSpaces,
                        out var dto))
                {
                    DateTime dt = keepWallClock ? dto.DateTime        // same clock time, no tz
                                                : dto.UtcDateTime;    // normalize to UTC
                    dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
                    return dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }

            // 2) Local (no offset) exact formats
            if (DateTime.TryParseExact(
                    dateStr,
                    LocalFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces,
                    out var dtExact))
            {
                dtExact = DateTime.SpecifyKind(dtExact, DateTimeKind.Unspecified);
                return dtExact.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            // 3) Last-resort general parse (still culture-invariant)
            if (DateTime.TryParse(dateStr, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AllowWhiteSpaces, out var dtAny))
            {
                dtAny = DateTime.SpecifyKind(dtAny, DateTimeKind.Unspecified);
                return dtAny.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            // If we cannot parse, return original so caller can decide what to do
            return dateStr;
        }
    }
}
