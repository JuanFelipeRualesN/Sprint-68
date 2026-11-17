using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    public class ProductRequestModel
    {
        /// <summary>
        /// Gets or sets the product model.
        /// </summary>
        public ProductModel ProductModel { get; set; }

        /// <summary>
        /// Gets or sets the relation between the item product and the product.
        /// </summary>
        public List<ProductItemProductModel> ProductItemProductModelList { get; set; }

    }
}