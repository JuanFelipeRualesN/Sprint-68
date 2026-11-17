using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models
{
    /// <summary>
    /// To handle the user advance responses.
    /// </summary>
    public class UserAdvanceResponseModel : IPageTotals
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

    public class UserAdvance : UserResponse
    {
        /// <summary>
        /// Gets or sets the user phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        public string AddressLine2 { get; set; }

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
        /// Gets or sets the country Iso 2.
        /// </summary>
        public string CountryIsoCode2 { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the ore id.
        /// </summary>
        public string OreId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string StatusId { get; set; }

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