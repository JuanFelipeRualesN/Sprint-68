using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Helpers shared by the Excel export and the chart specification. They work over the JSON of the response (the one the caller sees),
    /// so the workbook and the chart are always consistent with the query.
    /// </summary>
    public static class UsageInsightsColumnFormatter
    {
        /// <summary>
        /// Gets the readable title of a column. It comes from Messages:UsageInsightsColumn{name}; if it is not there, the PascalCase name is split into words.
        /// </summary>
        /// <param name="propertyName">The property name of the row, for example ConnectedUsers.</param>
        /// <param name="language">The language code.</param>
        /// <returns>Returns the title.</returns>
        public static string GetColumnTitle(string propertyName, string language)
        {
            var key = "UsageInsightsColumn" + propertyName;
            var title = UsageInsightsMessages.Get(language, key);
            if (title != key)
            {
                return title;
            }

            var words = new StringBuilder();
            foreach (var character in propertyName)
            {
                if (char.IsUpper(character) && words.Length > 0)
                {
                    words.Append(' ');
                }

                words.Append(words.Length == 0 ? char.ToUpperInvariant(character) : char.ToLowerInvariant(character));
            }

            return words.ToString();
        }

        /// <summary>
        /// Checks whether a column is a percentage or an average (written with the 0.0 format).
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Returns true for percentage columns.</returns>
        public static bool IsPercentage(string propertyName) =>
            propertyName.Contains("Percentage", StringComparison.Ordinal) || propertyName.StartsWith("Average", StringComparison.Ordinal);

        /// <summary>
        /// Flattens a JSON value into key/value pairs (nested names joined with a dot, array items as [n]); nulls are skipped.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="prefix">The prefix of the keys.</param>
        /// <returns>Returns the pairs.</returns>
        public static IEnumerable<(string Key, JsonNode Value)> Flatten(JsonNode? node, string prefix = "")
        {
            if (node is JsonObject jsonObject)
            {
                foreach (var (key, value) in jsonObject)
                {
                    foreach (var pair in Flatten(value, prefix.Length == 0 ? key : prefix + "." + key))
                    {
                        yield return pair;
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                for (var i = 0; i < jsonArray.Count; i++)
                {
                    foreach (var pair in Flatten(jsonArray[i], $"{prefix}[{i + 1}]"))
                    {
                        yield return pair;
                    }
                }
            }
            else if (node is not null)
            {
                yield return (prefix, node);
            }
        }

        /// <summary>
        /// Gets the text of a scalar JSON value (numbers with decimal point, booleans localized).
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="language">The language code.</param>
        /// <returns>Returns the text.</returns>
        public static string ToText(JsonNode? node, string language = "en") => node switch
        {
            null => string.Empty,
            JsonValue value when value.GetValueKind() == JsonValueKind.String => value.GetValue<string>(),
            JsonValue value when value.GetValueKind() == JsonValueKind.True => UsageInsightsMessages.Get(language, "UsageInsightsYes"),
            JsonValue value when value.GetValueKind() == JsonValueKind.False => UsageInsightsMessages.Get(language, "UsageInsightsNo"),
            _ => node.ToJsonString(UsageInsightsJson.Options).Trim('"'),
        };

        /// <summary>
        /// Gets the number of a JSON node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Returns the number, or null when the node is not a number.</returns>
        public static double? ToNumber(JsonNode? node)
        {
            if (node is not JsonValue value || value.GetValueKind() != JsonValueKind.Number)
            {
                return null;
            }

            return double.Parse(value.ToJsonString(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Builds the text of the Data sheet when there are no rows. When the requested period starts AFTER the last day with data, it gives that cause.
        /// </summary>
        /// <param name="response">The response JSON.</param>
        /// <param name="language">The language code.</param>
        /// <returns>Returns the message.</returns>
        public static string BuildNoResultsMessage(JsonNode response, string language)
        {
            var generic = UsageInsightsMessages.Get(language, "UsageInsightsExcelNoResults");
            if (response["DataUntil"]?.GetValue<string>() is not { } dataUntil
                || !DateOnly.TryParseExact(dataUntil, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var limit))
            {
                return generic;
            }

            // One period (From/To) or two (A and B in the comparisons): the cause only holds when ALL of them start after the data.
            var starts = new List<DateOnly>();
            void AddStart(JsonNode? period)
            {
                if (period?["From"]?.GetValue<string>() is { } text
                    && DateOnly.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var day))
                {
                    starts.Add(day);
                }
            }

            var periodNode = response["Period"];
            if (periodNode?["From"] is not null)
            {
                AddStart(periodNode);
            }
            else
            {
                AddStart(periodNode?["A"]);
                AddStart(periodNode?["B"]);
            }

            if (starts.Count == 0 || starts.Any(day => day <= limit))
            {
                return generic;
            }

            var query = response["Query"]?.GetValue<string>() ?? string.Empty;
            var kind = UsageInsightsMessages.Get(language, query is "subject-area-usage" or "compare-subject-areas" or "full-quiz-average"
                ? "UsageInsightsExcelKindTests"
                : "UsageInsightsExcelKindConnections");
            var begins = starts.Count == 1
                ? UsageInsightsMessages.Get(language, "UsageInsightsExcelPeriodStarts", starts[0].ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                : UsageInsightsMessages.Get(language, "UsageInsightsExcelPeriodsStart", starts[0].ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    starts[1].ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            return UsageInsightsMessages.Get(language, "UsageInsightsExcelNoDataAfter", kind, limit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), begins);
        }

        /// <summary>
        /// Checks whether the result was cut (there are more rows than the returned ones). It reads the structured field, never the text.
        /// </summary>
        /// <param name="response">The response JSON.</param>
        /// <returns>Returns true when the list is not complete.</returns>
        public static bool HasMoreRows(JsonNode response) => response["HasMoreRows"]?.GetValue<bool>() ?? false;

        private static string Sanitize(string text)
        {
            var result = new StringBuilder();
            foreach (var character in text.ToLowerInvariant())
            {
                result.Append(character is (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_' or '-' ? character : '_');
            }

            return result.ToString();
        }

        /// <summary>
        /// Builds the file name: query_from_to.xlsx (or the date, or query_a_..._b_...), only [a-z0-9_-].
        /// </summary>
        /// <param name="response">The response JSON.</param>
        /// <param name="extension">The file extension without the dot.</param>
        /// <returns>Returns the file name.</returns>
        public static string BuildFileName(JsonNode response, string extension = "xlsx")
        {
            var parts = new List<string> { response["Query"]?.GetValue<string>() ?? "query" };
            if (response["Period"] is JsonObject period)
            {
                if (period["A"] is not null && period["B"] is not null)
                {
                    parts.AddRange(new[] { "a", ToText(period["A"]!["From"]), ToText(period["A"]!["To"]), "b", ToText(period["B"]!["From"]), ToText(period["B"]!["To"]) });
                }
                else
                {
                    foreach (var key in new[] { "From", "To", "Date", "PeriodFrom" })
                    {
                        if (period[key] is { } value)
                        {
                            parts.Add(ToText(value));
                        }
                    }
                }
            }

            return Sanitize(string.Join('_', parts.Where(part => part.Length > 0))) + "." + extension;
        }

        /// <summary>
        /// Converts a response to the JSON node the caller sees.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>Returns the node.</returns>
        public static JsonNode ToJsonNode(object response) => UsageInsightsJson.ToNode(response);
    }
}
