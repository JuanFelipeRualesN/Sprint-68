using System.Collections.Generic;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Subscription status totals for all the users considered.
    /// </summary>
    /// <param name="Active">Users with an active subscription.</param>
    /// <param name="Inactive">Users with subscriptions but none active or canceled.</param>
    /// <param name="Canceled">Users with a canceled subscription and no active one.</param>
    /// <param name="WithoutSubscription">Users with no subscription history.</param>
    /// <param name="ActiveWithExpiredCancellationDate">Active users whose cancellation date already passed or is empty; null when the cancellation column does not exist.</param>
    /// <param name="OriginalStatuses">Users per real value of the status column.</param>
    public record SubscriptionStatusCount(int Active, int Inactive, int Canceled, int WithoutSubscription, int? ActiveWithExpiredCancellationDate,
        Dictionary<string, int> OriginalStatuses)
    {
        /// <summary>
        /// Gets the total number of users.
        /// </summary>
        public int Total => Active + Inactive + Canceled + WithoutSubscription;
    }
}
