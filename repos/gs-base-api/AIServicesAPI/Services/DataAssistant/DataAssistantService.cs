using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Services.Quotas;

namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// The data assistant: it receives a question, sends it to the model with the usage insights queries as tools, and runs the queries the model
    /// asks for against the data analysis module. The database is never exposed: the model only sees the aggregated JSON that the queries return,
    /// and every query goes with the TOKEN of the person, so the data analysis module validates the session again and applies the data scope
    /// (all schools, ORE or school) of that person. The scope is not an argument of any tool, so the model cannot widen it.
    /// </summary>
    public class DataAssistantService
    {
        /// <summary>
        /// Maximum number of model calls per question.
        /// </summary>
        public const int MaxSteps = 6;

        /// <summary>
        /// Maximum number of messages kept per conversation (the oldest turns are dropped first).
        /// </summary>
        public const int MaxHistoryMessages = 40;

        /// <summary>
        /// Maximum length of a question.
        /// </summary>
        public const int MaxQuestionLength = 1000;

        private const string DataAssistantSubject = "data-assistant";
        private static readonly string[] ChartParameterNames = { "format", "limit", "all", "includeZeros" };
        private static readonly string[] ExpandedChartParameterNames = { "all", "order", "includeZeros", "limit" };
        private static readonly Regex DefaultExcelRequestPattern = new(@"excel|xlsx|download|spreadsheet", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex DefaultChartRequestPattern = new(@"chart|graph|plot", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IDataAssistantChatClient _chatClient;
        private readonly IDataAnalysisFeignAPI _dataAnalysis;
        private readonly DataAssistantToolCatalog _catalog;
        private readonly IDataAssistantConversationStore _conversations;
        private readonly IDataAssistantDownloadRegistry _downloads;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataAssistantService> _logger;
        private readonly TimeProvider _timeProvider;
        private readonly Regex _excelRequestPattern;
        private readonly Regex _chartRequestPattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantService"/> class.
        /// </summary>
        /// <param name="chatClient">The model.</param>
        /// <param name="dataAnalysis">The data analysis module.</param>
        /// <param name="catalog">The tool catalog.</param>
        /// <param name="conversations">The conversation store.</param>
        /// <param name="downloads">The download registry.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="timeProvider">The clock.</param>
        public DataAssistantService(IDataAssistantChatClient chatClient, IDataAnalysisFeignAPI dataAnalysis, DataAssistantToolCatalog catalog,
            IDataAssistantConversationStore conversations, IDataAssistantDownloadRegistry downloads, IConfiguration configuration,
            ILogger<DataAssistantService> logger, TimeProvider? timeProvider = null)
        {
            _chatClient = chatClient;
            _dataAnalysis = dataAnalysis;
            _catalog = catalog;
            _conversations = conversations;
            _downloads = downloads;
            _configuration = configuration;
            _logger = logger;
            _timeProvider = timeProvider ?? TimeProvider.System;
            _excelRequestPattern = BuildPattern(configuration["DataAssistant:ExcelRequestPattern"], DefaultExcelRequestPattern);
            _chartRequestPattern = BuildPattern(configuration["DataAssistant:ChartRequestPattern"], DefaultChartRequestPattern);
        }

        /// <summary>
        /// Subject of the prompt monitoring records of the assistant.
        /// </summary>
        public static string MonitoringSubject => DataAssistantSubject;

        private static Regex BuildPattern(string? configured, Regex fallback) =>
            string.IsNullOrWhiteSpace(configured) ? fallback : new Regex(configured, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));

        // ── The question ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Answers a question.
        /// </summary>
        /// <param name="request">The question.</param>
        /// <param name="caller">The person who asks, as validated by the session.</param>
        /// <param name="cancellationToken">Token used to cancel the work.</param>
        /// <returns>The answer and the tokens used.</returns>
        /// <exception cref="ModelInvocationException">The model service failed, refused or filtered the message.</exception>
        public async Task<DataAssistantAskResult> AskAsync(DataAssistantAskRequest request, DataAssistantCaller caller, CancellationToken cancellationToken)
        {
            var question = TrimQuestion(request.Question);

            // A conversation belongs to who started it: with the id of somebody else a new one starts (same result as an unknown id).
            var existing = string.IsNullOrEmpty(request.ConversationId) ? null : _conversations.Get(request.ConversationId, caller.UserId);
            var conversationId = existing is not null ? request.ConversationId! : "c_" + Guid.NewGuid().ToString("N");
            var history = existing?.Messages ?? new List<DataAssistantChatMessage>();

            var contextText = BuildContextText(request.Context, request.Language);
            if (existing is not null)
            {
                // In a conversation that goes on, every message is a NEW question: the history is only context (rule 5b), not pending requests.
                contextText += (contextText.Length > 0 ? "\n" : string.Empty) + "[New message: answer ONLY this question; do not take up requests or clarifications of previous messages]";
            }

            var today = GetToday();
            var conversation = new List<DataAssistantChatMessage>(history) { new(DataAssistantChatRole.User, contextText + "\n" + question) };
            var messages = new List<DataAssistantChatMessage> { new(DataAssistantChatRole.System, BuildInstructions(today)) };
            messages.AddRange(conversation);

            var references = new List<JsonNode>();
            var executedQueries = new List<JsonNode>();
            var downloads = new List<JsonNode>();
            var charts = new List<JsonNode>();
            var notices = new List<string>();
            var toolOutputs = new List<string>();
            var toolArguments = new List<string>();
            var seenQueries = new HashSet<string>();
            long totalTokens = 0;
            string answer = string.Empty;

            for (var step = 0; step < MaxSteps; step++)
            {
                var reply = await _chatClient.CompleteAsync(messages, cancellationToken);
                totalTokens += reply.TotalTokens;

                if (reply.ToolCalls.Count == 0)
                {
                    answer = (reply.Content ?? string.Empty).Trim();
                    var finalMessage = new DataAssistantChatMessage(DataAssistantChatRole.Assistant, answer);
                    messages.Add(finalMessage);
                    conversation.Add(finalMessage);
                    break;
                }

                var toolCallMessage = new DataAssistantChatMessage(DataAssistantChatRole.Assistant, reply.Content, reply.ToolCalls);
                messages.Add(toolCallMessage);
                conversation.Add(toolCallMessage);

                foreach (var call in reply.ToolCalls)
                {
                    var result = await ExecuteToolAsync(caller, call.Name, call.ArgumentsJson, cancellationToken);
                    toolOutputs.Add(result.Body);
                    toolArguments.Add(call.ArgumentsJson);
                    executedQueries.Add(new JsonObject { ["Tool"] = call.Name, ["Arguments"] = result.Arguments?.DeepClone() ?? new JsonObject(), ["Status"] = result.Status });
                    if (result.Status == 200 && JsonNode.Parse(result.Body) is JsonObject body)
                    {
                        references.Add(BuildReference(body));
                        foreach (var notice in ReadNotices(body).Where(notice => !notices.Contains(notice)))
                        {
                            notices.Add(notice);
                        }

                        if (result.QueryName is not null && result.QueryString is not null && result.QueryName != "search-schools"
                            && seenQueries.Add(result.QueryName + "?" + result.QueryString))
                        {
                            await AttachDownloadAndChartAsync(result, request.Question ?? string.Empty, caller, downloads, charts, cancellationToken);
                        }
                    }

                    var toolMessage = new DataAssistantChatMessage(DataAssistantChatRole.Tool, result.Body, ToolCallId: call.Id);
                    messages.Add(toolMessage);
                    conversation.Add(toolMessage);
                }
            }

            if (string.IsNullOrWhiteSpace(answer))
            {
                answer = Localize("DataAssistantCouldNotComplete", caller.Language);
                conversation.Add(new DataAssistantChatMessage(DataAssistantChatRole.Assistant, answer));
            }

            var figures = AnswerFigureVerifier.Accumulate(existing?.Figures, question, toolOutputs, toolArguments);
            _conversations.Save(conversationId, new DataAssistantConversation(caller.UserId, TrimHistory(conversation), figures));

            // An answer without queries only explains the dashboard: the figures of the knowledge base ("100 %") are known. With queries they are not, so a number that came from nowhere is still reported.
            var knownContext = toolOutputs.Count == 0 ? contextText + "\n" + _catalog.KnowledgeBase : contextText;
            var unexplained = AnswerFigureVerifier.Verify(question, knownContext, answer, toolOutputs, toolArguments, today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), existing?.Figures);
            var warnings = new List<string>();
            if (unexplained.Count > 0)
            {
                warnings.Add(Localize(toolOutputs.Count == 0 ? "DataAssistantWarningFiguresWithoutQuery" : "DataAssistantWarningUnverifiedFigures", caller.Language, string.Join(", ", unexplained)));
            }

            var response = new DataAssistantAskResponse(answer, conversationId, references, notices, warnings, executedQueries,
                Downloads: downloads.Count > 0 ? downloads : null, Charts: charts.Count > 0 ? charts : null);
            return new DataAssistantAskResult(response, totalTokens);
        }

        /// <summary>
        /// Estimates the tokens of a question to reserve them in the quota: instructions, tools, history and question, plus the assumed output of every
        /// possible step (a question can make up to six model calls). The real consumption is settled afterwards.
        /// </summary>
        /// <param name="request">The question.</param>
        /// <param name="userId">The user who asks.</param>
        /// <returns>The estimated tokens.</returns>
        public long EstimateTokens(DataAssistantAskRequest request, string userId)
        {
            var existing = string.IsNullOrEmpty(request.ConversationId) ? null : _conversations.Get(request.ConversationId, userId);
            var texts = new List<string?> { _catalog.InstructionsTemplate, TrimQuestion(request.Question) };
            texts.AddRange(_catalog.Tools.Select(tool => tool.Description + tool.ParametersSchema));
            texts.AddRange((existing?.Messages ?? Array.Empty<DataAssistantChatMessage>()).Select(message => message.Content));
            return TokenEstimator.FromTexts(texts) + (TokenEstimator.AssumedOutputTokens * MaxSteps);
        }

        // ── Tools ───────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Runs one tool against the data analysis module, forwarding the token of the person: the module validates the session again and applies their
        /// scope, so the model cannot get out of the scope of the person. Arguments that the tool does not declare are dropped, so the model cannot change
        /// the result without anybody noticing.
        /// </summary>
        /// <param name="caller">The person who asks.</param>
        /// <param name="toolName">The tool the model asked for.</param>
        /// <param name="argumentsJson">The arguments as the model wrote them.</param>
        /// <param name="cancellationToken">Token used to cancel the call.</param>
        /// <returns>The result, with the arguments that were really used.</returns>
        public async Task<ToolExecutionResult> ExecuteToolAsync(DataAssistantCaller caller, string toolName, string argumentsJson, CancellationToken cancellationToken)
        {
            if (!_catalog.TryGetTool(toolName, out var tool))
            {
                return new ToolExecutionResult(400, JsonSerializer.Serialize(new { ErrorCode = "unknown_query", error = $"The tool '{toolName}' does not exist." }));
            }

            var arguments = ParseArguments(argumentsJson);
            foreach (var undeclared in arguments.Select(argument => argument.Key).Where(key => !tool.ParameterNames.Contains(key)).ToList())
            {
                arguments.Remove(undeclared);
            }

            var queryString = BuildQueryString(arguments);
            try
            {
                var response = await _dataAnalysis.GetUsageInsightAsync(tool.QueryName, queryString, caller.Token, caller.Language, cancellationToken);
                return new ToolExecutionResult(response.StatusCode, response.GetText(), tool.QueryName, queryString, arguments);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or InvalidOperationException)
            {
                // The model gets an error it can report; the token and the arguments are not logged.
                _logger.LogWarning("The data analysis module did not answer the tool {Tool}: {ExceptionType}", toolName, exception.GetType().Name);
                return new ToolExecutionResult(502, JsonSerializer.Serialize(new { ErrorCode = "data_unavailable", error = "The data could not be queried right now." }),
                    tool.QueryName, queryString, arguments);
            }
        }

        private static JsonObject ParseArguments(string argumentsJson)
        {
            try
            {
                return JsonNode.Parse(string.IsNullOrWhiteSpace(argumentsJson) ? "{}" : argumentsJson) as JsonObject ?? new JsonObject();
            }
            catch (JsonException)
            {
                return new JsonObject();
            }
        }

        private static string BuildQueryString(JsonObject arguments)
        {
            static string ValueOf(JsonNode? node) => node switch
            {
                JsonValue value when value.TryGetValue<string>(out var text) => text,
                null => string.Empty,
                _ => node.ToJsonString().Trim('"'),
            };

            return string.Join("&", arguments.Where(argument => argument.Value is not null)
                .Select(argument => $"{Uri.EscapeDataString(argument.Key)}={Uri.EscapeDataString(ValueOf(argument.Value))}"));
        }

        // ── Downloads and charts ────────────────────────────────────────────────────────

        private async Task AttachDownloadAndChartAsync(ToolExecutionResult result, string question, DataAssistantCaller caller, List<JsonNode> downloads, List<JsonNode> charts,
            CancellationToken cancellationToken)
        {
            // The server builds the download and the chart with the same figures of the query (the model does not write links nor figures).
            var queryName = result.QueryName!;
            var id = _downloads.Register(queryName, BuildExcelQuery(result.QueryString!), caller.UserId);
            downloads.Add(new JsonObject
            {
                ["Id"] = id,
                ["FileName"] = BuildFileName(queryName, result.Body),
                ["Query"] = queryName,
                ["Url"] = "/aiservicesapi/data-assistant/downloads/" + id,
                ["Highlighted"] = _excelRequestPattern.IsMatch(question),
            });

            // The chart comes from the export of the query (all the rows: sorted, without zeros, a preview of 10 and with its total),
            // not from the rows of the chat. With the id of the download the panel asks for the expanded chart later.
            // A chart that cannot be built never takes down the answer: the data and the download are already there, so the answer goes without that chart.
            try
            {
                var chart = await _dataAnalysis.ExportUsageInsightAsync(queryName, "chart", BuildChartQuery(result.QueryString!), caller.Token, caller.Language, cancellationToken);
                if (chart.IsSuccess && JsonNode.Parse(chart.GetText()) is JsonObject specification)
                {
                    specification["Id"] = id;
                    specification["Url"] = "/aiservicesapi/data-assistant/charts/" + id;
                    specification["Highlighted"] = _chartRequestPattern.IsMatch(question);
                    charts.Add(specification);
                }
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or InvalidOperationException or JsonException)
            {
                // Only the type is logged: no token, no arguments, no data.
                _logger.LogWarning("The chart of the query {Query} could not be built: {ExceptionType}", queryName, exception.GetType().Name);
            }
        }

        private static string BuildFileName(string queryName, string body)
        {
            // Same rule as the export of the data module: query_from_to.xlsx, only [a-z0-9_-].
            var parts = new List<string> { queryName };
            if (JsonNode.Parse(body)?["Period"] is JsonObject period)
            {
                if (period["A"] is JsonObject a && period["B"] is JsonObject b)
                {
                    parts.AddRange(new[] { "a", Text(a["From"]), Text(a["To"]), "b", Text(b["From"]), Text(b["To"]) });
                }
                else
                {
                    parts.AddRange(new[] { "From", "To", "Date", "PeriodFrom" }.Select(key => Text(period[key])));
                }
            }

            var name = string.Concat(string.Join('_', parts.Where(part => part.Length > 0)).ToLowerInvariant().Select(character => character is (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_' or '-' ? character : '_'));
            return name + ".xlsx";
        }

        private static string Text(JsonNode? node) => node is JsonValue value && value.TryGetValue<string>(out var text) ? text : string.Empty;

        /// <summary>
        /// Builds the query string of the Excel export of a chat query: the same filters WITHOUT limit (the file has the whole list, up to 5000 rows).
        /// </summary>
        /// <param name="queryString">The query string of the chat query.</param>
        /// <returns>The query string of the export (the format is added by the Feign client).</returns>
        public static string BuildExcelQuery(string queryString) =>
            string.Join("&", SplitQuery(queryString).Where(part => !part.StartsWith("limit=", StringComparison.Ordinal) && !part.StartsWith("format=", StringComparison.Ordinal)));

        /// <summary>
        /// Builds the query string of the chart of a chat query: the same filters, without limit (the chat query has few rows) and without chart parameters.
        /// A limit from 1 to 10 asked by the model is kept as the number of categories of the preview.
        /// </summary>
        /// <param name="queryString">The query string of the chat query.</param>
        /// <returns>The query string of the chart export (the format is added by the Feign client).</returns>
        public static string BuildChartQuery(string queryString)
        {
            var parts = SplitQuery(queryString).ToList();
            var limit = parts.FirstOrDefault(part => part.StartsWith("limit=", StringComparison.Ordinal));
            var kept = parts.Where(part => !ChartParameterNames.Any(name => part.StartsWith(name + "=", StringComparison.Ordinal))).ToList();
            if (limit is not null && int.TryParse(limit[6..], NumberStyles.None, CultureInfo.InvariantCulture, out var preview) && preview is >= 1 and <= 10)
            {
                kept.Add("limit=" + preview);
            }

            return string.Join("&", kept);
        }

        /// <summary>
        /// Builds the query string of the expanded chart of a registered download, with the options of the person (all, order, includeZeros, limit)
        /// over the ones of the chat.
        /// </summary>
        /// <param name="downloadQueryString">The query string registered with the download.</param>
        /// <param name="requested">The chart options requested by the person.</param>
        /// <returns>The query string of the chart export (the format is added by the Feign client).</returns>
        public static string BuildExpandedChartQuery(string downloadQueryString, IQueryCollection requested)
        {
            var replaced = ExpandedChartParameterNames.Where(requested.ContainsKey).ToHashSet();
            var parts = SplitQuery(downloadQueryString)
                .Where(part => !ChartParameterNames.Any(name => part.StartsWith(name + "=", StringComparison.Ordinal))
                    && !replaced.Any(name => part.StartsWith(name + "=", StringComparison.Ordinal)))
                .ToList();
            parts.AddRange(replaced.Select(name => $"{name}={Uri.EscapeDataString(requested[name].ToString())}"));
            return string.Join("&", parts);
        }

        private static IEnumerable<string> SplitQuery(string queryString) => queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);

        // ── Pieces of the answer ────────────────────────────────────────────────────────

        private static JsonObject BuildReference(JsonObject body)
        {
            // The reference is what the data API returned (query, source, date column, period, filters, scope, data until, rows, generation time) plus
            // what the panel needs to flag a cut list or a tie: HasMoreRows, Note and Summary (which carries Ties). The result rows are not copied.
            var reference = new JsonObject();
            foreach (var key in new[] { "Query", "Source", "DateColumn", "Period", "Filters", "Scope", "DataUntil", "TotalRows", "GeneratedAt", "HasMoreRows", "Note", "Summary" })
            {
                if (body[key] is { } value)
                {
                    reference[key] = value.DeepClone();
                }
            }

            return reference;
        }

        private static IEnumerable<string> ReadNotices(JsonObject body) =>
            body["Notices"] is JsonArray notices
                ? notices.Select(notice => notice is JsonValue value && value.TryGetValue<string>(out var text) ? text : string.Empty).Where(text => text.Length > 0)
                : Enumerable.Empty<string>();

        private static string TrimQuestion(string? question)
        {
            var text = (question ?? string.Empty).Trim();
            return text.Length > MaxQuestionLength ? text[..MaxQuestionLength] : text;
        }

        private static string Shorten(string? text)
        {
            var clean = (text ?? string.Empty).Replace('\n', ' ').Replace('«', '"').Replace('»', '"').Trim();
            return clean.Length > 80 ? clean[..80] : clean;
        }

        private static string BuildContextText(DashboardContextDto? context, string? language)
        {
            var parts = new List<string>();
            if (context is not null && !(string.IsNullOrWhiteSpace(context.Report) && string.IsNullOrWhiteSpace(context.Page)))
            {
                parts.Add($"[Dashboard context (orientation only, not a datum nor a period): report = \"{Shorten(context.Report)}\", page = \"{Shorten(context.Page)}\"]");
            }

            // Language of the PANEL (the one of the platform): it only breaks the tie when the question does not make its own language clear (rule 5a). It always goes.
            var isEnglish = language is not null && language.StartsWith("en", StringComparison.OrdinalIgnoreCase);
            parts.Add($"[Panel language: {(isEnglish ? "English" : "Spanish")}]");
            return string.Join("\n", parts);
        }

        // ── Dates and instructions ──────────────────────────────────────────────────────

        private DateOnly GetToday()
        {
            // The day is read in the local time of the usage insights (Puerto Rico, UTC-4, by default).
            var offset = int.TryParse(_configuration["AppSettings:usage_insights_utc_offset_minutes"], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var minutes) ? minutes : -240;
            return DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime.AddMinutes(offset));
        }

        /// <summary>
        /// Gets the default academic year (ASSUMPTION documented in the instructions: August to July). "The last academic year" is the most recent COMPLETE
        /// academic year; "this academic year" goes from the August 1 in force until today.
        /// </summary>
        /// <param name="today">Today.</param>
        /// <returns>The start and end of the last academic year and the start of the current one.</returns>
        public static (DateOnly From, DateOnly To, DateOnly CurrentFrom) GetAcademicYear(DateOnly today)
        {
            var startYear = today.Month >= 8 ? today.Year : today.Year - 1;   // the academic year in course started on August 1 of this year
            return (new DateOnly(startYear - 1, 8, 1), new DateOnly(startYear, 7, 31), new DateOnly(startYear, 8, 1));
        }

        private string BuildInstructions(DateOnly today)
        {
            // The relative dates are calculated by the code (the model gets "last 3 complete months" wrong).
            string Format(DateOnly date) => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var monthStart = new DateOnly(today.Year, today.Month, 1);
            var academicYear = GetAcademicYear(today);
            var instructions = _catalog.InstructionsTemplate
                .Replace("{today}", Format(today))
                .Replace("{quarter_from}", Format(monthStart.AddMonths(-3)))
                .Replace("{quarter_to}", Format(monthStart.AddDays(-1)))
                .Replace("{last_month_from}", Format(monthStart.AddMonths(-1)))
                .Replace("{last_month_to}", Format(monthStart.AddDays(-1)))
                .Replace("{year_start}", Format(new DateOnly(today.Year, 1, 1)))
                .Replace("{academic_year_from}", Format(academicYear.From))
                .Replace("{academic_year_to}", Format(academicYear.To))
                .Replace("{current_academic_year_from}", Format(academicYear.CurrentFrom));

            // The knowledge base goes after the placeholders are filled: it is plain text and must not be touched by them.
            return _catalog.KnowledgeBase.Length == 0
                ? instructions
                : instructions + "\n\nKNOWLEDGE BASE (explains the dashboard; it holds no data, so never take figures from it):\n" + _catalog.KnowledgeBase;
        }

        private static IReadOnlyList<DataAssistantChatMessage> TrimHistory(List<DataAssistantChatMessage> conversation)
        {
            // The oldest messages go first, but a conversation must start with a message of the person (a tool message alone would be invalid).
            var start = Math.Max(0, conversation.Count - MaxHistoryMessages);
            while (start < conversation.Count && conversation[start].Role != DataAssistantChatRole.User)
            {
                start++;
            }

            return conversation.Skip(start).ToList();
        }

        private string Localize(string key, string language, params object[] arguments)
        {
            var code = string.Equals(language, "es", StringComparison.OrdinalIgnoreCase) ? "es" : "en";
            var template = _configuration[$"Messages:{key}:{code}"] ?? _configuration[$"Messages:{key}:en"] ?? key;
            return arguments.Length == 0 ? template : string.Format(CultureInfo.InvariantCulture, template, arguments);
        }
    }
}
