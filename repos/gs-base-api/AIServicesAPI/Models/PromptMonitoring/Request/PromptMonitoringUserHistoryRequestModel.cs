namespace GSAIServicesAPI.Models.PromptMonitoring.Request
{
    /// <summary>
    /// Request for the prompt history of a single user, newest first. Opened from the student list
    /// when the caller wants to read what one user actually wrote.
    /// </summary>
    public class PromptMonitoringUserHistoryRequestModel
    {
        /// <summary>User whose history is being read. Required: the shard key.</summary>
        public string UserId { get; set; } = "";

        /// <summary>Optional start of the date range, inclusive. Comes from the client's period filter.</summary>
        public DateTime? From { get; set; }

        /// <summary>Optional end of the date range, inclusive. Comes from the client's period filter.</summary>
        public DateTime? To { get; set; }

        /// <summary>Page to read, 1-based. Defaults to the first page.</summary>
        public int Page { get; set; } = 1;

        /// <summary>How many prompts to return. Clamped by the service to a safe maximum.</summary>
        public int PageSize { get; set; } = 20;
    }
}
