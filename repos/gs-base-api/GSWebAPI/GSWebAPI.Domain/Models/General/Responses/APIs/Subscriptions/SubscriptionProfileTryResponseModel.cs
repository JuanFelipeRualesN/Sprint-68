using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions
{
    /// <summary>
    /// To handle the subjects, levels and tries for profiles.
    /// </summary>
    public class SubscriptionProfileTryResponseModel
    {

        /// <summary>
        /// Gets or sets the subscription Type. H for Homeschooler, S for Schools.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the subject's name.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the try max.
        /// </summary>
        public int TryMax { get; set; }
    }
}