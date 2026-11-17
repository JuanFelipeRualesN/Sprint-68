using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Messages = new List<string>();
        }

        public bool TransactionValid { get; set; }
        public List<string> Messages { get; set; }
    }
}
