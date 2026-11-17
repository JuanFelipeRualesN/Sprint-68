using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To handle the response of the rival.
    /// </summary>
    public class UserRivalResponseModel
    {
        /// <summary>
        /// Gets or sets the user rival id.
        /// </summary>
        public string UserRivalId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; }
    }
}