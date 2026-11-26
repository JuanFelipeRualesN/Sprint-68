namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.External
{
    /// <summary>
    /// Minimal, read-only mapping of the existing usqui_user_quiz_results table (owned by the existing DAOs): one row is
    /// one quiz attempt of a user. Excluded from migrations; only mapped so
    /// custom_plan_client_lesson_exam_dates and custom_plan_client_lesson_tests can have a real FK to it, and so the
    /// service can check that a result belongs to the student and the quiz, and read the scores.
    /// Read-only: the custom plan services never insert or update rows of this table.
    /// </summary>
    public class UserQuizResultRef
    {
        /// <summary>Primary key (signed INT in the existing table).</summary>
        public int Id { get; set; }

        /// <summary>ul_user_accounts.id of the student who took the quiz.</summary>
        public int? UserId { get; set; }

        /// <summary>Athenas quiz id.</summary>
        public int? QuizId { get; set; }

        /// <summary>Attempt number of the student at this quiz.</summary>
        public int? TryNo { get; set; }

        /// <summary>Quiz kind (Athenas IsExam): 1 = exam, 2 = pre-test, 3 = post-test. Null in older (legacy) exam rows, which are accepted as exams. Custom plans never read practice results from this table.</summary>
        public int? IsExam { get; set; }

        /// <summary>Athenas lesson id.</summary>
        public int? LessonId { get; set; }

        /// <summary>Correct answers of the attempt.</summary>
        public int? TotalCorrect { get; set; }

        /// <summary>Questions of the attempt.</summary>
        public int? TotalQuestions { get; set; }

        /// <summary>Score of the attempt, 0–100 (FLOAT).</summary>
        public float? TotalPercent { get; set; }

        /// <summary>When the quiz was taken (TIMESTAMP, UTC).</summary>
        public DateTime? CreatedAt { get; set; }
    }
}
