using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs
{

    /// <summary>
    /// To handle country lang details.
    /// </summary>
    public class CountryLangModel
    {
        /// <summary>
        /// Gets or sets the county id.
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Gets or sets the description (name of the country according to language).
        /// </summary>
        public string Desc { get; set; }

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