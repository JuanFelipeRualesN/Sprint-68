using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Utils;
using Microsoft.Extensions.Logging;
using Dal = DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper.UsageInsights;

namespace DataAnalysisAPI.DataAnalysisAPI.Services
{
    /// <summary>
    /// Logic of the 13 usage insights queries: validates the parameters, asks the database, and builds the response with totals, percentages,
    /// ties, "has more rows" and the localized notes. The routes and the exports share this code, so they share validation and figures.
    /// Nothing is detected by text: warnings travel as structured fields (Notices, HasMoreRows, Summary.Ties).
    /// </summary>
    public static class UsageInsights
    {
        /// <summary>
        /// Internal cap of the "complete" schools queries (the whole list is read to give exact totals, percentages and ties).
        /// </summary>
        public const int SchoolsCap = 1000;

        /// <summary>
        /// Internal cap of the "complete" teachers query.
        /// </summary>
        public const int TeachersCap = 5000;

        private delegate UsageInsightResponse QueryHandler(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language);

        private static readonly Dictionary<string, QueryHandler> Handlers = new(StringComparer.Ordinal)
        {
            ["subject-area-usage"] = GetSubjectAreaUsage,
            ["search-schools"] = SearchSchools,
            ["connections-by-school"] = GetConnectionsBySchool,
            ["connections-by-teacher"] = GetConnectionsByTeacher,
            ["compare-subject-areas"] = CompareSubjectAreas,
            ["compare-schools"] = CompareSchools,
            ["full-quiz-average"] = GetFullQuizAverage,
            ["connections-trend"] = GetConnectionsTrend,
            ["cumulative-connections"] = GetCumulativeConnections,
            ["teacher-activity"] = GetTeacherActivity,
            ["connection-patterns"] = GetConnectionPatterns,
            ["school-participation"] = GetSchoolParticipation,
            ["users-by-subscription"] = GetUsersBySubscription,
        };

        /// <summary>
        /// Gets the names of the queries (the route names).
        /// </summary>
        public static IReadOnlyCollection<string> QueryNames => Handlers.Keys;

        /// <summary>
        /// Checks whether a query name exists.
        /// </summary>
        /// <param name="queryName">The query name.</param>
        /// <returns>Returns true when the query exists.</returns>
        public static bool Exists(string queryName) => Handlers.ContainsKey(queryName);

        /// <summary>
        /// Runs a query with the scope of the caller and declares that scope in the response. No query receives an unscoped context from an HTTP route.
        /// </summary>
        /// <param name="queryName">The query name.</param>
        /// <param name="parser">The request parameters.</param>
        /// <param name="context">The query context (scope included).</param>
        /// <param name="language">The language code of the texts ("es" or "en").</param>
        /// <returns>Returns the response.</returns>
        public static UsageInsightResponse Execute(string queryName, UsageInsightsQueryParser parser, UsageInsightsQueryContext context, string language)
        {
            if (!Handlers.TryGetValue(queryName, out var handler))
            {
                throw new InvalidQueryParameterException("query", "UnknownQuery", queryName);
            }

            var response = handler(context, parser, language);
            return response with { Scope = DescribeScope(context, language) };
        }

        /// <summary>
        /// Gets the last day with data of each family of queries.
        /// </summary>
        /// <param name="context">The query context.</param>
        /// <returns>Returns the data status.</returns>
        public static UsageInsightsDataStatus GetDataStatus(UsageInsightsQueryContext context) =>
            new(GetTestsDataUntil(context), GetConnectionsDataUntil(context), UsageInsightsSettings.SourceName);

        /// <summary>
        /// Writes one line to the log when a database query fails: query name, exception type, MySQL code, a sanitized message (see <see cref="SanitizeEngineMessage"/>), and the start of the SQL
        /// WITHOUT parameters, data or Token.
        /// </summary>
        /// <param name="queryName">The query name.</param>
        /// <param name="exception">The failure.</param>
        /// <param name="logger">The logger.</param>
        public static void LogQueryFailure(string queryName, MySqlConnector.MySqlException exception, Microsoft.Extensions.Logging.ILogger logger)
        {
            logger.LogWarning("usage insights query failed. query={Query} type={Type} mysqlCode={Code} sqlState={State} message=\"{Message}\" sql=\"{Sql}\"",
                queryName, exception.GetType().Name, exception.Number, exception.SqlState, SanitizeEngineMessage(exception.Message), Shorten(exception.Data["sql_start"] as string, 80));
        }

        /// <summary>
        /// Makes a MySQL engine message safe to log: some messages quote values of the data (for example a duplicate or a truncated value), so every quoted text
        /// is replaced by a placeholder, the whitespace is collapsed and the length is capped. The error code and the SQL state travel separately.
        /// </summary>
        /// <param name="message">The message of the engine.</param>
        /// <returns>Returns the message without quoted values.</returns>
        public static string SanitizeEngineMessage(string? message)
        {
            var withoutValues = System.Text.RegularExpressions.Regex.Replace(message ?? string.Empty, @"'[^']*'|""[^""]*""|`[^`]*`", "'?'");
            return Shorten(withoutValues, 200);
        }

