using System;

namespace GSWebAPI.GSWebAPI.Services.UserQuizzes.CustomClasses
{
    /// <summary>
    /// To calculate the average.
    /// </summary>
    public static class AverageCalculator
    {
        /// <summary>
        /// Calculates the avarage of a double list.
        /// </summary>
        /// <param name="values">The values to calculate.</param>
        /// <param name="fixToTwoNumbersAfterDecimal">true to strip any number after the first two decimals.</param>
        /// <returns>Returns the average calculated.</returns>
        public static decimal CalculateAverage(decimal[] values, bool fixToTwoNumbersAfterDecimal)
        {
            decimal sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }

            decimal average = sum / values.Length;

            if (fixToTwoNumbersAfterDecimal)
            {
                return FixTwoDecimalsAfterPoint(average);
            }
            else
            {
                return average;
            }
        }

        /// <summary>
        /// Fixes two decimals after point.
        /// </summary>
        /// <param name="value">The value to fix.</param>
        /// <returns>Returns a doble with two decimals.</returns>
        public static decimal FixTwoDecimalsAfterPoint(decimal value)
        {
            if (100.ToString() == value.ToString())
            {
                return value;
            }

            return Math.Truncate(100 * value) / 100;
        }
    }
}
