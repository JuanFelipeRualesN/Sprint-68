using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To handle subscription relations with schools and programs.
    /// </summary>
    public class SubscriptionRelateRequestModel : PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the school ids.
        /// </summary>
        public List<string> SchoolIds { get; set; }

        /// <summary>
        /// Gets or sets the program ids.
        /// </summary>
        public List<string> ProgramIds { get; set; }

        public string EntityAcademicDetailId { get; set; }
    }
}