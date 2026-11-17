using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs
{
    /// <summary>
    /// Permission Session Request (for other APIs asking for User ID).
    /// </summary>
    public class PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets permission name.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the ip Address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets language.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        public string UserId { get; set; }
    }
}