namespace GSAIServicesAPI.Models.PromptMonitoring.Response
{
    /// <summary>
    /// Prompt activity of one user, condensed into counts. Deliberately carries no prompt text: this
    /// is what the teacher's list renders, and the prompts themselves are only fetched when they open
    /// a specific user's history.
    /// </summary>
    public class PromptMonitoringUserSummaryResponseModel
    {
        /// <summary>User who submitted the prompts.</summary>
        public string UserId { get; set; } = "";

        /// <summary>How many prompts the user sent within the requested date range.</summary>
        public long PromptCount { get; set; }

        /// <summary>How many of those prompts were flagged. Greater than zero drives the alert indicator.</summary>
        public long FlaggedCount { get; set; }

        /// <summary>
        /// How many flagged prompts the current viewer has not reviewed yet — i.e. classified after the
        /// last time this viewer opened the student's history. Greater than zero marks the row as "new".
        /// </summary>
        public long UnreadAlertCount { get; set; }

        /// <summary>When the user last sent a prompt within the requested date range.</summary>
        public DateTime LastPromptAtUtc { get; set; }
    }
}
