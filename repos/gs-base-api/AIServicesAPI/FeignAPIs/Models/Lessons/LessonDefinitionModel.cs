namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// A vocabulary or concept definition attached to a lesson.
    /// </summary>
    public class LessonDefinitionModel
    {
        /// <summary>Unique definition identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Term or concept name being defined.</summary>
        public string Name { get; set; }

        /// <summary>Full definition text.</summary>
        public string Desc { get; set; }

        /// <summary>Animation frame data associated with this definition (if any).</summary>
        public string Frames { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
