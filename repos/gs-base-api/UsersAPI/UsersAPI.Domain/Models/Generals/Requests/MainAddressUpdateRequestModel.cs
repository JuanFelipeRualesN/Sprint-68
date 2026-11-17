using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle the user’s addresses when requesting an insertion or modification.
    /// </summary>
    public class MainAddressUpdateRequestModel
    {
        /// <summary>
        /// Gets or sets address line 1.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets address line 2.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country ISO code.
        /// </summary>
        public string CountryIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the state iso code. 
        /// </summary>
        public string StateIsoCode { get; set; }
    }
}