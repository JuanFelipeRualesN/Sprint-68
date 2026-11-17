using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users
{
    public class UserAddressModel
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the person address name.
        /// </summary>
        public string PersonAddressName { get; set; }

        /// <summary>
        /// Gets or sets the address line 1.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state iso code.
        /// </summary>
        public string StateIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the ISO code.
        /// </summary>
        public string CountryIsoCode2 { get; set; }

        /// <summary>
        /// Gets or sets the address extra.
        /// </summary>
        public string AddressExtra { get; set; }

        /// <summary>
        /// Gets or sets the address phone.
        /// </summary>
        public string AddressPhone { get; set; }

        /// <summary>
        /// Gets or sets the main address.
        /// </summary>
        public string MainAddress { get; set; }

        /// <summary>
        /// Gets or sets if enable or not. "1" it is, "0" if not.
        /// </summary>
        public string Enable { get; set; }

    }
}