namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Core lesson record containing identification and configuration fields.
    /// </summary>
    public class LessonModel
    {
        /// <summary>Indicates whether the model passed validation.</summary>
        public bool Valid { get; set; }

        /// <summary>Validation or informational messages.</summary>
        public List<string> Messages { get; set; }

        /// <summary>Unique lesson identifier.</summary>
        public string Id { get; set; }

        /// <summary>Human-readable lesson number (e.g. "L01").</summary>
        public string LessonNo { get; set; }

        /// <summary>Grade or difficulty level code.</summary>
        public string LevelCode { get; set; }

        /// <summary>Subject area code (e.g. "MATH", "SCI").</summary>
        public string SubjectCode { get; set; }

        /// <summary>Display title of the lesson.</summary>
        public string LessonTitle { get; set; }

        /// <summary>URL or path to the lesson's thumbnail image.</summary>
        public string LessonImg { get; set; }

        /// <summary>ISO language code for the lesson's primary language.</summary>
        public string LessonLangCode { get; set; }

        /// <summary>Indicates whether the lesson includes an audio recorder component.</summary>
        public string HasAudioRecorder { get; set; }

        /// <summary>Indicates whether the lesson uses dynamic content generation.</summary>
        public string IsDynamic { get; set; }

        /// <summary>Indicates whether this is a gap-closing lesson.</summary>
        public string IsGapClosing { get; set; }

        /// <summary>Blueprint reference identifier.</summary>
        public string Blueprint { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
