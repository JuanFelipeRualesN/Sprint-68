using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the coupon response for validation.
    /// </summary>
    public class CouponResponseModel
    {
        /// <summary>
        /// Gets or sets whether the coupon is valid or not.
        /// </summary>
        public bool CouponValid { get; set; }
        /// <summary>
        /// Gets or sets the coupon reason message.
        /// </summary>
        public string CouponReason { get; set; }

        /// <summary>
        /// Gets or sets the coupon platform Id.
        /// </summary>
        public string CouponPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the coupon model.
        /// </summary>
        public CouponModel CouponModel { get; set; }
    }
}