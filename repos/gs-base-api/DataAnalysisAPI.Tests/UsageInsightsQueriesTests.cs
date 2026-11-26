using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Tests.Infrastructure;
using Xunit;
using static DataAnalysisAPI.Tests.Infrastructure.UsageInsightsTestHelper;
using Dal = DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper.UsageInsights;

namespace DataAnalysisAPI.Tests
{
    /// <summary>
    /// The 13 usage insights queries against the synthetic gs_data_analysis data (TestData/usage-insights-schema-and-data.sql).
    /// Days are read in Puerto Rico time (UTC-4). Every expected figure can be checked by hand against the SQL file.
    /// </summary>
    [Collection(UsageInsightsCollection.Name)]
    public class UsageInsightsQueriesTests
    {
        private const string September = "from=2026-09-01&to=2026-09-30";

        public UsageInsightsQueriesTests(UsageInsightsDatabaseFixture fixture)
        {
            _ = fixture;
            UsageInsights.ClearCache();
        }

        // ── infrastructure ──────────────────────────────────────────────────────────────

        [Fact]
        public void The_session_is_read_only()
        {
            using var connection = Dal.OpenReadOnlyConnection();
            using var command = new MySqlConnector.MySqlCommand("INSERT INTO role VALUES (99, 'must-fail')", connection);

            Assert.Throws<MySqlConnector.MySqlException>(() => command.ExecuteNonQuery());
        }

        private sealed class CapturingLogger : Microsoft.Extensions.Logging.ILogger
        {
            public List<string> Messages { get; } = new();

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => true;

            public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception? exception,
                Func<TState, Exception?, string> formatter) => Messages.Add(formatter(state, exception));
        }

        [Theory]
        [InlineData("Duplicate entry 'secret@example.invalid' for key 'user.email'", "Duplicate entry '?' for key '?'")]
        [InlineData("Truncated incorrect DOUBLE value: \"Maria Lopez\"", "Truncated incorrect DOUBLE value: '?'")]
        [InlineData("Unknown column `private_column` in 'field list'", "Unknown column '?' in '?'")]
        [InlineData(null, "")]
        public void The_engine_message_is_logged_without_the_values_it_quotes(string? message, string expected)
        {
            Assert.Equal(expected, UsageInsights.SanitizeEngineMessage(message));
        }

        [Fact]
        public void A_failed_query_is_logged_with_code_state_and_a_sanitized_message_and_no_values()
        {
            using var connection = Dal.OpenReadOnlyConnection();
            MySqlConnector.MySqlException failure;
            try
            {
                using var command = new MySqlConnector.MySqlCommand("SELECT * FROM table_that_is_not_there WHERE name = 'private-value'", connection);
                command.ExecuteNonQuery();
                throw new InvalidOperationException("The query had to fail.");
            }
            catch (MySqlConnector.MySqlException exception)
            {
                failure = exception;
                failure.Data["sql_start"] = "SELECT * FROM table_that_is_not_there WHERE name = @name";
            }

            var logger = new CapturingLogger();
            UsageInsights.LogQueryFailure("connections-by-school", failure, logger);

            var line = Assert.Single(logger.Messages);
            Assert.Contains("query=connections-by-school", line);
            Assert.Contains("mysqlCode=1146", line);
            Assert.Contains("sqlState=42S02", line);
            Assert.DoesNotContain("table_that_is_not_there'", line.Split("sql=")[0]);   // the quoted table name of the engine message is gone
            Assert.DoesNotContain("private-value", line);
        }

        [Fact]
        public void The_catalog_has_the_13_queries()
        {
            Assert.Equal(13, UsageInsights.QueryNames.Count);
            Assert.True(UsageInsights.Exists("connections-by-school"));
            Assert.False(UsageInsights.Exists("conexiones-por-escuela"));
        }

        [Fact]
        public void An_unknown_query_is_rejected()
        {
            Assert.Throws<InvalidQueryParameterException>(() => Run("nope", September));
        }

        // ── connections-by-school ───────────────────────────────────────────────────────

        [Fact]
        public void Connections_by_school_gives_figures_order_and_summary()
        {
            var response = Run("connections-by-school", September);
            var rows = Rows<SchoolConnectionsRow>(response);

            Assert.Equal(new[] { "North High", "South High", "East Academy", "Silent School" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 8, 6, 3, 0 }, rows.Select(row => row.Connections));
            Assert.Equal(new[] { 4, 2, 2, 0 }, rows.Select(row => row.ConnectedUsers));
            Assert.Equal(new double?[] { 47.1, 35.3, 17.6, 0.0 }, rows.Select(row => row.PercentageOfTotal));
            Assert.Equal(new double?[] { 2.0, 3.0, 1.5, null }, rows.Select(row => row.ConnectionsPerUser));
            Assert.Equal(new DateOnly?[] { new DateOnly(2026, 9, 15), new DateOnly(2026, 9, 30), new DateOnly(2026, 9, 30), null }, rows.Select(row => row.LastConnection));
            Assert.Equal(17, Summary(response)["TotalConnections"]);
            Assert.Equal(4, Summary(response)["SchoolsConsidered"]);
            Assert.Equal(3, Summary(response)["SchoolsWithConnections"]);
            Assert.Equal(1, Summary(response)["SchoolsWithoutConnections"]);
            Assert.Equal(false, Summary(response)["Ties"]);
            Assert.Equal("connections-by-school", response.Query);
            Assert.Equal("mysql:gs_data_analysis", response.Source);
            Assert.Equal(new DateOnly(2026, 10, 5), response.DataUntil);
            Assert.Equal("all schools", response.Scope);
            Assert.Equal(4, response.TotalRows);
            Assert.False(response.HasMoreRows);
            Assert.Empty(response.Notices);
            Assert.NotNull(response.Note);
        }

