using GSAIServicesAPI.FeignAPIs.Models.Lessons.Requests;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.FeignAPIs.Models.Lessons;
using GSAIServicesAPI.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text;

namespace GSAIServicesAPI.Services
{
    /// <summary>
    /// Retrieves lessons from the Athenas API, caches the full collection,
    /// and applies an optional in-memory search across key lesson fields.
    /// </summary>
    public class LessonsService
    {
        private readonly AthenasFeignAPI _athenasFeignAPI;
        private readonly IMemoryCache _cache;
        private readonly ILogger<LessonsService> _logger;
        private readonly string StatusPublished = "6";

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public LessonsService(
            AthenasFeignAPI athenasFeignAPI,
            IMemoryCache cache,
            ILogger<LessonsService> logger)
        {
            _athenasFeignAPI = athenasFeignAPI;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Returns lessons filtered by subject and level codes.
        /// The full result set is cached; <paramref name="searchTerm"/> is then applied
        /// in-memory so re-searches with different terms do not trigger a new upstream call.
        /// </summary>
        /// <param name="subjectCodes">Subject codes to pass to Athenas. May be <c>null</c> to omit subject filtering.</param>
        /// <param name="levelCodes">Level/grade codes to pass to Athenas.</param>
        /// <param name="searchTerm">
        /// Optional free-text term matched case-insensitively against:
        /// <list type="bullet">
        ///   <item><description><c>LessonModel</c> – LessonTitle, LessonNo, SubjectCode, LevelCode</description></item>
        ///   <item><description><c>LessonDefinitionModel</c> – Name, Desc</description></item>
        ///   <item><description><c>LessonObjectiveModel</c> – Desc</description></item>
        ///   <item><description><c>LessonDetailModel</c> – Description, Concept</description></item>
        ///   <item><description><c>ConstructedCodeHandleModel</c> – ConstructedCodeFull</description></item>
        /// </list>
        /// </param>
        /// <param name="langCode">Localisation language code.</param>
        /// <returns>
        /// A <see cref="LessonsFeignResponseModel"/> whose <c>LessonsDetails</c> list is
        /// filtered to matching <see cref="LessonModifierRequestModel"/> entries.
        /// </returns>
        public async Task<LessonsFeignResponseModel> GetLessonsAsync(
            List<string>? subjectCodes,  // ← nullable
            List<string>? levelCodes,
            string? searchTerm = null,
            string langCode = "en")
        {
            var isSearch = !string.IsNullOrWhiteSpace(searchTerm);
            var cacheKey = CacheKeys.Lessons(subjectCodes ?? [], levelCodes ?? [], langCode);

            // ── 1. Attempt cache retrieval ────────────────────────────────────────
            if (isSearch || !_cache.TryGetValue(cacheKey, out LessonsFeignResponseModel? cached) || cached is null)
            {
                _logger.LogInformation("Cache miss – fetching lessons from Athenas. Key: {Key}", cacheKey);

                var requestModel = new LessonAdvanceRequestModel
                {
                    SubjectCodes = subjectCodes ?? [],  // ← null-safe
                    LevelCodes = levelCodes ?? [],
                    PageNumber = 0,
                    Limit = 100,
                    StatusIds = [StatusPublished],
                    StandardCodes = [],
                    ExpectationCodes = [],
                    TeacherInputText = string.Empty,
                    ContentFilter = isSearch ? searchTerm!.Trim() : string.Empty
                };

                cached = await _athenasFeignAPI.GetLessonsAdvanceAsync(requestModel, langCode);

                var totalFetched = cached?.LessonsDetails?.Lessons.Count ?? 0;

                if (totalFetched > 0 && !isSearch)
                {
                    _cache.Set(cacheKey, cached, CacheDuration);
                    _logger.LogInformation(
                        "Cached {Count} lesson(s) under key: {Key}", totalFetched, cacheKey);
                }
                else if (totalFetched == 0)
                {
                    _logger.LogWarning("Athenas returned no lessons for key: {Key}", cacheKey);
                    return cached ?? new LessonsFeignResponseModel();
                }
            }
            else
            {
                _logger.LogDebug("Cache hit for lessons key: {Key}", cacheKey);
            }

            // ── 2. Apply in-memory search if a term was supplied ─────────────────
            if (string.IsNullOrWhiteSpace(searchTerm))
                return cached!;

            var filteredDetails = ApplySearch(cached!.LessonsDetails.Lessons, searchTerm);

            // Return a shallow copy so the cached original is never mutated.
            return new LessonsFeignResponseModel
            {
                // Filtered detail list; lightweight list is intentionally excluded
                // since it does not carry the sub-models needed for search.
                LessonsDetails = new LessonsResponseModel()
                {
                    Lessons = filteredDetails,
                    TotalResults = filteredDetails.Count
                }
            };
        }

        // ── Private helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Filters a list of <see cref="LessonModifierRequestModel"/> to those where
        /// any searchable field (across all nested sub-models) contains <paramref name="term"/>.
        /// </summary>
        private static List<LessonModifierRequestModel> ApplySearch(
            List<LessonModifierRequestModel>? lessons, string term)
        {
            if (lessons is null or { Count: 0 })
                return new List<LessonModifierRequestModel>();

            var cmp = StringComparison.OrdinalIgnoreCase;

            return lessons
                .Where(l => MatchesSearch(l, term, cmp))
                .ToList();
        }

        /// <summary>
        /// Determines whether a lesson matches the search term based on its fields.
        /// </summary>
        private static bool MatchesSearch(
            LessonModifierRequestModel lesson, string term, StringComparison cmp)
        {
            // ── LessonModel base fields ───────────────────────────────────────────
            if (lesson.LessonModel is { } lm)
            {
                if (Has(lm.LessonTitle, term, cmp)) return true;
                if (Has(lm.LessonNo, term, cmp)) return true;
                if (Has(lm.SubjectCode, term, cmp)) return true;
                if (Has(lm.LevelCode, term, cmp)) return true;
            }

            // ── LessonDefinitionModel list – Name + Desc ──────────────────────────
            if (lesson.LessonDefinitionModelList is { Count: > 0 } defs)
            {
                if (defs.Any(d => Has(d.Name, term, cmp) || Has(d.Desc, term, cmp)))
                    return true;
            }

            // ── LessonObjectiveModel list – Desc ──────────────────────────────────
            if (lesson.LessonObjectiveModelList is { Count: > 0 } objs)
            {
                if (objs.Any(o => Has(o.Desc, term, cmp)))
                    return true;
            }

            // ── LessonDetailModel – Description + Concept ─────────────────────────
            if (lesson.LessonDetailModel is { } detail)
            {
                if (Has(detail.Description, term, cmp)) return true;
                if (Has(detail.Concept, term, cmp)) return true;
            }

            // ── ConstructedCodeHandleModel list – ConstructedCodeFull ─────────────
            if (lesson.ConstructedCodeHandleModelList is { Count: > 0 } codes)
            {
                if (codes.Any(c => Has(c.ConstructedFullCode, term, cmp)))
                    return true;
            }

            return false;
        }

        /// <summary>Returns true when <paramref name="value"/> is non-null and contains <paramref name="term"/>.</summary>
        private static bool Has(string? value, string term, StringComparison cmp)
            => StripAccents(value).Contains(StripAccents(term), StringComparison.Ordinal);

        /// <summary>
        /// Lowercases and removes accent marks so accented text compares equal.
        /// </summary>
        private static string StripAccents(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var normalized = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(c));
            }

            return sb.ToString();
        }
    }
}