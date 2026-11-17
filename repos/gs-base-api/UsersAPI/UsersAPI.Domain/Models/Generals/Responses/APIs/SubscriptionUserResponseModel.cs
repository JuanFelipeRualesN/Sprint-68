using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    public class SubscriptionUserResponseModel
    {
        /// <summary>
        /// Gets or sets the subscription Id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the subscription profiles.
        /// </summary>
        public List<SubscriptionProfileModel> SubscriptionProfiles { get; set; }

        /// <summary>
        /// Gets or sets the profiles total quantity.
        /// </summary>
        public string ProfilesTotalQuantity { get; set; }

    }
}