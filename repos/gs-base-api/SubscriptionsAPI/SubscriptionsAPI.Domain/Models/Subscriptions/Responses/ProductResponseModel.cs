using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses
{
    public class ProductResponseModel : ProductModel
    {
        public List<ItemResponseModel> ItemsIncluded { get; set; }

        public List<ProductItemProductModel> ProductItemProductModelList { get; set; }

        public bool TransactionValid { get; set; }

        public List<string> Messages { get; set; }
    }
}
