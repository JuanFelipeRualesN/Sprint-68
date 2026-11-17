using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models
{
    /// <summary>
    /// To handle the authentication with Dapper. Added by POR 2021.10.15 for performance improvements.
    /// </summary>
    public class UserAuthenticationModel
    {
        /// <summary>
        /// Gets or sets the account's id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the account's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the account's hashed password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the invitation's code.
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// Gets or sets the account's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the account status.
        /// </summary>
        public string StatusId { get; set; }

        /// <summary>
        /// Gets or sets to allow access (1) or not (0).
        /// </summary>
        public int AllowAccess { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets if the account is the main account (1).
        /// </summary>
        public string MainUserAccount { get; set; }


    }
}