namespace GSAIServicesAPI.Models.DTOs
{
    /// <summary>
    /// A single chat message in the conversation history.
    /// </summary>
    public class ChatMessageDto
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }
}
