namespace GSAIServicesAPI.Models.PromptMonitoring
{
    /// <summary>
    /// The verdict the AI returns about a student's prompt, before it is copied onto the stored
    /// <see cref="PromptLog"/>. Property names mirror the log so the mapping stays obvious.
    /// </summary>
    public class PromptMonitoringClassificationModel
    {
        /// <summary>Whether the prompt broke the tutor rules and must be shown as an alert.</summary>
        public bool IsFlagged { get; set; }

        /// <summary>Category codes describing what was detected. Empty when nothing was.</summary>
        public List<string> AlertCategories { get; set; } = [];

        /// <summary>One sentence explaining the alert to the teacher. Null when nothing was flagged.</summary>
        public string? AlertReason { get; set; }
    }
}
