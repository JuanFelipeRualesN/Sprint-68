using AIServicesAPI.Tests.Infrastructure;
using GSAIServicesAPI.Services.DataAssistant;
using Xunit;

namespace AIServicesAPI.Tests
{
    /// <summary>
    /// The pieces of the assistant on their own: tool catalog, download registry and figure verifier.
    /// </summary>
    public class DataAssistantComponentsTests
    {
        // ── tool catalog ────────────────────────────────────────────────────────────────

        [Fact]
        public void The_catalog_loads_the_13_tools_with_the_names_of_the_contract()
        {
            var catalog = new DataAssistantToolCatalog();

            Assert.Equal(new[]
            {
                "get_subject_area_usage", "search_schools", "get_connections_by_school", "get_connections_by_teacher", "compare_subject_areas", "compare_schools",
                "get_full_quiz_average", "get_connections_trend", "get_cumulative_connections", "get_teacher_activity", "get_connection_patterns",
                "get_school_participation", "get_users_by_subscription",
            }, catalog.Tools.Select(tool => tool.Name));
            Assert.Equal(new[]
            {
                "subject-area-usage", "search-schools", "connections-by-school", "connections-by-teacher", "compare-subject-areas", "compare-schools",
                "full-quiz-average", "connections-trend", "cumulative-connections", "teacher-activity", "connection-patterns", "school-participation", "users-by-subscription",
            }, catalog.Tools.Select(tool => tool.QueryName));
        }

        [Fact]
        public void The_request_tools_are_function_tools_with_a_json_schema()
        {
            var catalog = new DataAssistantToolCatalog();

            var tools = catalog.BuildToolsJson();

            Assert.Equal(13, tools.Count);
            Assert.All(tools, tool => Assert.Equal("function", tool!["type"]!.GetValue<string>()));
            var schema = tools.Single(tool => tool!["function"]!["name"]!.GetValue<string>() == "compare_schools")!["function"]!["parameters"]!;
            Assert.Equal("object", schema["type"]!.GetValue<string>());
            Assert.Equal(new[] { "fromA", "toA", "fromB", "toB" }, schema["required"]!.AsArray().Select(item => item!.GetValue<string>()));
            Assert.Equal("string", schema["properties"]!["fromA"]!["type"]!.GetValue<string>());
            Assert.NotSame(catalog.BuildToolsJson(), tools);   // a node can only have one parent: every call builds a new array
        }

        [Fact]
        public void Every_tool_uses_english_names_and_the_parameters_of_the_contract()
        {
            var catalog = new DataAssistantToolCatalog();
            var parameterNames = catalog.Tools.SelectMany(tool => tool.ParameterNames).Distinct().OrderBy(name => name).ToList();

            Assert.Equal(new[]
            {
                "date", "from", "fromA", "fromB", "groupBy", "granularity", "limit", "maxConnections", "minConnections", "order", "periodFrom", "school", "text", "to", "toA", "toB",
            }.OrderBy(name => name), parameterNames);
            Assert.All(catalog.Tools, tool => Assert.Matches("^[a-z_]+$", tool.Name));
            Assert.All(catalog.Tools, tool => Assert.Matches("^[a-z-]+$", tool.QueryName));
        }

        [Fact]
        public void The_enum_values_are_the_english_ones()
        {
            var catalog = new DataAssistantToolCatalog();
            var schema = System.Text.Json.Nodes.JsonNode.Parse(catalog.Tools.Single(tool => tool.Name == "get_cumulative_connections").ParametersSchema.ToString())!;

            Assert.Equal(new[] { "day", "month", "quarter", "year" }, schema["properties"]!["granularity"]!["enum"]!.AsArray().Select(item => item!.GetValue<string>()));
        }

