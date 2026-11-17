using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class BillingIntervalResponseModel
    {
        public BillingIntervalModel BillingInterval { get; set; }
        public List<BillingIntervalLangModel> BillingIntervalLang { get; set; }
    }
}
