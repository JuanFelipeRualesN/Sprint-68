using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle the requests related to subscriptions
    /// </summary>
    public class SubscriptionRequestModel
    {
        /// <summary>
        /// Gets or sets the username related to subscription.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the first name related to subscription.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name related to subscription.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email related to subscription.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the date where it was created. (from)
        /// </summary>
        public string DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date where it was created. (to)
        /// </summary>
        public string DateTo { get; set; }

        /// <summary>
        /// Gets or sets the lists of subscriptions Ids to get.
        /// </summary>
        public List<string> SubscriptionIds { get; set; }

        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        public SubscriptionUserResponseModel Subscription { get; set; }

        /// <summary>
        /// Gets or sets the page from (for pagination).
        /// </summary>
        public int PageFrom { get; set; }

        /// <summary>
        /// Gets or sets how many items per page.
        /// </summary>
        public int Limiter { get; set; }

        /// <summary>
        /// Gets or sets the IP Address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets whether the subscription is for a school or not. (For commercial use)
        /// </summary>
        public string Type { get; set; }

    }
}