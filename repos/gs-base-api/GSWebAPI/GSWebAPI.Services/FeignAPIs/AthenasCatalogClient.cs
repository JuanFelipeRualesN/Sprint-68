using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>Lesson, subject, level and standard details owned by Athenas.</summary>
    /// <remarks>
    /// Display data only: when Athenas cannot be reached the dictionaries come back empty (or partial) and the caller
    /// returns the plan without titles or names, instead of failing the request.
    /// </remarks>
    public interface IAthenasCatalogClient
    {
        /// <summary>Title, number, subject and level of the given lessons, keyed by lesson id (cached per lesson).</summary>
        /// <param name="lessonIds">Athenas lesson ids; duplicates are ignored.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The lessons found; lessons Athenas did not return (or all of them, when Athenas cannot be reached) are missing.</returns>
        Task<IReadOnlyDictionary<ulong, AthenasLessonInfo>> GetLessonsAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default);

        /// <summary>The Athenas subject catalog, keyed by subject id (cached per language).</summary>
        /// <param name="langCode">Language sent as Accept-Language; null or blank means "en".</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Code and name of every subject; empty when Athenas cannot be reached.</returns>
        Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetSubjectsAsync(string? langCode, CancellationToken cancellationToken = default);

        /// <summary>The Athenas level (grade) catalog, keyed by level id (cached per language).</summary>
        /// <param name="langCode">Language sent as Accept-Language; null or blank means "en".</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Code and name of every level; empty when Athenas cannot be reached.</returns>
        Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetLevelsAsync(string? langCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Code and description of the given standards, keyed by standard id. Read from the standards catalog (Athenas codes
        /// of type standard, about 75), which is loaded whole and cached per language.
        /// </summary>
        /// <param name="standardIds">Athenas standard (code) ids; duplicates are ignored.</param>
        /// <param name="langCode">Language of the description; null or blank means "en". Falls back to the default description when there is no translation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The standards found; ids that are not in the catalog (or a catalog that could not be read) are missing.</returns>
        Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetStandardsAsync(IEnumerable<ulong> standardIds, string? langCode, CancellationToken cancellationToken = default);

        /// <summary>The quizzes (practices, exams, pre-tests and post-tests) of the given lessons, keyed by quiz id. Display data: failures are skipped.</summary>
        /// <param name="lessonIds">Athenas lesson ids; duplicates are ignored.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The quizzes of every lesson that could be read; lessons Athenas could not return are left out.</returns>
        Task<IReadOnlyDictionary<ulong, AthenasQuizInfo>> GetLessonQuizzesAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// The quizzes of one lesson, keyed by quiz id, for business rules (which practices and exams the lesson has).
        /// Null when Athenas cannot be reached, so callers can fail closed.
        /// </summary>
        /// <param name="lessonId">Athenas lesson id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The lesson quizzes (cached 10 minutes, possibly empty), or null when the request failed.</returns>
        Task<IReadOnlyDictionary<ulong, AthenasQuizInfo>?> TryGetLessonQuizzesAsync(ulong lessonId, CancellationToken cancellationToken = default);

        /// <summary>
        /// <see cref="TryGetLessonQuizzesAsync(ulong, CancellationToken)"/> for several lessons: the lessons that are not
        /// cached are read from Athenas in parallel (Athenas has one endpoint per lesson).
        /// </summary>
        /// <param name="lessonIds">Athenas lesson ids; duplicates are ignored.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>One entry per distinct lesson id: its quizzes (possibly empty), or null when that lesson could not be read.</returns>
        Task<IReadOnlyDictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>> TryGetLessonQuizzesAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default);
    }

    /// <summary>Display data of an Athenas lesson.</summary>
    /// <param name="Title">Lesson title.</param>
    /// <param name="LessonNo">Lesson number as shown in Athenas.</param>
    /// <param name="SubjectCode">Code of the lesson subject.</param>
    /// <param name="LevelCode">Code of the lesson level (grade).</param>
    public sealed record AthenasLessonInfo(string? Title, string? LessonNo, string? SubjectCode, string? LevelCode);

    /// <summary>IsExam as Athenas sends it: "0" practice, "1" exam, "2" pre-test, "3" post-test.</summary>
    /// <param name="Title">Quiz title.</param>
    /// <param name="LevelOfKnowledge">Level of knowledge Athenas assigns to the quiz.</param>
    /// <param name="IsExam">Quiz kind code (see the constants of this record).</param>
    public sealed record AthenasQuizInfo(string? Title, string? LevelOfKnowledge, string? IsExam)
    {
        /// <summary>IsExam value of a practice.</summary>
        public const string Practice = "0";

        /// <summary>IsExam value of an exam.</summary>
        public const string Exam = "1";

        /// <summary>IsExam value of a pre-test.</summary>
        public const string PreTest = "2";

        /// <summary>IsExam value of a post-test.</summary>
        public const string PostTest = "3";
    }

    /// <summary>Code and display name (for standards, the name is the description).</summary>
    /// <param name="Code">Catalog code.</param>
    /// <param name="Name">Display name (description for standards).</param>
    public sealed record AthenasCatalogEntry(string? Code, string? Name);

    /// <summary>
    /// Calls the same Athenas endpoints the existing code uses (AppSettings:athenas_service_url + X-API-KEY):
    /// <list type="bullet">
    /// <item>Lessons: <c>athenas_api_lessons_models_read</c> (POST, by lesson ids, as <c>AthenasDetailsHandler.GetLessonModels</c>).</item>
    /// <item>Subjects: <c>athenas_web_api_get_subjects</c> (GET, as <c>AthenasDetailsHandler.GetAthenasSubjects</c>).</item>
    /// <item>Levels: <c>athenas_web_api_get_grades</c> (GET, as TeachersAPI <c>GetAthenasGradesAsync</c>).</item>
    /// <item>
    /// Standards: <c>athenas_web_api_get_codes</c> (POST, as TeachersAPI <c>GetAthenasCodesAsync</c> for "standards"),
    /// filtered by <c>TypeId</c> = 1 and read page by page. <c>codes/query</c> does not filter by <c>Id</c>: a query by id
    /// scans all ~13,000 codes (about 20 seconds) and returns the first one, so standards are never looked up by id.
    /// Descriptions are translated (<c>ls_code_translations</c>) by also filtering by the subjects of the requested language.
    /// </item>
    /// <item>Lesson quizzes: <c>athenas_api_get_lesson_quizzes</c> (GET per lesson, as <c>AthenasDetailsHandler.GetLessonQuizzes</c>).</item>
    /// </list>
    /// Results are cached for 10 minutes (catalogs per language, lessons per id) and the standards catalog for 12 hours
    /// (per language). Lesson ids that Athenas answered without (deleted or unknown) are remembered for 2 minutes so they
    /// are not requested again on every call. Lesson quizzes need one request per lesson and send up to
    /// <see cref="MaxParallelCalls"/> at a time. Each call times out after 15 seconds; calls slower than 3 seconds are
    /// logged as warnings.
    /// </summary>
    public sealed class AthenasCatalogClient : IAthenasCatalogClient
    {
        /// <summary>Maximum Athenas requests in flight for one call that reads several lessons.</summary>
        public const int MaxParallelCalls = 8;

        /// <summary>Athenas code type of the standards (as TeachersAPI <c>CodesController</c> "standards").</summary>
        public const string StandardTypeId = "1";

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan NotFoundCacheDuration = TimeSpan.FromMinutes(2);

        // Standards are curriculum codes that rarely change.
        private static readonly TimeSpan StandardCacheDuration = TimeSpan.FromHours(12);

        // Standards catalog page size, and a stop in case Athenas keeps reporting more pages.
        private const int StandardPageSize = 500;
        private const int MaxStandardPages = 20;

        // Per call: a slow or down Athenas fails the call after this time (the call then returns empty / null).
        private static readonly TimeSpan DefaultCallTimeout = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan SlowCallThreshold = TimeSpan.FromSeconds(3);

        // Cached for ids Athenas answered without, so they are not requested again until the entry expires.
        private static readonly AthenasLessonInfo LessonNotFound = new(null, null, null, null);
        private static readonly JsonSerializerSettings JsonSettings = new() { NullValueHandling = NullValueHandling.Ignore };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AthenasCatalogClient> _logger;

        /// <summary>Creates the client.</summary>
        /// <param name="httpClient">HTTP client used for the Athenas calls.</param>
        /// <param name="configuration">Configuration with the Athenas base URL, API key and endpoint paths (AppSettings section).</param>
        /// <param name="cache">Memory cache for the results.</param>
        /// <param name="logger">Logger for failed Athenas calls.</param>
        public AthenasCatalogClient(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache, ILogger<AthenasCatalogClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<ulong, AthenasLessonInfo>> GetLessonsAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<ulong, AthenasLessonInfo>();
            var missing = new List<ulong>();
            foreach (var id in lessonIds.Distinct())
            {
                if (_cache.TryGetValue(LessonKey(id), out AthenasLessonInfo? cached) && cached != null)
                {
                    if (!ReferenceEquals(cached, LessonNotFound))
                    {
                        result[id] = cached;
                    }
                }
                else
                {
                    missing.Add(id);
                }
            }

            if (missing.Count == 0)
            {
                return result;
            }

            var body = new LessonResponseModel { LessonModelList = missing.Select(id => new LessonModel { Id = id.ToString() }).ToList() };
            var response = await SendAsync<LessonResponseModel>(HttpMethod.Post, Setting("athenas_api_lessons_models_read"), body, null, cancellationToken);
            if (response == null)
            {
                return result; // failure: nothing is cached, the next request tries again
            }

            foreach (var lesson in response.LessonModelList ?? new List<LessonModel>())
            {
                if (ulong.TryParse(lesson.Id, out var id))
                {
                    var info = new AthenasLessonInfo(lesson.LessonTitle, lesson.LessonNo, lesson.SubjectCode, lesson.LevelCode);
                    _cache.Set(LessonKey(id), info, CacheDuration);
                    result[id] = info;
                }
            }

            foreach (var id in missing.Where(id => !result.ContainsKey(id)))
            {
                _cache.Set(LessonKey(id), LessonNotFound, NotFoundCacheDuration);
            }

            return result;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetSubjectsAsync(string? langCode, CancellationToken cancellationToken = default) =>
            GetCatalogAsync<SubjectModel>("subjects", "athenas_web_api_get_subjects", langCode, s => (s.Id, new AthenasCatalogEntry(s.Code, s.Name)), cancellationToken);

        /// <inheritdoc/>
        public Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetLevelsAsync(string? langCode, CancellationToken cancellationToken = default) =>
            GetCatalogAsync<AthenasLevel>("levels", "athenas_web_api_get_grades", langCode, l => (l.Id, new AthenasCatalogEntry(l.Code, l.Name)), cancellationToken);

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetStandardsAsync(IEnumerable<ulong> standardIds, string? langCode, CancellationToken cancellationToken = default)
        {
            var ids = standardIds.Distinct().ToList();
            var result = new Dictionary<ulong, AthenasCatalogEntry>();
            if (ids.Count == 0)
            {
                return result;
            }

            var catalog = await GetStandardCatalogAsync(NormalizeLang(langCode), cancellationToken);
            foreach (var id in ids)
            {
                if (catalog.TryGetValue(id, out var entry))
                {
                    result[id] = entry;
                }
            }

            return result;
        }

        /// <summary>
        /// Every standard (Athenas codes of type <see cref="StandardTypeId"/>) in the given language, cached for 12 hours
        /// per language.
        /// </summary>
        /// <remarks>
        /// Athenas translates a code (<c>ls_code_translations</c>) by the language of the code's subject
        /// (<c>crs_subjects.lang_code</c>), not by Accept-Language, and with several subjects the description comes from
        /// whichever subject comes first. So the catalog is read twice: filtered by the subjects of the requested language
        /// (translated descriptions), and without that filter (the stored description, for standards that have no
        /// subject in that language). When a read fails, nothing is cached and the next call tries again.
        /// </remarks>
        private async Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetStandardCatalogAsync(string lang, CancellationToken cancellationToken)
        {
            var key = $"athenas:standards:{lang}";
            if (_cache.TryGetValue(key, out IReadOnlyDictionary<ulong, AthenasCatalogEntry>? cached) && cached != null)
            {
                return cached;
            }

            var subjectCodes = await GetSubjectCodesOfLanguageAsync(lang, cancellationToken);
            if (subjectCodes == null)
            {
                return new Dictionary<ulong, AthenasCatalogEntry>();
            }

            var catalog = await ReadStandardsAsync(new AthenasCode { TypeId = StandardTypeId }, lang, cancellationToken);
            if (catalog == null)
            {
                return new Dictionary<ulong, AthenasCatalogEntry>();
            }

            if (subjectCodes.Count > 0)
            {
                var filter = new AthenasCode
                {
                    TypeId = StandardTypeId,
                    CodeSubjectModels = subjectCodes.Select(code => new AthenasCodeSubject { SubjectCode = code }).ToList(),
                };
                var translated = await ReadStandardsAsync(filter, lang, cancellationToken);
                if (translated == null)
                {
                    return catalog; // untranslated: not cached
                }

                foreach (var (id, entry) in translated)
                {
                    catalog[id] = entry;
                }
            }

            if (catalog.Count > 0)
            {
                _cache.Set<IReadOnlyDictionary<ulong, AthenasCatalogEntry>>(key, catalog, StandardCacheDuration);
            }

            return catalog;
        }

        /// <summary>The standards that match the filter, read page by page; null when a page fails.</summary>
        private async Task<Dictionary<ulong, AthenasCatalogEntry>?> ReadStandardsAsync(AthenasCode filter, string lang, CancellationToken cancellationToken)
        {
            var standards = new Dictionary<ulong, AthenasCatalogEntry>();
            for (var pageIndex = 0; pageIndex < MaxStandardPages; pageIndex++)
            {
                var page = await SendAsync<AthenasCodePage>(HttpMethod.Post, $"{Setting("athenas_web_api_get_codes")}?fromPage={pageIndex}&limiter={StandardPageSize}",
                    filter, lang, cancellationToken);
                if (page == null)
                {
                    return null;
                }

                foreach (var code in page.Items ?? new List<AthenasCode>())
                {
                    if (ulong.TryParse(code.Id, out var id))
                    {
                        standards[id] = new AthenasCatalogEntry(code.Code, code.Description);
                    }
                }

                if (page.Items is not { Count: > 0 } || pageIndex + 1 >= page.TotalPages)
                {
                    break;
                }
            }

            return standards;
        }

        /// <summary>
        /// Codes of the Athenas subjects whose language is <paramref name="lang"/> (its primary tag: "es-PR" matches "es"),
        /// cached for 10 minutes; null when the subjects cannot be read.
        /// </summary>
        private async Task<IReadOnlyList<string>?> GetSubjectCodesOfLanguageAsync(string lang, CancellationToken cancellationToken)
        {
            var primary = lang.Split('-')[0];
            var key = $"athenas:subject-codes:{primary}";
            if (_cache.TryGetValue(key, out IReadOnlyList<string>? cached) && cached != null)
            {
                return cached;
            }

            var subjects = await SendAsync<List<SubjectModel>>(HttpMethod.Get, Setting("athenas_web_api_get_subjects"), null, lang, cancellationToken);
            if (subjects == null)
            {
                return null;
            }

            IReadOnlyList<string> codes = subjects
                .Where(s => !string.IsNullOrWhiteSpace(s.Code) && string.Equals(s.LangCode?.Trim(), primary, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Code)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            _cache.Set(key, codes, CacheDuration);
            return codes;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<ulong, AthenasQuizInfo>> GetLessonQuizzesAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<ulong, AthenasQuizInfo>();
            foreach (var quizzes in (await TryGetLessonQuizzesAsync(lessonIds, cancellationToken)).Values)
            {
                foreach (var (quizId, info) in quizzes ?? new Dictionary<ulong, AthenasQuizInfo>())
                {
                    result[quizId] = info;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<ulong, AthenasQuizInfo>?> TryGetLessonQuizzesAsync(ulong lessonId, CancellationToken cancellationToken = default)
        {
            var key = $"athenas:lesson-quizzes:{lessonId}";
            if (_cache.TryGetValue(key, out Dictionary<ulong, AthenasQuizInfo>? cached) && cached != null)
            {
                return cached;
            }

            var endpoint = Setting("athenas_api_get_lesson_quizzes").Replace("{0}", lessonId.ToString());
            var response = await SendAsync<LessonQuizzesResponseModel>(HttpMethod.Get, endpoint, null, null, cancellationToken);
            if (response == null)
            {
                return null;
            }

            var quizzes = new Dictionary<ulong, AthenasQuizInfo>();
            foreach (var quiz in response.Quizzes ?? new())
            {
                if (ulong.TryParse(quiz.Id, out var quizId))
                {
                    quizzes[quizId] = new AthenasQuizInfo(quiz.Title, quiz.LevelOfKnowledge, quiz.IsExam);
                }
            }

            _cache.Set(key, quizzes, CacheDuration);
            return quizzes;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>> TryGetLessonQuizzesAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default)
        {
            var result = new ConcurrentDictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>();
            await Parallel.ForEachAsync(lessonIds.Distinct(), ParallelCalls(cancellationToken), async (lessonId, ct) =>
            {
                result[lessonId] = await TryGetLessonQuizzesAsync(lessonId, ct);
            });

            return result;
        }

        private async Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetCatalogAsync<T>(
            string name, string endpointSetting, string? langCode, Func<T, (string? Id, AthenasCatalogEntry Entry)> map, CancellationToken cancellationToken)
        {
            var lang = NormalizeLang(langCode);
            var key = $"athenas:{name}:{lang}";
            if (_cache.TryGetValue(key, out IReadOnlyDictionary<ulong, AthenasCatalogEntry>? cached) && cached != null)
            {
                return cached;
            }

            var items = await SendAsync<List<T>>(HttpMethod.Get, Setting(endpointSetting), null, lang, cancellationToken);
            var result = new Dictionary<ulong, AthenasCatalogEntry>();
            foreach (var item in items ?? new List<T>())
            {
                var (id, entry) = map(item);
                if (ulong.TryParse(id, out var parsed))
                {
                    result[parsed] = entry;
                }
            }

            if (result.Count > 0)
            {
                _cache.Set<IReadOnlyDictionary<ulong, AthenasCatalogEntry>>(key, result, CacheDuration);
            }

            return result;
        }

        private async Task<T?> SendAsync<T>(HttpMethod method, string endpoint, object? body, string? langCode, CancellationToken cancellationToken, TimeSpan? timeout = null)
            where T : class
        {
            using var callTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            callTimeout.CancelAfter(timeout ?? DefaultCallTimeout);
            var started = System.Diagnostics.Stopwatch.GetTimestamp();
            try
            {
                var baseUrl = _configuration["AppSettings:athenas_service_url"]
                    ?? throw new InvalidOperationException("AppSettings:athenas_service_url is not configured.");
                using var request = new HttpRequestMessage(method, new Uri(new Uri(baseUrl), endpoint));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-API-KEY", _configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
                if (langCode != null)
                {
                    request.Headers.Add("Accept-Language", langCode);
                }

                if (body != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                }

                using var response = await _httpClient.SendAsync(request, callTimeout.Token);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Athenas {Endpoint} returned {StatusCode}.", endpoint, (int)response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync(callTimeout.Token);
                var elapsed = System.Diagnostics.Stopwatch.GetElapsedTime(started);
                if (elapsed > SlowCallThreshold)
                {
                    _logger.LogWarning("Athenas {Endpoint} took {ElapsedMs} ms.", endpoint, (long)elapsed.TotalMilliseconds);
                }

                return JsonConvert.DeserializeObject<T>(content, JsonSettings);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException or UriFormatException)
            {
                _logger.LogWarning(ex, "Could not read {Endpoint} from Athenas after {ElapsedMs} ms.", endpoint, (long)System.Diagnostics.Stopwatch.GetElapsedTime(started).TotalMilliseconds);
                return null;
            }
        }

        private string Setting(string key) => _configuration[$"AppSettings:{key}"] ?? throw new InvalidOperationException($"AppSettings:{key} is not configured.");

        private static string LessonKey(ulong id) => $"athenas:lesson:{id}";

        private static ParallelOptions ParallelCalls(CancellationToken cancellationToken) =>
            new() { MaxDegreeOfParallelism = MaxParallelCalls, CancellationToken = cancellationToken };

        private static string NormalizeLang(string? langCode) => string.IsNullOrWhiteSpace(langCode) ? "en" : langCode.Trim().ToLowerInvariant();

        // Deserialization shapes of the Athenas responses (only the fields used here).
        private sealed class AthenasLevel
        {
            public string? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }
        }

        private sealed class AthenasCode
        {
            public string? Id { get; set; }

            public string? Code { get; set; }

            public string? Description { get; set; }

            // Request filters: codes/query filters by type, subjects (and code, levels), never by Id.
            public string? TypeId { get; set; }

            public List<AthenasCodeSubject>? CodeSubjectModels { get; set; }
        }

        private sealed class AthenasCodeSubject
        {
            public string? SubjectCode { get; set; }
        }

        private sealed class AthenasCodePage
        {
            public List<AthenasCode>? Items { get; set; }

            public int TotalPages { get; set; }
        }
    }
}
