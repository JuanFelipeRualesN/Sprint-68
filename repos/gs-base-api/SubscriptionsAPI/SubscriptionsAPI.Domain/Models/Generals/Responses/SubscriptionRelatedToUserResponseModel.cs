using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    public class SubscriptionRelatedToUserResponseModel
    {
        /// <summary>
        /// Gets or sets the subscription model details.
        /// </summary>
        public SubscriptionModel SubscriptionModel { get; set; }

        /// <summary>
        /// Gets or sets the products (courses and levels) that are included in the subscription. 
        /// </summary>
        public List<SubscriptionProductModel> SubscriptionProductModels { get; set; }
    }
}