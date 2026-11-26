namespace GSAIServicesAPI.Models.PromptMonitoring.Request
{
    /// <summary>
    /// Request to summarize the prompt activity of a group of users — typically the students a
    /// teacher has in their school, so the UI can show who has activity and who raised an alert.
    /// The caller sends the list because only the caller knows the roster: this API stores prompts,
    /// not enrollments.
    /// </summary>
    public class PromptMonitoringUsersSummaryRequestModel
    {
        /// <summary>Ids of the users to summarize.</summary>
        public List<string> Users { get; set; } = [];

        /// <summary>Optional start of the date range, inclusive. Comes from the client's period filter.</summary>
        public DateTime? From { get; set; }

        /// <summary>Optional end of the date range, inclusive. Comes from the client's period filter.</summary>
        public DateTime? To { get; set; }
    }
}