        [Fact]
        public void The_day_boundaries_are_read_in_local_time()
        {
            // 2026-09-01 03:59:59 UTC is the local day 2026-08-31; 2026-10-01 03:59:59 UTC is the local day 2026-09-30.
            var august = Rows<SchoolConnectionsRow>(Run("connections-by-school", "from=2026-08-31&to=2026-08-31"));
            var lastDay = Rows<SchoolConnectionsRow>(Run("connections-by-school", "from=2026-09-30&to=2026-09-30"));

            Assert.Equal(1, august.Single(row => row.Name == "North High").Connections);
            Assert.Equal(3, lastDay.Sum(row => row.Connections));   // users 30 and 31 at noon, and user 21 at 23:59:59 local
            Assert.Equal(new DateOnly(2026, 9, 30), lastDay.Single(row => row.Name == "South High").LastConnection);
        }

        [Fact]
        public void A_limit_cuts_the_list_and_says_so_without_a_tie()
        {
            var response = Run("connections-by-school", September + "&limit=2");

            Assert.Equal(2, Rows<SchoolConnectionsRow>(response).Count);
            Assert.True(response.HasMoreRows);
            Assert.Equal(false, Summary(response)["Ties"]);
            Assert.Equal(4, Summary(response)["SchoolsConsidered"]);
            Assert.Equal(47.1, Rows<SchoolConnectionsRow>(response)[0].PercentageOfTotal);   // the percentage is over the total, not over the shown rows
        }

        [Fact]
        public void Test_accounts_are_excluded_by_default_and_included_on_demand()
        {
            var excluded = Rows<SchoolConnectionsRow>(Run("connections-by-school", September));
            var included = Run("connections-by-school", September, Context(excludeTestAccounts: false));
            var includedRows = Rows<SchoolConnectionsRow>(included);

            Assert.DoesNotContain(excluded, row => row.Name is "Demo School" or "Facilitators School");
            Assert.Equal(8, excluded.Single(row => row.Name == "North High").Connections);
            Assert.Equal(new[] { "North High", "South High", "East Academy", "Facilitators School", "Demo School", "Silent School" }, includedRows.Select(row => row.Name));
            Assert.Equal(9, includedRows[0].Connections);   // the internal-domain teacher counts when nothing is excluded
            Assert.Equal(20, Summary(included)["TotalConnections"]);
            Assert.Contains("included", ((Dictionary<string, object?>)included.Filters)["TestAccounts"]!.ToString());
        }

        [Fact]
        public void The_response_says_which_test_accounts_were_excluded()
        {
            var excluded = Run("connections-by-school", September);

            Assert.Equal("excluded", ((Dictionary<string, object?>)excluded.Filters)["TestAccounts"]);
            Assert.Contains("DEMOSTRACION", excluded.Note);
            Assert.Contains("Facilitators School", excluded.Note);
            Assert.Contains("@internal.example", excluded.Note);
        }

        [Fact]
        public void A_period_after_the_loaded_data_gets_a_notice()
        {
            var response = Run("connections-by-school", "from=2026-12-01&to=2026-12-31");

            Assert.Equal(0, Summary(response)["TotalConnections"]);
            Assert.Single(response.Notices);
            Assert.Contains("2026-10-05", response.Notices[0]);
            Assert.Contains("2026-12-31", response.Notices[0]);
        }

        [Fact]
        public void The_notice_is_localized()
        {
            var response = Run("connections-by-school", "from=2026-12-01&to=2026-12-31", language: "es");

            Assert.Contains("Los datos cargados llegan solo hasta 2026-10-05", response.Notices[0]);
            Assert.Equal("todas las escuelas", response.Scope);
        }

        [Fact]
        public void The_connections_filter_keeps_only_the_schools_that_meet_it()
        {
            var response = Run("connections-by-school", September + "&minConnections=3&maxConnections=6");

            Assert.Equal(new[] { "South High", "East Academy" }, Rows<SchoolConnectionsRow>(response).Select(row => row.Name));
            Assert.Equal(2, Summary(response)["SchoolsMeetingTheFilter"]);
            Assert.Equal(4, Summary(response)["SchoolsConsidered"]);
        }

        [Fact]
        public void The_school_filter_returns_only_that_school_with_the_global_percentage()
        {
            var response = Run("connections-by-school", September + "&school=South High");
            var rows = Rows<SchoolConnectionsRow>(response);

            Assert.Single(rows);
            Assert.Equal("South High", rows[0].Name);
            Assert.Equal(35.3, rows[0].PercentageOfTotal);
            Assert.False(response.HasMoreRows);
        }

        [Fact]
        public void An_unknown_school_is_rejected()
        {
            var exception = Assert.Throws<InvalidQueryParameterException>(() => Run("connections-by-school", September + "&school=Nowhere"));

            Assert.Equal("UnknownSchool", exception.ReasonCode);
            Assert.Equal("Nowhere", exception.Detail);
        }