        private static string Shorten(string? text, int max)
        {
            var singleLine = System.Text.RegularExpressions.Regex.Replace(text ?? string.Empty, @"\s+", " ").Trim();
            return singleLine.Length > max ? singleLine[..max] + "..." : singleLine;
        }

        /// <summary>
        /// Forgets the cached data-until dates (used by the tests).
        /// </summary>
        public static void ClearCache() => DataUntilCache.Clear();

        // ── Shared pieces ───────────────────────────────────────────────────────────────

        private static readonly ConcurrentDictionary<string, (DateTime Expires, DateOnly? Tests, DateOnly? Connections)> DataUntilCache = new();

        private static (DateOnly? Tests, DateOnly? Connections) ReadDataUntil(UsageInsightsQueryContext context)
        {
            // "Data until" belongs to the whole database (it reveals nothing about the rest), so it is computed with the all-schools scope and cached.
            var rootContext = context with { Scope = DataScope.All };
            var key = string.Join("|", rootContext.Exclusion.IsActive, string.Join(",", rootContext.Exclusion.Ores), string.Join(",", rootContext.Exclusion.EmailDomains),
                string.Join(",", rootContext.Exclusion.Schools), rootContext.UtcOffsetMinutes);
            if (DataUntilCache.TryGetValue(key, out var cached) && cached.Expires > DateTime.UtcNow)
            {
                return (cached.Tests, cached.Connections);
            }

            var tests = Dal.GetTestsDataUntil(rootContext);
            var connections = Dal.GetConnectionsDataUntil(rootContext);
            DataUntilCache[key] = (DateTime.UtcNow.AddMinutes(10), tests, connections);
            return (tests, connections);
        }

        private static DateOnly? GetTestsDataUntil(UsageInsightsQueryContext context) => ReadDataUntil(context).Tests;

        private static DateOnly? GetConnectionsDataUntil(UsageInsightsQueryContext context) => ReadDataUntil(context).Connections;

        private static string Message(string language, string key, params object?[] arguments) => UsageInsightsMessages.Get(language, key, arguments);

        private static string DescribeScope(UsageInsightsQueryContext context, string language)
        {
            if (Dal.GetScopeName(context) is not { } scope)
            {
                return Message(language, "UsageInsightsScopeAllSchools");
            }

            return Message(language, scope.IsSchool ? "UsageInsightsScopeSchool" : "UsageInsightsScopeOre", scope.Name);
        }

        private static string DescribeTimeZone(UsageInsightsQueryContext context, string language) => context.UtcOffsetMinutes switch
        {
            0 => "UTC",
            -240 => Message(language, "UsageInsightsTimeZonePuertoRico"),
            var offset => $"UTC{(offset > 0 ? "+" : "-")}{Math.Abs(offset) / 60.0:0.##}",
        };

        private static string TestsDateColumn(UsageInsightsQueryContext context, string language) =>
            Message(language, "UsageInsightsDateColumnTests", DescribeTimeZone(context, language));

        private static string ConnectionsDateColumn(UsageInsightsQueryContext context, string language) =>
            Message(language, "UsageInsightsDateColumnConnections", DescribeTimeZone(context, language));

        private static string ExclusionNote(UsageInsightsQueryContext context, string language)
        {
            var exclusion = context.Exclusion;
            if (!exclusion.IsActive)
            {
                return Message(language, "UsageInsightsExclusionOff");
            }

            var parts = new List<string>();
            if (exclusion.Ores.Length > 0)
            {
                parts.Add(Message(language, "UsageInsightsExclusionOres", string.Join("», «", exclusion.Ores)));
            }

            if (exclusion.Schools.Length > 0)
            {
                parts.Add(Message(language, "UsageInsightsExclusionSchools", string.Join("», «", exclusion.Schools)));
            }

            if (exclusion.EmailDomains.Length > 0)
            {
                parts.Add(Message(language, "UsageInsightsExclusionDomains", string.Join(" / @", exclusion.EmailDomains)));
            }

            return Message(language, "UsageInsightsExclusionOn", string.Join(", ", parts));
        }

        private static string TestAccountsValue(UsageInsightsQueryContext context) => context.Exclusion.IsActive ? "excluded" : "included";

