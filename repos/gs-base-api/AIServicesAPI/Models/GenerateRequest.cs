using GSAIServicesAPI.Models.DTOs;

namespace GSAIServicesAPI.Models
{
    /// <summary>
    /// Request payload for streaming AI chat generation.
    /// The client sends the conversation history and optional lesson context;
    /// the backend builds the system prompt (rules + context)
    /// Conversation Id is used for the RAG engine to maintain context across multiple requests.
    /// </summary>
    public record GenerateRequest(
        string Model,
        string? System,
        string? User,
        List<ChatMessageDto>? Messages,
        LessonContextDto? LessonContext,
        double? Temperature,
        int? MaxTokens,
        string? ConversationId = null
    );
}
