using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{
    /// <summary>
    /// To return user full names in a JSON.
    /// </summary>
    public class UserResponseModel : IPageTotals
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserResponseModel"/> class.
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
}