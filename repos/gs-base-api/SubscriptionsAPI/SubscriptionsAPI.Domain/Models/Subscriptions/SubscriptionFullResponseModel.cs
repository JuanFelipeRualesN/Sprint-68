using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class SubscriptionFullResponseModel
    {
        /// <summary>
        /// Gets or sets the subscription's model. 
        /// </summary>
        public SubscriptionModel SubscriptionModel { get; set; }

        /// <summary>
        /// Gets or sets the plan's model.
        /// </summary>
        public PlanModel PlanModel { get; set; }

        /// <summary>
        /// Gets or sets the item responses.
        /// </summary>
        public List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.ItemResponseModel> ItemResponseModelList { get; set; }

    }
}
