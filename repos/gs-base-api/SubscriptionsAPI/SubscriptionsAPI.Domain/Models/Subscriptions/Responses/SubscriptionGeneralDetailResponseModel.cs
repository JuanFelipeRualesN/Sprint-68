using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class SubscriptionGeneralDetailResponseModel
    {
        public PlanModel Plan { get; set; }
        public SubscriptionModel Subscription { get; set; }
    }
}
