using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions
{
    /// <summary>
    /// To handle the subscriptions. Please, keep this updated according to Subscription /Library.
    /// </summary>
    public class SubscriptionModel : IResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the transaction was valid or not.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets a message list.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the subscription's id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the platform's id (by Stripe).
        /// </summary>
        public string PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the customer's platform id. (by Stripe).
        /// </summary>
        public string CustomerPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the plan platform's id (by Stripe).
        /// </summary>
        public string PlanPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the quantity that the customer was subscribed to.
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the status of subscription.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets when the current period ends.
        /// </summary>
        public string CurrentPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets the platform's code. (STR for Stripe, PRI for private).
        /// </summary>
        public string PlatformCode { get; set; }

        /// <summary>
        /// Gets or sets the subscription's type. (H for homeschooling, S for school, and T for tutoring).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the user's id that modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets when the row was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the row was updated.
        /// </summary>
        public string UpdatedAt { get; set; }

    }
}