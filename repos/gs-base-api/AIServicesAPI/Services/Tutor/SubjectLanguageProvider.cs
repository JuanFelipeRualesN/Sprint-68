using GSAIServicesAPI.FeignAPIs;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// Looks up the language a subject is taught in.
    /// </summary>
    public class SubjectLanguageProvider
    {
        private readonly AthenasFeignAPI _athenasFeignApi;
        private readonly ILogger<SubjectLanguageProvider> _logger;

        public SubjectLanguageProvider(AthenasFeignAPI athenasFeignApi, ILogger<SubjectLanguageProvider> logger)
        {
            _athenasFeignApi = athenasFeignApi;
            _logger = logger;
        }

        /// <summary>
        /// Returns the language the given subject is taught in, or
        /// <see cref="LessonLanguage.Unknown"/> when the subject list cannot be fetched or holds
        /// no usable language for that code. The caller falls back to the code's suffix.
        /// </summary>
        /// <param name="subjectCode">The lesson's subject code.</param>
        public async Task<LessonLanguage> GetLanguageAsync(string? subjectCode)
        {
            if (string.IsNullOrWhiteSpace(subjectCode)) return LessonLanguage.Unknown;

            try
            {
                var subjects = await _athenasFeignApi.GetSubjects();

                var subject = subjects.FirstOrDefault(s =>
                    string.Equals(s.Code, subjectCode, StringComparison.OrdinalIgnoreCase));

                if (subject is null)
                {
                    _logger.LogWarning("Subject {SubjectCode} is not in the Athenas subject list; falling back to the code's suffix for the tutor's language.", subjectCode);
                    return LessonLanguage.Unknown;
                }

                var language = LessonLanguageResolver.FromLanguageCode(subject.LangCode);

                if (language == LessonLanguage.Unknown)
                {
                    _logger.LogWarning("Subject {SubjectCode} reports lang_code '{LangCode}', which is not a supported tutor language; falling back to the code's suffix.", subjectCode, subject.LangCode ?? "(null)");
                }

                return language;
            }
            catch (Exception ex)
            {
                // Uses the subject code's suffix as fall back.
                _logger.LogWarning(ex, "Could not fetch the Athenas subject list to resolve the language for {SubjectCode}.", subjectCode);
                return LessonLanguage.Unknown;
            }
        }
    }
}
