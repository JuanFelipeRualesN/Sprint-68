using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To receive the user full name.
    /// </summary>
    public class UserFullNameModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created at date.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at date.
        /// </summary>
        public string UpdatedAt { get; set; }

    }
}