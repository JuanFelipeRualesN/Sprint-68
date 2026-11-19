namespace GSBaseAPI.GPTModule.Models
{
    public sealed class ChatResponseDto
    {
        public string? ConversationId { get; set; }
        public string Response { get; set; } = "";
        public string Language { get; set; } = "es";
        public string? GeneratedQuizId { get; set; }

        // Optional auditing
        public string? ClientPrincipalId { get; set; }
        public string? ClientPrincipalName { get; set; }
    }
}
