using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class SubscriptionUserResponseModel : ResponseModel
    {
        public string SubscriptionId { get; set; } // The id of the subscriptions for reference 
        public string Quantity { get; set; }
        public string Status { get; set; }
        public string CurrentPeriodEnd { get; set; }
        public PlanResponseModel Plan { get; set; }
        public List<SubscriptionProfileModel> SubscriptionProfiles { get; set; }
        public List<ItemResponseModel> SubscriptionIncludedItems { get; set; }
        public List<CancellationLogModel> CancellationLogs { get; set; }
        public string ProfilesTotalQuantity { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }

    }
}
