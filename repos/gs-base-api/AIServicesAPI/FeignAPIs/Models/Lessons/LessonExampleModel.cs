namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// A worked example that illustrates lesson content.
    /// </summary>
    public class LessonExampleModel
    {
        /// <summary>Unique example identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Short title or label for the example.</summary>
        public string Name { get; set; }

        /// <summary>Example content or explanation text.</summary>
        public string Desc { get; set; }

        /// <summary>Animation frame data associated with this example (if any).</summary>
        public string Frames { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
