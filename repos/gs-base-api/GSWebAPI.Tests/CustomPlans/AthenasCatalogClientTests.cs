using System.Collections.Concurrent;
using System.Net;
using System.Text;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace GSWebAPI.Tests.CustomPlans
{
    /// <summary>Unit tests (no network) for the Athenas calls: endpoints, headers, parsing, cache and failure handling.</summary>
    public sealed class AthenasCatalogClientTests
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AppSettings:athenas_service_url"] = "https://athenas.test/",
            ["AppSettings:AthenasWebServiceApiAccessKey"] = "key-123",
            ["AppSettings:athenas_api_lessons_models_read"] = "api/lessons/by-lesson-models/false",
            ["AppSettings:athenas_web_api_get_subjects"] = "api/subjects/",
            ["AppSettings:athenas_web_api_get_grades"] = "api/grades/",
            ["AppSettings:athenas_web_api_get_codes"] = "api/codes/query",
            ["AppSettings:athenas_api_get_lesson_quizzes"] = "api/lessons/{0}/quizzes",
        }).Build();

        private static (AthenasCatalogClient Client, StubHandler Handler) Create(Func<HttpRequestMessage, string, (HttpStatusCode, string)> respond)
        {
            var handler = new StubHandler(respond);
            var client = new AthenasCatalogClient(new HttpClient(handler), Configuration, new MemoryCache(new MemoryCacheOptions()), NullLogger<AthenasCatalogClient>.Instance);
            return (client, handler);
        }

        [Fact]
        public async Task Lessons_are_read_in_one_call_and_cached()
        {
            var (client, handler) = Create((_, _) => (HttpStatusCode.OK,
                "{\"LessonModelList\":[{\"Id\":\"501\",\"LessonNo\":\"1.2\",\"LessonTitle\":\"Fractions\",\"SubjectCode\":\"MAT-EN\",\"LevelCode\":\"3\"}]}"));

            var lessons = await client.GetLessonsAsync(new[] { 501UL, 502UL });
            var again = await client.GetLessonsAsync(new[] { 501UL });

            Assert.Equal(new AthenasLessonInfo("Fractions", "1.2", "MAT-EN", "3"), lessons[501]);
            Assert.False(lessons.ContainsKey(502));
            Assert.Equal("Fractions", again[501].Title);
            var call = Assert.Single(handler.Calls);
            Assert.Equal("https://athenas.test/api/lessons/by-lesson-models/false", call.Url);
            Assert.Equal("key-123", call.ApiKey);
            Assert.Contains("\"Id\":\"501\"", call.Body);
            Assert.Contains("\"Id\":\"502\"", call.Body);
        }

        [Fact]
        public async Task Subjects_levels_and_standards_are_parsed()
        {
            var (client, handler) = Create((request, body) => request.RequestUri!.AbsolutePath switch
            {
                "/api/subjects/" => (HttpStatusCode.OK, Subjects),
                "/api/grades/" => (HttpStatusCode.OK, "[{\"Id\":\"3\",\"Code\":\"3\",\"Name\":\"Tercer grado\"}]"),
                _ => (HttpStatusCode.OK, body.Contains("mat-sp")
                    ? "{\"Total\":1,\"TotalPages\":1,\"Items\":[{\"Id\":\"77\",\"Code\":\"PR.3.N.1\",\"Description\":\"Números\"}]}"
                    : "{\"Total\":1,\"TotalPages\":1,\"Items\":[{\"Id\":\"77\",\"Code\":\"PR.3.N.1\",\"Description\":\"Numbers\"}]}"),
            });

            Assert.Equal(new AthenasCatalogEntry("mat-sp", "Matemáticas"), (await client.GetSubjectsAsync("es"))[4]);
            Assert.Equal("Tercer grado", (await client.GetLevelsAsync("es"))[3].Name);
            Assert.Equal(new AthenasCatalogEntry("PR.3.N.1", "Números"), (await client.GetStandardsAsync(new[] { 77UL }, "es"))[77]);
            Assert.All(handler.Calls, c => Assert.Equal("es", c.Language));
            Assert.Contains(handler.Calls, c => c.Url.StartsWith("https://athenas.test/api/codes/query?") && c.Body.Contains("\"TypeId\":\"1\""));
        }

        // Athenas subjects: two in English, two in Spanish.
        private const string Subjects = "[{\"Id\":\"76\",\"Code\":\"mat-en\",\"Name\":\"Math\",\"LangCode\":\"en\"},"
            + "{\"Id\":\"1\",\"Code\":\"en\",\"Name\":\"English\",\"LangCode\":\"en\"},"
            + "{\"Id\":\"4\",\"Code\":\"mat-sp\",\"Name\":\"Matemáticas\",\"LangCode\":\"es\"},"
            + "{\"Id\":\"2\",\"Code\":\"sp\",\"Name\":\"Español\",\"LangCode\":\"es\"}]";

        [Theory]
        [InlineData("es", "Álgebra")]
        [InlineData("es-PR", "Álgebra")]
        [InlineData("en", "Algebra")]
        [InlineData(null, "Algebra")]
        public async Task Standards_are_translated_through_the_subjects_of_the_requested_language(string? lang, string expected)
        {
            // Athenas translates a code by the language of the subject it is filtered by, not by Accept-Language;
            // unfiltered, the stored description is returned. Standard 16 has no English subject.
            string? translatedBody = null;
            var (client, _) = Create((request, body) =>
            {
                if (request.RequestUri!.AbsolutePath == "/api/subjects/")
                {
                    return (HttpStatusCode.OK, Subjects);
                }

                if (body.Contains("CodeSubjectModels\":["))
                {
                    translatedBody = body;
                    return (HttpStatusCode.OK, body.Contains("mat-en")
                        ? "{\"TotalPages\":1,\"Items\":[{\"Id\":\"3930\",\"Code\":\"A\",\"Description\":\"Algebra\"}]}"
                        : "{\"TotalPages\":1,\"Items\":[{\"Id\":\"3930\",\"Code\":\"A\",\"Description\":\"Álgebra\"},{\"Id\":\"16\",\"Code\":\"AO\",\"Description\":\"Comprensión oral\"}]}");
                }

                return (HttpStatusCode.OK, "{\"TotalPages\":1,\"Items\":[{\"Id\":\"3930\",\"Code\":\"A\",\"Description\":\"Álgebra\"},{\"Id\":\"16\",\"Code\":\"AO\",\"Description\":\"Comprensión oral\"}]}");
            });

            var standards = await client.GetStandardsAsync(new[] { 3930UL, 16UL }, lang);

            Assert.Equal(new AthenasCatalogEntry("A", expected), standards[3930]);
            Assert.Equal("Comprensión oral", standards[16].Name); // stored description when there is no translation
            var spanish = lang?.StartsWith("es") == true;
            Assert.Contains(spanish ? "\"SubjectCode\":\"mat-sp\"" : "\"SubjectCode\":\"mat-en\"", translatedBody);
            Assert.DoesNotContain(spanish ? "mat-en" : "mat-sp", translatedBody);
        }

        [Fact]
        public async Task A_failed_translation_is_not_cached()
        {
            var failTranslation = true;
            var (client, handler) = Create((request, body) =>
                request.RequestUri!.AbsolutePath == "/api/subjects/" ? (HttpStatusCode.OK, Subjects)
                : body.Contains("CodeSubjectModels\":[") && failTranslation ? (HttpStatusCode.ServiceUnavailable, "")
                : (HttpStatusCode.OK, "{\"TotalPages\":1,\"Items\":[{\"Id\":\"3930\",\"Code\":\"A\",\"Description\":\"Algebra\"}]}"));

            Assert.Equal("A", (await client.GetStandardsAsync(new[] { 3930UL }, "es"))[3930].Code); // stored description meanwhile
            failTranslation = false;
            await client.GetStandardsAsync(new[] { 3930UL }, "es");
            var queries = handler.Calls.Count(c => c.Url.Contains("codes/query"));
            await client.GetStandardsAsync(new[] { 3930UL }, "es");

            Assert.Equal(4, queries); // read again after the failure
            Assert.Equal(queries, handler.Calls.Count(c => c.Url.Contains("codes/query"))); // then cached
        }

        [Fact]
        public async Task Standards_come_from_the_standards_catalog_read_page_by_page_once()
        {
            // codes/query ignores Id: the client must filter by type and look the ids up itself.
            var (client, handler) = Create((request, _) => (HttpStatusCode.OK,
                request.RequestUri!.AbsolutePath == "/api/subjects/" ? Subjects
                : request.RequestUri!.Query.Contains("fromPage=0")
                    ? "{\"Total\":3,\"TotalPages\":2,\"Items\":[{\"Id\":\"1\",\"Code\":\"PK\"},{\"Id\":\"16\",\"Code\":\"N\"}]}"
                    : "{\"Total\":3,\"TotalPages\":2,\"Items\":[{\"Id\":\"3930\",\"Code\":\"A\",\"Description\":\"Álgebra\"}]}"));

            var standards = await client.GetStandardsAsync(new[] { 3930UL, 16UL, 999UL }, "en");
            var again = await client.GetStandardsAsync(new[] { 3930UL }, "en");

            Assert.Equal(new AthenasCatalogEntry("A", "Álgebra"), standards[3930]);
            Assert.Equal("N", standards[16].Code);
            Assert.False(standards.ContainsKey(999));
            Assert.Equal("A", again[3930].Code);
            var queries = handler.Calls.Where(c => c.Url.Contains("codes/query")).ToList();
            Assert.Equal(4, queries.Count); // two pages, without and with the language filter; then the cache
            Assert.All(queries, c => Assert.Contains("\"TypeId\":\"1\"", c.Body));
            Assert.Contains(queries, c => c.Url.Contains("fromPage=1"));
        }

        [Fact]
        public async Task Lesson_quizzes_give_practice_and_exam_titles()
        {
            var (client, handler) = Create((request, _) => (HttpStatusCode.OK, request.RequestUri!.AbsolutePath == "/api/lessons/501/quizzes"
                ? "{\"Quizzes\":[{\"Id\":\"5011\",\"Title\":\"Practice 1\",\"LevelOfKnowledge\":\"1\",\"IsExam\":\"0\"},{\"Id\":\"900\",\"Title\":\"Final\",\"IsExam\":\"1\"}]}"
                : "{\"Quizzes\":[]}"));

            var quizzes = await client.GetLessonQuizzesAsync(new[] { 501UL, 502UL });
            await client.GetLessonQuizzesAsync(new[] { 501UL });

            Assert.Equal(new AthenasQuizInfo("Practice 1", "1", "0"), quizzes[5011]);
            Assert.Equal("Final", quizzes[900].Title);
            Assert.Equal(2, handler.Calls.Count); // one per lesson, then cached
        }

        [Fact]
        public async Task The_lesson_quizzes_lookup_tells_a_failure_apart_from_an_empty_lesson()
        {
            var (down, _) = Create((_, _) => (HttpStatusCode.ServiceUnavailable, ""));
            Assert.Null(await down.TryGetLessonQuizzesAsync(501));

            var (empty, _) = Create((_, _) => (HttpStatusCode.OK, "{\"Quizzes\":[]}"));
            Assert.Empty((await empty.TryGetLessonQuizzesAsync(501))!);
        }

        [Fact]
        public async Task A_standard_missing_from_the_catalog_is_not_returned()
        {
            var (client, _) = Create((request, _) => (HttpStatusCode.OK,
                request.RequestUri!.AbsolutePath == "/api/subjects/" ? Subjects : "{\"Items\":[{\"Id\":\"1\",\"Code\":\"OTHER\"}]}"));

            Assert.Empty(await client.GetStandardsAsync(new[] { 77UL }, "en"));
        }

        [Fact]
        public async Task Failures_return_empty_results_instead_of_throwing()
        {
            var (client, _) = Create((_, _) => (HttpStatusCode.InternalServerError, "boom"));
            Assert.Empty(await client.GetLessonsAsync(new[] { 1UL }));
            Assert.Empty(await client.GetSubjectsAsync(null));

            var (broken, _) = Create((_, _) => throw new HttpRequestException("offline"));
            Assert.Empty(await broken.GetLevelsAsync("en"));
            Assert.Empty(await broken.GetStandardsAsync(new[] { 1UL }, "en"));
        }

        [Fact]
        public async Task Per_lesson_quiz_lookups_run_in_parallel_up_to_the_limit()
        {
            var inFlight = 0;
            var maxInFlight = 0;
            var handler = new StubHandler((_, _) =>
            {
                var now = Interlocked.Increment(ref inFlight);
                InterlockedMax(ref maxInFlight, now);
                Thread.Sleep(50);
                Interlocked.Decrement(ref inFlight);
                return (HttpStatusCode.OK, "{\"Quizzes\":[]}");
            });
            var client = new AthenasCatalogClient(new HttpClient(handler), Configuration, new MemoryCache(new MemoryCacheOptions()), NullLogger<AthenasCatalogClient>.Instance);
            var ids = Enumerable.Range(1, 20).Select(i => (ulong)i).ToList();

            var quizzes = await client.TryGetLessonQuizzesAsync(ids);

            Assert.Equal(20, quizzes.Count);
            Assert.All(quizzes.Values, q => Assert.NotNull(q));
            Assert.Equal(20, handler.Calls.Count);
            Assert.InRange(maxInFlight, 2, AthenasCatalogClient.MaxParallelCalls);
        }

        [Fact]
        public async Task Ids_athenas_does_not_return_are_not_requested_again_but_failures_are()
        {
            var (client, handler) = Create((request, _) => (HttpStatusCode.OK,
                request.RequestUri!.AbsolutePath.Contains("by-lesson-models") ? "{\"LessonModelList\":[]}"
                : request.RequestUri!.AbsolutePath == "/api/subjects/" ? Subjects
                : "{\"TotalPages\":1,\"Items\":[{\"Id\":\"5\",\"Code\":\"X\"}]}"));

            Assert.Empty(await client.GetLessonsAsync(new[] { 1UL }));
            Assert.Empty(await client.GetLessonsAsync(new[] { 1UL }));
            Assert.Empty(await client.GetStandardsAsync(new[] { 2UL }, "en"));
            Assert.Empty(await client.GetStandardsAsync(new[] { 2UL }, "en"));
            Assert.Equal(4, handler.Calls.Count); // lessons 1; subjects + standards with and without the filter 3; the second lookups were answered from the cache

            var (down, downHandler) = Create((_, _) => (HttpStatusCode.ServiceUnavailable, ""));
            await down.GetLessonsAsync(new[] { 1UL });
            await down.GetLessonsAsync(new[] { 1UL });
            await down.GetStandardsAsync(new[] { 2UL }, "en");
            await down.GetStandardsAsync(new[] { 2UL }, "en");
            Assert.Equal(4, downHandler.Calls.Count); // a failure is not cached
        }

        private static void InterlockedMax(ref int target, int value)
        {
            int current;
            while ((current = Volatile.Read(ref target)) < value && Interlocked.CompareExchange(ref target, value, current) != current)
            {
            }
        }

        private sealed class StubHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, string, (HttpStatusCode, string)> _respond;

            public StubHandler(Func<HttpRequestMessage, string, (HttpStatusCode, string)> respond)
            {
                _respond = respond;
            }

            // Thread-safe: the client sends per-id requests in parallel.
            public ConcurrentQueue<(string Url, string? ApiKey, string? Language, string Body)> Calls { get; } = new();

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var body = request.Content == null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken);
                Calls.Enqueue((request.RequestUri!.ToString(),
                    request.Headers.TryGetValues("X-API-KEY", out var key) ? key.Single() : null,
                    request.Headers.TryGetValues("Accept-Language", out var lang) ? lang.Single() : null,
                    body));
                var (status, content) = _respond(request, body);
                return new HttpResponseMessage(status) { Content = new StringContent(content, Encoding.UTF8, "application/json") };
            }
        }
    }
}
