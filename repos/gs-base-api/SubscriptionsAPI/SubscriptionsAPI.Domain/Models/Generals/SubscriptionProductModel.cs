using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals
{
    public class SubscriptionProductModel : ItemProductModel
    {
        /// <summary>
        /// Gets or sets the product id (the item product id as product id to avoid front-end changes).
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name (the item product name as product name to avoid front-end changes).
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the type of subscription.
        /// </summary>
        public string SubscriptionType { get; set; }
    }
}