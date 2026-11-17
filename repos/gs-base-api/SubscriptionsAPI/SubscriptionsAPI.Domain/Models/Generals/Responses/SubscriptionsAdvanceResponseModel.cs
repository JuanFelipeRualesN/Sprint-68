using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle subscription response with all details.
    /// </summary>
    public class SubscriptionsAdvanceResponseModel
    {
        /// <summary>
        /// Gets or sets the page totals (of a list of subscription).
        /// </summary>
        public int PageTotals { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public List<SubscriptionAdvanceResponseModel> Subscriptions { get; set; }
    }

}