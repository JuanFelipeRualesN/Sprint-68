using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{

    /// <summary>
    /// To handle the country states.
    /// </summary>
    public class StatesRegionsProvidenceModel
    {

        /// <summary>
        /// Gets or sets the ID of state.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ISO code.
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// Gets or sets the country ISO code.
        /// </summary>
        public string CountryIsoCode { get; set; }

        /// <summary>
        /// Gets or sets if the state or region is domestic.
        /// </summary>
        public string Domestic { get; set; }

        /// <summary>
        /// Gets or sets if available or not.
        /// </summary>
        public string Available { get; set; }

        /// <summary>
        /// Gets or sets the user id who modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets when the row was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the row was modified.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}