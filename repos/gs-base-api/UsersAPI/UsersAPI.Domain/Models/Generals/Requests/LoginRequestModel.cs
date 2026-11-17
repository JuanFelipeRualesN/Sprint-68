using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// Handles the login request to the APP. As 2021.05.04 the user can connect to the APP using the email or username, plus password.
    /// </summary>
    public class LoginRequestModel
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
        /// Gets or sets the IP address for internal purposes. 
        /// </summary>
        public string IpAddress { get; set; }
    }
}