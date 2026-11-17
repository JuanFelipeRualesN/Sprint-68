using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To handle customers as update the payment token, create customers, etc. 
    /// </summary>
    public class CustomerRequestModel : PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the stripe token.
        /// </summary>
        public string StripeToken { get; set; }
    }
}