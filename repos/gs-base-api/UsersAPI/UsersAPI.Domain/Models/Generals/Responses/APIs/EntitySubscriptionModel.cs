namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    /// <summary>
    /// To handle subscription entity model for reports.
    /// </summary>
    public class EntitySubscriptionModel
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the platform id.
        /// </summary>
        public string PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at. 
        /// </summary>
        public string UpdatedAt { get; set; }

    }
}