using System.Text.Json.Nodes;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Tests.Infrastructure;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Xunit;
using static DataAnalysisAPI.Tests.Infrastructure.UsageInsightsTestHelper;

namespace DataAnalysisAPI.Tests
{
    /// <summary>
    /// The Excel workbook and the chart specification are derived from the JSON the caller sees.
    /// </summary>
    [Collection(UsageInsightsCollection.Name)]
    public class UsageInsightsExportTests
    {
        private const string September = "from=2026-09-01&to=2026-09-30";

        public UsageInsightsExportTests(UsageInsightsDatabaseFixture fixture)
        {
            _ = fixture;
            UsageInsights.ClearCache();
        }

        private static JsonNode Node(string query, string queryString, UsageInsightsQueryContext? context = null, string language = "en", bool ignoreLimit = false) =>
            UsageInsightsColumnFormatter.ToJsonNode(Run(query, queryString, context, language, isExport: true, ignoreLimit: ignoreLimit));

        private static IWorkbook Workbook(JsonNode response, string language = "en") =>
            new XSSFWorkbook(new MemoryStream(UsageInsightsExcelExporter.BuildWorkbook(response, language)));

        private static string Text(ISheet sheet, int row, int column) => sheet.GetRow(row)?.GetCell(column)?.ToString() ?? string.Empty;

        private static Dictionary<string, string> References(ISheet sheet)
        {
            var map = new Dictionary<string, string>();
            for (var i = 0; i <= sheet.LastRowNum; i++)
            {
                var key = Text(sheet, i, 0);
                if (key.Length > 0 && !map.ContainsKey(key))
                {
                    map[key] = Text(sheet, i, 1);
                }
            }

            return map;
        }

        // ── Excel ───────────────────────────────────────────────────────────────────────

        [Fact]
        public void The_workbook_has_a_data_sheet_and_a_references_sheet()
        {
            using var workbook = Workbook(Node("connections-by-school", September));
            var data = workbook.GetSheet("Data");

            Assert.Equal(2, workbook.NumberOfSheets);
            Assert.NotNull(workbook.GetSheet("References"));
            Assert.Equal(new[] { "Name", "Connections", "Connected users", "% of total", "Connections per user", "Last connection" },
                Enumerable.Range(0, 6).Select(column => Text(data, 0, column)));
            Assert.Equal(4, data.LastRowNum);   // header + 4 schools
            Assert.Equal("North High", Text(data, 1, 0));
            Assert.Equal(CellType.Numeric, data.GetRow(1).GetCell(1).CellType);
            Assert.Equal(8, data.GetRow(1).GetCell(1).NumericCellValue);
            Assert.Equal(47.1, data.GetRow(1).GetCell(3).NumericCellValue);
            Assert.Equal("2026-09-15", Text(data, 1, 5));
            Assert.Null(data.GetRow(4).GetCell(4));   // Silent School has no connections per user: an empty cell, not a zero
        }

        [Fact]
        public void The_workbook_is_localized()
        {
            using var workbook = Workbook(Node("connections-by-school", September, language: "es"), "es");
            var data = workbook.GetSheet("Data");

            Assert.Equal(new[] { "Nombre", "Conexiones", "Usuarios conectados", "% del total" }, Enumerable.Range(0, 4).Select(column => Text(data, 0, column)));
            Assert.Equal("Consulta", Text(workbook.GetSheet("References"), 1, 0));
        }

        [Fact]
        public void The_references_sheet_says_where_the_figures_come_from()
        {
            using var workbook = Workbook(Node("connections-by-school", September));
            var references = References(workbook.GetSheet("References"));

            Assert.Equal("connections-by-school", references["Query"]);
            Assert.Equal("mysql:gs_data_analysis", references["Source"]);
            Assert.Contains("connected_date.connected_date", references["Date column"]);
            Assert.Equal("2026-09-01", references["Period · From"]);
            Assert.Equal("2026-09-30", references["Period · To"]);
            Assert.Equal("excluded", references["Test accounts"]);
            Assert.Equal("all schools", references["Scope"]);
            Assert.Equal("2026-10-05", references["Data until"]);
            Assert.Equal("4", references["Rows exported"].Replace(".0", string.Empty));
            Assert.StartsWith("Yes", references["Complete list"]);
            Assert.True(references.ContainsKey("Generated at (UTC)"));
            Assert.True(references.ContainsKey("Total connections"));   // the summary
        }

        [Fact]
        public void The_references_sheet_flags_an_incomplete_list_from_the_structured_field_and_keeps_the_notices()
        {
            using var workbook = Workbook(Node("connections-by-teacher", September + "&limit=1"));
            var references = References(workbook.GetSheet("References"));

            Assert.StartsWith("NO", references["Complete list"]);
            Assert.StartsWith("1 more row(s) share the same value (4)", references["Notice"]);
        }

