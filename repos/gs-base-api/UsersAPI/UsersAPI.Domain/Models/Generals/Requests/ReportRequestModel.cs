using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle the report request.
    /// </summary>
    public class ReportRequestModel
    {
        /// <summary>
        /// Gets or sets the entities ids.
        /// </summary>
        public List<string> EntityIds { get; set; }

        /// <summary>
        /// Gets or sets the subscription ids.
        /// </summary>
        public List<string> SubscriptionsIds { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public string DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public string DateTo { get; set; }

    }
}