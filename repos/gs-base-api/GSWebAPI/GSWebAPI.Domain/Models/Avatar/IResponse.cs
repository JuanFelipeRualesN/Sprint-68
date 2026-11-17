using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.Avatar
{
    /// <summary>
    /// Interface for response.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets or sets the message string list.
        /// </summary>
        List<string> MessageList { get; set; }

        /// <summary>
        /// Gets or sets if the transaction was valid or not.
        /// </summary>
        bool Valid { get; set; }
    }
}