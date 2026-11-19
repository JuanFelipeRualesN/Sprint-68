using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Codes
{
    /// <summary>
    /// To handle the constructed codes. 
    /// </summary>
    public class ConstructedCodeHandleModel: ConstructedCodeModel
    {
        /// <summary>
        /// Gets or sets the constructed codes parts.
        /// </summary>
        public List<ConstructedCodeCodeModel> ConstructedCodeCodeModels { get; set; }

        /// <summary>
        /// Gets or sets the constructed code full.
        /// </summary>
        public string ConstructedCodeFull { get; set; }
    }
}
