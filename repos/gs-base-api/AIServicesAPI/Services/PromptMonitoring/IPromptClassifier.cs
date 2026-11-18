namespace GSAIServicesAPI.Services.PromptMonitoring
{
    /// <summary>
    /// Runs the student-prompt safety classification as a standalone model call, for the flow that
    /// cannot ride it on the answer completion: the RAG tutor, whose answer comes from an external
    /// orchestrator we do not own. The generative flows classify inline instead and never need this.
    /// </summary>
    public interface IPromptClassifier
    {
        /// <summary>
        /// Classifies a single student prompt and returns the raw <c>classify_prompt</c> arguments as
        /// JSON, ready to hand to the capture path. Returns <c>null</c> when the prompt is empty, the
        /// model does not call the tool, or anything fails: classification is best-effort and never
        /// throws, so it can never block or break the answer the student already received.
        /// </summary>
        Task<string?> ClassifyAsync(string? prompt, CancellationToken cancellationToken = default);
    }
}
