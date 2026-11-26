using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>The published Athenas lessons of a subject and level: the lesson catalog the study-plan assistant builds from.</summary>
    public interface IAthenasLessonSearchClient
    {
        /// <summary>
        /// Every published lesson of the subject and level, in Athenas order (lesson number), with the ids of its codes
        /// (standards among them). Cached 30 minutes per subject, level and language.
        /// </summary>
        /// <param name="subjectCode">Athenas subject code, e.g. mat-sp.</param>
        /// <param name="levelCode">Athenas level code, e.g. 2.</param>
        /// <param name="langCode">Accept-Language sent to Athenas.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The lessons (possibly empty), or null when Athenas could not be read.</returns>
        Task<IReadOnlyList<AthenasCatalogLesson>?> GetPublishedLessonsAsync(string subjectCode, string levelCode, string? langCode, CancellationToken cancellationToken = default);
    }

    /// <summary>A published lesson of the catalog.</summary>
    /// <param name="LessonId">Athenas lesson id.</param>
    /// <param name="Title">Lesson title.</param>
    /// <param name="LessonNo">Lesson number as Athenas shows it.</param>
    /// <param name="Description">Plain-text description, shortened.</param>
    /// <param name="CodeIds">Athenas code ids linked to the lesson (its constructed codes); the standards are among them.</param>
    public sealed record AthenasCatalogLesson(ulong LessonId, string Title, string? LessonNo, string Description, IReadOnlyList<ulong> CodeIds);

    /// <summary>
    /// Reads <c>POST api/lessons/advance/</c> of Athenas (<c>AppSettings:athenas_api_advanced_lesson_search</c>, on
    /// <c>AppSettings:athenas_service_url</c> with <c>X-API-KEY</c>), filtered by subject, level and published status, page
    /// by page.
    /// </summary>
    public sealed class AthenasLessonSearchClient : IAthenasLessonSearchClient
    {
        private const string StatusPublished = "6";
        private const int PageSize = 100;
        private const int MaxPages = 20;
        private const int DescriptionMaxLength = 300;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan CallTimeout = TimeSpan.FromSeconds(30);
        private static readonly Regex HtmlTag = new("<[^>]+>", RegexOptions.Compiled);
        private static readonly JsonSerializerSettings JsonSettings = new() { NullValueHandling = NullValueHandling.Ignore };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AthenasLessonSearchClient> _logger;

        /// <summary>Creates the client.</summary>
        /// <param name="httpClient">HTTP client.</param>
        /// <param name="configuration">Athenas URL, key and endpoint (AppSettings section).</param>
        /// <param name="cache">Memory cache.</param>
        /// <param name="logger">Logger.</param>
        public AthenasLessonSearchClient(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache, ILogger<AthenasLessonSearchClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<AthenasCatalogLesson>?> GetPublishedLessonsAsync(string subjectCode, string levelCode, string? langCode, CancellationToken cancellationToken = default)
        {
            var lang = string.IsNullOrWhiteSpace(langCode) ? "en" : langCode.Trim().ToLowerInvariant();
            var key = $"athenas:published-lessons:{subjectCode.Trim().ToLowerInvariant()}:{levelCode.Trim().ToLowerInvariant()}:{lang}";
            if (_cache.TryGetValue(key, out IReadOnlyList<AthenasCatalogLesson>? cached) && cached != null)
            {
                return cached;
            }

            var lessons = new Dictionary<ulong, AthenasCatalogLesson>();
            for (var page = 0; page < MaxPages; page++)
            {
                var body = new SearchRequest
                {
                    SubjectCodes = new List<string> { subjectCode },
                    LevelCodes = new List<string> { levelCode },
                    StatusIds = new List<string> { StatusPublished },
                    PageNumber = page,
                    Limit = PageSize,
                };
                var response = await SendAsync(body, lang, cancellationToken);
                if (response == null)
                {
                    return null; // nothing cached: the next call tries again
                }

                var items = response.LessonsDetails?.Lessons ?? new List<SearchLesson>();
                foreach (var item in items)
                {
                    if (!ulong.TryParse(item.LessonModel?.Id, out var id) || string.IsNullOrWhiteSpace(item.LessonModel!.LessonTitle))
                    {
                        continue;
                    }

                    var codeIds = (item.ConstructedCodeHandleModelList ?? new List<ConstructedCode>())
                        .SelectMany(c => c.ConstructedCodeCodeModels ?? new List<ConstructedCodeCode>())
                        .Select(c => ulong.TryParse(c.CodeId, out var codeId) ? codeId : 0)
                        .Where(codeId => codeId != 0)
                        .Distinct()
                        .ToList();
                    lessons[id] = new AthenasCatalogLesson(id, item.LessonModel.LessonTitle.Trim(), item.LessonModel.LessonNo?.Trim(), PlainText(item.LessonDetailModel?.Description), codeIds);
                }

                if (items.Count < PageSize)
                {
                    break;
                }
            }

            IReadOnlyList<AthenasCatalogLesson> ordered = lessons.Values
                .OrderBy(l => SortKey(l.LessonNo))
                .ThenBy(l => l.LessonNo, StringComparer.OrdinalIgnoreCase)
                .ThenBy(l => l.LessonId)
                .ToList();
            _cache.Set(key, ordered, CacheDuration);
            return ordered;
        }

        /// <summary>The first number in the lesson number (lessons without one go last).</summary>
        private static decimal SortKey(string? lessonNo)
        {
            var match = Regex.Match(lessonNo ?? string.Empty, @"\d+(\.\d+)?");
            return match.Success && decimal.TryParse(match.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var value)
                ? value
                : decimal.MaxValue;
        }

        private static string PlainText(string? html)
        {
            var text = WebUtility.HtmlDecode(HtmlTag.Replace(html ?? string.Empty, " "));
            text = Regex.Replace(text, @"\s+", " ").Trim();
            return text.Length <= DescriptionMaxLength ? text : text[..DescriptionMaxLength];
        }

        private async Task<SearchResponse?> SendAsync(SearchRequest body, string lang, CancellationToken cancellationToken)
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(CallTimeout);
            var endpoint = _configuration["AppSettings:athenas_api_advanced_lesson_search"] ?? "api/lessons/advance/";
            try
            {
                var baseUrl = _configuration["AppSettings:athenas_service_url"]
                    ?? throw new InvalidOperationException("AppSettings:athenas_service_url is not configured.");
                using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(baseUrl), endpoint))
                {
                    Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
                };
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-API-KEY", _configuration["AppSettings:AthenasWebServiceApiAccessKey"]);
                request.Headers.Add("Accept-Language", lang);

                using var response = await _httpClient.SendAsync(request, timeout.Token);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Athenas {Endpoint} returned {StatusCode}.", endpoint, (int)response.StatusCode);
                    return null;
                }

                return JsonConvert.DeserializeObject<SearchResponse>(await response.Content.ReadAsStringAsync(timeout.Token), JsonSettings);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException or UriFormatException)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                _logger.LogWarning(ex, "Could not search the Athenas lessons ({Endpoint}).", endpoint);
                return null;
            }
        }

        // Request and response shapes of api/lessons/advance/ (only the fields used here).
        private sealed class SearchRequest
        {
            public List<string> SubjectCodes { get; set; } = new();

            public List<string> LevelCodes { get; set; } = new();

            public List<string> StatusIds { get; set; } = new();

            public List<object> StandardCodes { get; set; } = new();

            public List<object> ExpectationCodes { get; set; } = new();

            public string TeacherInputText { get; set; } = string.Empty;

            public string ContentFilter { get; set; } = string.Empty;

            public int PageNumber { get; set; }

            public int Limit { get; set; }
        }

        private sealed class SearchResponse
        {
            public SearchDetails? LessonsDetails { get; set; }
        }

        private sealed class SearchDetails
        {
            public List<SearchLesson>? Lessons { get; set; }
        }

        private sealed class SearchLesson
        {
            public SearchLessonModel? LessonModel { get; set; }

            public SearchLessonDetail? LessonDetailModel { get; set; }

            public List<ConstructedCode>? ConstructedCodeHandleModelList { get; set; }
        }

        private sealed class SearchLessonModel
        {
            public string? Id { get; set; }

            public string? LessonNo { get; set; }

            public string? LessonTitle { get; set; }
        }

        private sealed class SearchLessonDetail
        {
            public string? Description { get; set; }
        }

        private sealed class ConstructedCode
        {
            public List<ConstructedCodeCode>? ConstructedCodeCodeModels { get; set; }
        }

        private sealed class ConstructedCodeCode
        {
            public string? CodeId { get; set; }
        }
    }
}
