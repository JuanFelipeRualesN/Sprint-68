using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{

    /// <summary>
    /// To handle the country full response.
    /// </summary>
    public class ResponseCountryFullDetailsModel
    {
        /// <summary>
        /// Gets or sets the country Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ISO Code 1.
        /// </summary>
        public string IsoCode1 { get; set; }

        /// <summary>
        /// Gets or sets the ISO code 2.
        /// </summary>
        public string IsoCode2 { get; set; }

        /// <summary>
        /// Gets or sets if domestic or not.
        /// </summary>
        public string Domestic { get; set; }

        /// <summary>
        /// Gets or sets thename of the country.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets if available "1" or not "0".
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

        /// <summary>
        /// Gets or sets language details of the country.
        /// </summary>
        public List<CountryLangModel> CountryNameTrans { get; set; }

        /// <summary>
        /// Gets or sets the regions of the country.
        /// </summary>
        public List<StatesRegionsProvidenceModel> Regions { get; set; }
    }
}