        [Fact]
        public void An_export_without_a_limit_returns_every_row_and_a_limit_is_honoured()
        {
            using var all = Workbook(Node("connections-by-teacher", September));
            using var one = Workbook(Node("connections-by-teacher", September + "&limit=1"));

            Assert.Equal(4, all.GetSheet("Data").LastRowNum);
            Assert.Equal(1, one.GetSheet("Data").LastRowNum);
        }

        [Fact]
        public void An_empty_result_explains_it_when_the_period_starts_after_the_data()
        {
            using var workbook = Workbook(Node("full-quiz-average", "from=2027-01-01&to=2027-01-31"));

            Assert.Equal("There is no tests data after 2026-10-02; the requested period starts on 2027-01-01 (see the References sheet).", Text(workbook.GetSheet("Data"), 0, 0));
        }

        [Fact]
        public void An_empty_result_has_a_generic_message_otherwise()
        {
            using var workbook = Workbook(Node("teacher-activity", "date=2027-06-01"));

            Assert.Equal("No results for this filter (see the References sheet).", Text(workbook.GetSheet("Data"), 0, 0));
        }

        [Fact]
        public void Text_cells_are_never_formulas()
        {
            var node = JsonNode.Parse("""{"Query":"connections-by-school","Results":[{"Name":"=SUM(A1:A2)","Connections":1},{"Name":"+cmd|' /C calc'!A0","Connections":2}]}""")!;
            using var workbook = Workbook(node);
            var data = workbook.GetSheet("Data");

            Assert.Equal(CellType.String, data.GetRow(1).GetCell(0).CellType);
            Assert.Equal("=SUM(A1:A2)", data.GetRow(1).GetCell(0).StringCellValue);
            Assert.Equal(CellType.String, data.GetRow(2).GetCell(0).CellType);
        }

