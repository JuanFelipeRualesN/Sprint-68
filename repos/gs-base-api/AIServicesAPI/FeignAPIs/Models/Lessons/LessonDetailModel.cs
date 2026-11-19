namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Extended descriptive content for a lesson (description and concept overview).
    /// </summary>
    public class LessonDetailModel
    {
        /// <summary>Unique detail record identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Long-form description of the lesson.</summary>
        public string Description { get; set; }

        /// <summary>Key concept or big idea the lesson conveys.</summary>
        public string Concept { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }

        /// <summary>HTML content of the lesson's initial activity.</summary>
        public string ActivityInitial { get; set; }

        /// <summary>HTML content of the lesson's development activity.</summary>
        public string ActivityDevelopment { get; set; }

        /// <summary>HTML content of the lesson's closing activity.</summary>
        public string ActivityClosing { get; set; }
    }
}
