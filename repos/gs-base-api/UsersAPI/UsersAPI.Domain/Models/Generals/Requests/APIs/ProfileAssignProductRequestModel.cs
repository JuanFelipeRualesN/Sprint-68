using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To handle assign profile to subscription
    /// </summary>
    public class ProfileAssignProductRequestModel
    {
        /// Gets or sets the profile Id.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the Role Id.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the student role id. This is to identify if only count the student role as profiles quantity.
        /// Administration wanted to leave in the future is the teachers will be also discounted.
        /// </summary>
        public string StudentRoleId { get; set; }

        /// <summary>
        /// Gets or sets multiple profile ids.
        /// </summary>
        public List<string> ProfileIds { get; set; }

        /// <summary>
        /// Gets or sets the IP Address for internal purposes.
        /// </summary>
        public string IpAddress { get; set; }
    }
}