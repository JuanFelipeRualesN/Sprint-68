namespace GSAIServicesAPI.Models.DTOs
{
    /// <summary>
    /// Request payload for querying lessons by subject and level,
    /// with optional full-text search across lesson fields.
    /// </summary>
    public class LessonsQueryRequest
    {
        /// <summary>Subject codes to filter by (e.g. "MAT-SP", "SCI-EN").</summary>
        public List<string> SubjectCodes { get; set; } = new();

        /// <summary>Level/grade codes to filter by (e.g. "k", "1", "2").</summary>
        public List<string> LevelCodes { get; set; } = new();

        /// <summary>
        /// Optional free-text search term. When provided, results are filtered
        /// against Title, Objective, Description, and any other searchable fields.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>Language code for localization. Defaults to "en".</summary>
        public string LangCode { get; set; } = "en";
    }

}
