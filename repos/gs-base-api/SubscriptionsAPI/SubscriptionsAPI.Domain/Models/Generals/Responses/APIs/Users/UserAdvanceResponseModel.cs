using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users
{
    public class UserAdvanceResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAdvance{T}"/> list class.
        /// </summary>
        public UserAdvanceResponseModel()
        {
            UserInfo = new List<UserAdvance>();
        }

        /// <summary>
        /// Gets or sets the page totals.
        /// </summary>
        public int PageTotals { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        public List<UserAdvance> UserInfo { get; set; }
    }

    public class UserAdvance
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
        /// Gets or sets the user phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state iso code.
        /// </summary>
        public string StateIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the last sessions.
        /// </summary>
        public string LastSession { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public string CreatedAt { get; set; }

    }
}