        [Fact]
        public void A_test_school_cannot_be_asked_for_while_it_is_excluded()
        {
            Assert.Throws<InvalidQueryParameterException>(() => Run("connections-by-school", September + "&school=Demo School"));
            Assert.Single(Rows<SchoolConnectionsRow>(Run("connections-by-school", September + "&school=Demo School", Context(excludeTestAccounts: false))));
        }

        // ── scope ───────────────────────────────────────────────────────────────────────

        [Fact]
        public void An_ore_scope_only_sees_the_schools_of_its_ore()
        {
            var response = Run("connections-by-school", September, Context(new DataScope(2)));
            var rows = Rows<SchoolConnectionsRow>(response);

            Assert.Equal(new[] { "East Academy" }, rows.Select(row => row.Name));
            Assert.Equal(100.0, rows[0].PercentageOfTotal);
            Assert.Equal(3, Summary(response)["TotalConnections"]);
            Assert.Equal("ORE ORE TWO", response.Scope);
        }

        [Fact]
        public void A_school_scope_only_sees_its_school()
        {
            var response = Run("connections-by-school", September, Context(new DataScope(null, 1)));

            Assert.Equal(new[] { "North High" }, Rows<SchoolConnectionsRow>(response).Select(row => row.Name));
            Assert.Equal(8, Summary(response)["TotalConnections"]);
            Assert.Equal("school North High", response.Scope);
        }

        [Fact]
        public void A_school_out_of_scope_behaves_as_if_it_did_not_exist()
        {
            var outOfScope = Assert.Throws<InvalidQueryParameterException>(() => Run("connections-by-school", September + "&school=South High", Context(new DataScope(2))));
            var missing = Assert.Throws<InvalidQueryParameterException>(() => Run("connections-by-school", September + "&school=Nowhere", Context(new DataScope(2))));

            Assert.Equal("UnknownSchool", outOfScope.ReasonCode);
            Assert.Equal(missing.ReasonCode, outOfScope.ReasonCode);
            Assert.Equal(missing.ParameterName, outOfScope.ParameterName);
        }

        [Fact]
        public void The_none_scope_sees_nothing()
        {
            var rows = Rows<SchoolConnectionsRow>(Run("connections-by-school", September, Context(DataScope.None)));

            Assert.Empty(rows);
        }

        [Theory]
        [InlineData("connections-by-teacher", September)]
        [InlineData("connection-patterns", September)]
        [InlineData("connections-trend", September)]
        [InlineData("school-participation", September)]
        [InlineData("subject-area-usage", September)]
        [InlineData("full-quiz-average", September)]
        [InlineData("users-by-subscription", "")]
        [InlineData("teacher-activity", "date=2026-09-15")]
        public void Every_query_applies_the_scope_in_the_sql(string query, string queryString)
        {
            var unrestricted = UsageInsightsJson.ToNode(Run(query, queryString));
            var restricted = UsageInsightsJson.ToNode(Run(query, queryString, Context(new DataScope(2))));
            var noScope = UsageInsightsJson.ToNode(Run(query, queryString, Context(DataScope.None)));

            // ORE 2 only has "East Academy": it can never see more than the unrestricted caller, and the none scope gets no school-bound figures.
            Assert.True(CountOf(restricted, "TotalConnections", "TotalTeacherConnections", "TotalCompletedTests", "Teachers", "TotalUsers", "TotalActiveUsers")
                <= CountOf(unrestricted, "TotalConnections", "TotalTeacherConnections", "TotalCompletedTests", "Teachers", "TotalUsers", "TotalActiveUsers"));
            Assert.Equal(0, CountOf(noScope, "TotalConnections", "TotalTeacherConnections", "Teachers", "TotalUsers", "TotalActiveUsers"));
        }

        private static double CountOf(System.Text.Json.Nodes.JsonNode response, params string[] keys)
        {
            var summary = response["Summary"];
            foreach (var key in keys)
            {
                if (summary?[key] is { } value)
                {
                    return value.GetValue<double>();
                }
            }

            return 0;
        }

        // ── search-schools ──────────────────────────────────────────────────────────────

        [Fact]
        public void Search_schools_finds_by_part_of_the_name_inside_the_scope()
        {
            Assert.Equal(new[] { "North High", "South High" }, Rows<SchoolCatalogRow>(Run("search-schools", "text=HIGH")).Select(row => row.Name));
            Assert.Empty(Rows<SchoolCatalogRow>(Run("search-schools", "text=demo")));   // test schools are excluded
            Assert.Single(Rows<SchoolCatalogRow>(Run("search-schools", "text=demo", Context(excludeTestAccounts: false))));
            Assert.Empty(Rows<SchoolCatalogRow>(Run("search-schools", "text=high", Context(new DataScope(2)))));
            Assert.Single(Rows<SchoolCatalogRow>(Run("search-schools", "text=high&limit=1")));
        }

        [Fact]
        public void Search_schools_needs_at_least_two_characters_and_treats_wildcards_as_text()
        {
            var exception = Assert.Throws<InvalidQueryParameterException>(() => Run("search-schools", "text=h"));

            Assert.Equal("TextTooShort", exception.ReasonCode);
            Assert.Empty(Rows<SchoolCatalogRow>(Run("search-schools", "text=%25%25")));
        }

        // ── connections-by-teacher ──────────────────────────────────────────────────────

