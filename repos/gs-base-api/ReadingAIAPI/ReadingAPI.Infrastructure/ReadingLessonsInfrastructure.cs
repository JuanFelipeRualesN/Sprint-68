using Dapper;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;

namespace ReadingAPI.ReadingAPI.Infrastructure
{
    /// <summary>
    /// Resolves the Athenas lessons used by the reading lessons section.
    /// Only lessons tagged with the "R" or "LLI" standard codes, from kindergarten to fifth
    /// grade, are relevant, and their ids are the filter applied later over the quizes table.
    /// </summary>
    public class ReadingLessonsInfrastructure
    {
        private const string CacheKey = "reading:lessons";

        private const string TitlesCacheKey = "reading:lessons:titles";

        private const int CacheHours = 24;

        private const int StandardCodeTypeId = 1;

        private static readonly string[] ReadingStandardCodes = { "R", "LLI" };

        private static readonly string[] ReadingLevelCodes = { "k", "1", "2", "3", "4", "5" };

        public static readonly string[] ReadingSubjectCodes = { "adl", "sp", "en" };

        private static string ConnectionString => AppConfiguration.Configuration["ConnectionStrings:AthenasDBConnectionStringReader"];

        private readonly IMemoryCache _memoryCache;

        public ReadingLessonsInfrastructure(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Returns the ids of the Athenas lessons that carry an "R" or "LLI" standard code
        /// and belong to a kindergarten to fifth grade level.
        /// The match is done against the segment and its type to avoid including non standard codes.
        /// </summary>
        public async Task<List<int>> GetReadingLessonIds()
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<int>? cachedLessonIds) && cachedLessonIds != null)
                return cachedLessonIds;

            const string sql = @"
                SELECT DISTINCT cc.lesson_id
                FROM ls_constructed_codes cc
                INNER JOIN ls_constructed_code_codes seg ON seg.lesson_constructed_code_id = cc.id
                INNER JOIN ls_codes cod ON cod.id = seg.code_id
                INNER JOIN ls_lessons les ON les.id = cc.lesson_id
                WHERE cod.code IN @StandardCodes
                  AND cod.type_id = @StandardTypeId
                  AND les.level_code IN @LevelCodes
                  AND LOWER(les.subject_code) IN @SubjectCodes";

            using var connection = new MySqlConnection(ConnectionString);

            var lessonIds = (await connection.QueryAsync<int>(sql, new
            {
                StandardCodes = ReadingStandardCodes,
                StandardTypeId = StandardCodeTypeId,
                LevelCodes = ReadingLevelCodes,
                SubjectCodes = ReadingSubjectCodes,
            })).ToList();

            _memoryCache.Set(CacheKey, lessonIds, TimeSpan.FromHours(CacheHours));

            return lessonIds;
        }

        /// <summary>
        /// Returns the title of every reading lesson
        /// </summary>
        public async Task<Dictionary<int, string>> GetReadingLessonTitles()
        {
            if (_memoryCache.TryGetValue(TitlesCacheKey, out Dictionary<int, string>? cachedTitles) && cachedTitles != null)
                return cachedTitles;

            const string sql = @"
                SELECT DISTINCT cc.lesson_id AS LessonId,
                       les.lesson_title      AS Title
                FROM ls_constructed_codes cc
                INNER JOIN ls_constructed_code_codes seg ON seg.lesson_constructed_code_id = cc.id
                INNER JOIN ls_codes cod ON cod.id = seg.code_id
                INNER JOIN ls_lessons les ON les.id = cc.lesson_id
                WHERE cod.code IN @StandardCodes
                  AND cod.type_id = @StandardTypeId
                  AND les.level_code IN @LevelCodes
                  AND LOWER(les.subject_code) IN @SubjectCodes";

            using var connection = new MySqlConnection(ConnectionString);

            var rows = await connection.QueryAsync<LessonTitleRow>(sql, new
            {
                StandardCodes = ReadingStandardCodes,
                StandardTypeId = StandardCodeTypeId,
                LevelCodes = ReadingLevelCodes,
                SubjectCodes = ReadingSubjectCodes,
            });

            var titles = rows
                .GroupBy(row => row.LessonId)
                .ToDictionary(group => group.Key, group => group.First().Title ?? string.Empty);

            _memoryCache.Set(TitlesCacheKey, titles, TimeSpan.FromHours(CacheHours));

            return titles;
        }

        /// <summary>
        /// Row returned by the lesson titles query
        /// </summary>
        private sealed class LessonTitleRow
        {
            public int LessonId { get; set; }
            public string? Title { get; set; }
        }
    }
}