        [Fact]
        public void The_instructions_are_in_english_with_the_placeholders_the_service_fills()
        {
            var catalog = new DataAssistantToolCatalog();

            foreach (var placeholder in new[] { "{today}", "{quarter_from}", "{quarter_to}", "{last_month_from}", "{last_month_to}", "{year_start}", "{academic_year_from}",
                "{academic_year_to}", "{current_academic_year_from}" })
            {
                Assert.Contains(placeholder, catalog.InstructionsTemplate);
            }

            Assert.Contains("answer in the language of the CURRENT QUESTION", catalog.InstructionsTemplate);
            Assert.Contains("Do NOT write a block of references", catalog.InstructionsTemplate);
            Assert.DoesNotContain("Referencias", catalog.InstructionsTemplate);
        }

        [Fact]
        public void The_catalog_loads_the_dashboard_knowledge_base_with_the_pages_and_the_terms_and_without_data()
        {
            var catalog = new DataAssistantToolCatalog();

            Assert.Contains("% de Conexiones redesign (administrativo demo)", catalog.KnowledgeBase);
            Assert.Contains("Reporte Académico (end user-demo)", catalog.KnowledgeBase);
            Assert.Contains("Conteo_Conectados_Si_Total", catalog.KnowledgeBase);
            Assert.Contains("CAN PASS 100 %", catalog.KnowledgeBase);
            Assert.Contains("\n7. KNOWLEDGE BASE", catalog.InstructionsTemplate);
            Assert.DoesNotContain("{", catalog.KnowledgeBase);                          // no placeholders: the dates are not replaced in it
            Assert.DoesNotContain("@", catalog.KnowledgeBase);                          // no emails or other personal data
            Assert.Equal(string.Empty, new DataAssistantToolCatalog("[]", "instructions").KnowledgeBase);
        }

        [Fact]
        public void The_catalog_can_be_built_from_given_texts_and_finds_tools_by_name()
        {
            var catalog = new DataAssistantToolCatalog("""[{"Name":"t","Route":"r","Description":"d","Parameters":{"a":{"type":"string"}},"Required":["a"]}]""", "  instructions  ");

            Assert.True(catalog.TryGetTool("t", out var tool));
            Assert.Equal("r", tool.QueryName);
            Assert.Contains("a", tool.ParameterNames);
            Assert.False(catalog.TryGetTool("nope", out _));
            Assert.Equal("instructions", catalog.InstructionsTemplate);
        }

        // ── download registry ───────────────────────────────────────────────────────────

        [Fact]
        public void A_download_belongs_to_the_person_who_asked()
        {
            var registry = new DataAssistantDownloadRegistry(new ManualTimeProvider(DateTimeOffset.UtcNow));

            var id = registry.Register("connections-by-school", "from=2026-09-01", "1");

            Assert.Matches("^d_[0-9a-f]{32}$", id);
            Assert.NotNull(registry.Find(id, "1"));
            Assert.Null(registry.Find(id, "2"));          // somebody else's id: same result as an unknown one
            Assert.Null(registry.Find("d_unknown", "1"));
            Assert.Equal("connections-by-school", registry.Find(id, "1")!.QueryName);
            Assert.Equal("from=2026-09-01", registry.Find(id, "1")!.QueryString);
        }

        [Fact]
        public void A_download_expires_after_two_hours()
        {
            var clock = new ManualTimeProvider(new DateTimeOffset(2026, 11, 23, 12, 0, 0, TimeSpan.Zero));
            var registry = new DataAssistantDownloadRegistry(clock);
            var id = registry.Register("q", "a=1", "1");

            clock.Advance(TimeSpan.FromMinutes(119));
            Assert.NotNull(registry.Find(id, "1"));
            clock.Advance(TimeSpan.FromMinutes(2));
            Assert.Null(registry.Find(id, "1"));
        }

