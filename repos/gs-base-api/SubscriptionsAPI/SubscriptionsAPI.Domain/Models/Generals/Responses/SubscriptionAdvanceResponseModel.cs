using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle subscription advanced details.
    /// </summary>
    public class SubscriptionAdvanceResponseModel : SubscriptionModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the plan id.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the plan details.
        /// </summary>
        public PlanModel Plan { get; set; }
    }
}