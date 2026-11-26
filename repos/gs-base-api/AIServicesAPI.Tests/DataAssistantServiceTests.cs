using System.Text.Json.Nodes;
using AIServicesAPI.Tests.Infrastructure;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Services.DataAssistant;
using Xunit;

namespace AIServicesAPI.Tests
{
    /// <summary>
    /// The data assistant with a scripted model and a fake data analysis module: tool loop, arguments, scope, downloads, charts,
    /// figure verification, conversations and token accounting.
    /// </summary>
    public class DataAssistantServiceTests
    {
        private const string SeptemberArguments = """{"from":"2026-09-01","to":"2026-09-30"}""";

        // ── tool loop ───────────────────────────────────────────────────────────────────

        [Fact]
        public async Task A_question_runs_the_tool_the_model_asks_for_and_returns_the_answer_with_the_references_of_the_data_api()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("North High had 8 connections and South High 6.");

            var result = await factory.AskAsync("Which schools had the most connections in September 2026?");
            var response = result.Response;

            Assert.Equal("North High had 8 connections and South High 6.", response.Answer);
            Assert.Null(response.ErrorCode);
            Assert.StartsWith("c_", response.ConversationId);
            Assert.Equal(2, factory.Model.Calls.Count);
            Assert.Single(response.ExecutedQueries);
            Assert.Equal("get_connections_by_school", response.ExecutedQueries[0]["Tool"]!.GetValue<string>());
            Assert.Equal(200, response.ExecutedQueries[0]["Status"]!.GetValue<int>());
            Assert.Equal("2026-09-01", response.ExecutedQueries[0]["Arguments"]!["from"]!.GetValue<string>());

            // The references are copied from the answer of the data API, not written by the model.
            var reference = Assert.Single(response.References);
            Assert.Equal("connections-by-school", reference["Query"]!.GetValue<string>());
            Assert.Equal("mysql:gs_data_analysis", reference["Source"]!.GetValue<string>());
            Assert.Equal("all schools", reference["Scope"]!.GetValue<string>());
            Assert.Equal("2026-10-05", reference["DataUntil"]!.GetValue<string>());
            Assert.Equal(2, reference["TotalRows"]!.GetValue<int>());
            Assert.Equal("2026-11-23T15:00:00Z", reference["GeneratedAt"]!.GetValue<string>());
            Assert.Null(reference["Results"]);   // the data is not copied into the reference
            Assert.False(reference["HasMoreRows"]!.GetValue<bool>());
            Assert.Equal(17, reference["Summary"]!["TotalConnections"]!.GetValue<int>());
            Assert.StartsWith("A connection = a user with a login on a day", reference["Note"]!.GetValue<string>());

            var call = Assert.Single(factory.DataAnalysis.Calls, call => call.Kind == "query");
            Assert.Equal("connections-by-school", call.QueryName);
            Assert.Equal("from=2026-09-01&to=2026-09-30", call.QueryString);
            Assert.Empty(response.Warnings);
        }

        [Fact]
        public async Task The_model_receives_the_tool_result_the_instructions_with_the_dates_and_the_question()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("Done.");

            await factory.AskAsync("Connections last month?", context: new DashboardContextDto("Usage", "Connections"), language: "en");

            var firstCall = factory.Model.Calls[0];
            Assert.Equal(DataAssistantChatRole.System, firstCall[0].Role);
            Assert.Contains("Today is 2026-11-23", firstCall[0].Content);
            Assert.Contains("from 2026-10-01 to 2026-10-31", firstCall[0].Content);          // last month, calculated by the code
            Assert.Contains("from 2026-08-01 to 2026-10-31", firstCall[0].Content);          // last quarter (3 complete months)
            Assert.Contains("from 2025-08-01 to 2026-07-31", firstCall[0].Content);          // last academic year
            Assert.DoesNotContain("{today}", firstCall[0].Content);
            var userMessage = firstCall[^1];
            Assert.Equal(DataAssistantChatRole.User, userMessage.Role);
            Assert.Contains("report = \"Usage\", page = \"Connections\"", userMessage.Content);
            Assert.Contains("[Panel language: English]", userMessage.Content);
            Assert.EndsWith("Connections last month?", userMessage.Content);

