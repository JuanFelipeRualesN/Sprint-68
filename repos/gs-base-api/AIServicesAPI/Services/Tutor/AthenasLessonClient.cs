using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace GSAIServicesAPI.Services.Tutor
{
    public record LessonMetadata(string? Grade, string? SubjectCode);

    /// <summary>
    /// Resolves a lesson's metadata by calling the Athenas API.
    /// This is the source of truth for a lesson's grade and language
    /// </summary>
    public class AthenasLessonClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Caches lesson metadata by lesson ID, since it doesn't change
        /// </summary>
        private static readonly ConcurrentDictionary<string, LessonMetadata> MetadataCache = new();

        /// <summary>How much of an unexpected response body is written to the log.</summary>
        private const int MaxLoggedBodyChars = 500;

        private readonly ILogger<AthenasLessonClient> _logger;

        public AthenasLessonClient(ILogger<AthenasLessonClient> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the lesson's grade.
        /// </summary>
        /// <param name="lessonId">The lesson id</param>
        /// <param name="token"> The user token.</param>
        public async Task<string?> GetLessonGradeAsync(string lessonId, string token, CancellationToken cancellationToken)
        {
            var metadata = await GetLessonMetadataAsync(lessonId, token, cancellationToken);
            return metadata?.Grade;
        }

        /// <summary>
        /// Returns the lesson's grade, language and subject in a single lookup
        /// </summary>
        /// <param name="lessonId">The lesson id</param>
        /// <param name="token"> The user token.</param>
        public async Task<LessonMetadata?> GetLessonMetadataAsync(string lessonId, string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(lessonId)) return null;

            // Serve from cache when we've already resolved this lesson.
            if (MetadataCache.TryGetValue(lessonId, out var cached))
            {
                return cached;
            }

            var baseUrl = AppConfiguration.Configuration["AppSettings:athenas_web_service_url"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                _logger.LogWarning(
                    "AppSettings:athenas_web_service_url is not configured; lesson {LessonId} cannot be resolved and the tutor will fall back to the RAG engine.",
                    lessonId);
                return null;
            }

            var endpoint = $"{baseUrl.TrimEnd('/')}/api/lessons/lesson";

            var payload = JsonSerializer.Serialize(new
            {
                LessonId = lessonId,
                IsForOutsideSource = false,
                AddQuizDetails = false
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Token", token);

            try
            {
                using var response = await HttpClient.SendAsync(request, cancellationToken);

                var body = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Athenas returned {StatusCode} resolving lesson {LessonId}. Body: {Body}",
                        (int)response.StatusCode, lessonId, Truncate(body));
                    return null;
                }

                using var doc = JsonDocument.Parse(body);

                if (doc.RootElement.TryGetProperty("LessonModifierRequestModel", out var modifier) &&
                    modifier.ValueKind == JsonValueKind.Object &&
                    modifier.TryGetProperty("LessonModel", out var lessonModel) &&
                    lessonModel.ValueKind == JsonValueKind.Object)
                {
                    var metadata = new LessonMetadata(
                        ReadString(lessonModel, "LevelCode"),
                        ReadString(lessonModel, "SubjectCode"));

                    _logger.LogInformation(
                        "Athenas resolved lesson {LessonId}: LevelCode={Grade} SubjectCode={SubjectCode}",
                        lessonId,
                        metadata.Grade ?? "(null)",
                        metadata.SubjectCode ?? "(null)");

                    // An empty grade routes the tutor to RAG regardless of the configured grade list
                    if (string.IsNullOrWhiteSpace(metadata.Grade))
                    {
                        _logger.LogWarning(
                            "Athenas returned no LevelCode for lesson {LessonId}; the tutor will fall back to the RAG engine. Body: {Body}",
                            lessonId, Truncate(body));
                    }

                    // Cache only a real grade
                    if (!string.IsNullOrWhiteSpace(metadata.Grade))
                    {
                        MetadataCache[lessonId] = metadata;
                    }

                    return metadata;
                }

                // The call succeeded but the payload was not shaped as expected.
                _logger.LogWarning(
                    "Athenas response for lesson {LessonId} had no LessonModifierRequestModel.LessonModel; the tutor will fall back to the RAG engine. Body: {Body}",
                    lessonId, Truncate(body));

                return null;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                 _logger.LogWarning(ex, "Failed to resolve metadata for lesson {LessonId}", lessonId);
                return null;
            }
        }

        /// <summary>
        /// Truncate a response body so a diagnostic log line stays short.
        /// </summary>
        private static string Truncate(string? body)
        {
            if (string.IsNullOrEmpty(body)) return "(empty)";
            return body.Length <= MaxLoggedBodyChars ? body : body.Substring(0, MaxLoggedBodyChars) + "…";
        }

        /// <summary>
        /// Reads an optional string property, to avoid errors with missing or non string values.
        /// </summary>
        private static string? ReadString(JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
                ? property.GetString()
                : null;
    }
}
