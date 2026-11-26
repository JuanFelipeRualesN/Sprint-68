namespace GSAIServicesAPI.Models.PromptMonitoring.Response
{
    /// <summary>
    /// One stored prompt as the client sees it. Exposes only what a teacher or administrator needs to
    /// read: the storage fields of <see cref="PromptLog"/> that exist to let the collection evolve
    /// (schema version, unmapped extra elements) stay inside the API.
    /// </summary>
    public class PromptMonitoringPromptResponseModel
    {
        /// <summary>Identifier of the stored record.</summary>
        public string? Id { get; set; }

        /// <summary>The prompt the user wrote.</summary>
        public string Prompt { get; set; } = "";

        /// <summary>The AI answer, when one was captured.</summary>
        public string? Response { get; set; }

        /// <summary>Role the user acted as when they sent the prompt.</summary>
        public string? Role { get; set; }

        /// <summary>Whether the prompt was flagged by the classification.</summary>
        public bool IsFlagged { get; set; }

        /// <summary>Why it was flagged. Empty when it was not.</summary>
        public List<string> AlertCategories { get; set; } = [];

        /// <summary>Short reason the prompt was flagged, shown to the teacher. Null when it was not.</summary>
        public string? AlertReason { get; set; }

        /// <summary>
        /// Whether this is a flagged prompt the current viewer had not reviewed before opening this
        /// history — classified after their last visit. Always false for prompts that were not flagged.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>When the prompt was received.</summary>
        public DateTime CreatedAtUtc { get; set; }
    }
}
