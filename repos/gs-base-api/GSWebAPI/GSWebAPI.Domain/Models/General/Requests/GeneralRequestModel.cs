using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// Handles almost all requests.
    /// </summary>
    public class GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level code.
        /// </summary>
        public string LevelCode { get; set; }
    }
}