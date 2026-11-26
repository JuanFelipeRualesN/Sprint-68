namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// A learning objective that defines what a student should achieve after completing the lesson.
    /// </summary>
    public class LessonObjectiveModel
    {
        /// <summary>Unique objective identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Text of the learning objective.</summary>
        public string Desc { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
