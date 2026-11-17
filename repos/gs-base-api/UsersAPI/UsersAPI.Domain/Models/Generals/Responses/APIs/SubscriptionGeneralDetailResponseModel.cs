namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To handle the subscription with plan details.
    /// </summary>
    public class SubscriptionGeneralDetailResponseModel
    {
        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        public PlanResponseModel Plan { get; set; }

        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        public SubscriptionModel Subscription { get; set; }
    }
}