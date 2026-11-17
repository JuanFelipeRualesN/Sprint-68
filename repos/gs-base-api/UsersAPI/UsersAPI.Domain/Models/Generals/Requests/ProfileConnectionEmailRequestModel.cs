using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To make a request for profile connection.
    /// </summary>
    public class ProfileConnectionEmailRequestModel
    {
        /// <summary>
        /// Gets or sets the user ids to request the connection.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Gets or sets the email of the tutor.
        /// </summary>
        public string Email { get; set; }
    }
}