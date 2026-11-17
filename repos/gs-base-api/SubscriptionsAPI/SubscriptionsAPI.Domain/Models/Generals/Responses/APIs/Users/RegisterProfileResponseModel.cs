using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users
{
    /// <summary>
    /// To handle the register profile response.
    /// </summary>
    public class RegisterProfileResponseModel
    {
        /// <summary>
        /// Gets or sets the quantity of subscriptions left.
        /// </summary>
        public bool SpacesInSubscriptionLeft { get; set; }

        /// <summary>
        /// Gets or sets the messages' list about the transaction's details.
        /// </summary>
        public List<string> Messages { get; set; }
    }
}