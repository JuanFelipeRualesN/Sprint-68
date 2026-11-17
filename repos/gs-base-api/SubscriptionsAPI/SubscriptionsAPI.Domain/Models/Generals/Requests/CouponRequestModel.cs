using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle coupons on requests.
    /// </summary>
    public class CouponRequestModel
    {
        /// <summary>
        /// Gets or sets the coupon details
        /// </summary>
        public CouponModel Coupon { get; set; }

        /// <summary>
        /// Gets or sets the page from (for pagination).
        /// </summary>
        public int PageFrom { get; set; }

        /// <summary>
        /// Gets or sets how many items per page.
        /// </summary>
        public int Limiter { get; set; }

        /// <summary>
        /// Gets or sets whether order by date or not.
        /// </summary>
        public bool OrderByDate { get; set; }

    }
}