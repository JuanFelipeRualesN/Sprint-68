using System;
using System.Collections.Generic;
using System.Linq;

namespace GSPlanning.Services.Utilities
{
    public static class DateUtil
    {
        private static readonly DateTime MySqlMinDate = new DateTime(1000, 1, 1);
        private static readonly DateTime MySqlMaxDate = new DateTime(9999, 12, 31);

        /// <summary>
        /// Validates a list of dates to ensure they fall within MySQL's valid range and returns them with time stripped.
        /// </summary>
        /// <param name="dates">The list of dates to validate.</param>
        /// <returns>Returns a normalized array of dates.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If any date is outside MySQL's valid range.</exception>
        public static DateTime ValidateMySqlDate(DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException(nameof(date), "Date cannot be null.");

            var dateOnly = date.Date;

            if (dateOnly < MySqlMinDate || dateOnly > MySqlMaxDate)
                throw new ArgumentOutOfRangeException(nameof(date),
                    $"Date '{dateOnly:yyyy-MM-dd}' is outside MySQL's supported range ({MySqlMinDate:yyyy-MM-dd} to {MySqlMaxDate:yyyy-MM-dd}).");

            return dateOnly;
        }

        public static DateTime[] CreateDates(DateRange oldRange, DateRange newRange, DateTime[] oldSelection)
        {
            var oldDates = CreateDateTimeInRange(oldRange);
            var newDates = CreateDateTimeInRange(newRange);

            var newSelection = new List<DateTime>();

            var oldDatesDictionary = oldDates.Select((date, index) => new { date, position = index + 1 })
                .ToDictionary(x => x.date, x => x.position);

            var newDatesDictionary = newDates.Select((date, index) => new { date, position = index + 1 })
                .ToDictionary(x => x.position, x => x.date);

            foreach (var selectedDate in oldSelection.Select(x => x.Date).ToList())
            {
                if (oldDatesDictionary.TryGetValue(selectedDate, out int pos) && newDatesDictionary.TryGetValue(pos, out DateTime newDate))
                {
                    newSelection.Add(newDatesDictionary[oldDatesDictionary[selectedDate]]);
                }
            }

            return newSelection.ToArray();
        }

        /// <summary>
        /// To create basic dates range. Mostly use when the period between the clonings are different.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>Returns an array of dates.</returns>
        public static DateTime[] CreateDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> dates = new List<DateTime>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates.ToArray();
        }

        private static DateTime[] CreateDateTimeInRange(DateRange range)
        {
            var currentDate = range.From;
            var dates = new List<DateTime>();
            while (currentDate <= range.To)
            {
                dates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return dates.ToArray();
        }
    }

    public sealed class DateRange
    {
        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
    }
}