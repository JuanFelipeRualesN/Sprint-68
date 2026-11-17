using System.Collections.Generic;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class CouponSearchResponseModel
    {
        public int PageTotals { get; set; }
        public List<CouponModel> Coupons { get; set; }
    }
}
