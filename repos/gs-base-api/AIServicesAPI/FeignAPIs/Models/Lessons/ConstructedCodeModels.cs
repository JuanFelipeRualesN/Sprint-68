namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Represents a fully assembled curriculum standard code linked to a lesson,
    /// composed of one or more individual code segments.
    /// </summary>
    public class ConstructedCodeHandleModel
    {
        /// <summary>Indicates whether the model passed validation.</summary>
        public bool Valid { get; set; }

        /// <summary>Validation or informational messages.</summary>
        public List<string> Messages { get; set; }

        /// <summary>Unique constructed-code record identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Root/main segment of the standard code.</summary>
        public string MainCode { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; } = null;

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }

        /// <summary>The fully assembled standard code string.</summary>
        public string ConstructedFullCode { get; set; }

        /// <summary>Individual code segments that make up the full code.</summary>
        public List<ConstructedCodeCodeModel> ConstructedCodeCodeModels { get; set; }
    }

    /// <summary>
    /// A single segment of a constructed curriculum standard code.
    /// </summary>
    public class ConstructedCodeCodeModel
    {
        /// <summary>Indicates whether the model passed validation.</summary>
        public bool Valid { get; set; }

        /// <summary>Validation or informational messages.</summary>
        public List<string> Messages { get; set; }

        /// <summary>Unique segment identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the parent <see cref="ConstructedCodeHandleModel"/>.</summary>
        public string LessonConstructedCodeId { get; set; }

        /// <summary>Foreign key referencing the underlying code lookup table.</summary>
        public string CodeId { get; set; }

        /// <summary>Separator character to append after this segment when building the full code.</summary>
        public string DotAfter { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }

        /// <summary>The raw code text for this segment.</summary>
        public string Code { get; set; }

        /// <summary>Human-readable description of what this code segment represents.</summary>
        public string CodeDescription { get; set; }

        /// <summary>Display name of the code's classification type.</summary>
        public string CodeTypeName { get; set; }

        /// <summary>Identifier of the code's classification type.</summary>
        public string CodeTypeId { get; set; }
    }
}
