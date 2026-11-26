using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions
{
    /// <summary>
    /// All DATETIME/TIMESTAMP values are UTC. Writes convert local times to UTC (unspecified is taken as UTC);
    /// reads come back with <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    public sealed class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        /// <summary>Creates the converter: <see cref="ToUtc"/> on write, kind set to UTC on read.</summary>
        public UtcDateTimeConverter()
            : base(v => ToUtc(v), v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {
        }

        /// <summary>
        /// Normalizes a value to UTC: UTC is kept, local is converted with the server's time zone, and unspecified is
        /// taken as UTC without shifting.
        /// </summary>
        /// <param name="value">The value to normalize.</param>
        /// <returns>The value with <see cref="DateTimeKind.Utc"/>.</returns>
        public static DateTime ToUtc(DateTime value)
        {
            return value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            };
        }
    }
}
