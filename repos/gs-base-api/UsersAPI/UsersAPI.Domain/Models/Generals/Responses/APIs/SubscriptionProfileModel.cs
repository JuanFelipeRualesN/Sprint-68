using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To handle subscriptions profiles.
    /// </summary>
    public class SubscriptionProfileModel
    {
        /// <summary>
        /// Gets or sets the id of subscription profile.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subscription Id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the profile id.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who modified by. 
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created at date.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at date.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}