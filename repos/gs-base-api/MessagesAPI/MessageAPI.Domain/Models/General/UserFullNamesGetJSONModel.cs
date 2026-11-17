using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    /// <summary>
    /// To make the call to Genial API to retrieve names
    /// </summary>
    public class UserFullNamesGetJSONModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets user Ids to call.
        /// </summary>
        public List<string> UserIds { get; set; }
    }
}