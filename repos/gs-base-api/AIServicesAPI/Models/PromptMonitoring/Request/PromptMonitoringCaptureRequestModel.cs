namespace GSAIServicesAPI.Models.PromptMonitoring.Request
{
    /// <summary>
    /// What the API knows about one prompt when it is captured. Shared by the student tutor and the
    /// teacher assistant, so adding a flow means filling this model and nothing else.
    /// </summary>
    public class PromptMonitoringCaptureRequestModel
    {
        /// <summary>Who sent the prompt. Taken from the validated session, never from the payload.</summary>
        public string UserId { get; set; } = "";

        /// <summary>Role from the session. Decides classification: only a student's prompt is classified.</summary>
        public string? Role { get; set; }

        /// <summary>The verbatim prompt text.</summary>
        public string Prompt { get; set; } = "";

        /// <summary>The AI answer, accumulated as it was streamed to the client.</summary>
        public string? Response { get; set; }

        /// <summary>Lesson subject being tutored. Null outside the tutor flow.</summary>
        public string? Subject { get; set; }

        /// <summary>
        /// The classification the AI returned, still as raw JSON, or null when there was none.
        /// Unparsed so the caller stays free of classification logic.
        /// </summary>
        public string? ClassificationJson { get; set; }

        /// <summary>Origin IP, kept for traceability.</summary>
        public string? IpAddress { get; set; }
    }
}
