namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// What an engine hands back to the controller once a tutor turn finishes streaming, so the
    /// exchange can be audited. It is a copy of what was already produced during the stream, so
    /// building it costs no extra streamed tokens.
    /// </summary>
    /// <param name="Answer">
    /// The full answer that was streamed to the student, accumulated token by token.
    /// </param>
    /// <param name="ClassificationJson">
    /// The raw <c>classify_prompt</c> tool-call arguments when the engine classified the prompt with
    /// its own standalone call, or <c>null</c> when it left that to the capture step. Neither engine
    /// classifies inline on the streamed completion.
    /// </param>
    public sealed record TutorCaptureData(string Answer, string? ClassificationJson);
}
