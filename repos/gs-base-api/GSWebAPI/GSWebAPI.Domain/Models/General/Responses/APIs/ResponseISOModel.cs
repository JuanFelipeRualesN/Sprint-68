using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{

    /// <summary>
    /// To handle the country response.
    /// </summary>
    public class ResponseISOModel
    {
        /// <summary>
        /// Gets or sets the iso code.
        /// </summary>
        public string IsoCode2 { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the country iso code.
        /// </summary>
        public string CountryISOCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the country has states or not.
        /// </summary>
        public bool HasStates { get; set; }
    }
}