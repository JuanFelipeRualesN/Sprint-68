using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs
{
    /// <summary>
    /// To relate a user to school.
    /// </summary>
    public class UserRelatedSchoolRequestModel : PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the school user identifier.
        /// </summary>
        public string SchoolUserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the student id.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the school id.
        /// </summary>
        public string SchoolId { get; set; }

    }
}