using System.Collections.Concurrent;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Models.PromptMonitoring.Request;
using GSAIServicesAPI.Models.PromptMonitoring.Response;
using GSAIServicesAPI.Services.DataAssistant;
using GSAIServicesAPI.Services.PromptMonitoring;

namespace AIServicesAPI.Tests.Infrastructure
{
    /// <summary>
    /// Plays the model: every call takes the next scripted answer and records the conversation it received.
    /// </summary>
    internal sealed class FakeDataAssistantChatClient : IDataAssistantChatClient
    {
        private readonly Queue<Func<IReadOnlyList<DataAssistantChatMessage>, DataAssistantModelReply>> _script = new();

        /// <summary>
        /// Gets the conversations received, one per call (copies, so later changes do not alter them).
        /// </summary>
        public List<IReadOnlyList<DataAssistantChatMessage>> Calls { get; } = new();

        /// <summary>
        /// Gets or sets the answer used when the script is empty.
        /// </summary>
        public Func<IReadOnlyList<DataAssistantChatMessage>, DataAssistantModelReply>? Fallback { get; set; }

        public FakeDataAssistantChatClient Reply(string text, long tokens = 100) => Enqueue(_ => new DataAssistantModelReply(text, Array.Empty<DataAssistantToolCall>(), tokens));

        public FakeDataAssistantChatClient CallTool(string name, string argumentsJson, long tokens = 100, string id = "call-1") =>
            Enqueue(_ => new DataAssistantModelReply(null, new[] { new DataAssistantToolCall(id, name, argumentsJson) }, tokens));

        public FakeDataAssistantChatClient Throw(Exception exception) => Enqueue(_ => throw exception);

        public FakeDataAssistantChatClient Enqueue(Func<IReadOnlyList<DataAssistantChatMessage>, DataAssistantModelReply> step)
        {
            _script.Enqueue(step);
            return this;
        }

        public Task<DataAssistantModelReply> CompleteAsync(IReadOnlyList<DataAssistantChatMessage> messages, CancellationToken cancellationToken)
        {
            Calls.Add(messages.ToList());
            var step = _script.Count > 0 ? _script.Dequeue() : Fallback ?? throw new InvalidOperationException("The fake model has no more scripted answers.");
            return Task.FromResult(step(messages));
        }
    }

    /// <summary>
    /// A call received by the fake data analysis module. The token and the language are the ones forwarded by the assistant.
    /// </summary>
    internal sealed record DataAnalysisCall(string Kind, string QueryName, string Format, string QueryString, string Token, string Language);

    /// <summary>
    /// Plays the data analysis module: it answers canned JSON and records what it was asked, so the tests can check that the token is forwarded.
    /// </summary>
    internal sealed class FakeDataAnalysisFeignAPI : IDataAnalysisFeignAPI
    {
        public ConcurrentQueue<DataAnalysisCall> Calls { get; } = new();

        /// <summary>
        /// Gets or sets the answer to a query; the default is a small connections-by-school answer.
        /// </summary>
        public Func<string, string, DataAnalysisApiResponse> OnQuery { get; set; } = (queryName, token) => Json(200, ConnectionsBySchool(queryName));

