using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users
{
    /// <summary>
    /// To handle the User role responses.
    /// </summary>
    public class UserRoleModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the role ids.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user ids.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user id who modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}