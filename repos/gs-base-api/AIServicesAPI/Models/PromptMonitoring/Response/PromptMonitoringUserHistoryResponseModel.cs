namespace GSAIServicesAPI.Models.PromptMonitoring.Response
{
    /// <summary>
    /// One page of a user's prompt history. <see cref="Total"/> counts every prompt that matches the
    /// query across all pages, so the client can render pagination; <see cref="Prompts"/> holds only
    /// the records of the requested page.
    /// </summary>
    public class PromptMonitoringUserHistoryResponseModel
    {
        /// <summary>The prompts of this page, newest first.</summary>
        public List<PromptMonitoringPromptResponseModel> Prompts { get; set; } = [];

        /// <summary>How many prompts match the query in total, across every page.</summary>
        public long Total { get; set; }

        /// <summary>The page that was returned, 1-based.</summary>
        public int Page { get; set; }

        /// <summary>The page size that was applied, after clamping.</summary>
        public int PageSize { get; set; }
    }
}
