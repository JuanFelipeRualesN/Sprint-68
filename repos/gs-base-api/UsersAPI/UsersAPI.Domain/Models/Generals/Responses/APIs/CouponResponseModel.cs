using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    public class CouponResponseModel
    {
        public bool CouponValid { get; set; }
        public string CouponReason { get; set; }
        public string CouponPlatformId { get; set; }
        public CouponModel CouponModel { get; set; }
    }

    public class CouponModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PlatformId { get; set; }
        public string Currency { get; set; }
        public string PercentOff { get; set; }
        public string AmountOff { get; set; }
        public string Duration { get; set; }
        public string DurationInMonths { get; set; }
        public string MaxRedemptions { get; set; }
        public string MaxRedemptionBySingleUser { get; set; }
        public string RedeemBy { get; set; }
        public string Available { get; set; }
        public string Hidden { get; set; }
        public string IntervalIntended { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}