using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools
{
    /// <summary>
    /// To handle entity details, including name, etc.
    /// </summary>
    public class EntityModel
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the entity's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the entity's license given by the Department of State.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the entity's type id.
        /// </summary>
        public string EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets if entity is private (1) or public (0).
        /// </summary>
        public string Private { get; set; }

        /// <summary>
        /// Gets or sets the user's id that modified the row.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date when the entity was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the entity was updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}