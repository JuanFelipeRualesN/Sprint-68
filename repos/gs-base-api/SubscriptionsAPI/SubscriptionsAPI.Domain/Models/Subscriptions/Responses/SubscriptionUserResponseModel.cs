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

        /// <summary>
        /// The users related to the subscription (subst_subscription_profiles) with their names and role, from the Users API.
        /// Only filled when requested (for example <c>GET subscriptions/clients?includeProfiles=true</c>); null otherwise.
        /// </summary>
        public List<SubscriptionsAPI.Domain.Models.Generals.Responses.SubscriptionProfileUserResponseModel>? SubscriptionProfileUsers { get; set; }

        /// <summary>
        /// The courses (subject, subject name, level) of the subscription's plan product, through subst_product_item_products,
        /// subst_item_products and gwp_subscription_products. Same item shape as the courses of <c>active/related/user</c>
        /// (ProductId and Id = the item id). Only filled when requested (<c>includeCourses=true</c>); null otherwise.
        /// </summary>
        public List<SubscriptionsAPI.Domain.Models.Generals.SubscriptionProductModel>? SubscriptionCourses { get; set; }
        public List<ItemResponseModel> SubscriptionIncludedItems { get; set; }
        public List<CancellationLogModel> CancellationLogs { get; set; }
        public string ProfilesTotalQuantity { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }

    }
}
