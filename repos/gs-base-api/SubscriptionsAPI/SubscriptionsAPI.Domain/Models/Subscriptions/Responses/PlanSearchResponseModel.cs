using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class PlanSearchResponseModel
    {
        public int PageTotals { get; set; }
        public List<PlanResponseModel> Plans { get; set; }
    }
}