        [Fact]
        public void Connections_by_teacher_ranks_teachers_with_a_school_and_flags_the_ties()
        {
            var all = Run("connections-by-teacher", September);
            var cut = Run("connections-by-teacher", September + "&limit=1");

            Assert.Equal(new[] { "Tom Teacher", "Sam Second", "Eva East", "Sol Silent" }, Rows<TeacherConnectionsRow>(all).Select(row => row.Name));
            Assert.Equal(new[] { 4, 4, 2, 0 }, Rows<TeacherConnectionsRow>(all).Select(row => row.Connections));
            Assert.Equal(new double?[] { 40.0, 40.0, 20.0, 0.0 }, Rows<TeacherConnectionsRow>(all).Select(row => row.PercentageOfTotal));
            Assert.Equal("North High", Rows<TeacherConnectionsRow>(all)[0].School);
            Assert.Equal(new DateOnly(2026, 9, 10), Rows<TeacherConnectionsRow>(all)[0].LastConnection);
            Assert.Equal(10, Summary(all)["TotalTeacherConnections"]);
            Assert.Equal(4, Summary(all)["TeachersConsidered"]);
            Assert.Equal(false, Summary(all)["Ties"]);
            Assert.False(all.HasMoreRows);

            Assert.Equal(true, Summary(cut)["Ties"]);
            Assert.True(cut.HasMoreRows);
            Assert.Contains(cut.Notices, notice => notice.StartsWith("1 more row(s) share the same value (4)"));
        }

        [Fact]
        public void Connections_by_teacher_leaves_out_teachers_without_a_school_and_internal_domains()
        {
            var names = Rows<TeacherConnectionsRow>(Run("connections-by-teacher", September)).Select(row => row.Name).ToList();
            var withTestAccounts = Rows<TeacherConnectionsRow>(Run("connections-by-teacher", September, Context(excludeTestAccounts: false))).Select(row => row.Name).ToList();

            Assert.DoesNotContain("Nora NoSchool", names);
            Assert.DoesNotContain("Tina Teacher", names);
            Assert.Contains("Tina Teacher", withTestAccounts);
            Assert.DoesNotContain("Nora NoSchool", withTestAccounts);   // no school: never counted
        }

        [Fact]
        public void Connections_by_teacher_filters_by_school_and_by_number_of_connections()
        {
            var bySchool = Rows<TeacherConnectionsRow>(Run("connections-by-teacher", September + "&school=North High"));
            var once = Rows<TeacherConnectionsRow>(Run("connections-by-teacher", "from=2026-09-01&to=2026-09-01&minConnections=1&maxConnections=1"));

            Assert.Equal(new[] { "Tom Teacher" }, bySchool.Select(row => row.Name));
            Assert.Equal(new[] { "Tom Teacher" }, once.Select(row => row.Name));
        }

        // ── compare-subject-areas / subject-area-usage ──────────────────────────────────

        [Fact]
        public void Subject_area_usage_counts_the_cohort_by_assignment_date()
        {
            var response = Run("subject-area-usage", September);
            var rows = Rows<SubjectAreaUsageRow>(response);

            Assert.Equal(new[] { "English", "Spanish", "Mathematics", "Old" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 4, 2, 2, 0 }, rows.Select(row => row.AssignedTests));
            Assert.Equal(new[] { 2, 1, 1, 0 }, rows.Select(row => row.CompletedTests));
            Assert.Equal(new double?[] { 50.0, 50.0, 50.0, null }, rows.Select(row => row.CompletedPercentage));
            Assert.Equal(new double?[] { 50.0, 25.0, 25.0, 0.0 }, rows.Select(row => row.PercentageOfTotalCompleted));
            Assert.Equal(8, Summary(response)["TotalAssignedTests"]);
            Assert.Equal(4, Summary(response)["TotalCompletedTests"]);
            Assert.Equal(50.0, Summary(response)["GlobalCompletedPercentage"]);
            Assert.Equal(4, Summary(response)["AreasConsidered"]);
            Assert.Equal(3, Summary(response)["AreasWithAssignedTests"]);
            Assert.Equal(1, Summary(response)["AreasWithoutAssignedTests"]);
            Assert.DoesNotContain(rows, row => row.Name == "Unused");   // a subject that never had a test is not an area
            Assert.Equal(new DateOnly(2026, 10, 2), response.DataUntil);
        }

        [Fact]
        public void Subject_area_usage_ignores_teachers_and_excludes_test_accounts()
        {
            var excluded = Rows<SubjectAreaUsageRow>(Run("subject-area-usage", September)).Single(row => row.Name == "English");
            var included = Rows<SubjectAreaUsageRow>(Run("subject-area-usage", September, Context(excludeTestAccounts: false))).Single(row => row.Name == "English");

            Assert.Equal(4, excluded.AssignedTests);   // the teacher test and the demo student test are not counted
            Assert.Equal(5, included.AssignedTests);
            Assert.Equal(3, included.CompletedTests);
        }

        [Fact]
        public void Subject_area_usage_orders_ascending_and_flags_a_cut_list()
        {
            var response = Run("subject-area-usage", September + "&order=asc&limit=2");

            Assert.Equal(new[] { "Old", "Mathematics" }, Rows<SubjectAreaUsageRow>(response).Select(row => row.Name));
            Assert.True(response.HasMoreRows);
        }

