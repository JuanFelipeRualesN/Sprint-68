using Dapper;
using ReadingAPI.Domain.DTOs;
using MySqlConnector;
using ReadingAPI;

namespace ReadingAPI.Infrastructure
{
    public class CustomReadingDiagnosticInfrastructure
    {
        private static string ConnectionString => AppConfiguration.Configuration["ConnectionStrings:ReadingAIAPIDBConnectionReader"];

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        #region Diagnostics CRUD

        public int CreateDiagnostic(CustomReadingDiagnosticDto diagnostic)
        {
            const string sql = @"
            INSERT INTO custom_reading_diagnostics
                (title, subject_code, level_code, teacher_id, published, modified_by, created_at, updated_at)
            VALUES
                (@Title, @SubjectCode, @LevelCode, @TeacherId, @Published, @ModifiedBy, @CreatedAt, @UpdatedAt);
            SELECT LAST_INSERT_ID();";

            if (diagnostic.CreatedAt == default) diagnostic.CreatedAt = DateTime.UtcNow;
            if (diagnostic.UpdatedAt == default) diagnostic.UpdatedAt = DateTime.UtcNow;

            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    var id = connection.ExecuteScalar<int>(sql, diagnostic);
                    diagnostic.Id = id;
                    return id;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    connection.Close();
                    return 0;
                }
            }
        }

        public void UpdateDiagnostic(CustomReadingDiagnosticDto diagnostic)
        {
            const string sql = @"
            UPDATE custom_reading_diagnostics
            SET
                title        = @Title,
                subject_code = @SubjectCode,
                level_code   = @LevelCode,
                teacher_id   = @TeacherId,
                published    = @Published,
                modified_by  = @ModifiedBy,
                updated_at   = @UpdatedAt
            WHERE id = @Id;";

            diagnostic.UpdatedAt = DateTime.UtcNow;

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, diagnostic);
            }
        }

        public CustomReadingDiagnosticDto GetDiagnosticById(int id)
        {
            const string sql = @"
            SELECT
                id            AS Id,
                title         AS Title,
                subject_code  AS SubjectCode,
                level_code    AS LevelCode,
                teacher_id    AS TeacherId,
                published     AS Published,
                modified_by   AS ModifiedBy,
                created_at    AS CreatedAt,
                updated_at    AS UpdatedAt
            FROM custom_reading_diagnostics
            WHERE id = @Id;";

            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<CustomReadingDiagnosticDto>(sql, new { Id = id });
            }
        }

        public IEnumerable<CustomReadingDiagnosticDto> GetDiagnosticsByTeacher(string teacherId)
        {
            const string sql = @"
            SELECT
                id            AS Id,
                title         AS Title,
                subject_code  AS SubjectCode,
                level_code    AS LevelCode,
                teacher_id    AS TeacherId,
                published     AS Published,
                modified_by   AS ModifiedBy,
                created_at    AS CreatedAt,
                updated_at    AS UpdatedAt
            FROM custom_reading_diagnostics
            WHERE teacher_id = @TeacherId
            ORDER BY created_at DESC;";

            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.Query<CustomReadingDiagnosticDto>(sql, new { TeacherId = teacherId }).ToList();
            }
        }

        public void DeleteDiagnostic(int id)
        {
            const string sql = @"DELETE FROM custom_reading_diagnostics WHERE id = @Id;";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { Id = id });
            }
        }

        #endregion

        #region Diagnostic Questions CRUD

        public int CreateDiagnosticQuestion(CustomReadingDiagnosticQuestionDto question)
        {
            const string sql = @"
            INSERT INTO custom_reading_diagnostic_questions
                (custom_diagnostic_id, question_id, modified_by, created_at, updated_at)
            VALUES
                (@CustomDiagnosticId, @QuestionId, @ModifiedBy, @CreatedAt, @UpdatedAt);
            SELECT LAST_INSERT_ID();";

            if (question.CreatedAt == default) question.CreatedAt = DateTime.UtcNow;
            if (question.UpdatedAt == default) question.UpdatedAt = DateTime.UtcNow;

            using (var connection = GetConnection())
            {
                connection.Open();
                var id = connection.ExecuteScalar<int>(sql, question);
                question.Id = id;
                return id;
            }
        }

        public void CreateDiagnosticQuestionsBulk(IEnumerable<CustomReadingDiagnosticQuestionDto> questions)
        {
            const string sql = @"
            INSERT INTO custom_reading_diagnostic_questions
                (custom_diagnostic_id, question_id, modified_by, created_at, updated_at)
            VALUES
                (@CustomDiagnosticId, @QuestionId, @ModifiedBy, @CreatedAt, @UpdatedAt);";

            var now = DateTime.UtcNow;
            foreach (var q in questions)
            {
                if (q.CreatedAt == default) q.CreatedAt = now;
                q.UpdatedAt = now;
            }

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, questions);
            }
        }

        public IEnumerable<CustomReadingDiagnosticQuestionDto> GetQuestionsByDiagnosticId(int diagnosticId)
        {
            const string sql = @"
            SELECT
                id                   AS Id,
                custom_diagnostic_id AS CustomDiagnosticId,
                question_id          AS QuestionId,
                modified_by          AS ModifiedBy,
                created_at           AS CreatedAt,
                updated_at           AS UpdatedAt
            FROM custom_reading_diagnostic_questions
            WHERE custom_diagnostic_id = @CustomDiagnosticId
            ORDER BY created_at;";

            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.Query<CustomReadingDiagnosticQuestionDto>(
                    sql,
                    new { CustomDiagnosticId = diagnosticId }).ToList();
            }
        }

        public void DeleteQuestionsByDiagnosticId(int diagnosticId)
        {
            const string sql = @"
            DELETE FROM custom_reading_diagnostic_questions
            WHERE custom_diagnostic_id = @CustomDiagnosticId;";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { CustomDiagnosticId = diagnosticId });
            }
        }

        public void DeleteDiagnosticQuestion(int diagnosticId, int questionId)
        {
            const string sql = @"
            DELETE FROM custom_reading_diagnostic_questions
            WHERE custom_diagnostic_id = @CustomDiagnosticId
              AND question_id          = @QuestionId;";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { CustomDiagnosticId = diagnosticId, QuestionId = questionId });
            }
        }

        public void UpdateDiagnosticQuestion(CustomReadingDiagnosticQuestionDto question)
        {
            const string sql = @"
            UPDATE custom_reading_diagnostic_questions
            SET
                modified_by = @ModifiedBy,
                updated_at  = @UpdatedAt
            WHERE custom_diagnostic_id = @CustomDiagnosticId
              AND question_id          = @QuestionId;";

            question.UpdatedAt = DateTime.UtcNow;

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, question);
            }
        }

        #endregion
    }
}