using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses
{
    public class EntityGeneralDetailsResponseModel
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public string EntityIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the entity type id.
        /// </summary>
        public string EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets if Private "1" or "0"
        /// </summary>
        public string IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the adress line 1.
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
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the state.
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
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }
    }
}