        [Fact]
        public void The_registry_never_holds_more_than_1000_downloads_and_drops_the_oldest()
        {
            var clock = new ManualTimeProvider(new DateTimeOffset(2026, 11, 23, 12, 0, 0, TimeSpan.Zero));
            var registry = new DataAssistantDownloadRegistry(clock);
            var first = registry.Register("q", "a=0", "1");
            var ids = new List<string>();
            for (var i = 1; i <= DataAssistantDownloadRegistry.MaxEntries; i++)
            {
                clock.Advance(TimeSpan.FromSeconds(1));
                ids.Add(registry.Register("q", $"a={i}", "1"));
            }

            Assert.Null(registry.Find(first, "1"));
            Assert.NotNull(registry.Find(ids[^1], "1"));
            Assert.NotNull(registry.Find(ids[0], "1"));
            Assert.Equal(1000, DataAssistantDownloadRegistry.MaxEntries);
        }

        [Fact]
        public void The_ids_are_opaque_and_unique()
        {
            var registry = new DataAssistantDownloadRegistry();

            var ids = Enumerable.Range(0, 50).Select(_ => registry.Register("q", "a=1", "1")).ToList();

            Assert.Equal(50, ids.Distinct().Count());
        }

        // ── figure verifier ─────────────────────────────────────────────────────────────

        [Fact]
        public void A_figure_that_comes_from_a_query_is_not_flagged()
        {
            var result = AnswerFigureVerifier.Verify("question", string.Empty, "North High had 8 connections and 47.1 %.", new[] { """{"Connections":8,"PercentageOfTotal":47.1}""" },
                new[] { "{}" }, "2026-11-23");

            Assert.Empty(result);
        }

        [Fact]
        public void Figures_are_compared_by_their_digits_so_separators_do_not_matter()
        {
            var result = AnswerFigureVerifier.Verify("question", string.Empty, "There were 1,234 connections.", new[] { """{"Connections":1234}""" }, Array.Empty<string>(), "2026-11-23");

            Assert.Empty(result);
        }

        [Fact]
        public void An_invented_figure_is_flagged_shortest_first_and_at_most_eight()
        {
            var answer = "Numbers: 11111111 222 33 4 5 6 7 8 9 10 11 12.";

            var result = AnswerFigureVerifier.Verify("question", string.Empty, answer, new[] { """{"Connections":1}""" }, Array.Empty<string>(), "2026-11-23");

            Assert.Equal(8, result.Count);
            Assert.Equal("4", result[0]);                 // the shortest figures go first
            Assert.DoesNotContain("1", result);           // known from the query
            Assert.DoesNotContain("11", result);          // known from today's date
        }

        [Fact]
        public void The_numbering_of_lists_the_question_the_context_and_today_are_not_flagged()
        {
            var answer = "1. First\n2. Second\nAs of 2026-11-23, about 500 items for the page 3.";

            var result = AnswerFigureVerifier.Verify("Is it 500?", "[Dashboard context: page 3]", answer, new[] { "{}" }, Array.Empty<string>(), "2026-11-23");

            Assert.Empty(result);
        }

        [Theory]
        [InlineData("From 1 de enero to today.")]
        [InlineData("From January 1 to today, 1 January 2026.")]
        public void Without_queries_the_days_of_a_month_written_in_words_are_not_data(string answer)
        {
            var result = AnswerFigureVerifier.Verify("Year to date?", string.Empty, answer, Array.Empty<string>(), Array.Empty<string>(), "2026-11-23");

            Assert.DoesNotContain("1", result);
        }

        [Fact]
        public void The_figures_of_previous_turns_are_known()
        {
            var previous = AnswerFigureVerifier.Accumulate(null, "first question 77", new[] { """{"Connections":4321}""" }, new[] { """{"from":"2026-09-01"}""" });

            var result = AnswerFigureVerifier.Verify("and the ones above?", string.Empty, "Still 4321 and 77.", Array.Empty<string>(), Array.Empty<string>(), "2026-11-23", previous);

            Assert.Empty(result);
            Assert.Contains("4321", previous);
            Assert.Contains("9", previous);   // the month of the date argument
        }

        [Theory]
        [InlineData("0042", "42")]
        [InlineData("1,234.5", "12345")]
        [InlineData("000", "0")]
        public void Numbers_are_normalized_to_their_digits(string text, string expected)
        {
            Assert.Equal(expected, AnswerFigureVerifier.Normalize(text));
        }
    }
}
