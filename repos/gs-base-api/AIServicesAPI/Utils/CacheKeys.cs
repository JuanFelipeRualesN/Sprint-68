namespace GSAIServicesAPI.Helpers
{
    /// <summary>
    /// Centralises cache key construction to avoid magic strings scattered across services.
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// Builds a deterministic cache key for a codes query.
        /// Keys are sorted so that ["MAT","SCI"] and ["SCI","MAT"] map to the same entry.
        /// </summary>
        public static string Codes(IEnumerable<string> subjectCodes, IEnumerable<string> levelCodes,
            int fromPage, int limiter, bool uniqueExpectations, string langCode, int typeId)
        {
            var subjects = string.Join(",", subjectCodes.OrderBy(s => s));
            var levels   = string.Join(",", levelCodes.OrderBy(l => l));
            return $"codes:t={typeId}:s={subjects}:l={levels}:p={fromPage}:n={limiter}:u={uniqueExpectations}:lang={langCode}";
        }

        /// <summary>
        /// Builds a deterministic cache key for a lessons query (without search term,
        /// because search is applied in-memory on the cached collection).
        /// </summary>
        public static string Lessons(IEnumerable<string>? subjectCodes, IEnumerable<string> levelCodes,
       string langCode)
        {
            var subjects = string.Join(",", (subjectCodes ?? []).OrderBy(s => s));
            var levels = string.Join(",", levelCodes.OrderBy(l => l));
            return $"lessons:s={subjects}:l={levels}:lang={langCode}";
        }
    }
}
