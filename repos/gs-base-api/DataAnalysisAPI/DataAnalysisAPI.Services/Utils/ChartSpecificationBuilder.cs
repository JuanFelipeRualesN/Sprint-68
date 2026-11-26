using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Builds the chart specification (JSON) of a query result in a deterministic way. It calculates nothing new: Data copies the numbers of Results
    /// as they are (nulls stay null). The client draws it with Recharts: Data = array of flat objects {Category, Series1, Series2...};
    /// every Series[].Key is a key of those objects; Type = "line" | "bar" | "pie"; Layout (bars only) = "horizontal" (columns) | "vertical" (horizontal bars),
    /// like the layout prop of BarChart. Bars with several series carry Grouping = "grouped". A pie carries Variant = "donut" and TotalValue and is only used
    /// with 6 categories or fewer (with more it goes back to bars). Extra = a measure in ANOTHER scale (for example the participation percentage) that travels in
    /// every point of Data for the tooltip and the label, not as a series. IsTimeSeries = the X axis is time.
    /// </summary>
    public static class ChartSpecificationBuilder
    {
        /// <summary>
        /// Maximum number of categories of a donut (with more it goes back to bars).
        /// </summary>
        public const int MaxDonutSlices = 6;

        /// <summary>
        /// Key of the X axis inside every object of Data.
        /// </summary>
        public const string CategoryKey = "Category";

        private sealed record SeriesDefinition(string Key);

        private sealed record Mold(string Type, string? Layout, string Unit, string YAxisKey, SeriesDefinition[] Series, string? OrderKey = null,
            SeriesDefinition? Extra = null, string? ExtraUnit = null, bool IsTimeSeries = false, string? Variant = null);

        private static SeriesDefinition S(string key) => new(key);

        private static readonly Dictionary<string, Mold> Molds = new()
        {
            ["connections-trend"] = new("line", null, "connections", "Connections", new[] { S("Connections") }, IsTimeSeries: true),
            ["cumulative-connections"] = new("line", null, "connections", "Accumulated", new[] { S("CumulativeConnections"), S("CumulativeUsers") }, IsTimeSeries: true),
            ["connection-patterns"] = new("bar", "horizontal", "connections", "Connections", new[] { S("Connections") }),
            ["connections-by-school"] = new("bar", "vertical", "connections", "Connections", new[] { S("Connections") }),
            ["connections-by-teacher"] = new("bar", "vertical", "connections", "Connections", new[] { S("Connections") }),
            // Active and connected users side by side; the participation percentage (another scale) goes in the tooltip and the label, and is the sort criterion.
            ["school-participation"] = new("bar", "vertical", "users", "Users", new[] { S("ActiveUsers"), S("ConnectedUsers") }, "ParticipationPercentage",
                S("ParticipationPercentage"), "%"),
            ["subject-area-usage"] = new("bar", "horizontal", "tests", "Tests", new[] { S("AssignedTests"), S("CompletedTests") }, "CompletedTests"),
            ["compare-subject-areas"] = new("bar", "horizontal", "tests", "CompletedTests", new[] { S("CompletedTestsA"), S("CompletedTestsB") }),
            ["compare-schools"] = new("bar", "horizontal", "connections", "Connections", new[] { S("ConnectionsA"), S("ConnectionsB") }),
            // Assigned and completed tests side by side; the Full Quiz average (another scale) goes in the tooltip and the label, and is the sort criterion.
            ["full-quiz-average"] = new("bar", "vertical", "tests", "Tests", new[] { S("AssignedTests"), S("CompletedTests") }, "AverageCompletedOnly",
                S("AverageCompletedOnly"), "%"),
            ["users-by-subscription"] = new("bar", "vertical", "users", "Users",
                new[] { S("Active"), S("Inactive"), S("Canceled"), S("WithoutSubscription") }, "TotalUsers"),
            ["teacher-activity"] = new("bar", "horizontal", "teachers", "Teachers", new[] { S("Teachers"), S("WithConnections"), S("WithAssignments") }),
        };

        /// <summary>
        /// Checks whether a query has a chart.
        /// </summary>
        /// <param name="queryName">The query name.</param>
        /// <returns>Returns true when the query is chartable.</returns>
        public static bool IsChartable(string queryName) => Molds.ContainsKey(queryName);

        /// <summary>
        /// Builds the chart specification.
        /// BARS: sorted by value (largest first by default), without the categories at 0 (by default) and limited to 10 in the preview;
        /// TotalCategories = the ones that exist with those options, ShownCategories = the ones in Data, ZeroCategories = the ones with value 0.
        /// LINES and weekday patterns: natural order, every category.
        /// </summary>
        /// <param name="response">The response JSON of the query (computed with every row).</param>
        /// <param name="options">The chart options.</param>
        /// <param name="language">The language code of the titles.</param>
        /// <returns>Returns the specification, or null when the query is not chartable or fewer than 2 categories remain.</returns>
        public static JsonObject? Build(JsonNode response, ChartOptions? options, string language)
        {
            var query = response["Query"]?.GetValue<string>() ?? string.Empty;
            if (!Molds.TryGetValue(query, out var mold))
            {
                return null;
            }

            var filters = response["Filters"];

            // groupBy=total: one row per status (Name, Users), not the pivot by school or role. By default a donut with the % of each slice.
            if (query == "users-by-subscription" && Text(filters, "GroupBy", "total") == "total")
            {
                mold = new Mold("pie", "vertical", "users", "Users", new[] { S("Users") }, null, S("PercentageOfTotal"), "%", Variant: "donut");
            }

            if (response["Results"] is not JsonArray rawRows || rawRows.Count < 2)
            {
                return null;
            }

            var chartOptions = options ?? new ChartOptions();
            var rows = rawRows.Select(row => row!).ToList();

            // A series without any data in these rows (for example Inactive when no user has that status) is not drawn and has no legend entry.
            var seriesWithData = mold.Series.Where(series => rows.Any(row => row[series.Key] is not null)).ToArray();
            if (seriesWithData.Length == 0)
            {
                return null;
            }

            if (seriesWithData.Length != mold.Series.Length)
            {
                mold = mold with { Series = seriesWithData };
            }

            var isDonut = mold.Variant == "donut";
            var sorts = (mold.Type == "bar" || isDonut) && !(query == "connection-patterns" && Text(filters, "GroupBy", "weekday") == "weekday");
            var zeroCategories = 0;
            var type = mold.Type;
            if (sorts)
            {
                var key = mold.OrderKey ?? mold.Series[0].Key;
                double ValueOf(JsonNode row) => UsageInsightsColumnFormatter.ToNumber(row[key]) ?? 0;
                bool IsZero(JsonNode row) => mold.Series.All(series => (UsageInsightsColumnFormatter.ToNumber(row[series.Key]) ?? 0) == 0);
                zeroCategories = rows.Count(IsZero);
                if (!chartOptions.IncludeZeros)
                {
                    rows = rows.Where(row => !IsZero(row)).ToList();
                }

                if (isDonut && rows.Count > MaxDonutSlices)
                {
                    type = "bar";   // too many slices: back to bars
                }

                if (type != "pie")   // the donut keeps the natural order of the statuses
                {
                    rows = (chartOptions.Order == "asc" ? rows.OrderBy(ValueOf) : rows.OrderByDescending(ValueOf))
                        .ThenBy(row => GetCategory(query, row), StringComparer.Ordinal).ToList();
                }
            }

            var totalCategories = rows.Count;
            if (sorts && type == "bar" && !chartOptions.All)
            {
                rows = rows.Take(chartOptions.Limit).ToList();
            }

            if (rows.Count < 2)
            {
                return null;
            }

            var labels = rows.Select(row => GetCategory(query, row)).ToList();

            // Repeated labels (two teachers with the same name; a school in several academic periods) are told apart so Recharts does not mix them up.
            if (labels.Distinct().Count() != labels.Count)
            {
                if (query == "teacher-activity")
                {
                    labels = rows.Select(row => $"{UsageInsightsColumnFormatter.ToText(row["School"])} ({UsageInsightsColumnFormatter.ToText(row["PeriodFrom"])})").ToList();
                }
                else if (query == "connections-by-teacher")
                {
                    labels = rows.Select(row => $"{UsageInsightsColumnFormatter.ToText(row["Name"])} ({UsageInsightsColumnFormatter.ToText(row["School"])})").ToList();
                }
            }

            if (labels.Distinct().Count() != labels.Count)   // still repeated (same name and same school): they are numbered
            {
                var seen = new Dictionary<string, int>();
                labels = labels.Select(label => seen[label] = seen.GetValueOrDefault(label) + 1).Zip(labels, (number, label) => number == 1 ? label : $"{label} #{number}").ToList();
            }

            var data = new JsonArray();   // flat objects for Recharts; null stays null
            for (var i = 0; i < rows.Count; i++)
            {
                var point = new JsonObject { [CategoryKey] = labels[i] };
                foreach (var series in mold.Series)
                {
                    point[series.Key] = rows[i][series.Key]?.DeepClone();
                }

                if (mold.Extra is { } extra)
                {
                    point[extra.Key] = rows[i][extra.Key]?.DeepClone();   // another scale: not a series
                }

                data.Add(point);
            }

            var seriesArray = new JsonArray();
            foreach (var series in mold.Series)
            {
                seriesArray.Add(new JsonObject { ["Key"] = series.Key, ["Name"] = GetSeriesName(query, series, response, language) });
            }

            string Message(string key, params object?[] arguments) => UsageInsightsMessages.Get(language, key, arguments);
            var specification = new JsonObject
            {
                ["Query"] = query,
                ["Type"] = type,
                ["Title"] = GetTitle(query, filters, language),
                ["XAxisKey"] = CategoryKey,
                ["XAxisLabel"] = GetXAxisLabel(query, filters, language),
                ["YAxisLabel"] = Message("UsageInsightsChartAxis" + mold.YAxisKey),
                ["Unit"] = mold.Unit,
                ["Series"] = seriesArray,
                ["Data"] = data,
                ["Period"] = response["Period"]?.DeepClone(),
                ["Filters"] = filters?.DeepClone(),
                ["DataUntil"] = response["DataUntil"]?.DeepClone(),
                ["Note"] = response["Note"]?.DeepClone(),
                ["Rows"] = rows.Count,
                ["TotalCategories"] = totalCategories,
                ["ShownCategories"] = rows.Count,
                ["HasMoreRows"] = UsageInsightsColumnFormatter.HasMoreRows(response),
            };
            if (sorts)
            {
                specification["ZeroCategories"] = zeroCategories;
                specification["IncludeZeros"] = chartOptions.IncludeZeros;
                if (type == "bar")
                {
                    specification["Order"] = chartOptions.Order;
                    specification["All"] = chartOptions.All;
                }
            }

            if (type == "bar" && mold.Layout is not null)
            {
                specification["Layout"] = mold.Layout;   // only in bar (omitted in line and pie)
            }

            if (type == "bar" && mold.Series.Length > 1)
            {
                specification["Grouping"] = "grouped";   // several series = grouped bars (side by side)
            }

            if (type == "pie")
            {
                specification["Variant"] = "donut";
                specification["TotalValue"] = rows.Sum(row => UsageInsightsColumnFormatter.ToNumber(row[mold.Series[0].Key]) ?? 0);   // the center total of the donut is calculated by the service
            }

            if (mold.Extra is { } extraSeries)
            {
                specification["Extra"] = new JsonObject
                {
                    ["Key"] = extraSeries.Key,
                    ["Name"] = UsageInsightsColumnFormatter.GetColumnTitle(extraSeries.Key, language),
                    ["Unit"] = mold.ExtraUnit ?? string.Empty,
                };
            }

            if (mold.IsTimeSeries)
            {
                specification["IsTimeSeries"] = true;
            }

            return specification;
        }

        private static string GetCategory(string query, JsonNode row) => query switch
        {
            "connections-trend" or "cumulative-connections" => UsageInsightsColumnFormatter.ToText(row["Period"]),
            "teacher-activity" => UsageInsightsColumnFormatter.ToText(row["School"]),
            "full-quiz-average" => string.Join(" · ", new[] { row["School"], row["Subject"] }.Select(node => UsageInsightsColumnFormatter.ToText(node)).Where(text => text.Length > 0)),
            _ => UsageInsightsColumnFormatter.ToText(row["Name"]),
        };

        private static string GetSeriesName(string query, SeriesDefinition series, JsonNode response, string language)
        {
            if (query is "compare-subject-areas" or "compare-schools")
            {
                var side = series.Key.EndsWith("A", StringComparison.Ordinal) ? "A" : "B";
                var period = response["Period"]?[side];
                return UsageInsightsMessages.Get(language, "UsageInsightsChartPeriodSeries", side, UsageInsightsColumnFormatter.ToText(period?["From"]),
                    UsageInsightsColumnFormatter.ToText(period?["To"]));
            }

            return UsageInsightsColumnFormatter.GetColumnTitle(series.Key, language);
        }

        private static string GetTitle(string query, JsonNode? filters, string language)
        {
            string Message(string key, params object?[] arguments) => UsageInsightsMessages.Get(language, key, arguments);
            return query switch
            {
                "connections-trend" => Message("UsageInsightsChartTitleConnectionsTrend", Message("UsageInsightsGranularity" + Capitalize(Text(filters, "Granularity", "month")))),
                "connection-patterns" => Message(Text(filters, "GroupBy", "weekday") == "role" ? "UsageInsightsChartTitleConnectionPatternsRole" : "UsageInsightsChartTitleConnectionPatternsWeekday"),
                "users-by-subscription" => Message("UsageInsightsChartTitleUsersBySubscription" + Capitalize(Text(filters, "GroupBy", "total"))),
                _ => Message("UsageInsightsChartTitle" + ToPascalCase(query)),
            };
        }

        private static string GetXAxisLabel(string query, JsonNode? filters, string language)
        {
            var key = query switch
            {
                "connections-trend" or "cumulative-connections" => "Period",
                "connection-patterns" => Text(filters, "GroupBy", "weekday") == "role" ? "Role" : "Weekday",
                "connections-by-teacher" => "Teacher",
                "users-by-subscription" => Text(filters, "GroupBy", "total") switch { "role" => "Role", "school" => "School", _ => "Status" },
                "subject-area-usage" or "compare-subject-areas" => "Subject",
                "full-quiz-average" => Text(filters, "GroupBy", "school_subject") switch { "school" => "School", "subject" => "Subject", _ => "SchoolSubject" },
                _ => "School",
            };
            return UsageInsightsMessages.Get(language, "UsageInsightsChartAxis" + key);
        }

        private static string Text(JsonNode? filters, string key, string defaultValue) => filters?[key] is { } value ? UsageInsightsColumnFormatter.ToText(value) : defaultValue;

        private static string Capitalize(string text) => text.Length == 0 ? text : char.ToUpperInvariant(text[0]) + text[1..];

        private static string ToPascalCase(string kebabCase) => string.Concat(kebabCase.Split('-').Select(Capitalize));
    }
}
