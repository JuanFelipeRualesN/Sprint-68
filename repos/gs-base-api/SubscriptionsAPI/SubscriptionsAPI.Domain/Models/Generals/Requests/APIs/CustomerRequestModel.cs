using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To handle customers as update the payment token, create customers, etc. 
    /// </summary>
    public class CustomerRequestModel
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the permission name.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the IP Address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the stripe token.
        /// </summary>
        public string StripeToken { get; set; }
    }
}