namespace GSBaseAPI.GPTModule.Infrastructure
{
    using Dapper;
    using GSBaseAPI.GPTModule.Models;
    using MySqlConnector;

    public static class AiConsultingConnections
    {
        /// <summary>
        /// Retrieves lesson consulting data before AI usage (DEPR compliance).
        /// </summary>
        /// <param name="lessonId">Optional: Filter by lesson id.</param>
        /// <param name="subjectName">Optional: Filter by subject.</param>
        /// <param name="lessonLevel">Optional: Filter by level.</param>
        /// <returns>List of consulting lessons.</returns>
        public static List<AiConsultingLessonModel> GetLessonConsultingData(
            string lessonId = null,
            string subjectName = null,
            string lessonLevel = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            DynamicParameters parameters = new DynamicParameters();

            string query = @"
        SELECT 
            lesson_id,
            lesson_title,
            subject_name,
            lesson_level,
            lesson_description,
            concept,
            all_examples,
            standard_codes,
            standard_descriptions,
            level_codes,
            level_descriptions,
            expectation_codes,
            expectation_descriptions,
            other_codes,
            other_descriptions,
            all_constructed_full_codes,
            units,
            units_descriptions,
            transversal_themes,
            strategies,
            objectives
        FROM athenasv2_db_prod_v2.ai_consulting_table_final";

            bool whereUsed = false;

            if (!string.IsNullOrWhiteSpace(lessonId))
            {
                query += whereUsed ? " AND lesson_id = @lessonId" : " WHERE lesson_id = @lessonId";
                parameters.Add("@lessonId", lessonId);
                whereUsed = true;
            }

            if (!string.IsNullOrWhiteSpace(subjectName))
            {
                query += whereUsed ? " AND subject_name = @subjectName" : " WHERE subject_name = @subjectName";
                parameters.Add("@subjectName", subjectName);
                whereUsed = true;
            }

            if (!string.IsNullOrWhiteSpace(lessonLevel))
            {
                query += whereUsed ? " AND lesson_level = @lessonLevel" : " WHERE lesson_level = @lessonLevel";
                parameters.Add("@lessonLevel", lessonLevel);
                whereUsed = true;
            }

            using var connection = new MySqlConnection(AppConfiguration.Configuration["ConnectionStrings:AthenasDBConnectionStringReader"]);

            return connection.Query<AiConsultingLessonModel>(query, parameters).ToList();
        }

        public static List<AiConsultingLessonModel> SearchLessonsByKeywords(List<string> keywords, int limit = 6)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            if (keywords == null || keywords.Count == 0)
                return new List<AiConsultingLessonModel>();

            var whereParts = new List<string>();
            var parameters = new DynamicParameters();

            for (int i = 0; i < keywords.Count; i++)
            {
                var p = $"@q{i}";
                parameters.Add(p, keywords[i]);

                whereParts.Add($@"
lesson_title LIKE CONCAT('%', {p}, '%')
OR subject_name LIKE CONCAT('%', {p}, '%')
OR lesson_description LIKE CONCAT('%', {p}, '%')
OR concept LIKE CONCAT('%', {p}, '%')
OR all_examples LIKE CONCAT('%', {p}, '%')
OR standard_descriptions LIKE CONCAT('%', {p}, '%')
OR expectation_descriptions LIKE CONCAT('%', {p}, '%')
OR objectives LIKE CONCAT('%', {p}, '%')
");
            }

            var query = $@"
SELECT 
    lesson_id,
    lesson_title,
    subject_name,
    lesson_level,
    lesson_description,
    concept,
    all_examples,
    standard_codes,
    standard_descriptions,
    level_codes,
    level_descriptions,
    expectation_codes,
    expectation_descriptions,
    other_codes,
    other_descriptions,
    all_constructed_full_codes,
    units,
    units_descriptions,
    transversal_themes,
    strategies,
    objectives
FROM athenasv2_db_prod_v2.ai_consulting_table_final
WHERE ({string.Join(") OR (", whereParts)})
LIMIT @limit;";

            parameters.Add("@limit", limit);

            using var connection = new MySqlConnection(AppConfiguration.Configuration["ConnectionStrings:AthenasDBConnectionStringReader"]);
            return connection.Query<AiConsultingLessonModel>(query, parameters).ToList();
        }
    }
}
