using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle taxes responses.
    /// </summary>
    public class TaxesResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the taxes model list.
        /// </summary>
        public List<TaxModel> Taxes { get; set; }
    }
}