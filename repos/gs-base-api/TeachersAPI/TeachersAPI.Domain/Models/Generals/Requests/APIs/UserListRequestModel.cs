using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To ask for user details.
    /// </summary>
    public class UserListRequestModel
    {
        /// <summary>
        /// Gets or sets user Ids to call.
        /// </summary>
        public List<string> UserIds { get; set; }
    }
}