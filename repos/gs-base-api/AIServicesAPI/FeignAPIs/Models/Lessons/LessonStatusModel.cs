namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Records a workflow status assignment for a lesson at a point in time.
    /// </summary>
    public class LessonStatusModel
    {
        /// <summary>Unique status-assignment identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Foreign key referencing the status lookup table.</summary>
        public string StatusId { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
