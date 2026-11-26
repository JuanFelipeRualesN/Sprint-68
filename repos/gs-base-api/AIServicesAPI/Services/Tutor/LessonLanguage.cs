namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// The language a lesson is taught in. The tutor answers in this language regardless of
    /// the language the student writes in
    /// </summary>
    public enum LessonLanguage
    {
        Unknown,
        English,
        Spanish
    }

    /// <summary>
    /// Resolves the language a lesson is taught in.
    /// </summary>
    /// <remarks>
    /// The language comes from the subject's <c>lang_code</c> (crs_subjects table),
    /// the lesson endpoint returns no language field. The subject code's suffix is only a fallback.
    /// When nothing resolves, the language stays <see cref="LessonLanguage.Unknown"/> and the
    /// caller falls back to following the student.
    /// </remarks>
    public static class LessonLanguageResolver
    {
        /// <summary>
        /// Maps a language code to a language
        /// </summary>
        public static LessonLanguage FromLanguageCode(string? languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)) return LessonLanguage.Unknown;

            var code = languageCode.Trim();

            if (string.Equals(code, "en", StringComparison.OrdinalIgnoreCase))
                return LessonLanguage.English;

            if (string.Equals(code, "es", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(code, "sp", StringComparison.OrdinalIgnoreCase))
                return LessonLanguage.Spanish;

            return LessonLanguage.Unknown;
        }

        /// <summary>
        /// Reads the language a subject is taught in off its code.
        /// Anything that is not "EN" or "-EN" is taken as Spanish.
        /// </summary>
        public static LessonLanguage FromSubjectCode(string? subjectCode)
        {
            if (string.IsNullOrWhiteSpace(subjectCode)) return LessonLanguage.Unknown;

            var code = subjectCode.Trim();

            var isEnglish = string.Equals(code, "EN", StringComparison.OrdinalIgnoreCase)
                || code.EndsWith("-EN", StringComparison.OrdinalIgnoreCase);

            return isEnglish ? LessonLanguage.English : LessonLanguage.Spanish;
        }

        /// <summary>
        /// The language's name, as written into a prompt (English or Spanish).
        /// Returns null when the language is unknown
        /// </summary>
        public static string? ToPromptName(this LessonLanguage language) => language switch
        {
            LessonLanguage.English => "English",
            LessonLanguage.Spanish => "Spanish",
            _ => null
        };

        /// <summary>
        /// The ISO code for the language, used when passing the language to
        /// another service. Returns null when the language is unknown.
        /// </summary>
        public static string? ToIsoCode(this LessonLanguage language) => language switch
        {
            LessonLanguage.English => "en",
            LessonLanguage.Spanish => "es",
            _ => null
        };
    }
}
