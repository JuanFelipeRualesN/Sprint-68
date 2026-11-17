using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Manages the general responses
    /// </summary>
    public class GeneralResponseModel
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