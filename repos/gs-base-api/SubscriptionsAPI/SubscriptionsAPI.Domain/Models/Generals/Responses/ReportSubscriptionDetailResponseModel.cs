using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Handles the report subscription details (for the document generated).
    /// </summary>
    public class ReportSubscriptionDetailResponseModel : SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel
    {
        /// <summary>
        /// Gets or sets the report path.
        /// </summary>
        public string ReportPath { get; set; }
    }
}