using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Schools
{
    /// <summary>
    /// To handle the responses' details.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets or sets a message's list.
        /// </summary>
        List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the transaction was valid or not.
        /// </summary>
        bool Valid { get; set; }
    }
}