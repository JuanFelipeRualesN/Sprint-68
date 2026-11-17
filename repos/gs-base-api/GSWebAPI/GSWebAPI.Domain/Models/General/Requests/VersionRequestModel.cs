using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// Handles the versions requests.
    /// </summary>
    public class VersionRequestModel
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the release date.
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}