using System;
using System.Globalization;
using TeachersAPI.Domain.Models.Events;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Calculates the functional status of an event (see EventStatuses).
    /// This is independent from EventModel.Available, which only controls the CRUD soft-delete flag.
    /// </summary>
    public static class EventStatusCalculator
    {
        /// <summary>
        /// Calculates the functional status for the given event.
        /// </summary>
        /// <param name="eventModel">The event to evaluate.</param>
        /// <returns>One of the values in EventStatuses.</returns>
        public static string Calculate(EventModel eventModel)
        {
            if (eventModel.Available == "0")
            {
                return EventStatuses.Cancelled;
            }

            if (!TryParseInvariant(eventModel.StartDate, out DateTime startDate))
            {
                return EventStatuses.Upcoming;
            }

            // EndDate is optional in the form. Without one, treat the event as punctual:
            // it starts and ends at the same instant instead of staying "ongoing" forever.
            if (!TryParseInvariant(eventModel.EndDate, out DateTime endDate))
            {
                endDate = startDate;
            }

            DateTime now = EventTimeProvider.NowInZone(eventModel.TimeZoneCode);

            if (now < startDate)
            {
                return EventStatuses.Upcoming;
            }

            if (now > endDate)
            {
                return EventStatuses.Finished;
            }

            return EventStatuses.Ongoing;
        }

        private static bool TryParseInvariant(string value, out DateTime result)
        {
            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}