        /// <summary>
        /// Gets or sets the answer to an export; the default is a chart for format chart and a small xlsx for format xlsx.
        /// </summary>
        public Func<string, string, string, DataAnalysisApiResponse> OnExport { get; set; } = (queryName, format, token) =>
            format == "chart"
                ? Json(200, $$"""{"Query":"{{queryName}}","Type":"bar","Title":"Connections by school","Series":[{"Key":"Connections","Name":"Connections"}],"Data":[{"Category":"North High","Connections":8},{"Category":"South High","Connections":6}],"TotalCategories":2,"ShownCategories":2}""")
                : new DataAnalysisApiResponse(200, new byte[] { 0x50, 0x4B, 3, 4, 1, 2, 3 }, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{queryName}.xlsx");

        public static DataAnalysisApiResponse Json(int status, string body) => new(status, System.Text.Encoding.UTF8.GetBytes(body), "application/json", null);

        public static string ConnectionsBySchool(string queryName) => $$"""
            {"Query":"{{queryName}}","Source":"mysql:gs_data_analysis","DateColumn":"connected_date.connected_date; day in Puerto Rico time (UTC-4)",
             "Period":{"From":"2026-09-01","To":"2026-09-30"},"Filters":{"Order":"desc","Limit":10,"School":"all","TestAccounts":"excluded"},"DataUntil":"2026-10-05",
             "Results":[{"Name":"North High","Connections":8,"ConnectedUsers":4,"PercentageOfTotal":47.1},{"Name":"South High","Connections":6,"ConnectedUsers":2,"PercentageOfTotal":35.3}],
             "Note":"A connection = a user with a login on a day.","Summary":{"TotalConnections":17,"SchoolsConsidered":4},
             "Notices":["The loaded data only goes until 2026-10-05; the requested period ends on 2026-10-31, so later days are not included."],
             "HasMoreRows":false,"TotalRows":2,"Scope":"all schools","GeneratedAt":"2026-11-23T15:00:00Z"}
            """;

        public Task<DataAnalysisApiResponse> GetUsageInsightAsync(string queryName, string queryString, string token, string language, CancellationToken cancellationToken = default)
        {
            Calls.Enqueue(new DataAnalysisCall("query", queryName, string.Empty, queryString, token, language));
            return Task.FromResult(OnQuery(queryName, token));
        }

        public Task<DataAnalysisApiResponse> ExportUsageInsightAsync(string queryName, string format, string queryString, string token, string language,
            CancellationToken cancellationToken = default)
        {
            Calls.Enqueue(new DataAnalysisCall("export", queryName, format, queryString, token, language));
            return Task.FromResult(OnExport(queryName, format, token));
        }
    }

    /// <summary>
    /// Records the prompt monitoring captures.
    /// </summary>
    internal sealed class FakePromptMonitoringService : IPromptMonitoringService
    {
        public List<PromptMonitoringCaptureRequestModel> Captures { get; } = new();

        /// <summary>
        /// Gets or sets an exception to throw instead of recording (a monitoring store that is down).
        /// </summary>
        public Exception? Failure { get; set; }

        public Task CaptureAsync(PromptMonitoringCaptureRequestModel capture, CancellationToken cancellationToken = default)
        {
            if (Failure is not null)
            {
                throw Failure;
            }

            Captures.Add(capture);
            return Task.CompletedTask;
        }

        public Task CaptureGenerateExchangeAsync(string userId, string? role, GenerateRequest request, bool isTutorFlow, string answer, string? ipAddress,
            string? classificationJson) => throw new NotSupportedException();

        public Task<PromptMonitoringUserHistoryResponseModel> GetHistoryAsync(string viewerId, string userId, DateTime? from, DateTime? to, int page, int pageSize,
            CancellationToken cancellationToken = default) => throw new NotSupportedException();

        public Task<IReadOnlyList<PromptMonitoringUserSummaryResponseModel>> GetUsersSummaryAsync(string viewerId, IReadOnlyCollection<string> userIds, DateTime? from,
            DateTime? to, CancellationToken cancellationToken = default) => throw new NotSupportedException();

        public Task<long> CountUnreadAlertsForViewerAsync(string viewerId, string? viewerRole, string viewerToken, DateTime? from, DateTime? to,
            CancellationToken cancellationToken = default) => throw new NotSupportedException();
    }

    /// <summary>
    /// A clock that only moves when the test says so.
    /// </summary>
    internal sealed class ManualTimeProvider : TimeProvider
    {
        private DateTimeOffset _now;

        public ManualTimeProvider(DateTimeOffset now) => _now = now;

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan time) => _now += time;
    }
}
