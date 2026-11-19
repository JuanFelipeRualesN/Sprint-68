namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Represents a level within a learning taxonomy (e.g. Bloom's Taxonomy),
    /// used to classify the cognitive demand of a quiz.
    /// </summary>
    public class TaxonomyLevelModel
    {
        /// <summary>Unique taxonomy-level identifier.</summary>
        public string Id { get; set; }

        /// <summary>Ordinal number of this level within the taxonomy hierarchy.</summary>
        public string TaxLevelNumber { get; set; }

        /// <summary>Display name of the taxonomy level (e.g. "Remember", "Analyze").</summary>
        public string TaxLevelName { get; set; }

        /// <summary>Foreign key linking to the associated psychomotor taxonomy category.</summary>
        public string TaxPsychToTaxPsychId { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
