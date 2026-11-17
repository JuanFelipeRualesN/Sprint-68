using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions
{
    /// <summary>
    /// To handle the exam tries per subscription.
    /// </summary>
    public class ExamTryModel : IResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the transaction is valid or not.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets a list of messages.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subscription's id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the max quantity.
        /// </summary>
        public string TryMax { get; set; }

        /// <summary>
        /// Gets or sets the user's id that modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date when the row was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date whan the row was updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}