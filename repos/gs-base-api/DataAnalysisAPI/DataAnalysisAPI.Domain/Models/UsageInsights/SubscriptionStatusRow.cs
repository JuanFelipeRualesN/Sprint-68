namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Users by subscription status for one school or role (active + inactive + canceled + none = total).
    /// </summary>
    /// <param name="Name">School or role name.</param>
    /// <param name="Active">Users with an active subscription.</param>
    /// <param name="Inactive">Users with subscriptions but none active or canceled.</param>
    /// <param name="Canceled">Users with a canceled subscription and no active one.</param>
    /// <param name="WithoutSubscription">Users with no subscription history.</param>
    /// <param name="TotalUsers">Total users in the group.</param>
    public record SubscriptionStatusRow(string Name, int Active, int Inactive, int Canceled, int WithoutSubscription, int TotalUsers);
}
