namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle cancellations of a subscription.
    /// </summary>
    public class SubscriptionCancelationRequestModel
    {
        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets cancel at period end.
        /// </summary>
        public bool CancelAtPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets the cancellation reason's id. (Predetermined reasons). Added on 2021.09.15
        /// </summary>
        public string CancellationReasonId { get; set; }

        /// <summary>
        /// Gets or sets cancellation reason.
        /// </summary>
        public string CancellationReason { get; set; }
    }
}