        private static object PeriodOf(DateOnly from, DateOnly to) => new { From = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), To = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) };

        private static Dictionary<string, object?> Map(params (string Key, object? Value)[] items)
        {
            var map = new Dictionary<string, object?>();
            foreach (var (key, value) in items)
            {
                if (value is not null)
                {
                    map[key] = value;   // like the rest of the response, null values are omitted
                }
            }

            return map;
        }

        private static string OrderText(bool descending) => descending ? "desc" : "asc";

        /// <summary>
        /// Warns when the requested period ends after the last day with data, so the caller does not hide it.
        /// </summary>
        private static string? DataUntilNotice(DateOnly to, DateOnly? dataUntil, string language) =>
            dataUntil is { } limit && to > limit
                ? Message(language, "UsageInsightsNoticeDataUntil", limit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                : null;

        /// <summary>
        /// When the value of the last shown row repeats in rows that stayed out of the limit, the list is not exhaustive.
        /// </summary>
        private static string? TiesNotice<T>(List<T> all, int limit, Func<T, int> value, string language, bool isComplete = false)
        {
            if (all.Count <= limit)
            {
                return null;
            }

            var lastValue = value(all[limit - 1]);
            var more = all.Skip(limit).TakeWhile(row => value(row) == lastValue).Count();
            if (more == 0)
            {
                return null;
            }

            var atLeast = !isComplete && all.Count >= 50 && more == all.Count - limit;
            return Message(language, atLeast ? "UsageInsightsNoticeTiesAtLeast" : "UsageInsightsNoticeTies", more, lastValue);
        }

        /// <summary>The list was asked with cap+1 rows: if more than the cap came back, it is cut to the cap.</summary>
        private static (List<T> List, bool IsCut) ApplyCap<T>(List<T> requested, int cap) =>
            requested.Count > cap ? (requested.Take(cap).ToList(), true) : (requested, false);

        private static string? CapNotice(bool isCut, string noun, int cap, string language) =>
            isCut ? Message(language, "UsageInsightsNoticeCap", cap, Message(language, noun)) : null;

        private static List<string> Notices(params string?[] notices) => notices.Where(notice => !string.IsNullOrEmpty(notice)).Select(notice => notice!).ToList();

        // ── subject-area-usage ──────────────────────────────────────────────────────────

        private static UsageInsightResponse GetSubjectAreaUsage(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var (descending, limit) = parser.ParseOrderAndLimit();
            var all = Dal.GetSubjectAreaUsage(context, from, to, descending, SchoolsCap);
            int assigned = all.Sum(row => row.AssignedTests), completed = all.Sum(row => row.CompletedTests);
            var rows = all.Take(limit).Select(row => row with { PercentageOfTotalCompleted = UsageInsightsQueryParser.Percentage(row.CompletedTests, completed) }).ToList();
            var dataUntil = GetTestsDataUntil(context);
            return new UsageInsightResponse("subject-area-usage", UsageInsightsSettings.SourceName, TestsDateColumn(context, language), PeriodOf(from, to),
                Map(("Order", OrderText(descending)), ("Limit", limit), ("School", "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                Message(language, "UsageInsightsNoteSubjectArea", ExclusionNote(context, language)),
                Map(("TotalAssignedTests", assigned), ("TotalCompletedTests", completed), ("GlobalCompletedPercentage", UsageInsightsQueryParser.Percentage(completed, assigned)),
                    ("AreasConsidered", all.Count), ("AreasWithAssignedTests", all.Count(row => row.AssignedTests > 0)),
                    ("AreasWithoutAssignedTests", all.Count(row => row.AssignedTests == 0))))
            {
                HasMoreRows = all.Count > rows.Count,
                Notices = Notices(DataUntilNotice(to, dataUntil, language)),
            };
        }

        // ── search-schools ──────────────────────────────────────────────────────────────

        private static UsageInsightResponse SearchSchools(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var text = parser.GetValue("text")?.Trim();
            if (string.IsNullOrEmpty(text) || text.Length < 2)
            {
                throw new InvalidQueryParameterException("text", "TextTooShort");
            }

            var (_, limit) = parser.ParseOrderAndLimit();
            return new UsageInsightResponse("search-schools", UsageInsightsSettings.SourceName, Message(language, "UsageInsightsNotApplicable"),
                Message(language, "UsageInsightsNotApplicable"), Map(("Text", text), ("TestAccounts", TestAccountsValue(context))), null,
                Dal.SearchSchools(context, text, limit),
                UsageInsightsMessages.Join(Message(language, "UsageInsightsNoteSearchSchools"), ConnectionsNote(context, language)));
        }

        // ── connections-by-school ───────────────────────────────────────────────────────

        private static string ConnectionsNote(UsageInsightsQueryContext context, string language) =>
            Message(language, "UsageInsightsNoteConnections", ExclusionNote(context, language));

        private static UsageInsightResponse GetConnectionsBySchool(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var (descending, limit) = parser.ParseOrderAndLimit();
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var (minConnections, maxConnections) = parser.ParseConnectionsRange();
            var (allSchools, isCut) = ApplyCap(Dal.GetConnectionsBySchool(context, from, to, descending, SchoolsCap + 1), SchoolsCap);
            var total = allSchools.Sum(row => row.Connections);
            var candidates = school is null ? allSchools : allSchools.Where(row => row.Name == school).ToList();
            var hasCountFilter = minConnections > 0 || maxConnections < int.MaxValue;
            if (hasCountFilter)
            {
                candidates = candidates.Where(row => row.Connections >= minConnections && row.Connections <= maxConnections).ToList();
            }

            var rows = candidates.Take(limit).Select(row => row with
            {
                PercentageOfTotal = UsageInsightsQueryParser.Percentage(row.Connections, total),
                ConnectionsPerUser = row.ConnectedUsers == 0 ? null : Math.Round((double)row.Connections / row.ConnectedUsers, 1),
            }).ToList();
            var ties = TiesNotice(candidates, limit, row => row.Connections, language, isComplete: true);
            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("connections-by-school", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language), PeriodOf(from, to),
                Map(("Order", OrderText(descending)), ("Limit", limit), ("School", school ?? "all"),
                    ("MinConnections", hasCountFilter ? minConnections : null), ("MaxConnections", hasCountFilter && maxConnections < int.MaxValue ? maxConnections : null),
                    ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                UsageInsightsMessages.Join(ConnectionsNote(context, language), hasCountFilter ? Message(language, "UsageInsightsNoteConnectionsCountFilter") : null),
                Map(("TotalConnections", total), ("SchoolsConsidered", allSchools.Count), ("SchoolsWithConnections", allSchools.Count(row => row.Connections > 0)),
                    ("SchoolsWithoutConnections", allSchools.Count(row => row.Connections == 0)), ("SchoolsMeetingTheFilter", hasCountFilter ? candidates.Count : null),
                    ("Ties", ties is not null)))
            {
                HasMoreRows = isCut || candidates.Count > rows.Count,
                Notices = Notices(DataUntilNotice(to, dataUntil, language), ties, CapNotice(isCut, "UsageInsightsNounSchools", SchoolsCap, language)),
            };
        }

        // ── connections-by-teacher ──────────────────────────────────────────────────────

        private static UsageInsightResponse GetConnectionsByTeacher(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var (descending, limit) = parser.ParseOrderAndLimit();
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var (minConnections, maxConnections) = parser.ParseConnectionsRange();
            var (allTeachers, isCut) = ApplyCap(Dal.GetConnectionsByTeacher(context, from, to, descending, TeachersCap + 1, school), TeachersCap);
            var total = allTeachers.Sum(row => row.Connections);
            var hasCountFilter = minConnections > 0 || maxConnections < int.MaxValue;
            var candidates = hasCountFilter ? allTeachers.Where(row => row.Connections >= minConnections && row.Connections <= maxConnections).ToList() : allTeachers;
            var rows = candidates.Take(limit).Select(row => row with { PercentageOfTotal = UsageInsightsQueryParser.Percentage(row.Connections, total) }).ToList();
            var ties = TiesNotice(candidates, limit, row => row.Connections, language, isComplete: true);
            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("connections-by-teacher", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language), PeriodOf(from, to),
                Map(("Order", OrderText(descending)), ("Limit", limit), ("School", school ?? "all"),
                    ("MinConnections", hasCountFilter ? minConnections : null), ("MaxConnections", hasCountFilter && maxConnections < int.MaxValue ? maxConnections : null),
                    ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                UsageInsightsMessages.Join(ConnectionsNote(context, language), hasCountFilter ? Message(language, "UsageInsightsNoteConnectionsCountFilter") : null),
                Map(("TotalTeacherConnections", total), ("TeachersConsidered", allTeachers.Count), ("TeachersWithConnections", allTeachers.Count(row => row.Connections > 0)),
                    ("TeachersWithoutConnections", allTeachers.Count(row => row.Connections == 0)), ("TeachersMeetingTheFilter", hasCountFilter ? candidates.Count : null),
                    ("Ties", ties is not null)))
            {
                HasMoreRows = isCut || candidates.Count > rows.Count,
                Notices = Notices(DataUntilNotice(to, dataUntil, language), ties, CapNotice(isCut, "UsageInsightsNounTeachers", TeachersCap, language)),
            };
        }

        // ── compare-subject-areas ───────────────────────────────────────────────────────

        private static UsageInsightResponse CompareSubjectAreas(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (fromA, toA) = parser.ParsePeriod("fromA", "toA");
            var (fromB, toB) = parser.ParsePeriod("fromB", "toB");
            var dataUntil = GetTestsDataUntil(context);
            return new UsageInsightResponse("compare-subject-areas", UsageInsightsSettings.SourceName, TestsDateColumn(context, language),
                new { A = PeriodOf(fromA, toA), B = PeriodOf(fromB, toB) }, Map(("School", "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil,
                Dal.CompareSubjectAreas(context, fromA, toA, fromB, toB),
                UsageInsightsMessages.Join(Message(language, "UsageInsightsNoteSubjectArea", ExclusionNote(context, language)), Message(language, "UsageInsightsNoteUnequalPeriods")))
            {
                Notices = Notices(DataUntilNotice(UsageInsightsQueryParser.MaxDate(toA, toB), dataUntil, language)),
            };
        }

        // ── compare-schools ─────────────────────────────────────────────────────────────

        private static UsageInsightResponse CompareSchools(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (fromA, toA) = parser.ParsePeriod("fromA", "toA");
            var (fromB, toB) = parser.ParsePeriod("fromB", "toB");
            var (descending, limit) = parser.ParseOrderAndLimit();
            var (listA, cutA) = ApplyCap(Dal.GetConnectionsBySchool(context, fromA, toA, true, SchoolsCap + 1), SchoolsCap);
            var (listB, cutB) = ApplyCap(Dal.GetConnectionsBySchool(context, fromB, toB, true, SchoolsCap + 1), SchoolsCap);
            var valuesA = listA.ToDictionary(row => row.Name, row => row.Connections);
            var valuesB = listB.ToDictionary(row => row.Name, row => row.Connections);
            var compared = valuesA.Keys.Union(valuesB.Keys).Select(name =>
            {
                int a = valuesA.GetValueOrDefault(name), b = valuesB.GetValueOrDefault(name), difference = b - a;
                return new SchoolComparisonRow(name, a, b, difference, a == 0 ? null : Math.Round(100.0 * difference / a, 1));
            }).Where(row => row.ConnectionsA > 0 || row.ConnectionsB > 0).ToList();
            var ordered = (descending
                ? compared.OrderByDescending(row => row.Difference).ThenByDescending(row => row.Name, StringComparer.Ordinal)
                : compared.OrderBy(row => row.Difference).ThenBy(row => row.Name, StringComparer.Ordinal)).ToList();
            int totalA = valuesA.Values.Sum(), totalB = valuesB.Values.Sum();
            var rows = ordered.Take(limit).ToList();
            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("compare-schools", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language),
                new { A = PeriodOf(fromA, toA), B = PeriodOf(fromB, toB) },
                Map(("Order", OrderText(descending)), ("Limit", limit), ("School", "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                UsageInsightsMessages.Join(ConnectionsNote(context, language), Message(language, "UsageInsightsNoteCompareSchools")),
                Map(("TotalConnectionsA", totalA), ("TotalConnectionsB", totalB), ("TotalDifference", totalB - totalA),
                    ("TotalVariationPercentage", totalA == 0 ? null : Math.Round(100.0 * (totalB - totalA) / totalA, 1)), ("SchoolsCompared", ordered.Count)))
            {
                HasMoreRows = cutA || cutB || ordered.Count > rows.Count,
                Notices = Notices(DataUntilNotice(UsageInsightsQueryParser.MaxDate(toA, toB), dataUntil, language),
                    CapNotice(cutA || cutB, "UsageInsightsNounSchoolsInOnePeriod", SchoolsCap, language)),
            };
        }

        // ── full-quiz-average ───────────────────────────────────────────────────────────

        private static UsageInsightResponse GetFullQuizAverage(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var (descending, limit) = parser.ParseOrderAndLimit();
            var groupBy = parser.ParseChoice("groupBy", "school_subject", "school", "subject", "school_subject");
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var all = Dal.GetFullQuizAverage(context, from, to, groupBy, descending, limit + 1, school);   // one extra row tells whether the list was cut
            var hasMore = all.Count > limit;
            var rows = all.Take(limit).ToList();
            var dataUntil = GetTestsDataUntil(context);
            return new UsageInsightResponse("full-quiz-average", UsageInsightsSettings.SourceName, TestsDateColumn(context, language), PeriodOf(from, to),
                Map(("GroupBy", groupBy), ("Order", OrderText(descending)), ("Limit", limit), ("School", school ?? "all"), ("TestAccounts", TestAccountsValue(context))),
                dataUntil, rows, Message(language, "UsageInsightsNoteFullQuizAverage", ExclusionNote(context, language)))
            {
                HasMoreRows = hasMore,
                Notices = Notices(DataUntilNotice(to, dataUntil, language), hasMore ? Message(language, "UsageInsightsNoticeLimitReached", limit, parser.MaxLimit) : null),
            };
        }

        // ── connections-trend ───────────────────────────────────────────────────────────

        private static string BuildPeriodKey(DateOnly day, string granularity) => granularity switch
        {
            "day" => day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            "month" => $"{day.Year}-{day.Month:00}",
            "quarter" => $"{day.Year}-T{(day.Month - 1) / 3 + 1}",
            _ => day.Year.ToString(CultureInfo.InvariantCulture),
        };

        private static UsageInsightResponse GetConnectionsTrend(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var granularity = parser.ParseChoice("granularity", "month", "month", "quarter", "year");
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var perDay = Dal.GetConnectionsTrend(context, from, to, school);
            var accumulated = new SortedDictionary<string, int>(StringComparer.Ordinal);
            for (var day = from; day <= to; day = day.AddDays(1))
            {
                accumulated.TryAdd(BuildPeriodKey(day, granularity), 0);   // includes the periods with zero connections
            }

            foreach (var (day, connections) in perDay)
            {
                accumulated[BuildPeriodKey(day, granularity)] += connections;
            }

            var series = accumulated.Select(item => new ConnectionsTrendRow(item.Key, item.Value)).ToList();
            var withVariation = series.Select((row, index) => index == 0 ? row : row with
            {
                DifferenceWithPreviousPeriod = row.Connections - series[index - 1].Connections,
                VariationPercentage = series[index - 1].Connections == 0 ? null : Math.Round(100.0 * (row.Connections - series[index - 1].Connections) / series[index - 1].Connections, 1),
            }).ToList();
            var highest = series.OrderByDescending(row => row.Connections).ThenBy(row => row.Period, StringComparer.Ordinal).FirstOrDefault();
            var lowest = series.OrderBy(row => row.Connections).ThenBy(row => row.Period, StringComparer.Ordinal).FirstOrDefault();
            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("connections-trend", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language), PeriodOf(from, to),
                Map(("Granularity", granularity), ("School", school ?? "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil, withVariation,
                UsageInsightsMessages.Join(ConnectionsNote(context, language), Message(language, "UsageInsightsNoteTrend")),
                Map(("TotalConnections", series.Sum(row => row.Connections)), ("PeriodWithMostConnections", highest?.Period), ("ConnectionsOfTheHighest", highest?.Connections),
                    ("PeriodWithFewestConnections", lowest?.Period), ("ConnectionsOfTheLowest", lowest?.Connections)))
            {
                Notices = Notices(DataUntilNotice(to, dataUntil, language)),
            };
        }

        // ── cumulative-connections ──────────────────────────────────────────────────────

        private static UsageInsightResponse GetCumulativeConnections(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var granularity = parser.ParseChoice("granularity", "month", "day", "month", "quarter", "year");
            if (granularity == "day" && to.DayNumber - from.DayNumber > 92)
            {
                throw new InvalidQueryParameterException("granularity", "DayRangeTooLong");
            }

            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var perPeriod = new SortedDictionary<string, (int Connections, int NewUsers)>(StringComparer.Ordinal);
            for (var day = from; day <= to; day = day.AddDays(1))
            {
                perPeriod.TryAdd(BuildPeriodKey(day, granularity), (0, 0));   // includes the periods with zero connections
            }

            foreach (var row in Dal.GetDailyConnectionsAndNewUsers(context, from, to, school))
            {
                var key = BuildPeriodKey(row.Day, granularity);
                perPeriod[key] = (perPeriod[key].Connections + row.Connections, perPeriod[key].NewUsers + row.NewUsers);
            }

            int cumulativeConnections = 0, cumulativeUsers = 0;
            var rows = perPeriod.Select(item =>
            {
                cumulativeConnections += item.Value.Connections;
                cumulativeUsers += item.Value.NewUsers;
                return new CumulativeConnectionsRow(item.Key, item.Value.Connections, cumulativeConnections, item.Value.NewUsers, cumulativeUsers);
            }).ToList();
            var active = school is null ? Dal.GetActiveUsersCount(context) : null;
            static double? Percentage2(int part, int total) => total == 0 ? null : Math.Round(100.0 * part / total, 2);
            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("cumulative-connections", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language), PeriodOf(from, to),
                Map(("Granularity", granularity), ("School", school ?? "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                UsageInsightsMessages.Join(ConnectionsNote(context, language), Message(language, "UsageInsightsNoteCumulative"),
                    school is null ? Message(language, "UsageInsightsNoteActiveUsers") : Message(language, "UsageInsightsNoteCumulativeSchoolFilter")),
                Map(("TotalConnections", cumulativeConnections), ("ConnectedUsers", cumulativeUsers), ("ActiveUsers", active?.ActiveUsers),
                    ("ConnectionUsersPercentage", active is null ? null : Percentage2(cumulativeUsers, active.ActiveUsers)),
                    ("EverConnectedUsers", active?.EverConnectedUsers),
                    ("EverConnectedPercentage", active is null ? null : Percentage2(active.EverConnectedUsers, active.ActiveUsers))))
            {
                Notices = Notices(DataUntilNotice(to, dataUntil, language)),
            };
        }

        // ── teacher-activity ────────────────────────────────────────────────────────────

        private static UsageInsightResponse GetTeacherActivity(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var periodFrom = parser.ParseOptionalDate("periodFrom");
            var date = periodFrom is null
                ? parser.ParseOptionalDate("date") ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMinutes(context.UtcOffsetMinutes))   // today in the local day
                : (DateOnly?)null;
            var (_, limit) = parser.ParseOrderAndLimit();
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var (all, total) = Dal.GetTeacherActivity(context, date, periodFrom, school);
            var rows = all.Take(limit).ToList();
            object selection = periodFrom is { } from
                ? new { PeriodFrom = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) }
                : new { Date = date!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) };
            var hasMore = all.Count > limit;
            return new UsageInsightResponse("teacher-activity", UsageInsightsSettings.SourceName, Message(language, "UsageInsightsDateColumnTeacherActivity"), selection,
                Map(("Limit", limit), ("School", school ?? "all"), ("TestAccounts", TestAccountsValue(context))), null, rows,
                Message(language, "UsageInsightsNoteTeacherActivity", ExclusionNote(context, language)),
                Map(("Schools", all.Select(row => row.School).Distinct().Count()), ("SchoolPeriodCombinations", all.Count), ("Teachers", total.Teachers),
                    ("WithConnections", total.WithConnections), ("WithPlans", total.WithPlans), ("PlansWithData", total.PlansWithData),
                    ("WithGroups", total.WithGroups), ("WithAssignments", total.WithAssignments), ("TeachersWithoutConnections", total.Teachers - total.WithConnections)))
            {
                HasMoreRows = hasMore,
                Notices = Notices(all.Count == 0 ? Message(language, "UsageInsightsNoticeNoTeacherData") : null,
                    hasMore ? Message(language, "UsageInsightsNoticeSummaryHasTotal", limit, all.Count, Message(language, "UsageInsightsNounSchoolPeriods")) : null),
            };
        }

        // ── connection-patterns ─────────────────────────────────────────────────────────

        private static UsageInsightResponse GetConnectionPatterns(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var groupBy = parser.ParseChoice("groupBy", "weekday", "weekday", "role");
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var all = Dal.GetConnectionPatterns(context, from, to, groupBy, school);
            var total = all.Sum(row => row.Connections);
            var rows = all.Select(row => row with
            {
                Name = groupBy == "weekday" ? Message(language, "UsageInsightsWeekday" + row.Name) : row.Name,
                PercentageOfTotal = UsageInsightsQueryParser.Percentage(row.Connections, total),
            }).ToList();
            var highest = rows.OrderByDescending(row => row.Connections).FirstOrDefault();
            var lowest = rows.OrderBy(row => row.Connections).FirstOrDefault();
            var tieForMost = highest is not null && rows.Count(row => row.Connections == highest.Connections) > 1;
            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("connection-patterns", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language), PeriodOf(from, to),
                Map(("GroupBy", groupBy), ("School", school ?? "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                UsageInsightsMessages.Join(ConnectionsNote(context, language),
                    Message(language, groupBy == "weekday" ? "UsageInsightsNotePatternsWeekday" : "UsageInsightsNotePatternsRole")),
                Map(("TotalConnections", total), ("GroupWithMostConnections", highest?.Name), ("ConnectionsOfTheHighest", highest?.Connections),
                    ("GroupWithFewestConnections", lowest?.Name), ("ConnectionsOfTheLowest", lowest?.Connections), ("Ties", tieForMost)))
            {
                Notices = Notices(DataUntilNotice(to, dataUntil, language)),
            };
        }

        // ── school-participation ────────────────────────────────────────────────────────

        private static UsageInsightResponse GetSchoolParticipation(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var (from, to) = parser.ParsePeriod();
            var (descending, limit) = parser.ParseOrderAndLimit();
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var allSchools = Dal.GetSchoolParticipation(context, from, to);
            var ordered = (descending
                ? allSchools.OrderBy(row => row.ParticipationPercentage is null).ThenByDescending(row => row.ParticipationPercentage)
                : allSchools.OrderBy(row => row.ParticipationPercentage is null).ThenBy(row => row.ParticipationPercentage)).ToList();   // stable: ties by name; no active users last
            if (school is not null)
            {
                ordered = ordered.Where(row => row.Name == school).ToList();
            }

            int active = allSchools.Sum(row => row.ActiveUsers), connected = allSchools.Sum(row => row.ConnectedUsers);
            var rows = ordered.Take(limit).ToList();
            var hasMore = ordered.Count > limit;
            string? moreRowsNotice = null;
            if (hasMore)
            {
                var withoutConnected = school is null
                    ? Message(language, "UsageInsightsNoticeParticipationZero", allSchools.Count(row => row.ConnectedUsers == 0), ordered.Count)
                    : null;
                moreRowsNotice = UsageInsightsMessages.Join(Message(language, "UsageInsightsNoticeParticipationMore", limit, ordered.Count, ordered.Count - limit), withoutConnected);
            }

            var dataUntil = GetConnectionsDataUntil(context);
            return new UsageInsightResponse("school-participation", UsageInsightsSettings.SourceName, ConnectionsDateColumn(context, language), PeriodOf(from, to),
                Map(("Order", OrderText(descending)), ("Limit", limit), ("School", school ?? "all"), ("TestAccounts", TestAccountsValue(context))), dataUntil, rows,
                UsageInsightsMessages.Join(ConnectionsNote(context, language), Message(language, "UsageInsightsNoteParticipation", ExclusionNote(context, language))),
                Map(("SchoolsConsidered", allSchools.Count), ("TotalActiveUsers", active), ("TotalConnectedUsers", connected),
                    ("GlobalParticipationPercentage", UsageInsightsQueryParser.Percentage(connected, active)),
                    ("SchoolsWithoutConnectedUsers", allSchools.Count(row => row.ConnectedUsers == 0)),
                    ("SchoolsWith100OrMore", allSchools.Count(row => row.ParticipationPercentage >= 100))))
            {
                HasMoreRows = hasMore,
                Notices = Notices(DataUntilNotice(to, dataUntil, language), moreRowsNotice),
            };
        }

        // ── users-by-subscription ───────────────────────────────────────────────────────

        private static UsageInsightResponse GetUsersBySubscription(UsageInsightsQueryContext context, UsageInsightsQueryParser parser, string language)
        {
            var groupBy = parser.ParseChoice("groupBy", "total", "total", "school", "role");
            var (descending, limit) = parser.ParseOrderAndLimit();
            var school = parser.ResolveSchool(name => Dal.SchoolExists(context, name));
            var (all, total) = Dal.GetUsersBySubscriptionStatus(context, groupBy, school);
            var ordered = (descending
                ? all.OrderByDescending(row => row.TotalUsers).ThenBy(row => row.Name, StringComparer.Ordinal)
                : all.OrderBy(row => row.TotalUsers).ThenBy(row => row.Name, StringComparer.Ordinal)).ToList();

            // "Inactive" is only a fallback for future statuses (the real database only has active and canceled): when it is 0 it does not appear.
            var hasInactive = total.Inactive > 0;
            var byStatus = new List<SubscriptionStatusTotalRow> { new(Message(language, "UsageInsightsStatusActive"), total.Active, UsageInsightsQueryParser.Percentage(total.Active, total.Total)) };
            if (hasInactive)
            {
                byStatus.Add(new(Message(language, "UsageInsightsStatusInactive"), total.Inactive, UsageInsightsQueryParser.Percentage(total.Inactive, total.Total)));
            }

            byStatus.Add(new(Message(language, "UsageInsightsStatusCanceled"), total.Canceled, UsageInsightsQueryParser.Percentage(total.Canceled, total.Total)));
            byStatus.Add(new(Message(language, "UsageInsightsStatusWithoutSubscription"), total.WithoutSubscription, UsageInsightsQueryParser.Percentage(total.WithoutSubscription, total.Total)));
            object rows = groupBy == "total"
                ? byStatus
                : ordered.Take(limit).Select(row => new SubscriptionStatusViewRow(row.Name, row.Active, hasInactive ? row.Inactive : null, row.Canceled, row.WithoutSubscription, row.TotalUsers)).ToList();
            var hasMore = groupBy != "total" && ordered.Count > limit;
            return new UsageInsightResponse("users-by-subscription", UsageInsightsSettings.SourceName, Message(language, "UsageInsightsDateColumnSubscription"),
                Message(language, "UsageInsightsNotApplicable"),
                Map(("GroupBy", groupBy), ("School", school ?? "all"), ("Order", groupBy == "total" ? null : OrderText(descending)), ("Limit", groupBy == "total" ? null : limit),
                    ("TestAccounts", TestAccountsValue(context))), null, rows,
                Message(language, "UsageInsightsNoteSubscription", ExclusionNote(context, language)),
                Map(("TotalUsers", total.Total), ("Active", total.Active), ("Inactive", hasInactive ? total.Inactive : null), ("Canceled", total.Canceled),
                    ("WithoutSubscription", total.WithoutSubscription), ("ActivePercentage", UsageInsightsQueryParser.Percentage(total.Active, total.Total)),
                    ("ActiveWithExpiredCancellationDate", total.ActiveWithExpiredCancellationDate), ("GroupsConsidered", groupBy == "total" ? null : all.Count),
                    ("OriginalStatuses", total.OriginalStatuses)))
            {
                HasMoreRows = hasMore,
                Notices = Notices(hasMore
                    ? Message(language, "UsageInsightsNoticeSummaryHasTotal", limit, ordered.Count, Message(language, groupBy == "school" ? "UsageInsightsNounSchools" : "UsageInsightsNounRoles"))
                    : null),
            };
        }
    }
}
