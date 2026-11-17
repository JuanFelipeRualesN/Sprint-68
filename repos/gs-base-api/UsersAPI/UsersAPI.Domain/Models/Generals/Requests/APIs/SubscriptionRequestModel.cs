using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To handle subscription request model.
    /// </summary>
    public class SubscriptionRequestModel : PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        public Subscription Subscription { get; set; }

        /// <summary>
        /// Gets or sets the Subscription Ids.
        /// </summary>
        public List<string> SubscriptionIds { get; set; }
    }

    /// <summary>
    /// Used to request a subscription details.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }
    }
}