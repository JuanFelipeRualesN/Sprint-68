namespace GSBaseAPI.GPTModule.Models
{
    public sealed class StudentReply
    {
        public string? ConversationId { get; set; }
        public string Language { get; init; } = "es";
        public string Message { get; init; } = "";
        public string? GeneratedQuizId { get; init; }
        public string? GeneratedQuizJson { get; init; }
    }
}
