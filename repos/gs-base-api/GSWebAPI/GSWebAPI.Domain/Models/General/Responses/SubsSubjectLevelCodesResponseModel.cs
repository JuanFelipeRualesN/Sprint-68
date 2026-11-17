using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the user subject and levels.
    /// </summary>
    public class SubsSubjectLevelCodesResponseModel : SubscriptionProductModel
    {
        /// <summary>
        /// Gets or sets the name of the subject and grade.
        /// </summary>
        public string Name { get; set; }
    }
}