        [Fact]
        public void Compare_subject_areas_compares_two_periods()
        {
            var response = Run("compare-subject-areas", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30");
            var rows = Rows<SubjectAreaComparisonRow>(response);

            Assert.Equal(new[] { "English", "Mathematics", "Old", "Spanish" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 1, 0, 0, 0 }, rows.Select(row => row.CompletedTestsA));
            Assert.Equal(new[] { 2, 1, 0, 1 }, rows.Select(row => row.CompletedTestsB));
            Assert.Equal(new[] { 1, 1, 0, 1 }, rows.Select(row => row.Difference));
            Assert.Equal(new double?[] { 100.0, null, null, null }, rows.Select(row => row.VariationPercentage));
            Assert.Contains("\"Period\":{\"A\":{\"From\":\"2026-08-01\",\"To\":\"2026-08-31\"},\"B\":{\"From\":\"2026-09-01\",\"To\":\"2026-09-30\"}}", UsageInsightsJson.Serialize(response));
            Assert.Empty(response.Notices);
        }

        // ── compare-schools ─────────────────────────────────────────────────────────────

        [Fact]
        public void Compare_schools_gives_the_difference_between_two_periods()
        {
            var response = Run("compare-schools", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30");
            var rows = Rows<SchoolComparisonRow>(response);

            Assert.Equal(new[] { "North High", "South High", "East Academy" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 1, 0, 0 }, rows.Select(row => row.ConnectionsA));
            Assert.Equal(new[] { 8, 6, 3 }, rows.Select(row => row.ConnectionsB));
            Assert.Equal(new[] { 7, 6, 3 }, rows.Select(row => row.Difference));
            Assert.Equal(new double?[] { 700.0, null, null }, rows.Select(row => row.VariationPercentage));
            Assert.Equal(1, Summary(response)["TotalConnectionsA"]);
            Assert.Equal(17, Summary(response)["TotalConnectionsB"]);
            Assert.Equal(16, Summary(response)["TotalDifference"]);
            Assert.Equal(1600.0, Summary(response)["TotalVariationPercentage"]);
            Assert.Equal(3, Summary(response)["SchoolsCompared"]);
        }

        [Fact]
        public void Compare_schools_ascending_puts_the_biggest_falls_first()
        {
            var rows = Rows<SchoolComparisonRow>(Run("compare-schools", "fromA=2026-08-01&toA=2026-08-31&fromB=2026-09-01&toB=2026-09-30&order=asc&limit=2"));

            Assert.Equal(new[] { "East Academy", "South High" }, rows.Select(row => row.Name));
        }

        // ── full-quiz-average ───────────────────────────────────────────────────────────

        [Fact]
        public void Full_quiz_average_groups_by_school_and_subject()
        {
            var response = Run("full-quiz-average", September);
            var rows = Rows<FullQuizAverageRow>(response);

            Assert.Equal(new[] { "East Academy", "South High", "North High", "Silent School", "South High" }, rows.Select(row => row.School));
            Assert.Equal(new[] { "Spanish", "Mathematics", "English", "English", "Spanish" }, rows.Select(row => row.Subject));
            Assert.Equal(new double?[] { 100.0, 90.0, 70.0, null, null }, rows.Select(row => row.AverageCompletedOnly));
            Assert.Equal(new[] { 100.0, 45.0, 46.67, 0.0, 0.0 }, rows.Select(row => row.AverageAllAssigned));
            Assert.Equal(new[] { 1, 2, 3, 1, 1 }, rows.Select(row => row.AssignedTests));
            Assert.Equal(new[] { 1, 1, 2, 0, 0 }, rows.Select(row => row.CompletedTests));
            Assert.False(response.HasMoreRows);
        }

        [Fact]
        public void Full_quiz_average_can_group_by_subject_only_and_by_school_only()
        {
            var bySubject = Rows<FullQuizAverageRow>(Run("full-quiz-average", September + "&groupBy=subject"));
            var bySchool = Rows<FullQuizAverageRow>(Run("full-quiz-average", September + "&groupBy=school&school=North High"));

            Assert.All(bySubject, row => Assert.Null(row.School));
            Assert.Equal(new[] { "Spanish", "Mathematics", "English" }, bySubject.Select(row => row.Subject));
            Assert.Equal(new[] { 50.0, 45.0, 35.0 }, bySubject.Select(row => row.AverageAllAssigned));
            Assert.Single(bySchool);
            Assert.Null(bySchool[0].Subject);
            Assert.Equal(3, bySchool[0].AssignedTests);
        }

        [Fact]
        public void Full_quiz_average_flags_a_cut_list_with_a_notice()
        {
            var response = Run("full-quiz-average", September + "&limit=2");

            Assert.Equal(2, Rows<FullQuizAverageRow>(response).Count);
            Assert.True(response.HasMoreRows);
            Assert.Contains(response.Notices, notice => notice.StartsWith("Only the first 2 are shown"));
        }

        [Fact]
        public void Full_quiz_average_with_no_tests_in_the_period_is_empty_and_warns_about_the_data()
        {
            var response = Run("full-quiz-average", "from=2027-01-01&to=2027-01-31");

            Assert.Empty(Rows<FullQuizAverageRow>(response));
            Assert.Contains(response.Notices, notice => notice.Contains("2026-10-02"));
        }

        [Fact]
        public void Group_by_has_a_closed_set_of_values()
        {
            var exception = Assert.Throws<InvalidQueryParameterException>(() => Run("full-quiz-average", September + "&groupBy=school_materia"));

            Assert.Equal("groupBy", exception.ParameterName);
            Assert.Equal("school, subject, school_subject", exception.Detail);
        }

        // ── connections-trend / cumulative-connections ──────────────────────────────────

        [Fact]
        public void Connections_trend_fills_empty_periods_and_gives_the_variation()
        {
            var response = Run("connections-trend", "from=2026-08-01&to=2026-10-31");
            var rows = Rows<ConnectionsTrendRow>(response);

            Assert.Equal(new[] { "2026-08", "2026-09", "2026-10" }, rows.Select(row => row.Period));
            Assert.Equal(new[] { 1, 17, 2 }, rows.Select(row => row.Connections));
            Assert.Equal(new int?[] { null, 16, -15 }, rows.Select(row => row.DifferenceWithPreviousPeriod));
            Assert.Equal(new double?[] { null, 1600.0, -88.2 }, rows.Select(row => row.VariationPercentage));
            Assert.Equal(20, Summary(response)["TotalConnections"]);
            Assert.Equal("2026-09", Summary(response)["PeriodWithMostConnections"]);
            Assert.Equal(17, Summary(response)["ConnectionsOfTheHighest"]);
            Assert.Equal("2026-08", Summary(response)["PeriodWithFewestConnections"]);
            Assert.Contains(response.Notices, notice => notice.Contains("2026-10-05"));
        }

        [Fact]
        public void Connections_trend_supports_quarter_and_year()
        {
            var quarter = Rows<ConnectionsTrendRow>(Run("connections-trend", "from=2026-07-01&to=2026-09-30&granularity=quarter"));
            var year = Rows<ConnectionsTrendRow>(Run("connections-trend", "from=2026-01-01&to=2027-01-31&granularity=year"));

            Assert.Equal(new[] { "2026-T3" }, quarter.Select(row => row.Period));
            Assert.Equal(18, quarter[0].Connections);
            Assert.Equal(new[] { "2026", "2027" }, year.Select(row => row.Period));
            Assert.Equal(new[] { 20, 0 }, year.Select(row => row.Connections));
        }

        [Fact]
        public void Cumulative_connections_accumulate_and_count_new_users()
        {
            var response = Run("cumulative-connections", "from=2026-08-01&to=2026-10-31");
            var rows = Rows<CumulativeConnectionsRow>(response);

            Assert.Equal(new[] { "2026-08", "2026-09", "2026-10" }, rows.Select(row => row.Period));
            Assert.Equal(new[] { 1, 17, 2 }, rows.Select(row => row.Connections));
            Assert.Equal(new[] { 1, 18, 20 }, rows.Select(row => row.CumulativeConnections));
            Assert.Equal(new[] { 1, 7, 0 }, rows.Select(row => row.NewUsers));
            Assert.Equal(new[] { 1, 8, 8 }, rows.Select(row => row.CumulativeUsers));
            Assert.Equal(20, Summary(response)["TotalConnections"]);
            Assert.Equal(8, Summary(response)["ConnectedUsers"]);
            Assert.Equal(8, Summary(response)["ActiveUsers"]);
            Assert.Equal(100.0, Summary(response)["ConnectionUsersPercentage"]);
            Assert.Equal(7, Summary(response)["EverConnectedUsers"]);
            Assert.Equal(87.5, Summary(response)["EverConnectedPercentage"]);
        }

        [Fact]
        public void Cumulative_connections_by_day_are_limited_to_93_days_and_a_school_filter_drops_the_active_users()
        {
            var exception = Assert.Throws<InvalidQueryParameterException>(() => Run("cumulative-connections", "from=2026-01-01&to=2026-12-31&granularity=day"));
            var daily = Run("cumulative-connections", "from=2026-09-01&to=2026-09-05&granularity=day&school=South High");

            Assert.Equal("DayRangeTooLong", exception.ReasonCode);
            Assert.Equal(5, Rows<CumulativeConnectionsRow>(daily).Count);
            Assert.Equal(new[] { 0, 2, 1, 1, 1 }, Rows<CumulativeConnectionsRow>(daily).Select(row => row.Connections));
            Assert.False(Summary(daily).ContainsKey("ActiveUsers") && Summary(daily)["ActiveUsers"] is not null);
        }

        // ── teacher-activity ────────────────────────────────────────────────────────────

        [Fact]
        public void Teacher_activity_counts_distinct_teachers_in_the_period_of_each_school()
        {
            var response = Run("teacher-activity", "date=2026-09-15");
            var rows = Rows<TeacherActivityRow>(response);

            Assert.Equal(new[] { "North High", "East Academy", "South High" }, rows.Select(row => row.School));
            var north = rows[0];
            Assert.Equal((2, 1, 1, 1, 1, 2), (north.Teachers, north.WithConnections, north.WithPlans, north.PlansWithData, north.WithGroups, north.WithAssignments));
            Assert.Equal("2026-08-28", north.PeriodFrom);
            Assert.Equal("2026-10-23", north.PeriodTo);
            Assert.Equal(3, Summary(response)["Schools"]);
            Assert.Equal(4, Summary(response)["Teachers"]);
            Assert.Equal(3, Summary(response)["WithConnections"]);
            Assert.Equal(2, Summary(response)["WithPlans"]);
            Assert.Equal(2, Summary(response)["PlansWithData"]);
            Assert.Equal(3, Summary(response)["WithGroups"]);
            Assert.Equal(3, Summary(response)["WithAssignments"]);
            Assert.Equal(1, Summary(response)["TeachersWithoutConnections"]);
            Assert.False(response.HasMoreRows);
            Assert.Empty(response.Notices);
        }

        [Fact]
        public void Teacher_activity_can_pick_the_period_by_its_start_and_filter_by_school()
        {
            var byPeriod = Rows<TeacherActivityRow>(Run("teacher-activity", "periodFrom=2026-10-24"));
            var bySchool = Rows<TeacherActivityRow>(Run("teacher-activity", "date=2026-09-15&school=South High"));

            Assert.Single(byPeriod);
            Assert.Equal("2027-01-08", byPeriod[0].PeriodTo);
            Assert.Equal((1, 0, 0, 0, 0, 0), (byPeriod[0].Teachers, byPeriod[0].WithConnections, byPeriod[0].WithPlans, byPeriod[0].PlansWithData, byPeriod[0].WithGroups, byPeriod[0].WithAssignments));
            Assert.Equal(new[] { "South High" }, bySchool.Select(row => row.School));
        }

        [Fact]
        public void Teacher_activity_without_a_matching_period_says_there_is_no_data()
        {
            var response = Run("teacher-activity", "date=2027-06-01");

            Assert.Empty(Rows<TeacherActivityRow>(response));
            Assert.Contains(response.Notices, notice => notice.StartsWith("NO DATA"));
            Assert.Equal(0, Summary(response)["Teachers"]);
        }

        [Fact]
        public void Teacher_activity_applies_the_scope_by_school_and_ore_name_and_the_exclusions()
        {
            var ore = Rows<TeacherActivityRow>(Run("teacher-activity", "date=2026-09-15", Context(new DataScope(2))));
            var school = Rows<TeacherActivityRow>(Run("teacher-activity", "date=2026-09-15", Context(new DataScope(null, 2))));
            var withTestAccounts = Rows<TeacherActivityRow>(Run("teacher-activity", "date=2026-09-15", Context(excludeTestAccounts: false)));

            Assert.Equal(new[] { "East Academy" }, ore.Select(row => row.School));
            Assert.Equal(new[] { "South High" }, school.Select(row => row.School));
            Assert.Contains(withTestAccounts, row => row.School == "Demo School");
            Assert.Contains(withTestAccounts, row => row.School == "Facilitators School");
        }

        [Fact]
        public void Teacher_activity_flags_a_cut_list()
        {
            var response = Run("teacher-activity", "date=2026-09-15&limit=1");

            Assert.Single(Rows<TeacherActivityRow>(response));
            Assert.True(response.HasMoreRows);
            Assert.Contains(response.Notices, notice => notice.StartsWith("1 of 3 school-period combinations are shown"));
            Assert.Equal(4, Summary(response)["Teachers"]);
        }

        // ── connection-patterns ─────────────────────────────────────────────────────────

        [Fact]
        public void Connection_patterns_by_weekday_always_give_the_seven_days()
        {
            var response = Run("connection-patterns", September);
            var rows = Rows<ConnectionPatternRow>(response);

            Assert.Equal(new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 0, 2, 8, 5, 1, 1, 0 }, rows.Select(row => row.Connections));
            Assert.Equal(new[] { 0, 2, 6, 4, 1, 1, 0 }, rows.Select(row => row.ConnectedUsers));
            Assert.Equal(47.1, rows[2].PercentageOfTotal);
            Assert.Equal(17, Summary(response)["TotalConnections"]);
            Assert.Equal("Wednesday", Summary(response)["GroupWithMostConnections"]);
            Assert.Equal(false, Summary(response)["Ties"]);
        }

        [Fact]
        public void Connection_patterns_localize_the_weekday_names()
        {
            var rows = Rows<ConnectionPatternRow>(Run("connection-patterns", September, language: "es"));

            Assert.Equal(new[] { "lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo" }, rows.Select(row => row.Name));
        }

        [Fact]
        public void Connection_patterns_by_role_leave_out_the_client_role()
        {
            var rows = Rows<ConnectionPatternRow>(Run("connection-patterns", September + "&groupBy=role"));

            Assert.Equal(new[] { "teacher", "student", "director" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 10, 6, 1 }, rows.Select(row => row.Connections));
            Assert.Equal(new[] { 3, 4, 1 }, rows.Select(row => row.ConnectedUsers));
        }

        [Fact]
        public void Connection_patterns_detect_a_tie_for_the_most_connections()
        {
            var response = Run("connection-patterns", "from=2026-09-04&to=2026-09-05");   // one connection on Friday and one on Saturday

            Assert.Equal(true, Summary(response)["Ties"]);
            Assert.Equal(1, Summary(response)["ConnectionsOfTheHighest"]);
        }

        // ── school-participation ────────────────────────────────────────────────────────

        [Fact]
        public void School_participation_divides_connected_users_by_active_users()
        {
            var response = Run("school-participation", September);
            var rows = Rows<SchoolParticipationRow>(response);

            Assert.Equal(new[] { "North High", "East Academy", "South High", "Silent School" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 3, 2, 2, 1 }, rows.Select(row => row.ActiveUsers));
            Assert.Equal(new[] { 4, 2, 2, 0 }, rows.Select(row => row.ConnectedUsers));
            Assert.Equal(new double?[] { 133.3, 100.0, 100.0, 0.0 }, rows.Select(row => row.ParticipationPercentage));
            Assert.Equal(4, Summary(response)["SchoolsConsidered"]);
            Assert.Equal(8, Summary(response)["TotalActiveUsers"]);
            Assert.Equal(8, Summary(response)["TotalConnectedUsers"]);
            Assert.Equal(100.0, Summary(response)["GlobalParticipationPercentage"]);
            Assert.Equal(1, Summary(response)["SchoolsWithoutConnectedUsers"]);
            Assert.Equal(3, Summary(response)["SchoolsWith100OrMore"]);
        }

        [Fact]
        public void School_participation_flags_a_cut_list_and_orders_ascending()
        {
            var cut = Run("school-participation", September + "&limit=2");
            var ascending = Rows<SchoolParticipationRow>(Run("school-participation", September + "&order=asc"));

            Assert.True(cut.HasMoreRows);
            Assert.Contains(cut.Notices, notice => notice.StartsWith("2 of 4 schools are shown"));
            Assert.Equal("Silent School", ascending[0].Name);
        }

        // ── users-by-subscription / active users ────────────────────────────────────────

        [Fact]
        public void Users_by_subscription_gives_one_row_per_status_with_the_total_grouping()
        {
            var response = Run("users-by-subscription", string.Empty);
            var rows = Rows<SubscriptionStatusTotalRow>(response);

            Assert.Equal(new[] { "Active", "Canceled", "Without subscription" }, rows.Select(row => row.Name));   // Inactive is omitted when it is 0
            Assert.Equal(new[] { 8, 2, 1 }, rows.Select(row => row.Users));
            Assert.Equal(new double?[] { 72.7, 18.2, 9.1 }, rows.Select(row => row.PercentageOfTotal));
            Assert.Equal(11, Summary(response)["TotalUsers"]);
            Assert.Equal(8, Summary(response)["Active"]);
            Assert.False(Summary(response).ContainsKey("Inactive") && Summary(response)["Inactive"] is not null);
            Assert.Equal(2, Summary(response)["ActiveWithExpiredCancellationDate"]);
            var statuses = (Dictionary<string, int>)Summary(response)["OriginalStatuses"]!;
            Assert.Equal(8, statuses["active"]);
            Assert.Equal(3, statuses["canceled"]);
        }

        [Fact]
        public void Users_by_subscription_by_school_orders_by_total_and_breaks_ties_by_name()
        {
            var response = Run("users-by-subscription", "groupBy=school");
            var rows = Rows<SubscriptionStatusViewRow>(response);

            Assert.Equal(new[] { "North High", "South High", "East Academy", "Silent School" }, rows.Select(row => row.Name));
            Assert.Equal(new[] { 3, 2, 2, 1 }, rows.Select(row => row.Active));
            Assert.Equal(new[] { 1, 0, 0, 1 }, rows.Select(row => row.Canceled));
            Assert.Equal(new[] { 0, 1, 0, 0 }, rows.Select(row => row.WithoutSubscription));
            Assert.Equal(new[] { 4, 3, 2, 2 }, rows.Select(row => row.TotalUsers));
            Assert.All(rows, row => Assert.Null(row.Inactive));
            Assert.False(response.HasMoreRows);
        }

        [Fact]
        public void Users_by_subscription_by_role_and_with_a_cut_list()
        {
            var byRole = Rows<SubscriptionStatusViewRow>(Run("users-by-subscription", "groupBy=role"));
            var cut = Run("users-by-subscription", "groupBy=school&limit=1");

            Assert.Equal(11, byRole.Sum(row => row.TotalUsers));
            Assert.Contains(byRole, row => row.Name == "student");
            Assert.True(cut.HasMoreRows);
            Assert.Contains(cut.Notices, notice => notice.StartsWith("1 of 4 schools are shown"));
        }

        [Fact]
        public void The_active_users_card_ignores_the_school_filter_but_not_the_scope()
        {
            var all = Dal.GetActiveUsersCount(Context());
            var ore = Dal.GetActiveUsersCount(Context(new DataScope(2)));
            var withTestAccounts = Dal.GetActiveUsersCount(Context(excludeTestAccounts: false));

            Assert.Equal(new ActiveUsersCount(8, 7), all);
            Assert.Equal(new ActiveUsersCount(2, 2), ore);
            Assert.True(withTestAccounts.ActiveUsers > all.ActiveUsers);   // the internal-domain teacher is active too
        }

        // ── data status and scope names ─────────────────────────────────────────────────

        [Fact]
        public void The_data_status_gives_the_last_day_of_each_family_ignoring_the_scope()
        {
            var status = UsageInsights.GetDataStatus(Context(new DataScope(2)));

            Assert.Equal(new DateOnly(2026, 10, 2), status.TestsDataUntil);
            Assert.Equal(new DateOnly(2026, 10, 5), status.ConnectionsDataUntil);
            Assert.Equal("mysql:gs_data_analysis", status.Source);
        }

        [Fact]
        public void The_school_of_a_user_is_read_from_the_database()
        {
            Assert.Equal(1, Dal.GetUserSchoolId(Context(), "10"));
            Assert.Null(Dal.GetUserSchoolId(Context(), "1"));          // admin without school
            Assert.Null(Dal.GetUserSchoolId(Context(), "9999"));
            Assert.Null(Dal.GetUserSchoolId(Context(), "abc"));
        }

        [Fact]
        public void Every_response_serializes_without_nulls_and_in_pascal_case()
        {
            var json = UsageInsightsJson.Serialize(Run("connections-by-school", September + "&limit=4"));

            Assert.Contains("\"Query\":\"connections-by-school\"", json);
            Assert.Contains("\"HasMoreRows\":false", json);
            Assert.Contains("\"TotalRows\":4", json);
            Assert.Contains("\"LastConnection\":\"2026-09-15\"", json);
            Assert.DoesNotContain(":null", json);
            Assert.DoesNotContain("\"query\"", json);
        }
    }
}
