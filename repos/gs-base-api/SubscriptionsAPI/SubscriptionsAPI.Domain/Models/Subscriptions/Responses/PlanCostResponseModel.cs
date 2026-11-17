using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class PlanCostResponseModel : PlanResponseModel
    {
        public string SubTotal { get; set; }
        public List<TaxModel> Taxes { get; set; }
        public string TaxTotal { get; set; }
        public string TotalCost { get; set; }
        public string Discount { get; set; }
        public string SubTotalWithDiscount { get; set; }
        public string CouponDetails { get; set; }
        public string TotalWithDiscounts { get; set; }
        public string TaxTotalWithDiscount { get; set; }
    }
}
