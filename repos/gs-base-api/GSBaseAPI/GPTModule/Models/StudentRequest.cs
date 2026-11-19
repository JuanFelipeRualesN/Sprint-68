namespace GSBaseAPI.GPTModule.Models
{
    public class StudentRequest
    {
        public string? ConversationId { get; set; }
        public string StudentPrompt { get; init; } = "";
        public string LangCode { get; init; } = "es";           // platform language context for planning/attach
        public string GradeLevel { get; init; } = "3";               // required for grade-appropriate responses
        public string LessonId { get; init; } = "";             // your lesson key (DB)
        public string PlanningId { get; init; } = "";           // e.g., "1966"
        public string DueDate { get; init; } = "2026-03-04 00:00:00";
    }
}
