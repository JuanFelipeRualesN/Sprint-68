using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas
{
    /// <summary>
    /// To return HTML Viewer
    /// </summary>
    public class HTMLViewerResponseModel
    {
        /// <summary>
        /// The HTML Code
        /// </summary>
        public string HTMLCode { get; set; }

        /// <summary>
        /// Prepare Link 
        /// </summary>
        public string Link { get; set; }
    }
}