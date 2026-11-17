using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    /// <summary>
    /// Response for coupon validation to see if can be applied by the user 
    /// </summary>
    public class CouponValidationResponseModel : ResponseModel
    {
        public CouponModel CouponModel { get; set; }
    }
}
