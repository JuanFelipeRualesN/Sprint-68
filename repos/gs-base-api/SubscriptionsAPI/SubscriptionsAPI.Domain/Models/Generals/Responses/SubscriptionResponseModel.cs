using System.Collections.Generic;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    public class SubscriptionResponseModel
    {
        public SubscriptionResponseModel()
        {
            Messages = new List<string>();
        }
        /// <summary>
        /// Gets or sets the subscriptions details.
        /// </summary>
        public List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.SubscriptionUserResponseModel> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the main account user id.
        /// </summary>
        public string MainAccountUserId { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the messages related to the transaction.
        /// </summary>
        public List<string> Messages { get; set; }
    }
}