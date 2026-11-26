namespace ReadingAPI.Domain.DTOs
{
    public class ReadingLessonDiagnosticQuestionRow
    {
        public string Question { get; set; }
        public string Response { get; set; }
        public bool Correct { get; set; }
        public string ConstructedFullCode { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
    }

    public class DiagnosticResponseDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class ReadingDiagnosticQuestionDto
    {
        public string Question { get; set; }
        public string ConstructedFullCode { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public List<DiagnosticResponseDto> Responses { get; set; } = new List<DiagnosticResponseDto>();
    }
}
