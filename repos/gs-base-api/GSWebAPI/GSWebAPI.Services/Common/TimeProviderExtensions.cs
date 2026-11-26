namespace GSWebAPI.GSWebAPI.Services.Common
{
    internal static class TimeProviderExtensions
    {
        /// <summary>UTC now truncated to whole seconds (DATETIME columns have no fractional part).</summary>
        public static DateTime UtcNowSeconds(this TimeProvider timeProvider)
        {
            var now = timeProvider.GetUtcNow().UtcDateTime;
            return new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
        }

        /// <summary>True if <paramref name="timezone"/> is a known IANA (or Windows) time zone id.</summary>
        public static bool IsValidTimeZone(string? timezone)
        {
            if (string.IsNullOrWhiteSpace(timezone) || timezone.Length > 64)
            {
                return false;
            }

            return TimeZoneInfo.TryFindSystemTimeZoneById(timezone, out _);
        }
    }
}
