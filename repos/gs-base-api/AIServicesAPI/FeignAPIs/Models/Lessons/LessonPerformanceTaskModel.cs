namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// A performance task that requires learners to demonstrate mastery of lesson content.
    /// </summary>
    public class LessonPerformanceTaskModel
    {
        /// <summary>Unique performance task identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Description of the task learners must perform.</summary>
        public string Desc { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
