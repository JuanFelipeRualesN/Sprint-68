using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle a report of subscriptions
    /// </summary>
    public class ReportSubscriptionResponseModel
    {
        /// <summary>
        /// Gets or sets the report subscription result list.
        /// </summary>
        public List<ReportSubscription> ReportSubscriptionResult { get; set; }

        /// <summary>
        /// Gets or sets the total active.
        /// </summary>
        public int TotalActive { get; set; }

        /// <summary>
        /// Gets or sets the total cancelled.
        /// </summary>
        public int TotalCanceled { get; set; }

        /// <summary>
        /// Gets or sets the total trialing.
        /// </summary>
        public int TotalTrialing { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// To handle subscription details for the Report Subscription.
    /// </summary>
    public class ReportSubscription
    {
        /// <summary>
        /// Gets or sets the plan name.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Gets or sets the plan id.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the plan platform id.
        /// </summary>
        public string PlanPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the total of active.
        /// </summary>
        public int Active { get; set; }

        /// <summary>
        /// Gets or sets the cancelled total.
        /// </summary>
        public int Canceled { get; set; }

        /// <summary>
        /// Gets or sets the trialing total.
        /// </summary>
        public int Trialing { get; set; }
    }
}