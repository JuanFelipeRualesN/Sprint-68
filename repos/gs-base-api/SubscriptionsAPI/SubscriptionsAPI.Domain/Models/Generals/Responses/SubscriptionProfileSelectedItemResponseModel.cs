using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    public class SubscriptionProfileSelectedItemResponseModel : SubscriptionsAPI.Domain.Models.Subscriptions.Responses.SubscriptionProfileSelectedItemResponseModel
    {
        public SubscriptionProductModel SubscriptionProductModel { get; set; }
    }
}