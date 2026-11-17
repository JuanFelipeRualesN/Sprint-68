using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To send user public details.
    /// </summary>
    public class UserSharedDetail
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the invitation code.
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets if the user has the control.
        /// </summary>
        public bool UserHasControl { get; set; }

        /// <summary>
        /// Gets or sets the student record for school.
        /// </summary>
        public string SIE { get; set; }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        public string RoleName { get; set; }
    }
}