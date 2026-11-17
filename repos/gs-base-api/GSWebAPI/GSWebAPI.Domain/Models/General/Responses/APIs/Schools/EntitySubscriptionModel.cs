using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Schools
{
    /// <summary>
    /// To handle the entity subscriptions.
    /// </summary>
    public class EntitySubscriptionModel : IResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the transaction was valid or not.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the messages related to the transaction.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity-subscription.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the entity's id.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the platform's id.
        /// </summary>
        public string PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who modified the row.
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