        [Fact]
        public void The_comparison_workbooks_have_the_two_periods_in_the_references_and_the_file_name()
        {
            var node = Node("compare-schools", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30");
            using var workbook = Workbook(node);
            var references = References(workbook.GetSheet("References"));

            Assert.Equal("2026-08-01", references["Period · A.From"]);
            Assert.Equal("2026-09-30", references["Period · B.To"]);
            Assert.Equal("compare-schools_a_2026-08-01_2026-08-31_b_2026-09-01_2026-09-30.xlsx", UsageInsightsColumnFormatter.BuildFileName(node));
            Assert.Equal("Difference (B − A)", Text(workbook.GetSheet("Data"), 0, 3));
        }

        [Fact]
        public void The_file_names_only_use_safe_characters()
        {
            Assert.Equal("connections-by-school_2026-09-01_2026-09-30.xlsx", UsageInsightsColumnFormatter.BuildFileName(Node("connections-by-school", September)));
            Assert.Equal("teacher-activity_2026-09-15.xlsx", UsageInsightsColumnFormatter.BuildFileName(Node("teacher-activity", "date=2026-09-15")));
            Assert.Equal("teacher-activity_2026-10-24.xlsx", UsageInsightsColumnFormatter.BuildFileName(Node("teacher-activity", "periodFrom=2026-10-24")));
            Assert.Equal("users-by-subscription.xlsx", UsageInsightsColumnFormatter.BuildFileName(Node("users-by-subscription", string.Empty)));
            Assert.Matches("^[a-z0-9_.-]+$", UsageInsightsColumnFormatter.BuildFileName(JsonNode.Parse("""{"Query":"Strange Name/../x","Period":{"From":"a b"}}""")!));
        }

        [Theory]
        [InlineData("subject-area-usage", September)]
        [InlineData("connections-by-school", September)]
        [InlineData("connections-by-teacher", September)]
        [InlineData("compare-subject-areas", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30")]
        [InlineData("compare-schools", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30")]
        [InlineData("full-quiz-average", September)]
        [InlineData("connections-trend", "from=2026-08-01&to=2026-10-31")]
        [InlineData("cumulative-connections", "from=2026-08-01&to=2026-10-31")]
        [InlineData("teacher-activity", "date=2026-09-15")]
        [InlineData("connection-patterns", September)]
        [InlineData("school-participation", September)]
        [InlineData("users-by-subscription", "groupBy=school")]
        public void Every_exportable_query_produces_a_workbook_with_a_header_per_column(string query, string queryString)
        {
            using var workbook = Workbook(Node(query, queryString));
            var data = workbook.GetSheet("Data");

            Assert.True(data.LastRowNum >= 2);
            Assert.True(data.GetRow(0).LastCellNum >= 2);
            Assert.All(Enumerable.Range(0, data.GetRow(0).LastCellNum), column => Assert.False(string.IsNullOrWhiteSpace(Text(data, 0, column))));
        }

        // ── Charts ──────────────────────────────────────────────────────────────────────

        private static JsonObject Chart(string query, string queryString, string chartQuery = "", UsageInsightsQueryContext? context = null, string language = "en")
        {
            var parser = new UsageInsightsQueryParser(Query(queryString + (chartQuery.Length > 0 ? "&" + chartQuery : string.Empty)), isExport: true, ignoreLimit: true);
            var options = parser.ParseChartOptions();
            var response = UsageInsightsColumnFormatter.ToJsonNode(UsageInsights.Execute(query, parser, context ?? Context(), language));
            return ChartSpecificationBuilder.Build(response, options, language)!;
        }

        private static string[] Categories(JsonObject chart) => chart["Data"]!.AsArray().Select(point => point!["Category"]!.GetValue<string>()).ToArray();

        [Fact]
        public void A_bar_chart_leaves_out_the_zero_categories_by_default_and_counts_them()
        {
            var chart = Chart("connections-by-school", September);

            Assert.Equal("bar", chart["Type"]!.GetValue<string>());
            Assert.Equal("vertical", chart["Layout"]!.GetValue<string>());
            Assert.Equal("connections-by-school", chart["Query"]!.GetValue<string>());
            Assert.Equal("Connections by school", chart["Title"]!.GetValue<string>());
            Assert.Equal("School", chart["XAxisLabel"]!.GetValue<string>());
            Assert.Equal("Connections", chart["YAxisLabel"]!.GetValue<string>());
            Assert.Equal("connections", chart["Unit"]!.GetValue<string>());
            Assert.Equal(new[] { "North High", "South High", "East Academy" }, Categories(chart));
            Assert.Equal(3, chart["TotalCategories"]!.GetValue<int>());
            Assert.Equal(3, chart["ShownCategories"]!.GetValue<int>());
            Assert.Equal(1, chart["ZeroCategories"]!.GetValue<int>());
            Assert.False(chart["HasMoreRows"]!.GetValue<bool>());
            Assert.Equal("Connections", chart["Series"]![0]!["Key"]!.GetValue<string>());
            Assert.Equal("Connections", chart["Series"]![0]!["Name"]!.GetValue<string>());
            Assert.Equal(8, chart["Data"]![0]!["Connections"]!.GetValue<int>());
            Assert.Null(chart["Grouping"]);
        }

        [Fact]
        public void A_bar_chart_can_include_zeros_sort_ascending_and_limit_the_preview()
        {
            var withZeros = Chart("connections-by-school", September, "includeZeros=true&order=asc");
            var preview = Chart("connections-by-school", September, "limit=2");
            var everything = Chart("connections-by-school", September, "limit=2&all=true");

            Assert.Equal(new[] { "Silent School", "East Academy", "South High", "North High" }, Categories(withZeros));
            Assert.Equal(new[] { "North High", "South High" }, Categories(preview));
            Assert.Equal(3, preview["TotalCategories"]!.GetValue<int>());
            Assert.Equal(2, preview["ShownCategories"]!.GetValue<int>());
            Assert.Equal(3, Categories(everything).Length);
        }

        [Fact]
        public void A_line_chart_keeps_the_time_order_and_is_a_time_series()
        {
            var chart = Chart("connections-trend", "from=2026-08-01&to=2026-10-31");

            Assert.Equal("line", chart["Type"]!.GetValue<string>());
            Assert.True(chart["IsTimeSeries"]!.GetValue<bool>());
            Assert.Equal(new[] { "2026-08", "2026-09", "2026-10" }, Categories(chart));
            Assert.Null(chart["Layout"]);
            Assert.Equal("Connections by month", chart["Title"]!.GetValue<string>());
        }

        [Fact]
        public void A_cumulative_chart_has_two_series()
        {
            var chart = Chart("cumulative-connections", "from=2026-08-01&to=2026-10-31");

            Assert.Equal(new[] { "CumulativeConnections", "CumulativeUsers" }, chart["Series"]!.AsArray().Select(series => series!["Key"]!.GetValue<string>()));
            Assert.Equal(20, chart["Data"]![2]!["CumulativeConnections"]!.GetValue<int>());
        }

        [Fact]
        public void Weekday_patterns_keep_the_natural_order_with_every_day()
        {
            var chart = Chart("connection-patterns", September);

            Assert.Equal(new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, Categories(chart));
            Assert.Null(chart["ZeroCategories"]);   // not sorted, so zeros stay and are not counted
        }

        [Fact]
        public void The_total_subscription_chart_is_a_donut_with_the_center_total()
        {
            var chart = Chart("users-by-subscription", string.Empty);

            Assert.Equal("pie", chart["Type"]!.GetValue<string>());
            Assert.Equal("donut", chart["Variant"]!.GetValue<string>());
            Assert.Equal(11, chart["TotalValue"]!.GetValue<double>());
            Assert.Equal(new[] { "Active", "Canceled", "Without subscription" }, Categories(chart));   // natural order, Inactive omitted
            Assert.Equal("PercentageOfTotal", chart["Extra"]!["Key"]!.GetValue<string>());
            Assert.Equal("%", chart["Extra"]!["Unit"]!.GetValue<string>());
        }

        [Fact]
        public void The_subscription_chart_by_school_is_a_grouped_bar_without_the_empty_series()
        {
            var chart = Chart("users-by-subscription", "groupBy=school");

            Assert.Equal("bar", chart["Type"]!.GetValue<string>());
            Assert.Equal("grouped", chart["Grouping"]!.GetValue<string>());
            Assert.Equal(new[] { "Active", "Canceled", "WithoutSubscription" }, chart["Series"]!.AsArray().Select(series => series!["Key"]!.GetValue<string>()));
            Assert.Equal(new[] { "North High", "South High", "East Academy", "Silent School" }, Categories(chart));
        }

        [Fact]
        public void A_comparison_chart_names_the_series_with_their_periods()
        {
            var chart = Chart("compare-schools", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30");

            Assert.Equal("horizontal", chart["Layout"]!.GetValue<string>());
            Assert.Equal("Period A (2026-08-01 to 2026-08-31)", chart["Series"]![0]!["Name"]!.GetValue<string>());
            Assert.Equal("Period B (2026-09-01 to 2026-09-30)", chart["Series"]![1]!["Name"]!.GetValue<string>());
        }

        [Fact]
        public void A_chart_with_an_extra_measure_carries_it_in_every_point()
        {
            var chart = Chart("school-participation", September);

            Assert.Equal("ParticipationPercentage", chart["Extra"]!["Key"]!.GetValue<string>());
            Assert.Equal(133.3, chart["Data"]![0]!["ParticipationPercentage"]!.GetValue<double>());
            Assert.Equal(new[] { "ActiveUsers", "ConnectedUsers" }, chart["Series"]!.AsArray().Select(series => series!["Key"]!.GetValue<string>()));
            Assert.Equal("North High", Categories(chart)[0]);   // the participation is the sort criterion
        }

        [Fact]
        public void Charts_are_localized()
        {
            var chart = Chart("connections-by-school", September, language: "es");

            Assert.Equal("Conexiones por escuela", chart["Title"]!.GetValue<string>());
            Assert.Equal("Escuela", chart["XAxisLabel"]!.GetValue<string>());
        }

        [Fact]
        public void Repeated_labels_are_told_apart()
        {
            var response = JsonNode.Parse("""{"Query":"connections-by-teacher","Results":[{"Name":"Ana","School":"A","Connections":3},{"Name":"Ana","School":"B","Connections":2},{"Name":"Ana","School":"B","Connections":1}]}""")!;
            var chart = ChartSpecificationBuilder.Build(response, new ChartOptions(), "en")!;

            Assert.Equal(new[] { "Ana (A)", "Ana (B)", "Ana (B) #2" }, Categories(chart));
        }

        [Fact]
        public void A_chart_needs_at_least_two_categories_and_a_chartable_query()
        {
            var oneSchool = UsageInsightsColumnFormatter.ToJsonNode(Run("connections-by-school", September, Context(new DataScope(2)), isExport: true));
            var search = UsageInsightsColumnFormatter.ToJsonNode(Run("search-schools", "text=high"));

            Assert.Null(ChartSpecificationBuilder.Build(oneSchool, new ChartOptions(), "en"));
            Assert.Null(ChartSpecificationBuilder.Build(search, new ChartOptions(), "en"));
            Assert.False(ChartSpecificationBuilder.IsChartable("search-schools"));
            Assert.True(ChartSpecificationBuilder.IsChartable("connections-trend"));
        }

        [Fact]
        public void The_chart_reads_the_structured_has_more_rows_flag()
        {
            var cut = JsonNode.Parse("""{"Query":"connections-by-school","HasMoreRows":true,"Results":[{"Name":"A","Connections":3},{"Name":"B","Connections":2}]}""")!;
            var complete = JsonNode.Parse("""{"Query":"connections-by-school","Note":"HAS MORE ROWS in the text must not matter","Results":[{"Name":"A","Connections":3},{"Name":"B","Connections":2}]}""")!;

            Assert.True(ChartSpecificationBuilder.Build(cut, new ChartOptions(), "en")!["HasMoreRows"]!.GetValue<bool>());
            Assert.False(ChartSpecificationBuilder.Build(complete, new ChartOptions(), "en")!["HasMoreRows"]!.GetValue<bool>());
            Assert.Equal(3, Chart("connections-by-teacher", September)["TotalCategories"]!.GetValue<int>());   // Sol Silent has 0 connections and is left out
        }
    }
}
