namespace GSBaseAPI.Domain.DTOs.Chatbot
{
    public class ChatRequestDto
    {
        /// <summary>
        /// The conversation id that is stored in Cosmos DB to follow the conversation
        /// </summary>
        public string ConversationId { get; set; }

        /// <summary>
        /// The user query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The module used by the RAG process to determine the corresponding instructions (e.g., lessons, assignments, etc.).
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// The profile used by the RAG process to determine the corresponding instructions (e.g., teachers, students, etc.).
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Parameter to create a quick or a detailed planning
        /// </summary>
        public string DetailedPlanning { get; set; }
    }
}
