using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs
{

    /// <summary>
    /// To get the user fullname response of other web api.
    /// </summary>
    public class UserResponseModel : IPageTotals
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserResponse{T}"/> list class.
        /// </summary>
        public UserResponseModel()
        {
            UserInfo = new List<UserResponse>();
        }

        /// <summary>
        /// Gets or sets the page totals.
        /// </summary>
        public int PageTotals { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        public List<UserResponse> UserInfo { get; set; }
    }

    public class UserResponse
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user Token (friend code).
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets whether the user has control 1 or not 0.
        /// </summary>
        public int UserHasControl { get; set; }

        /// <summary>
        /// Gets or sets the User School Record
        /// </summary>
        public string UserSchoolRecord { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        public string Email { get; set; }
    }
}