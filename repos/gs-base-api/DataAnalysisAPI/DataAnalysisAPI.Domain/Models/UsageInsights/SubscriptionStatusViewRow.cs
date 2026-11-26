namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Like SubscriptionStatusRow, but Inactive is only present when there are users in that status.
    /// </summary>
    /// <param name="Name">School or role name.</param>
    /// <param name="Active">Users with an active subscription.</param>
    /// <param name="Inactive">Inactive users; omitted when zero.</param>
    /// <param name="Canceled">Canceled users.</param>
    /// <param name="WithoutSubscription">Users with no subscription.</param>
    /// <param name="TotalUsers">Total users in the group.</param>
    public record SubscriptionStatusViewRow(string Name, int Active, int? Inactive, int Canceled, int WithoutSubscription, int TotalUsers);
}
