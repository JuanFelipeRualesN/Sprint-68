using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.FeignAPIs.Models.Lessons;
using GSAIServicesAPI.FeignAPIs.Models.Lessons.Requests;
using GSAIServicesAPI.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace GSAIServicesAPI.Services.Plannings
{
    /// <summary>
    /// An Athenas lesson the model can recommend for a planning.
    /// </summary>
    public class AthenasLessonCandidate
    {
        /// <summary>
        /// The lesson's id in the DB
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The lesson's title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// What the lesson covers, for the model to judge if it fits
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Finds the published Athenas lessons related to what a plan will cover. They are read from the DB,
    /// so they always exist and are published when the teacher adds them.
    /// </summary>
    public class AthenasLessonsService
    {
        /// <summary>
        /// The most lessons offered to the model, the ones that best match the plan.
        /// </summary>
        private const int MaxCandidates = 15;
        private const int MaxTerms = 80;
        private const int DescriptionMaxLength = 400;
        private const int LessonPageSize = 100;
        private const int MaxLessonPages = 20;
        private const int TitleWeight = 3;
        private const string StatusPublished = "6";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        private static readonly Regex NonWordPattern = new(@"[^\p{L}\p{N}]+", RegexOptions.Compiled);
        private static readonly Regex HtmlTagPattern = new("<[^>]+>", RegexOptions.Compiled);

        private readonly AthenasFeignAPI _athenasFeignAPI;
        private readonly IMemoryCache _cache;

        public AthenasLessonsService(AthenasFeignAPI athenasFeignAPI, IMemoryCache cache)
        {
            _athenasFeignAPI = athenasFeignAPI;
            _cache = cache;
        }

        /// <summary>
        /// To find the Athenas lessons related to a plan, among the published lessons of its subject and level.
        /// With a unit, they are the lessons mapped to it, ranked by how much they share with the plan.
        /// Without a unit, or when the unit has none mapped, they are the lessons of the subject and level
        /// that share words with the plan, so a lesson about another topic is not offered.
        /// </summary>
        /// <param name="subjectCode">The subject's code of the plan.</param>
        /// <param name="levelCode">The level's code of the plan.</param>
        /// <param name="unitId">The unit's id, or null when the teacher wrote a topic.</param>
        /// <param name="langCode">The language code.</param>
        /// <param name="topicTexts">The texts that describe what the plan covers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the lessons, most relevant first.</returns>
        public async Task<List<AthenasLessonCandidate>> FindAsync(string subjectCode, string levelCode, string? unitId, string langCode, IReadOnlyList<string?> topicTexts, CancellationToken cancellationToken = default)
        {
            var (redirectedSubject, level) = PlanningSubjectMap.ResolveUnitsSource(subjectCode, levelCode);
            // Athenas keeps lessons under both codes of a redirected subject
            var subjects = new[] { subjectCode.Trim().ToLowerInvariant(), redirectedSubject }.Distinct().ToList();

            var lessons = await GetPublishedLessonsAsync(subjects, level, langCode, cancellationToken);
            var mapped = string.IsNullOrWhiteSpace(unitId)
                ? []
                : lessons.Where(lesson => lesson.UnitIds.Contains(unitId)).ToList();

            return mapped.Count > 0
                ? Rank(mapped, lessons, topicTexts, onlyMatching: false)
                : Rank(lessons, lessons, topicTexts, onlyMatching: true);
        }

        /// <summary>
        /// A published lesson with only what the ranking and the prompt use, so the cache does not keep
        /// the whole lesson of Athenas for every subject and level. The words of its title and description
        /// are kept already normalized
        /// </summary>
        private sealed record AthenasLesson(string Id, string Title, string Description, HashSet<string> UnitIds)
        {
            public HashSet<string> TitleWords { get; } = Words(Title).ToHashSet(StringComparer.Ordinal);
            public HashSet<string> DescriptionWords { get; } = Words(Description).ToHashSet(StringComparer.Ordinal);
        }

        /// <summary>
        /// To get every published lesson of the subjects and level.
        /// </summary>
        private Task<List<AthenasLesson>> GetPublishedLessonsAsync(List<string> subjects, string level, string langCode, CancellationToken cancellationToken)
            => _cache.GetOrLoadOnceAsync(
                $"plan_athenas_lessons_{string.Join("_", subjects)}_{level}_{langCode}",
                () => LoadPublishedLessonsAsync(subjects, level, langCode),
                CacheDuration,
                lessons => lessons.Count > 0,
                cancellationToken);

        /// <summary>
        /// To read every published lesson of the subjects and level, across all the pages of Athenas.
        /// </summary>
        private async Task<List<AthenasLesson>> LoadPublishedLessonsAsync(List<string> subjects, string level, string langCode)
        {
            var lessons = new List<AthenasLesson>();
            for (var page = 0; page < MaxLessonPages; page++)
            {
                var response = await _athenasFeignAPI.GetLessonsAdvanceAsync(new LessonAdvanceRequestModel
                {
                    SubjectCodes = subjects,
                    LevelCodes = [level],
                    PageNumber = page,
                    Limit = LessonPageSize,
                    StatusIds = [StatusPublished],
                    StandardCodes = [],
                    ExpectationCodes = [],
                    TeacherInputText = string.Empty,
                    ContentFilter = string.Empty
                }, langCode);

                var pageLessons = response.LessonsDetails?.Lessons ?? [];
                lessons.AddRange(pageLessons
                    .Where(lesson => !string.IsNullOrWhiteSpace(lesson.LessonModel?.Id) && !string.IsNullOrWhiteSpace(lesson.LessonModel?.LessonTitle))
                    .Select(lesson => new AthenasLesson(
                        lesson.LessonModel.Id,
                        lesson.LessonModel.LessonTitle.Trim(),
                        Truncate(PlainText(lesson.LessonDetailModel?.Description)),
                        (lesson.LessonCurricularMapModelList ?? [])
                            .Select(map => map.UnityId)
                            .Where(id => !string.IsNullOrWhiteSpace(id))
                            .ToHashSet())));
                if (pageLessons.Count < LessonPageSize)
                    break;
            }

            return lessons;
        }

        /// <summary>
        /// To keep the lessons that best share the plan's words. Each word weighs by how rare it is among the
        /// lessons of the subject and level
        /// </summary>
        /// <param name="lessons">The lessons to rank.</param>
        /// <param name="allLessons">Every lesson of the subject and level, which the weights are measured on,
        /// so they also make sense for a unit with only a few lessons.</param>
        /// <param name="topicTexts">The texts that describe what the plan covers.</param>
        /// <param name="onlyMatching">Whether a lesson sharing no word is left out. The lessons mapped to the unit
        /// stay anyway, since the mapping already relates them to the plan.</param>
        private static List<AthenasLessonCandidate> Rank(List<AthenasLesson> lessons, List<AthenasLesson> allLessons, IReadOnlyList<string?> topicTexts, bool onlyMatching)
        {
            var weights = GetTerms(topicTexts)
                .Select(term => (Term: term, Lessons: allLessons.Count(lesson => lesson.TitleWords.Contains(term) || lesson.DescriptionWords.Contains(term))))
                .Where(item => item.Lessons > 0 && item.Lessons * 2 <= allLessons.Count)
                .Select(item => (item.Term, Weight: Math.Log((allLessons.Count + 1d) / (item.Lessons + 1))))
                .ToList();

            return lessons
                .Select(lesson => (Lesson: lesson, Score: weights.Sum(item =>
                    (lesson.TitleWords.Contains(item.Term) ? item.Weight * TitleWeight : 0)
                    + (lesson.DescriptionWords.Contains(item.Term) ? item.Weight : 0))))
                .Where(item => !onlyMatching || item.Score > 0)
                .OrderByDescending(item => item.Score)
                .Select(item => item.Lesson)
                .Take(MaxCandidates)
                .Select(lesson => new AthenasLessonCandidate { Id = lesson.Id, Title = lesson.Title, Description = lesson.Description })
                .ToList();
        }

        private static string PlainText(string? html)
            => WebUtility.HtmlDecode(HtmlTagPattern.Replace(html ?? string.Empty, " "));

        /// <summary>
        /// To get the distinct words of the plan's texts, normalized like the lessons words.
        /// </summary>
        private static IEnumerable<string> GetTerms(IEnumerable<string?> texts)
            => texts.SelectMany(Words).Distinct(StringComparer.Ordinal).Take(MaxTerms);

        /// <summary>
        /// To get the normalized words of a text. The words of up to two letters are left out.
        /// </summary>
        private static IEnumerable<string> Words(string? text)
            => NonWordPattern.Split(text ?? string.Empty)
                .Where(word => word.Length > 2)
                .Select(NormalizeWord);

        /// <summary>
        /// To compare a word regardless of case, accents and a simple plural.
        /// </summary>
        private static string NormalizeWord(string word)
        {
            var decomposed = word.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var plain = new StringBuilder(decomposed.Length);
            foreach (var character in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                    plain.Append(character);
            }

            var normalized = plain.ToString();

            if (normalized.Length > 4 && normalized.EndsWith("es") && PluralEsConsonants.Contains(normalized[^3]))
                return normalized[..^2];
            if (normalized.Length > 3 && normalized.EndsWith('s') && !normalized.EndsWith("ss"))
                return normalized[..^1];
            return normalized;
        }

        private const string PluralEsConsonants = "nrldzj";

        /// <summary>
        /// Where the description of many lessons ends and their list of suggested activities starts,
        /// which says nothing about the lesson's topic.
        /// </summary>
        private const string SuggestedActivitiesHeading = "Actividades sugeridas";

        private static string Truncate(string? value)
        {
            var text = (value ?? string.Empty).Trim();
            var activities = text.IndexOf(SuggestedActivitiesHeading, StringComparison.OrdinalIgnoreCase);
            if (activities > 0)
                text = text[..activities].TrimEnd();

            text = WhitespacePattern.Replace(text, " ");
            return text.Length <= DescriptionMaxLength ? text : $"{text[..DescriptionMaxLength]}...";
        }

        private static readonly Regex WhitespacePattern = new(@"\s+", RegexOptions.Compiled);
    }
}
