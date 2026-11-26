namespace GSAIServicesAPI.Models.DTOs
{
    /// <summary>
    /// Request payload for querying codes by subject and level.
    /// </summary>
    public class CodesQueryRequest
    {
        /// <summary>Subject codes to filter by (e.g. "MAT-SP", "SCI-EN").</summary>
        public List<string> SubjectCodes { get; set; } = new();

        /// <summary>Level/grade codes to filter by (e.g. "k", "1", "2").</summary>
        public List<string> LevelCodes { get; set; } = new();

        /// <summary>Page index (zero-based). Defaults to 0.</summary>
        public int FromPage { get; set; } = 0;

        /// <summary>Number of items per page. Defaults to 50.</summary>
        public int Limiter { get; set; } = 50;

        /// <summary>When true, returns only unique expectations.</summary>
        public bool UniqueExpectations { get; set; } = false;

        /// <summary>Language code for localization. Defaults to "en".</summary>
        public string LangCode { get; set; } = "en";
    }

}
