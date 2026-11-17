using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// User list request model to request users.
    /// </summary>
    public class UserListRequestModel
    {
        /// <summary>
        /// Gets or sets the role id to filter.
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// Gets or sets user Ids to call.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Gets or sets the Ip Address of the User for internal purposes.
        /// </summary>
        public string IpAddress { get; set; }
    }
}