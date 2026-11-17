using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To handle the request.
    /// </summary>
    public class UserListRequestModel : PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets user Ids to call.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Gets or sets the school id.
        /// </summary>
        public List<string> SchoolIds { get; set; }
    }
}