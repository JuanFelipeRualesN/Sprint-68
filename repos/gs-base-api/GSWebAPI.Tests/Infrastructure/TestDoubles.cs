using System.Collections.Concurrent;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;

namespace GSWebAPI.Tests.Infrastructure
{
    /// <summary>Controllable clock.</summary>
    public sealed class TestTimeProvider : TimeProvider
    {
        private DateTimeOffset _now = new(2026, 11, 5, 15, 30, 0, TimeSpan.Zero);

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now = _now.Add(by);
    }

    /// <summary>Users API stand-in: which students each client can manage, and display names.</summary>
    public sealed class FakeUserDirectory : IUserDirectoryClient
    {
        private readonly ConcurrentDictionary<ulong, HashSet<ulong>> _studentsByClient = new();

        public bool Available { get; set; } = true;

        public void AddStudent(ulong clientId, ulong studentId) =>
            _studentsByClient.GetOrAdd(clientId, _ => new HashSet<ulong>()).Add(studentId);

        public Task<IReadOnlyDictionary<ulong, string>> GetFullNamesAsync(RequestUser user, IEnumerable<ulong> userIds, CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<ulong, string> names = userIds.Distinct().ToDictionary(id => id, id => $"User {id}");
            return Task.FromResult(names);
        }

        public Task<bool?> IsStudentOfClientAsync(RequestUser client, ulong studentId, CancellationToken cancellationToken = default)
        {
            if (!Available)
            {
                return Task.FromResult<bool?>(null);
            }

            return Task.FromResult<bool?>(_studentsByClient.TryGetValue(client.UserId, out var students) && students.Contains(studentId));
        }

        public Task<IReadOnlyList<StudentProfile>?> GetStudentsOfClientAsync(RequestUser client, CancellationToken cancellationToken = default)
        {
            if (!Available)
            {
                return Task.FromResult<IReadOnlyList<StudentProfile>?>(null);
            }

            IReadOnlyList<StudentProfile> students = _studentsByClient.TryGetValue(client.UserId, out var ids)
                ? ids.OrderBy(id => id).Select(id => new StudentProfile(id, $"User {id}")).ToList()
                : new List<StudentProfile>();
            return Task.FromResult<IReadOnlyList<StudentProfile>?>(students);
        }
    }

    /// <summary>Athenas stand-in: lesson titles, subjects, levels and standards built from the ids.</summary>
    public sealed class FakeAthenasCatalog : IAthenasCatalogClient
    {
        public bool Available { get; set; } = true;

        public Task<IReadOnlyDictionary<ulong, AthenasLessonInfo>> GetLessonsAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default)
        {
            var ids = lessonIds.Distinct().ToList();
            ids.ForEach(LessonLookups.Enqueue);
            return Task.FromResult<IReadOnlyDictionary<ulong, AthenasLessonInfo>>(Available
                ? ids.ToDictionary(id => id, id => new AthenasLessonInfo($"Lesson {id}", $"L{id}", "MAT-EN", "3"))
                : new Dictionary<ulong, AthenasLessonInfo>());
        }

        public Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetSubjectsAsync(string? langCode, CancellationToken cancellationToken = default) =>
            Catalog(new() { [10] = new("MAT-EN", "Math"), [11] = new("SCI-EN", "Science") });

        public Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetLevelsAsync(string? langCode, CancellationToken cancellationToken = default) =>
            Catalog(new() { [3] = new("3", "Third grade") });

        /// <summary>Delay of the standards lookup, to play a slow Athenas.</summary>
        public TimeSpan StandardsDelay { get; set; } = TimeSpan.Zero;

        public async Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> GetStandardsAsync(IEnumerable<ulong> standardIds, string? langCode, CancellationToken cancellationToken = default)
        {
            var ids = standardIds.Distinct().ToList();
            if (StandardsDelay > TimeSpan.Zero)
            {
                await Task.Delay(StandardsDelay, CancellationToken.None);
            }

            return await Catalog(ids.ToDictionary(id => id, id => new AthenasCatalogEntry($"STD-{id}", $"Standard {id}")));
        }

