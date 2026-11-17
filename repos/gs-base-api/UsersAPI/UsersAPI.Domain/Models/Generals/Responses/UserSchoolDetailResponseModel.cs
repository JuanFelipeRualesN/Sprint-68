using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the user’s schools, with school’s details.
    /// </summary>
    public class UserSchoolDetailResponseModel
    {
        /// <summary>
        /// Gets or sets the user school id.
        /// </summary>
        public string UserSchoolId { get; set; }

        /// <summary>
        /// Gets or sets the id of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the institution id.
        /// </summary>
        public string InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        public string InstitutionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the institution name.
        /// </summary>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the School City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the institution State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user institution is confirmed or not.
        /// </summary>
        public bool Confirmed { get; set; }

    }
}