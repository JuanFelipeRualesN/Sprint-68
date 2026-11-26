using System;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Single source of "now" for Event status/date calculations.
    /// </summary>
    public static class EventTimeProvider
    {
        public static DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Returns the current wall-clock time in the given IANA timezone (e.g.
        /// "America/Puerto_Rico"). EventModel.StartDate/EndDate are stored as naive local time in
        /// the event's own chosen zone (the same convention Microsoft Graph's dateTimeTimeZone
        /// resource uses), so "now" must be converted into that same zone before comparing — the
        /// server's own local time zone is otherwise irrelevant and can silently disagree with it.
        /// Falls back to the server's local time if the zone id can't be resolved.
        /// </summary>
        /// <param name="timeZoneCode">The event's IANA timezone id.</param>
        public static DateTime NowInZone(string timeZoneCode)
        {
            if (string.IsNullOrWhiteSpace(timeZoneCode))
            {
                return DateTime.Now;
            }

            try
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneCode);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                return DateTime.Now;
            }
            catch (InvalidTimeZoneException)
            {
                return DateTime.Now;
            }
        }
    }
}
