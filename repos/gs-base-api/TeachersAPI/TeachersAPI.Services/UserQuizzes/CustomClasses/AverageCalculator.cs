using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System;

namespace TeachersAPI.TeachersAPI.Services.UserQuizzes.CustomClasses
{
    public static class AverageCalculator
    {
        /// <summary>
        /// Calculates the average of a decimal array.
        /// </summary>
        /// <param name="values">Numbers to average.</param>
        /// <param name="fixToTwoNumbersAfterDecimal">If true, truncate to 2 decimal places.</param>
        /// <returns>The average.</returns>
        public static decimal CalculateAverage(decimal[] values, bool fixToTwoNumbersAfterDecimal)
        {
            if (values == null || values.Length == 0)
                return 0m;

            decimal sum = 0m;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }

            decimal average = sum / values.Length;

            return fixToTwoNumbersAfterDecimal
                ? FixTwoDecimalsAfterPoint(average)
                : average;
        }

        /// <summary>
        /// Calculates the average of a decimal array and truncates to 2 decimal places.
        /// </summary>
        /// <param name="decimals">Numbers to average.</param>
        /// <returns>The truncated average.</returns>
        public static decimal CalculateAverage(decimal[] decimals)
            => CalculateAverage(decimals, fixToTwoNumbersAfterDecimal: true);

        /// <summary>
        /// Truncates a value to 2 decimal places (no rounding).
        /// </summary>
        /// <param name="value">The value to truncate.</param>
        /// <returns>The truncated value.</returns>
        public static decimal FixTwoDecimalsAfterPoint(decimal value)
        {
            if (value == 100m)
                return value;

            return Math.Truncate(100m * value) / 100m;
        }
    }
}