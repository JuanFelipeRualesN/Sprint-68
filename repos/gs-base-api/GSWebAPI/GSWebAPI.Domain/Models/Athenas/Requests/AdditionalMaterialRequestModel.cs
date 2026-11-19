using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests
{
    /// <summary>
    /// To handle the requests for additional materials. Including from page, how many items, subjects, etc.
    /// </summary>
    public class AdditionalMaterialRequestModel
    {
        /// <summary>
        /// Gets or sets from page.
        /// </summary>
        public int FromPage { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the subject codes.
        /// </summary>
        public List<string> SubjectCodes { get; set; }

        /// <summary>
        /// Gets or sets the level codes.
        /// </summary>
        public List<string> LevelCodes { get; set; }
    }
}
