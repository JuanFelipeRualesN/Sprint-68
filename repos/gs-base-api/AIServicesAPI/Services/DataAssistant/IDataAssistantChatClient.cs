namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// Role of a message of the conversation with the model.
    /// </summary>
    public enum DataAssistantChatRole
    {
        /// <summary>The instructions.</summary>
        System,

        /// <summary>A message of the person.</summary>
        User,

        /// <summary>A message of the model (text, tool calls or both).</summary>
        Assistant,

        /// <summary>The result of a tool call.</summary>
        Tool,
    }

    /// <summary>
    /// A tool call that the model asked for.
    /// </summary>
    /// <param name="Id">Id of the call, to answer it.</param>
    /// <param name="Name">Name of the tool.</param>
    /// <param name="ArgumentsJson">The arguments as the model wrote them (JSON text).</param>
    public sealed record DataAssistantToolCall(string Id, string Name, string ArgumentsJson);

    /// <summary>
    /// A message of the conversation with the model, independent of the model SDK.
    /// </summary>
    /// <param name="Role">Who wrote it.</param>
    /// <param name="Content">The text.</param>
    /// <param name="ToolCalls">The tool calls of an assistant message.</param>
    /// <param name="ToolCallId">The call a tool message answers.</param>
    public sealed record DataAssistantChatMessage(DataAssistantChatRole Role, string? Content, IReadOnlyList<DataAssistantToolCall>? ToolCalls = null, string? ToolCallId = null);

    /// <summary>
    /// One answer of the model.
    /// </summary>
    /// <param name="Content">The text, empty when the model only asked for tools.</param>
    /// <param name="ToolCalls">The tools the model asked for.</param>
    /// <param name="TotalTokens">Tokens used by this call (prompt plus answer); 0 when the service reported none.</param>
    public sealed record DataAssistantModelReply(string? Content, IReadOnlyList<DataAssistantToolCall> ToolCalls, long TotalTokens);

    /// <summary>
    /// Talks to the model with the tools of the data assistant. It is an interface so the tests can play the model.
    /// </summary>
    public interface IDataAssistantChatClient
    {
        /// <summary>
        /// Sends the conversation to the model and returns its answer.
        /// </summary>
        /// <param name="messages">The conversation, system message first.</param>
        /// <param name="cancellationToken">Token used to cancel the call.</param>
        /// <returns>The answer of the model.</returns>
        /// <exception cref="Models.DataAssistant.ModelInvocationException">The model service failed, refused or filtered the message.</exception>
        Task<DataAssistantModelReply> CompleteAsync(IReadOnlyList<DataAssistantChatMessage> messages, CancellationToken cancellationToken);
    }
}
