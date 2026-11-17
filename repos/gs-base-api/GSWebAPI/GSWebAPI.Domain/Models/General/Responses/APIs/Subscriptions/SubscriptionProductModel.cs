using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions
{
    /// <summary>
    /// To handle the item responses.
    /// </summary>
    public class SubscriptionProductModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the item product ids.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the subject's name.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the level's code.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the user's id that modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date when the row was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the row was updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}