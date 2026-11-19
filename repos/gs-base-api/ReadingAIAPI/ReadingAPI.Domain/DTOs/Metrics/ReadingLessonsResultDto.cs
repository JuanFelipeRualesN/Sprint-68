namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// A single reading lesson evaluation taken by a student, as shown in the
    /// dashboard results table.
    /// </summary>
    public class ReadingLessonsResultDto
    {
        public int UserQuizResultId { get; set; }

        public int UserId { get; set; }

        public string? GivenName { get; set; }

        public string? Surname { get; set; }

        public string? Username { get; set; }

        public DateTime TakenAt { get; set; }

        public decimal TotalPercent { get; set; }

        public int LessonId { get; set; }

        public string? LessonTitle { get; set; }

        public string? Subject { get; set; }

        public string? LevelCode { get; set; }

        public int? ExamType { get; set; }
    }
}
