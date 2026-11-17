using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs
{
    /// <summary>
    /// Class for the Login Request. Username or email, password, and ipAddress should be sent.
    /// </summary>
    public class LoginRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the IP Address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a main account or not.
        /// </summary>
        public bool IsMainAccount { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the OS Version.
        /// </summary>
        public string OsVersion { get; set; }
    }
}