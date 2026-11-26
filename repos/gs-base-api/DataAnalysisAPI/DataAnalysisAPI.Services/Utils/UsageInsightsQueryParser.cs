using System;
using System.Globalization;
using System.Linq;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using Microsoft.AspNetCore.Http;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Reads and validates the query string parameters of the usage insights queries. Every failure throws an
    /// <see cref="InvalidQueryParameterException"/> that the controller turns into a 400 response.
    /// </summary>
    public class UsageInsightsQueryParser
    {
        /// <summary>
        /// Maximum limit of the JSON routes.
        /// </summary>
        public const int MaxJsonLimit = 50;

        /// <summary>
        /// Maximum limit of the exports.
        /// </summary>
        public const int MaxExportLimit = 5000;

        private const int DefaultJsonLimit = 10;
        private readonly IQueryCollection _query;
        private readonly bool _isExport;
        private readonly bool _ignoreLimit;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageInsightsQueryParser"/> class.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <param name="isExport">True for an export: the limit goes up to 5000 and, without limit, every row (up to 5000) comes back.</param>
        /// <param name="ignoreLimit">True for a chart export, where the limit belongs to the chart preview and not to the query.</param>
        public UsageInsightsQueryParser(IQueryCollection query, bool isExport = false, bool ignoreLimit = false)
        {
            _query = query;
            _isExport = isExport;
            _ignoreLimit = ignoreLimit;
        }

        /// <summary>
        /// Gets the maximum limit allowed for this request.
        /// </summary>
        public int MaxLimit => _isExport ? MaxExportLimit : MaxJsonLimit;

        /// <summary>
        /// Gets the raw value of a parameter, or null when it is absent or empty.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns>Returns the first value.</returns>
        public string? GetValue(string name)
        {
            var value = _query[name].FirstOrDefault();
            return string.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// Calculates a percentage with one decimal.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="total">The total.</param>
        /// <returns>Returns the percentage, or null when the total is zero.</returns>
        public static double? Percentage(double part, double total) => total == 0 ? null : Math.Round(100.0 * part / total, 1);

        /// <summary>
        /// Gets the latest of two dates.
        /// </summary>
        /// <param name="first">First date.</param>
        /// <param name="second">Second date.</param>
        /// <returns>Returns the latest date.</returns>
        public static DateOnly MaxDate(DateOnly first, DateOnly second) => first > second ? first : second;

        /// <summary>
        /// Parses a required date in the yyyy-MM-dd format.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns>Returns the date.</returns>
        public DateOnly ParseDate(string name)
        {
            var value = GetValue(name) ?? throw new InvalidQueryParameterException(name, "Missing");
            return DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : throw new InvalidQueryParameterException(name, "DateFormat");
        }

        /// <summary>
        /// Parses an optional date.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns>Returns the date, or null when the parameter is absent.</returns>
        public DateOnly? ParseOptionalDate(string name) => GetValue(name) is null ? null : ParseDate(name);

        /// <summary>
        /// Parses a period given by two required dates.
        /// </summary>
        /// <param name="fromName">Name of the start parameter.</param>
        /// <param name="toName">Name of the end parameter.</param>
        /// <returns>Returns the period.</returns>
        public (DateOnly From, DateOnly To) ParsePeriod(string fromName = "from", string toName = "to")
        {
            var from = ParseDate(fromName);
            var to = ParseDate(toName);
            return from > to ? throw new InvalidQueryParameterException(fromName, "DateOrder", toName) : (from, to);
        }

        /// <summary>
        /// Parses the optional minConnections and maxConnections parameters.
        /// </summary>
        /// <returns>Returns the range; max is int.MaxValue when it is absent.</returns>
        public (int Min, int Max) ParseConnectionsRange()
        {
            int ReadValue(string name, int defaultValue)
            {
                var text = GetValue(name);
                if (text is null)
                {
                    return defaultValue;
                }

                return int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out var value)
                    ? value
                    : throw new InvalidQueryParameterException(name, "NonNegativeInteger");
            }

            var min = ReadValue("minConnections", 0);
            var max = ReadValue("maxConnections", int.MaxValue);
            return min > max ? throw new InvalidQueryParameterException("minConnections", "MinGreaterThanMax", "maxConnections") : (min, max);
        }

        /// <summary>
        /// Parses the order and the limit. In an export the limit goes up to 5000 and, without limit, every row comes back.
        /// </summary>
        /// <returns>Returns whether the order is descending and the limit.</returns>
        public (bool Descending, int Limit) ParseOrderAndLimit()
        {
            var order = GetValue("order") ?? "desc";
            if (order is not ("asc" or "desc"))
            {
                throw new InvalidQueryParameterException("order", "OrderValue");
            }

            var limit = _isExport ? MaxLimit : DefaultJsonLimit;
            var text = _ignoreLimit ? null : GetValue("limit");
            if (text is not null && !int.TryParse(text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out limit))
            {
                throw new InvalidQueryParameterException("limit", "NotInteger");
            }

            return limit < 1 || limit > MaxLimit
                ? throw new InvalidQueryParameterException("limit", "LimitRange", MaxLimit.ToString(CultureInfo.InvariantCulture))
                : (order == "desc", limit);
        }

        /// <summary>
        /// Parses the chart parameters: all, order, includeZeros and limit (1 to 5000; default 10).
        /// </summary>
        /// <returns>Returns the chart options.</returns>
        public ChartOptions ParseChartOptions()
        {
            bool ReadBoolean(string name)
            {
                var text = GetValue(name);
                if (text is null)
                {
                    return false;
                }

                return bool.TryParse(text, out var value) ? value : throw new InvalidQueryParameterException(name, "NotBoolean");
            }

            var order = GetValue("order") ?? "desc";
            if (order is not ("asc" or "desc"))
            {
                throw new InvalidQueryParameterException("order", "OrderValue");
            }

            var limit = ChartOptions.PreviewLimit;
            var text = GetValue("limit");
            if (text is not null && !int.TryParse(text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out limit))
            {
                throw new InvalidQueryParameterException("limit", "NotInteger");
            }

            return limit < 1 || limit > MaxExportLimit
                ? throw new InvalidQueryParameterException("limit", "LimitRange", MaxExportLimit.ToString(CultureInfo.InvariantCulture))
                : new ChartOptions(ReadBoolean("all"), order, ReadBoolean("includeZeros"), limit);
        }

        /// <summary>
        /// Reads a parameter that must be one of a closed set of values.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="defaultValue">Value when the parameter is absent.</param>
        /// <param name="allowedValues">The allowed values.</param>
        /// <returns>Returns the value.</returns>
        public string ParseChoice(string name, string defaultValue, params string[] allowedValues)
        {
            var value = GetValue(name) ?? defaultValue;
            return allowedValues.Contains(value)
                ? value
                : throw new InvalidQueryParameterException(name, "AllowedValues", string.Join(", ", allowedValues));
        }

        /// <summary>
        /// Reads the optional school parameter and checks that it exists inside the scope. A school out of scope behaves as if it did not exist.
        /// </summary>
        /// <param name="schoolExists">Checks a school name against the database and the caller scope.</param>
        /// <returns>Returns the exact school name, or null when no school was requested.</returns>
        public string? ResolveSchool(Func<string, bool> schoolExists)
        {
            var school = GetValue("school");
            return school is not null && !schoolExists(school) ? throw new InvalidQueryParameterException("school", "UnknownSchool", school) : school;
        }
    }
}
