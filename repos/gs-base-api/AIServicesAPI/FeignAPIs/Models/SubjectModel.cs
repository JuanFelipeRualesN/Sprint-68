namespace GSAIServicesAPI.FeignAPIs.Models
{
    /// <summary>
    /// Represents a subject entity (e.g., Mathematics, Science) from the Athenas API, 
    /// including its identification, language localization, and audit metadata.
    /// </summary>
    public class SubjectModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the subject entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the unique code representing the subject (e.g., "MATH", "SCI").
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the display name of the subject (e.g., "Mathematics", "Science").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language code or locale identifier for the subject (e.g., "en", "es").
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user or system that last modified the subject record.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the subject record was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the subject record was last updated.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}