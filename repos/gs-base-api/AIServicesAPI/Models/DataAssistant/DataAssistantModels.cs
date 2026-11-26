using System.Text.Json.Nodes;

namespace GSAIServicesAPI.Models.DataAssistant
{
    /// <summary>
    /// Dashboard context sent by the panel: which report and page the person is looking at. It only orients the assistant;
    /// it is never a datum nor a period.
    /// </summary>
    /// <param name="Report">Name of the report.</param>
    /// <param name="Page">Name of the active page.</param>
    public record DashboardContextDto(string? Report, string? Page);

    /// <summary>
    /// A question for the data assistant.
    /// </summary>
    /// <param name="Question">The question, in the language of the person.</param>
    /// <param name="ConversationId">Id of the conversation to continue; null or unknown starts a new one.</param>
    /// <param name="Context">Dashboard context.</param>
    /// <param name="Language">Language of the panel ("es" or "en").</param>
    public record DataAssistantAskRequest(string? Question, string? ConversationId, DashboardContextDto? Context, string? Language = null);

    /// <summary>
    /// The answer of the data assistant.
    /// </summary>
    /// <param name="Answer">Text to show (without any references block).</param>
    /// <param name="ConversationId">Id of the conversation, to send back in the next question.</param>
    /// <param name="References">One per executed query, copied from the answer of the data API (never written by the model).</param>
    /// <param name="Notices">Localized notices that the data API returned (data loaded only until a date, ties, cut lists).</param>
    /// <param name="Warnings">Automatic verification: figures of the answer that do not come from the queries.</param>
    /// <param name="ExecutedQueries">Which tool ran with which arguments (transparency): {Tool, Arguments, Status}.</param>
    /// <param name="ErrorCode">Error code, or null when everything went well.</param>
    /// <param name="Downloads">One Excel file per successful query, registered by the server: {Id, FileName, Query, Url, Highlighted}.</param>
    /// <param name="Charts">Chart specifications (Recharts format) of the chartable queries, calculated from the same figures of the query.</param>
    public record DataAssistantAskResponse(string Answer, string ConversationId, List<JsonNode> References, List<string> Notices, List<string> Warnings,
        List<JsonNode> ExecutedQueries, string? ErrorCode = null, List<JsonNode>? Downloads = null, List<JsonNode>? Charts = null);

    /// <summary>
    /// Result of running a tool against the data analysis module. QueryName and QueryString are what was really asked (sanitized arguments).
    /// </summary>
    /// <param name="Status">HTTP status of the data API.</param>
    /// <param name="Body">Body returned by the data API; it is what the model sees.</param>
    /// <param name="QueryName">Route name of the query that ran.</param>
    /// <param name="QueryString">Query string that was sent.</param>
    /// <param name="Arguments">The arguments that were kept.</param>
    public record ToolExecutionResult(int Status, string Body, string? QueryName = null, string? QueryString = null, JsonObject? Arguments = null);

    /// <summary>
    /// A registered download: only the query (no data), tied to the person who asked. The file is rebuilt when they click.
    /// </summary>
    /// <param name="QueryName">Route name of the query.</param>
    /// <param name="QueryString">Query string of the export.</param>
    /// <param name="UserId">The user who asked.</param>
    /// <param name="ExpiresAtUtc">When the registration expires.</param>
    public record DownloadRegistration(string QueryName, string QueryString, string UserId, DateTime ExpiresAtUtc);

    /// <summary>
    /// The caller of the assistant, as validated by the platform session. The token is forwarded to the data API and never logged.
    /// </summary>
    /// <param name="UserId">User id of the session.</param>
    /// <param name="Token">Session token.</param>
    /// <param name="Language">Language of the request ("es" or "en").</param>
    public record DataAssistantCaller(string UserId, string Token, string Language);

    /// <summary>
    /// What the service gives back: the response and the tokens the model used in total (all its calls), for the quota.
    /// </summary>
    /// <param name="Response">The assistant response.</param>
    /// <param name="TotalTokens">Sum of the tokens reported by every model call; 0 when none was reported.</param>
    public record DataAssistantAskResult(DataAssistantAskResponse Response, long TotalTokens);

    /// <summary>
    /// The model could not be used: the service failed or refused. Content filter and access denied are told apart because they get their own answer.
    /// </summary>
    public class ModelInvocationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelInvocationException"/> class.
        /// </summary>
        /// <param name="statusCode">HTTP status of the model service, or 0 when there was none.</param>
        /// <param name="isContentFiltered">True when the content filter of Azure blocked the message.</param>
        /// <param name="innerException">The original failure.</param>
        public ModelInvocationException(int statusCode, bool isContentFiltered = false, Exception? innerException = null)
            : base($"The model service answered {statusCode}.", innerException)
        {
            StatusCode = statusCode;
            IsContentFiltered = isContentFiltered;
        }

        /// <summary>
        /// Gets the HTTP status of the model service.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Gets a value indicating whether the content filter blocked the message.
        /// </summary>
        public bool IsContentFiltered { get; }

        /// <summary>
        /// Gets a value indicating whether the model service refused the credentials.
        /// </summary>
        public bool IsAccessDenied => StatusCode is 401 or 403;
    }
}