            var secondCall = factory.Model.Calls[1];
            Assert.Equal(DataAssistantChatRole.Assistant, secondCall[^2].Role);
            Assert.Single(secondCall[^2].ToolCalls!);
            Assert.Equal(DataAssistantChatRole.Tool, secondCall[^1].Role);
            Assert.Equal("call-1", secondCall[^1].ToolCallId);
            Assert.Contains("\"Query\":\"connections-by-school\"", secondCall[^1].Content);
        }

        [Fact]
        public async Task The_panel_language_is_spanish_unless_the_panel_says_english()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("Hola.").Reply("Hello.");

            await factory.AskAsync("Hola");
            await factory.AskAsync("Hello", language: "en-US");

            Assert.Contains("[Panel language: Spanish]", factory.Model.Calls[0][^1].Content);
            Assert.Contains("[Panel language: English]", factory.Model.Calls[1][^1].Content);
        }

        [Fact]
        public async Task The_instructions_carry_the_knowledge_base_after_the_rules()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("Ok.");

            await factory.AskAsync("What does % Conexión mean?");

            var instructions = factory.Model.Calls[0][0].Content!;
            var rules = instructions.IndexOf("\n7. KNOWLEDGE BASE", StringComparison.Ordinal);
            var knowledge = instructions.IndexOf("KNOWLEDGE BASE (explains the dashboard", StringComparison.Ordinal);
            Assert.True(rules > 0 && knowledge > rules);
            Assert.Contains("Conteo_Conectados_Si_Total", instructions[knowledge..]);
        }

        [Fact]
        public async Task An_explanation_of_the_dashboard_without_queries_does_not_warn_about_figures_of_the_knowledge_base()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("The percentage can pass 100 % because it counts every connected user.");

            var response = (await factory.AskAsync("Why can the connection percentage go above the total?")).Response;

            Assert.Empty(factory.DataAnalysis.Calls);
            Assert.Empty(response.Warnings);
        }

        [Fact]
        public async Task A_question_that_needs_no_tool_gets_a_direct_answer_without_queries()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("For which period?");

            var response = (await factory.AskAsync("How many connections?")).Response;

            Assert.Equal("For which period?", response.Answer);
            Assert.Empty(response.ExecutedQueries);
            Assert.Empty(response.References);
            Assert.Null(response.Downloads);
            Assert.Null(response.Charts);
            Assert.Empty(factory.DataAnalysis.Calls);
        }

        [Fact]
        public async Task The_tool_loop_stops_after_six_steps()
        {
            var factory = new DataAssistantTestFactory { };
            factory.Model.Fallback = _ => new DataAssistantModelReply(null, new[] { new DataAssistantToolCall("loop", "get_connections_by_school", SeptemberArguments) }, 10);

            var result = await factory.AskAsync("Keep asking");

            Assert.Equal(DataAssistantService.MaxSteps, factory.Model.Calls.Count);
            Assert.Equal(6, DataAssistantService.MaxSteps);
            Assert.Equal(6, factory.DataAnalysis.Calls.Count(call => call.Kind == "query"));
            Assert.Equal("I could not complete the query. Try again in a moment.", result.Response.Answer);
            Assert.Equal(60, result.TotalTokens);
        }

        [Fact]
        public async Task The_fallback_answer_is_localized()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Fallback = _ => new DataAssistantModelReply(null, new[] { new DataAssistantToolCall("loop", "search_schools", """{"text":"high"}""") }, 1);

            var result = await factory.AskAsync("loop", DataAssistantTestFactory.Caller(language: "es"));

            Assert.Equal("No pude completar la consulta. Intenta de nuevo en un momento.", result.Response.Answer);
        }

        [Fact]
        public async Task The_tokens_of_every_model_call_are_added_up()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments, tokens: 1200).CallTool("search_schools", """{"text":"high"}""", tokens: 300, id: "call-2").Reply("Done.", tokens: 450);

            var result = await factory.AskAsync("Question");

            Assert.Equal(1950, result.TotalTokens);
        }

        [Fact]
        public void The_estimate_covers_the_instructions_the_tools_and_the_output_of_every_step()
        {
            var factory = new DataAssistantTestFactory();

            var estimate = factory.Service.EstimateTokens(new DataAssistantAskRequest("A question about the platform usage", null, null), "42");

            Assert.True(estimate > 1500 * DataAssistantService.MaxSteps);
            Assert.True(estimate < 1500 * DataAssistantService.MaxSteps + 20000);
        }

        // ── arguments, scope and token ──────────────────────────────────────────────────

        [Fact]
        public async Task Arguments_the_tool_does_not_declare_are_dropped()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", """{"from":"2026-09-01","to":"2026-09-30","scope":"all","oreId":5,"role":"admin","token":"x","limit":3}""").Reply("Done.");

            var response = (await factory.AskAsync("Question")).Response;

            var call = Assert.Single(factory.DataAnalysis.Calls, call => call.Kind == "query");
            Assert.Equal("from=2026-09-01&to=2026-09-30&limit=3", call.QueryString);
            var arguments = response.ExecutedQueries[0]["Arguments"]!.AsObject();
            Assert.Equal(new[] { "from", "to", "limit" }, arguments.Select(argument => argument.Key));
        }

        [Fact]
        public async Task The_model_cannot_change_the_scope_because_the_token_is_forwarded_and_the_scope_is_not_an_argument()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", """{"from":"2026-09-01","to":"2026-09-30","ore":"1","oreId":"1","school":"South High","scope":"all"}""").Reply("Done.");

            await factory.AskAsync("Question", DataAssistantTestFactory.Caller("77", "the-session-token-of-77", "es"));

            foreach (var call in factory.DataAnalysis.Calls)
            {
                Assert.Equal("the-session-token-of-77", call.Token);
                Assert.Equal("es", call.Language);
                Assert.DoesNotContain("ore", call.QueryString.Replace("school", string.Empty));
                Assert.DoesNotContain("scope", call.QueryString);
            }

            Assert.Contains(factory.DataAnalysis.Calls, call => call.QueryString.Contains("school=South%20High"));
        }

        [Fact]
        public void No_tool_declares_a_parameter_that_could_change_the_data_scope_or_the_identity()
        {
            var catalog = new DataAssistantToolCatalog();
            var forbidden = new[] { "scope", "ore", "oreId", "schoolId", "role", "roleId", "user", "userId", "token", "permission", "format", "exclude", "testAccounts" };

            Assert.Equal(13, catalog.Tools.Count);
            foreach (var tool in catalog.Tools)
            {
                Assert.Empty(tool.ParameterNames.Intersect(forbidden, StringComparer.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task An_unknown_tool_is_answered_with_an_error_and_never_reaches_the_data_api()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("drop_all_tables", "{}").Reply("I cannot do that.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Empty(factory.DataAnalysis.Calls);
            Assert.Equal(400, response.ExecutedQueries[0]["Status"]!.GetValue<int>());
            Assert.Contains("unknown_query", factory.Model.Calls[1][^1].Content);
            Assert.Empty(response.References);
        }

        [Fact]
        public async Task Malformed_arguments_are_treated_as_empty()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", "{not json").Reply("Done.");

            await factory.AskAsync("Question");

            Assert.Equal(string.Empty, Assert.Single(factory.DataAnalysis.Calls, call => call.Kind == "query").QueryString);
        }

        [Fact]
        public async Task A_data_api_error_is_given_to_the_model_without_references_downloads_or_charts()
        {
            var factory = new DataAssistantTestFactory();
            factory.DataAnalysis.OnQuery = (_, _) => FakeDataAnalysisFeignAPI.Json(400, """{"error":"'from' cannot be later than 'to'.","ErrorCode":"invalid_parameters"}""");
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("The dates are wrong.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Equal(400, response.ExecutedQueries[0]["Status"]!.GetValue<int>());
            Assert.Empty(response.References);
            Assert.Null(response.Downloads);
            Assert.Null(response.Charts);
            Assert.Contains("invalid_parameters", factory.Model.Calls[1][^1].Content);
        }

        [Fact]
        public async Task A_data_api_that_does_not_answer_is_reported_to_the_model_and_the_conversation_goes_on()
        {
            var factory = new DataAssistantTestFactory();
            factory.DataAnalysis.OnQuery = (_, _) => throw new HttpRequestException("connection refused");
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("The data is not available right now.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Equal(502, response.ExecutedQueries[0]["Status"]!.GetValue<int>());
            Assert.Contains("data_unavailable", factory.Model.Calls[1][^1].Content);
            Assert.Equal("The data is not available right now.", response.Answer);
        }

        // ── downloads and charts ────────────────────────────────────────────────────────

        [Fact]
        public async Task A_successful_query_registers_a_download_and_builds_a_chart_with_the_same_id()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", """{"from":"2026-09-01","to":"2026-09-30","limit":5,"order":"desc"}""").Reply("Done.");

            var response = (await factory.AskAsync("Question", DataAssistantTestFactory.Caller("42"))).Response;

            var download = Assert.Single(response.Downloads!);
            var id = download["Id"]!.GetValue<string>();
            Assert.StartsWith("d_", id);
            Assert.Equal("connections-by-school_2026-09-01_2026-09-30.xlsx", download["FileName"]!.GetValue<string>());
            Assert.Equal("connections-by-school", download["Query"]!.GetValue<string>());
            Assert.Equal("/aiservicesapi/data-assistant/downloads/" + id, download["Url"]!.GetValue<string>());
            Assert.False(download["Highlighted"]!.GetValue<bool>());

            var chart = Assert.Single(response.Charts!);
            Assert.Equal(id, chart["Id"]!.GetValue<string>());
            Assert.Equal("/aiservicesapi/data-assistant/charts/" + id, chart["Url"]!.GetValue<string>());
            Assert.Equal("bar", chart["Type"]!.GetValue<string>());
            Assert.False(chart["Highlighted"]!.GetValue<bool>());

            // The Excel has the whole list (no limit); the chart goes with a preview limit between 1 and 10 and no chart parameters of the chat.
            var registration = factory.Downloads.Find(id, "42")!;
            Assert.Equal("from=2026-09-01&to=2026-09-30&order=desc", registration.QueryString);
            var export = Assert.Single(factory.DataAnalysis.Calls, call => call.Kind == "export");
            Assert.Equal("chart", export.Format);
            Assert.Equal("from=2026-09-01&to=2026-09-30&order=desc&limit=5", export.QueryString);
        }

        [Fact]
        public async Task A_request_for_a_file_or_a_chart_highlights_it()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("Done.");
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("Done.");

            var excel = (await factory.AskAsync("Give me an Excel with the connections by school")).Response;
            var chart = (await factory.AskAsync("Draw a chart of the connections by school", DataAssistantTestFactory.Caller("43", "token-43"))).Response;
            var spanish = new DataAssistantTestFactory();
            spanish.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("Listo.");
            var downloadAnswer = (await spanish.AskAsync("Quiero descargar las conexiones por escuela")).Response;

            Assert.True(excel.Downloads![0]["Highlighted"]!.GetValue<bool>());
            Assert.True(chart.Charts![0]["Highlighted"]!.GetValue<bool>());
            Assert.True(downloadAnswer.Downloads![0]["Highlighted"]!.GetValue<bool>());
        }

        [Fact]
        public async Task The_school_search_has_no_download_and_no_chart()
        {
            var factory = new DataAssistantTestFactory();
            factory.DataAnalysis.OnQuery = (queryName, _) => FakeDataAnalysisFeignAPI.Json(200, """{"Query":"search-schools","Results":[{"Name":"North High"}],"TotalRows":1,"Notices":[]}""");
            factory.Model.CallTool("search_schools", """{"text":"north"}""").Reply("North High.");

            var response = (await factory.AskAsync("Is there a school called north?")).Response;

            Assert.Null(response.Downloads);
            Assert.Null(response.Charts);
            Assert.Single(response.References);
            Assert.DoesNotContain(factory.DataAnalysis.Calls, call => call.Kind == "export");
        }

        [Fact]
        public async Task A_query_without_a_chart_still_has_its_download()
        {
            var factory = new DataAssistantTestFactory();
            factory.DataAnalysis.OnExport = (queryName, format, _) => format == "chart"
                ? FakeDataAnalysisFeignAPI.Json(422, """{"error":"no chart","ErrorCode":"chart_unavailable"}""")
                : new DataAnalysisApiResponse(200, new byte[] { 1 }, "application/octet-stream", "x.xlsx");
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("Done.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Single(response.Downloads!);
            Assert.Null(response.Charts);
        }

        [Theory]
        [InlineData("http")]
        [InlineData("timeout")]
        [InlineData("not-json")]
        [InlineData("not-configured")]
        public async Task A_chart_that_cannot_be_built_does_not_drop_the_answer(string failure)
        {
            var factory = new DataAssistantTestFactory();
            factory.DataAnalysis.OnExport = (queryName, format, _) => format != "chart"
                ? new DataAnalysisApiResponse(200, new byte[] { 1 }, "application/octet-stream", "x.xlsx")
                : failure switch
                {
                    "http" => throw new HttpRequestException("down"),
                    "timeout" => throw new TaskCanceledException("timeout"),
                    "not-configured" => throw new InvalidOperationException("Missing config: AppSettings:data_analysis_api_url"),
                    _ => FakeDataAnalysisFeignAPI.Json(200, "<html>not json</html>"),
                };
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("North High had 8 connections.");

            var result = await factory.AskAsync("Question");
            var response = result.Response;

            Assert.Equal("North High had 8 connections.", response.Answer);
            Assert.Null(response.ErrorCode);
            Assert.Single(response.References);
            Assert.Single(response.Downloads!);   // the download was registered before the chart was asked for
            Assert.Null(response.Charts);
            Assert.Equal(200, response.ExecutedQueries[0]["Status"]!.GetValue<int>());
        }

        [Fact]
        public async Task The_same_query_asked_twice_in_one_question_registers_one_download()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).CallTool("get_connections_by_school", SeptemberArguments, id: "call-2").Reply("Done.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Single(response.Downloads!);
            Assert.Equal(2, response.References.Count);
            Assert.Equal(2, response.ExecutedQueries.Count);
        }

        [Fact]
        public async Task The_notices_of_the_data_api_are_returned_without_repeating()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).CallTool("get_connections_by_school", """{"from":"2026-09-01","to":"2026-10-31"}""", id: "call-2").Reply("Done.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Single(response.Notices);
            Assert.StartsWith("The loaded data only goes until 2026-10-05", response.Notices[0]);
        }

        [Theory]
        [InlineData("limit=5&format=xlsx&order=desc", "order=desc")]
        [InlineData("from=2026-09-01&to=2026-09-30", "from=2026-09-01&to=2026-09-30")]
        public void The_excel_query_drops_the_limit_and_the_format(string chatQuery, string expected)
        {
            Assert.Equal(expected, DataAssistantService.BuildExcelQuery(chatQuery));
        }

        [Theory]
        [InlineData("from=2026-09-01&to=2026-09-30&limit=5", "from=2026-09-01&to=2026-09-30&limit=5")]
        [InlineData("from=2026-09-01&limit=50", "from=2026-09-01")]
        [InlineData("from=2026-09-01&limit=0", "from=2026-09-01")]
        [InlineData("from=2026-09-01&all=true&includeZeros=true&format=xlsx", "from=2026-09-01")]
        public void The_chart_query_keeps_a_preview_limit_from_1_to_10_and_drops_the_chart_parameters(string chatQuery, string expected)
        {
            Assert.Equal(expected, DataAssistantService.BuildChartQuery(chatQuery));
        }

        [Fact]
        public void The_expanded_chart_query_puts_the_options_of_the_person_over_the_ones_of_the_chat()
        {
            var requested = new Microsoft.AspNetCore.Http.QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                ["all"] = "true", ["order"] = "asc", ["limit"] = "7", ["ignored"] = "x",
            });

            var query = DataAssistantService.BuildExpandedChartQuery("from=2026-09-01&to=2026-09-30&order=desc", requested);

            Assert.Equal(new[] { "from=2026-09-01", "to=2026-09-30", "all=true", "order=asc", "limit=7" }.OrderBy(part => part), query.Split('&').OrderBy(part => part));
        }

        // ── verification of the figures ─────────────────────────────────────────────────

        [Fact]
        public async Task An_answer_with_a_figure_that_no_query_returned_gets_a_warning()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("North High had 8 connections, which is 99999 more than last year.");

            var response = (await factory.AskAsync("Question")).Response;

            Assert.Single(response.Warnings);
            Assert.Contains("99999", response.Warnings[0]);
            Assert.StartsWith("There are figures in the answer that do not come from the queries", response.Warnings[0]);
        }

        [Fact]
        public async Task Figures_without_any_query_get_their_own_warning()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("There were 12345 connections.");

            var response = (await factory.AskAsync("How many connections were there?")).Response;

            Assert.StartsWith("The answer has figures but no query was run", response.Warnings[0]);
        }

        [Fact]
        public async Task Dates_of_the_question_the_arguments_and_today_are_not_unexplained_figures()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("From 2026-09-01 to 2026-09-30 North High had 8 connections (47.1 %) on 2026-11-23 and 1 of 17.");

            var response = (await factory.AskAsync("Connections in September 2026?")).Response;

            Assert.Empty(response.Warnings);
        }

        // ── conversations ───────────────────────────────────────────────────────────────

        [Fact]
        public async Task A_conversation_continues_with_its_history_and_marks_every_message_as_new()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.CallTool("get_connections_by_school", SeptemberArguments).Reply("North High had 8.").Reply("Second answer.");

            var first = (await factory.AskAsync("First question")).Response;
            var second = (await factory.AskAsync("And the next month?", conversationId: first.ConversationId)).Response;

            Assert.Equal(first.ConversationId, second.ConversationId);
            var lastCall = factory.Model.Calls[^1];
            Assert.Contains(lastCall, message => message.Role == DataAssistantChatRole.Tool);                      // the history has the previous tool result
            Assert.Contains(lastCall, message => message.Content?.Contains("First question") == true);
            Assert.Contains("[New message: answer ONLY this question", lastCall[^1].Content);
            Assert.EndsWith("And the next month?", lastCall[^1].Content);
            Assert.Equal(DataAssistantChatRole.System, lastCall[0].Role);
            Assert.Single(lastCall, message => message.Role == DataAssistantChatRole.System);
        }

        [Fact]
        public async Task A_conversation_is_tied_to_its_user_and_the_id_of_somebody_else_starts_a_new_one()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("Mine.").Reply("Not yours.");

            var mine = (await factory.AskAsync("Private question", DataAssistantTestFactory.Caller("1", "token-1"))).Response;
            var intruder = (await factory.AskAsync("Another question", DataAssistantTestFactory.Caller("2", "token-2"), mine.ConversationId)).Response;

            Assert.NotEqual(mine.ConversationId, intruder.ConversationId);
            var intruderCall = factory.Model.Calls[1];
            Assert.DoesNotContain(intruderCall, message => message.Content?.Contains("Private question") == true);
            Assert.DoesNotContain("[New message", intruderCall[^1].Content);
            Assert.Null(factory.Conversations.Get(mine.ConversationId, "2"));
            Assert.NotNull(factory.Conversations.Get(mine.ConversationId, "1"));
        }

        [Fact]
        public async Task An_unknown_conversation_id_starts_a_new_conversation()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("Hi.");

            var response = (await factory.AskAsync("Question", conversationId: "c_doesnotexist")).Response;

            Assert.NotEqual("c_doesnotexist", response.ConversationId);
        }

        [Fact]
        public async Task A_conversation_expires()
        {
            var factory = new DataAssistantTestFactory(conversationLifetime: TimeSpan.FromMilliseconds(150));
            factory.Model.Reply("First.").Reply("Second.");

            var first = (await factory.AskAsync("First question")).Response;
            Assert.NotNull(factory.Conversations.Get(first.ConversationId, "42"));
            await Task.Delay(400);

            Assert.Null(factory.Conversations.Get(first.ConversationId, "42"));
            var second = (await factory.AskAsync("Second question", conversationId: first.ConversationId)).Response;
            Assert.NotEqual(first.ConversationId, second.ConversationId);
        }

        [Fact]
        public void A_conversation_lives_two_hours_by_default()
        {
            Assert.Equal(TimeSpan.FromHours(2), DataAssistantConversationStore.ConversationLifetime);
            Assert.Equal(TimeSpan.FromHours(2), DataAssistantDownloadRegistry.Lifetime);
        }

        [Fact]
        public async Task The_history_is_capped_and_always_starts_with_a_message_of_the_person()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Fallback = messages => messages[^1].Role == DataAssistantChatRole.Tool
                ? new DataAssistantModelReply("Answer.", Array.Empty<DataAssistantToolCall>(), 5)
                : new DataAssistantModelReply(null, new[] { new DataAssistantToolCall("c", "search_schools", """{"text":"high"}""") }, 5);
            string? conversationId = null;

            for (var turn = 0; turn < 12; turn++)
            {
                conversationId = (await factory.AskAsync("Question " + turn, conversationId: conversationId)).Response.ConversationId;
            }

            var saved = factory.Conversations.Get(conversationId!, "42")!;
            Assert.True(saved.Messages.Count <= DataAssistantService.MaxHistoryMessages);
            Assert.Equal(DataAssistantChatRole.User, saved.Messages[0].Role);
            Assert.Contains("Question 11", saved.Messages[^4].Content);
        }

        [Fact]
        public async Task A_long_question_is_cut_to_1000_characters()
        {
            var factory = new DataAssistantTestFactory();
            factory.Model.Reply("Ok.");

            await factory.AskAsync(new string('x', 5000));

            var sent = factory.Model.Calls[0][^1].Content!;
            Assert.Equal(1000, sent.Split('\n')[^1].Length);
        }

        // ── academic year ───────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(2026, 11, 23, "2025-08-01", "2026-07-31", "2026-08-01")]
        [InlineData(2026, 8, 1, "2025-08-01", "2026-07-31", "2026-08-01")]
        [InlineData(2026, 7, 31, "2024-08-01", "2025-07-31", "2025-08-01")]
        [InlineData(2026, 1, 15, "2024-08-01", "2025-07-31", "2025-08-01")]
        public void The_default_academic_year_goes_from_august_to_july(int year, int month, int day, string from, string to, string currentFrom)
        {
            var (academicFrom, academicTo, academicCurrentFrom) = DataAssistantService.GetAcademicYear(new DateOnly(year, month, day));

            Assert.Equal(from, academicFrom.ToString("yyyy-MM-dd"));
            Assert.Equal(to, academicTo.ToString("yyyy-MM-dd"));
            Assert.Equal(currentFrom, academicCurrentFrom.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async Task Today_is_read_in_the_local_time_of_the_usage_insights()
        {
            var factory = new DataAssistantTestFactory(new DateTimeOffset(2026, 11, 24, 2, 0, 0, TimeSpan.Zero));   // 22:00 of the 23rd in Puerto Rico
            factory.Model.Reply("Ok.");

            await factory.AskAsync("Question");

            Assert.Contains("Today is 2026-11-23", factory.Model.Calls[0][0].Content);
        }
    }
}
