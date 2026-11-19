namespace GSAIServicesAPI.FeignAPIs.Models
{
    /// <summary>
    /// To handle the level (grade as 8th grade, kindergarten, etc) model from the Athenas API. This model represents a level entity with properties such as Id, Code, Name, ModifiedBy, CreatedAt, and UpdatedAt.
    /// </summary>
    public class LevelModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the level entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the unique code representing the level (e.g., "K", "G8").
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the display name of the level (e.g., "Kindergarten", "8th Grade").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that last modified the level record.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the level record was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the level record was last updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}