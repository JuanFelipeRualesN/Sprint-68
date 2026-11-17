using System.Collections.Generic;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    public class CouponAdvanceResponseModel
    {
        /// <summary>
        /// Gets or sets how many pages per result.
        /// </summary>
        public int PageTotals { get; set; }

        /// <summary>
        /// Gets or sets the coupon details.
        /// </summary>
        public List<CouponModel> Coupons { get; set; }
    }
}