        /// <summary>
        /// Quizzes of a test lesson: practices lessonId*10 + 1..3, pre-test lessonId*10 + 8, post-test lessonId*10 + 9,
        /// and exam 900 for lesson 501 only.
        /// </summary>
        public static Dictionary<ulong, AthenasQuizInfo> QuizzesOf(ulong lessonId)
        {
            var quizzes = new Dictionary<ulong, AthenasQuizInfo>();
            for (ulong n = 1; n <= 3; n++)
            {
                quizzes[lessonId * 10 + n] = new AthenasQuizInfo($"Quiz {lessonId * 10 + n}", n.ToString(), AthenasQuizInfo.Practice);
            }

            quizzes[lessonId * 10 + 8] = new AthenasQuizInfo($"Quiz {lessonId * 10 + 8}", "1", AthenasQuizInfo.PreTest);
            quizzes[lessonId * 10 + 9] = new AthenasQuizInfo($"Quiz {lessonId * 10 + 9}", "3", AthenasQuizInfo.PostTest);
            if (lessonId == 501)
            {
                quizzes[900] = new AthenasQuizInfo("Quiz 900", "2", AthenasQuizInfo.Exam);
            }

            return quizzes;
        }

        /// <summary>No cache in the fake; kept so tests read the same as with the real client.</summary>
        public void ClearCache()
        {
        }

        public Task<IReadOnlyDictionary<ulong, AthenasQuizInfo>> GetLessonQuizzesAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default)
        {
            var quizzes = new Dictionary<ulong, AthenasQuizInfo>();
            if (Available)
            {
                foreach (var (quizId, info) in lessonIds.Distinct().SelectMany(QuizzesOf))
                {
                    quizzes[quizId] = info;
                }
            }

            return Task.FromResult<IReadOnlyDictionary<ulong, AthenasQuizInfo>>(quizzes);
        }

        /// <summary>Lesson ids whose quizzes were requested (single or batch lookup), to check what callers ask Athenas for.</summary>
        public ConcurrentQueue<ulong> QuizLookups { get; } = new();

        /// <summary>Lesson ids whose details (titles) were requested.</summary>
        public ConcurrentQueue<ulong> LessonLookups { get; } = new();

        public Task<IReadOnlyDictionary<ulong, AthenasQuizInfo>?> TryGetLessonQuizzesAsync(ulong lessonId, CancellationToken cancellationToken = default)
        {
            QuizLookups.Enqueue(lessonId);
            return Task.FromResult<IReadOnlyDictionary<ulong, AthenasQuizInfo>?>(Available ? QuizzesOf(lessonId) : null);
        }

        public async Task<IReadOnlyDictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>> TryGetLessonQuizzesAsync(IEnumerable<ulong> lessonIds, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>();
            foreach (var lessonId in lessonIds.Distinct())
            {
                result[lessonId] = await TryGetLessonQuizzesAsync(lessonId, cancellationToken);
            }

            return result;
        }

        private Task<IReadOnlyDictionary<ulong, AthenasCatalogEntry>> Catalog(Dictionary<ulong, AthenasCatalogEntry> entries) =>
            Task.FromResult<IReadOnlyDictionary<ulong, AthenasCatalogEntry>>(Available ? entries : new Dictionary<ulong, AthenasCatalogEntry>());
    }

    /// <summary>Blob Storage stand-in: keeps the uploaded bytes in memory.</summary>
    public sealed class FakeBlobStorage : IBlobFileStorage
    {
        public ConcurrentDictionary<string, (byte[] Bytes, string ContentType)> Blobs { get; } = new();

        public bool Available { get; set; } = true;

        public async Task<string?> UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
        {
            if (!Available)
            {
                return null;
            }

            using var copy = new MemoryStream();
            await content.CopyToAsync(copy, cancellationToken);
            Blobs[blobName] = (copy.ToArray(), contentType);
            return $"https://storage.test/gsweb-api/{blobName}";
        }

        public string GetReadUrl(string blobName) => $"https://storage.test/gsweb-api/{blobName}?sas=read";
    }

    public static class TestUsers
    {
        private static long _next = 100_000;

        /// <summary>A new user id, unique in the test run.</summary>
        public static ulong NewId() => (ulong)Interlocked.Increment(ref _next);

        public static RequestUser Client(ulong id) => new() { UserId = id, RoleId = "1", Token = "test" };

        public static RequestUser User(ulong id, bool isStaff = false) => new() { UserId = id, RoleId = "1", IsStaff = isStaff, Token = "test" };
    }
}
