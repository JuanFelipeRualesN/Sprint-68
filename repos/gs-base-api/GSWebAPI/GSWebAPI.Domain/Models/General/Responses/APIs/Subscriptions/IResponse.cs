using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions
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
