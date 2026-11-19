using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
{
    /// <summary>
    /// Manages the general responses
    /// </summary>
    public class AthenasGeneralResponseModel
    {
        /// <summary>
        /// General messages to show what's going on 
        /// </summary>
        public List<string> Messages { get; set; }
        /// <summary>
        /// The user id
        /// </summary>
        public string UserId { get; set; }
    }
}
