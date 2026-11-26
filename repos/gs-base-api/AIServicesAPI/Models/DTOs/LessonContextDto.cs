namespace GSAIServicesAPI.Models.DTOs
{
    /// <summary>
    /// Lesson context sent by the client so the backend can build a grounded system prompt
    /// </summary>
    public class LessonContextDto
    {
        public string? Subject { get; set; }
        public string? Title { get; set; }
        public string? Level { get; set; }
        public string? Concept { get; set; }
        public string? Vocabulary { get; set; }
        public string? Examples { get; set; }
        public string? LessonId { get; set; }
        public string? SubjectCode { get; set; }
    }
}
