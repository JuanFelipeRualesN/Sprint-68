using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To return user full names in a JSON.
    /// </summary>
    public class UserSharedDetailsResponseModel
    {
        /// <summary>
        /// Gets or sets the user full name model list.
        /// </summary>
        public List<UserSharedDetail> UserSharedDetailsList { get; set; }

        /// <summary>
        /// Gets or sets the total pages of the results.
        /// </summary>
        public int TotalPages { get; set; }
    }
}