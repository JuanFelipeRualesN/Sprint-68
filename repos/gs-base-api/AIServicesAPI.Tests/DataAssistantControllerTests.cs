using System.Net;
using System.Reflection;
using System.Text.Json;
using AIServicesAPI.Tests.Infrastructure;
using GSAIServicesAPI.Controllers;
using GSAIServicesAPI.Filters;
using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Services;
using GSAIServicesAPI.Services.DataAssistant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AIServicesAPI.Tests
{
    /// <summary>
    /// The HTTP contract of the assistant: session, role, AI switch, quota (a real MySQL), monitoring, error mapping and the owner of downloads and charts.
    /// </summary>
    [Collection(AssistantDatabaseCollection.Name)]
    public class DataAssistantControllerTests
    {
        private readonly AssistantDatabaseFixture _fixture;

        public DataAssistantControllerTests(AssistantDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private sealed class CapturingLogger<T> : ILogger<T>
        {
            public List<string> Messages { get; } = new();

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
                Messages.Add(formatter(state, exception));
        }

        private sealed class FixedRoleNames : IRoleNameResolver
        {
            public string? Resolve(string? roleId) => roleId switch { "2" => "admin", "8" => "admin-ore", "6" => "director", _ => roleId };
        }

        private sealed class Scenario
        {
            public required DataAssistantTestFactory Assistant { get; init; }

            public required FakePromptMonitoringService Monitoring { get; init; }

            public required CapturingLogger<DataAssistantController> ControllerLog { get; init; }

            public required Func<string?, string?, string, QueryString, DataAssistantController> CreateController { get; init; }
        }

        private Scenario CreateScenario()
        {
            var assistant = new DataAssistantTestFactory(configuration: _fixture.Configuration);
            var monitoring = new FakePromptMonitoringService();
            var controllerLog = new CapturingLogger<DataAssistantController>();

            DataAssistantController Create(string? token, string? language, string method, QueryString query)
            {
                var httpContext = new DefaultHttpContext();
                httpContext.Connection.RemoteIpAddress = IPAddress.Parse("203.0.113.9");
                httpContext.Request.Method = method;
                httpContext.Request.QueryString = query;
                if (token is not null)
                {
                    httpContext.Request.Headers["Token"] = token;
                }

                if (language is not null)
                {
                    httpContext.Request.Headers["Accept-Language"] = language;
                }

                return new DataAssistantController(_fixture.Configuration, assistant.Service, assistant.Downloads, assistant.DataAnalysis,
                    new GSAIServicesAPI.Services.Quotas.QuotaService(_fixture.CreateContext()), monitoring, new FixedRoleNames(), controllerLog)
                {
                    ControllerContext = new ControllerContext { HttpContext = httpContext },
                };
            }

            return new Scenario { Assistant = assistant, Monitoring = monitoring, ControllerLog = controllerLog, CreateController = Create };
        }

        private static string TokenOf(string userId, int roleId) => $"u{userId}-r{roleId}";

        private static JsonElement Body(IActionResult result) => JsonDocument.Parse(((ContentResult)result).Content!).RootElement;

        private static int Status(IActionResult result) => result switch
        {
            ContentResult content => content.StatusCode ?? 200,
            ObjectResult objectResult => objectResult.StatusCode ?? 200,
            FileResult => 200,
            _ => throw new InvalidOperationException(result.GetType().Name),
        };

        private static DataAssistantAskRequest Question(string text = "Connections by school in September 2026?", string? conversationId = null) =>
            new(text, conversationId, new DashboardContextDto("Usage", "Connections"), "en");

        private const string September = """{"from":"2026-09-01","to":"2026-09-30"}""";

        // ── session and role ────────────────────────────────────────────────────────────

        [Fact]
        public async Task A_request_without_a_valid_session_is_unauthorized_and_never_reaches_the_model()
        {
            var scenario = CreateScenario();

            var noToken = await scenario.CreateController(null, null, "POST", default).Ask(Question());
            var badToken = await scenario.CreateController("nobody", null, "POST", default).Ask(Question());

            Assert.Equal(401, Status(noToken));
            Assert.Equal(401, Status(badToken));
            Assert.Equal("unauthorized", Body(noToken).GetProperty("ErrorCode").GetString());
            Assert.Equal("Unauthorized. Please sign in and try again.", Body(noToken).GetProperty("error").GetString());
            Assert.Empty(scenario.Assistant.Model.Calls);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(1)]
        public async Task A_role_without_access_is_forbidden_for_ask_downloads_and_charts(int roleId)
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            var token = TokenOf(userId, roleId);

            var ask = await scenario.CreateController(token, null, "POST", default).Ask(Question());
            var download = await scenario.CreateController(token, null, "GET", default).GetDownload("d_x");
            var chart = await scenario.CreateController(token, null, "GET", default).GetChart("d_x");

            Assert.Equal(403, Status(ask));
            Assert.Equal(403, Status(download));
            Assert.Equal(403, Status(chart));
            Assert.Equal("forbidden", Body(ask).GetProperty("ErrorCode").GetString());
            Assert.Empty(scenario.Assistant.Model.Calls);
            Assert.Empty(scenario.Assistant.DataAnalysis.Calls);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(8)]
        [InlineData(6)]
        public async Task Administrators_ore_administrators_and_directors_can_ask(int roleId)
        {
            var scenario = CreateScenario();
            scenario.Assistant.Model.Reply("Ok.");

            var result = await scenario.CreateController(TokenOf(_fixture.NewUserId(), roleId), null, "POST", default).Ask(Question());

            Assert.Equal(200, Status(result));
        }

        [Fact]
        public async Task An_empty_question_is_a_bad_request_with_a_localized_message()
        {
            var scenario = CreateScenario();

            var english = await scenario.CreateController(TokenOf(_fixture.NewUserId(), 2), "en", "POST", default).Ask(Question("   "));
            var spanish = await scenario.CreateController(TokenOf(_fixture.NewUserId(), 2), "es", "POST", default).Ask(new DataAssistantAskRequest(null, null, null));

            Assert.Equal(400, Status(english));
            Assert.Equal("empty_question", Body(english).GetProperty("ErrorCode").GetString());
            Assert.Equal("The question is empty. Write your question about the platform usage.", Body(english).GetProperty("error").GetString());
            Assert.Equal("La pregunta está vacía. Escribe tu pregunta sobre el uso de la plataforma.", Body(spanish).GetProperty("error").GetString());
            Assert.Empty(scenario.Assistant.Model.Calls);
        }

        // ── the answer ──────────────────────────────────────────────────────────────────

        [Fact]
        public async Task A_question_is_answered_with_the_contract_fields_and_the_token_of_the_person_reaches_the_data_api()
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            var token = TokenOf(userId, 8);
            scenario.Assistant.Model.CallTool("get_connections_by_school", September).Reply("North High had 8 connections.", tokens: 321);

            var result = await scenario.CreateController(token, "es", "POST", default).Ask(Question());
            var body = Body(result);

            Assert.Equal(200, Status(result));
            Assert.Equal("North High had 8 connections.", body.GetProperty("Answer").GetString());
            Assert.StartsWith("c_", body.GetProperty("ConversationId").GetString());
            Assert.Equal(JsonValueKind.Null, body.GetProperty("ErrorCode").ValueKind);
            Assert.Equal("connections-by-school", body.GetProperty("References")[0].GetProperty("Query").GetString());
            Assert.Equal(1, body.GetProperty("Notices").GetArrayLength());
            Assert.Equal(0, body.GetProperty("Warnings").GetArrayLength());
            Assert.Equal("get_connections_by_school", body.GetProperty("ExecutedQueries")[0].GetProperty("Tool").GetString());
            var download = body.GetProperty("Downloads")[0];
            Assert.StartsWith("/aiservicesapi/data-assistant/downloads/d_", download.GetProperty("Url").GetString());
            Assert.StartsWith("/aiservicesapi/data-assistant/charts/d_", body.GetProperty("Charts")[0].GetProperty("Url").GetString());

            Assert.All(scenario.Assistant.DataAnalysis.Calls, call =>
            {
                Assert.Equal(token, call.Token);
                Assert.Equal("en", call.Language);   // the panel language of the body (Question() sends "en") wins over Accept-Language "es"
            });
        }

        [Theory]
        [InlineData("en", "es", "es")]       // the panel language in the body wins
        [InlineData("es", "en-US", "en")]
        [InlineData("es-PR,es;q=0.9", null, "es")]   // without it, Accept-Language decides
        [InlineData("en", "", "en")]
        [InlineData(null, null, "en")]
        public async Task The_panel_language_of_the_body_wins_over_accept_language(string? header, string? panelLanguage, string expected)
        {
            var scenario = CreateScenario();
            scenario.Assistant.Model.CallTool("get_connections_by_school", September).Reply("Ok.");

            await scenario.CreateController(TokenOf(_fixture.NewUserId(), 2), header, "POST", default)
                .Ask(new DataAssistantAskRequest("Question", null, null, panelLanguage));

            Assert.All(scenario.Assistant.DataAnalysis.Calls, call => Assert.Equal(expected, call.Language));
        }

        [Fact]
        public async Task The_prompt_monitoring_records_who_asked_what_with_the_data_assistant_subject()
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            scenario.Assistant.Model.Reply("The answer.");

            await scenario.CreateController(TokenOf(userId, 2), null, "POST", default).Ask(Question("What is the activity?"));

            var capture = Assert.Single(scenario.Monitoring.Captures);
            Assert.Equal(userId, capture.UserId);
            Assert.Equal("admin", capture.Role);
            Assert.Equal("data-assistant", capture.Subject);
            Assert.Equal("What is the activity?", capture.Prompt);
            Assert.Equal("The answer.", capture.Response);
            Assert.Null(capture.ClassificationJson);
            Assert.Equal("203.0.113.9", capture.IpAddress);
        }

        [Fact]
        public async Task The_prompt_is_not_recorded_when_the_question_fails()
        {
            var scenario = CreateScenario();
            scenario.Assistant.Model.Throw(new ModelInvocationException(503));

            await scenario.CreateController(TokenOf(_fixture.NewUserId(), 2), null, "POST", default).Ask(Question());

            Assert.Empty(scenario.Monitoring.Captures);
        }

        [Fact]
        public async Task A_monitoring_store_that_is_down_does_not_break_the_answer_and_the_quota_is_still_settled()
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            await _fixture.SetUserLimitAsync(userId, 1_000_000);
            scenario.Monitoring.Failure = new TimeoutException("mongo is down: the prompt text must not be logged");
            scenario.Assistant.Model.Reply("The answer.", tokens: 250);

            var result = await scenario.CreateController(TokenOf(userId, 2), null, "POST", default).Ask(Question("Sensitive question text"));

            Assert.Equal(200, Status(result));
            Assert.Equal("The answer.", Body(result).GetProperty("Answer").GetString());
            Assert.Equal(250, await _fixture.GetConsumedAsync(userId));
            Assert.Contains(scenario.ControllerLog.Messages, message => message.Contains("prompt monitoring could not store"));
            Assert.All(scenario.ControllerLog.Messages, message => Assert.DoesNotContain("Sensitive question text", message));
        }

        // ── AI switch ───────────────────────────────────────────────────────────────────

        [Fact]
        public void The_ask_action_requires_the_ai_switch_and_the_other_routes_do_not_call_the_model()
        {
            var ask = typeof(DataAssistantController).GetMethod(nameof(DataAssistantController.Ask))!;
            var download = typeof(DataAssistantController).GetMethod(nameof(DataAssistantController.GetDownload))!;
            var chart = typeof(DataAssistantController).GetMethod(nameof(DataAssistantController.GetChart))!;

            Assert.NotNull(ask.GetCustomAttribute<RequiresAiEnabledAttribute>());
            Assert.Null(download.GetCustomAttribute<RequiresAiEnabledAttribute>());
            Assert.Null(chart.GetCustomAttribute<RequiresAiEnabledAttribute>());
            Assert.Equal("aiservicesapi/data-assistant", typeof(DataAssistantController).GetCustomAttribute<RouteAttribute>()!.Template);
            Assert.Equal("ask", ask.GetCustomAttribute<HttpPostAttribute>()!.Template);
            Assert.Equal("downloads/{id}", download.GetCustomAttribute<HttpGetAttribute>()!.Template);
            Assert.Equal("charts/{id}", chart.GetCustomAttribute<HttpGetAttribute>()!.Template);
        }

        [Fact]
        public async Task When_the_ai_is_switched_off_the_filter_answers_503_before_the_action_runs()
        {
            await _fixture.SetAiEnabledAsync(false);
            try
            {
                var services = new ServiceCollection()
                    .AddSingleton<IConfiguration>(_fixture.Configuration)
                    .AddScoped(_ => _fixture.CreateContext())
                    .BuildServiceProvider();
                var httpContext = new DefaultHttpContext { RequestServices = services };
                httpContext.Request.Headers["Accept-Language"] = "es";
                var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
                var executing = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object?>(), new object());
                var nextCalled = false;

                await new RequiresAiEnabledAttribute().OnActionExecutionAsync(executing, () =>
                {
                    nextCalled = true;
                    return Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new object()));
                });

                Assert.False(nextCalled);
                var result = Assert.IsType<ObjectResult>(executing.Result);
                Assert.Equal(503, result.StatusCode);
                var payload = JsonSerializer.Serialize(result.Value);
                Assert.Contains("\"ErrorCode\":\"ai_disabled\"", payload);
                Assert.Contains("\"error\":", payload);   // the existing field is still there
            }
            finally
            {
                await _fixture.SetAiEnabledAsync(true);
            }
        }

        // ── quota ───────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task The_quota_is_settled_with_the_sum_of_the_tokens_of_every_model_call()
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            await _fixture.SetUserLimitAsync(userId, 1_000_000);
            scenario.Assistant.Model.CallTool("get_connections_by_school", September, tokens: 1200).CallTool("search_schools", """{"text":"high"}""", tokens: 300, id: "call-2").Reply("Done.", tokens: 450);

            var result = await scenario.CreateController(TokenOf(userId, 2), null, "POST", default).Ask(Question());

            Assert.Equal(200, Status(result));
            Assert.Equal(1950, await _fixture.GetConsumedAsync(userId));   // reserved the estimate, then settled the real sum
        }

        [Fact]
        public async Task A_spent_quota_gets_429_with_the_remaining_amount_and_the_reset_date_and_never_calls_the_model()
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            await _fixture.SetUserLimitAsync(userId, 100);
            scenario.Assistant.Model.Reply("First.", tokens: 400);

            var first = await scenario.CreateController(TokenOf(userId, 2), "en", "POST", default).Ask(Question());
            var second = await scenario.CreateController(TokenOf(userId, 2), "en", "POST", default).Ask(Question());

            Assert.Equal(200, Status(first));
            Assert.Equal(429, Status(second));
            var payload = JsonSerializer.Serialize(((ObjectResult)second).Value);
            Assert.Contains("You have reached your AI usage limit", payload);
            Assert.Contains("\"ErrorCode\":\"quota_exceeded\"", payload);
            Assert.Contains("remaining", payload);
            Assert.Contains("resetsAt", payload);
            Assert.Single(scenario.Assistant.Model.Calls);   // only the first question reached the model
        }

        [Fact]
        public async Task A_user_without_a_limit_is_not_enforced_and_still_gets_an_answer()
        {
            var scenario = CreateScenario();
            scenario.Assistant.Model.Reply("Ok.");

            var result = await scenario.CreateController(TokenOf(_fixture.NewUserId(), 2), null, "POST", default).Ask(Question());

            Assert.Equal(200, Status(result));
        }

        [Fact]
        public async Task The_reservation_is_released_when_the_model_fails_and_the_answers_are_mapped()
        {
            var scenario = CreateScenario();
            var unavailable = _fixture.NewUserId();
            var denied = _fixture.NewUserId();
            var blocked = _fixture.NewUserId();
            var network = _fixture.NewUserId();
            foreach (var user in new[] { unavailable, denied, blocked, network })
            {
                await _fixture.SetUserLimitAsync(user, 1_000_000);
            }

            scenario.Assistant.Model.Throw(new ModelInvocationException(503))
                .Throw(new ModelInvocationException(401))
                .Throw(new ModelInvocationException(400, isContentFiltered: true))
                .Throw(new HttpRequestException("network"));

            var unavailableResult = await scenario.CreateController(TokenOf(unavailable, 2), "en", "POST", default).Ask(Question());
            var deniedResult = await scenario.CreateController(TokenOf(denied, 2), "en", "POST", default).Ask(Question());
            var blockedResult = await scenario.CreateController(TokenOf(blocked, 2), "es", "POST", default).Ask(Question("Something blocked", "c_previous"));
            var networkResult = await scenario.CreateController(TokenOf(network, 2), "en", "POST", default).Ask(Question());

            Assert.Equal(502, Status(unavailableResult));
            Assert.Equal("model_unavailable", Body(unavailableResult).GetProperty("ErrorCode").GetString());
            Assert.Equal("The assistant is not available right now. Try again.", Body(unavailableResult).GetProperty("error").GetString());
            Assert.Equal(503, Status(deniedResult));
            Assert.Equal("model_access_denied", Body(deniedResult).GetProperty("ErrorCode").GetString());
            Assert.Equal(200, Status(blockedResult));   // like the Azure content filter of the MVP: 200 with an error code
            Assert.Equal("content_blocked", Body(blockedResult).GetProperty("ErrorCode").GetString());
            Assert.Equal("No puedo responder a ese mensaje. Reformula tu pregunta sobre el uso de la plataforma.", Body(blockedResult).GetProperty("Answer").GetString());
            Assert.Equal(502, Status(networkResult));
            foreach (var user in new[] { unavailable, denied, blocked, network })
            {
                Assert.Equal(0, await _fixture.GetConsumedAsync(user));   // nothing is charged when the question fails
            }

            Assert.Empty(scenario.Monitoring.Captures);
        }

        [Fact]
        public async Task The_token_never_appears_in_the_logs_of_the_failure_paths()
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            var token = TokenOf(userId, 2);
            scenario.Assistant.Model.Throw(new ModelInvocationException(401)).Throw(new ModelInvocationException(503));
            scenario.Assistant.DataAnalysis.OnExport = (_, _, _) => throw new HttpRequestException("down");

            await scenario.CreateController(token, null, "POST", default).Ask(Question());
            await scenario.CreateController(token, null, "POST", default).Ask(Question());

            Assert.NotEmpty(scenario.ControllerLog.Messages);
            Assert.All(scenario.ControllerLog.Messages, message => Assert.DoesNotContain(token, message));
        }

        // ── downloads and charts ────────────────────────────────────────────────────────

        private async Task<(Scenario Scenario, string UserId, string Id)> AskForADownloadAsync(int roleId = 2)
        {
            var scenario = CreateScenario();
            var userId = _fixture.NewUserId();
            scenario.Assistant.Model.CallTool("get_connections_by_school", September).Reply("Done.");
            var result = await scenario.CreateController(TokenOf(userId, roleId), null, "POST", default).Ask(Question());
            var id = Body(result).GetProperty("Downloads")[0].GetProperty("Id").GetString()!;
            return (scenario, userId, id);
        }

        [Fact]
        public async Task The_owner_downloads_the_excel_rebuilt_with_their_own_token()
        {
            var (scenario, userId, id) = await AskForADownloadAsync();

            var result = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", default).GetDownload(id);

            var file = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file.ContentType);
            Assert.Equal("connections-by-school.xlsx", file.FileDownloadName);
            var export = scenario.Assistant.DataAnalysis.Calls.Last(call => call.Kind == "export");
            Assert.Equal("xlsx", export.Format);
            Assert.Equal(TokenOf(userId, 2), export.Token);
            Assert.Equal("from=2026-09-01&to=2026-09-30", export.QueryString);
        }

        [Fact]
        public async Task The_id_of_another_person_an_unknown_id_and_an_expired_id_give_the_same_404()
        {
            var (scenario, userId, id) = await AskForADownloadAsync();
            var intruder = TokenOf(_fixture.NewUserId(), 2);

            var otherPerson = await scenario.CreateController(intruder, "en", "GET", default).GetDownload(id);
            var unknown = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", default).GetDownload("d_doesnotexist");
            var otherChart = await scenario.CreateController(intruder, "en", "GET", default).GetChart(id);
            scenario.Assistant.Clock.Advance(TimeSpan.FromHours(3));
            var expired = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", default).GetDownload(id);

            foreach (var result in new[] { otherPerson, unknown, otherChart, expired })
            {
                Assert.Equal(404, Status(result));
                Assert.Equal("download_expired", Body(result).GetProperty("ErrorCode").GetString());
                Assert.Equal("The link expired; ask again.", Body(result).GetProperty("error").GetString());
            }

            Assert.DoesNotContain(scenario.Assistant.DataAnalysis.Calls, call => call.Token == intruder);
        }

        [Fact]
        public async Task The_expanded_chart_uses_the_options_of_the_person_and_carries_the_id_of_the_download()
        {
            var (scenario, userId, id) = await AskForADownloadAsync();

            var result = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", new QueryString("?all=true&order=asc&includeZeros=true&limit=7")).GetChart(id);
            var body = Body(result);

            Assert.Equal(200, Status(result));
            Assert.Equal(id, body.GetProperty("Id").GetString());
            Assert.Equal("/aiservicesapi/data-assistant/charts/" + id, body.GetProperty("Url").GetString());
            Assert.Equal("bar", body.GetProperty("Type").GetString());
            var export = scenario.Assistant.DataAnalysis.Calls.Last(call => call.Kind == "export");
            Assert.Equal("chart", export.Format);
            Assert.Equal(TokenOf(userId, 2), export.Token);
            Assert.Equal(new[] { "all=true", "from=2026-09-01", "includeZeros=true", "limit=7", "order=asc", "to=2026-09-30" }, export.QueryString.Split('&').OrderBy(part => part));
        }

        [Fact]
        public async Task A_failure_of_the_data_api_is_passed_on_and_a_data_api_that_is_down_gives_502()
        {
            var (scenario, userId, id) = await AskForADownloadAsync();
            scenario.Assistant.DataAnalysis.OnExport = (_, _, _) => FakeDataAnalysisFeignAPI.Json(403, """{"error":"Your role cannot see the usage insights.","ErrorCode":"forbidden"}""");

            var forbidden = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", default).GetDownload(id);
            scenario.Assistant.DataAnalysis.OnExport = (_, _, _) => throw new HttpRequestException("down");
            var down = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", default).GetDownload(id);
            var downChart = await scenario.CreateController(TokenOf(userId, 2), "en", "GET", default).GetChart(id);

            Assert.Equal(403, Status(forbidden));
            Assert.Equal("forbidden", Body(forbidden).GetProperty("ErrorCode").GetString());
            Assert.Equal(502, Status(down));
            Assert.Equal("data_unavailable", Body(down).GetProperty("ErrorCode").GetString());
            Assert.Equal(502, Status(downChart));
        }

        [Fact]
        public async Task Downloads_and_charts_need_a_valid_session()
        {
            var (scenario, _, id) = await AskForADownloadAsync();

            var download = await scenario.CreateController(null, "en", "GET", default).GetDownload(id);
            var chart = await scenario.CreateController("nobody", "en", "GET", default).GetChart(id);

            Assert.Equal(401, Status(download));
            Assert.Equal(401, Status(chart));
        }
    }
}
