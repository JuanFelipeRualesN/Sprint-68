using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To request a password reset.
    /// </summary>
    public class PasswordResetRequestModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the password retype.
        /// </summary>
        public string PasswordRetype { get; set; }

        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the reset token.
        /// </summary>
        public string Token { get; set; }
    }
}