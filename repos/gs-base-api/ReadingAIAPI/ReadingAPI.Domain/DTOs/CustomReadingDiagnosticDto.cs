namespace ReadingAPI.Domain.DTOs
{
    public class CustomReadingDiagnosticDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public string TeacherId { get; set; }
        public bool Published { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CustomReadingDiagnosticQuestionDto
    {
        public int Id { get; set; }
        public int CustomDiagnosticId { get; set; }
        public int QuestionId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
