using GSAIServicesAPI.Models.DTOs;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// Engine-agnostic input for a single tutor turn. The TutorController builds one of these from the incoming request and hands it 
    /// to whichever engine it picks for the lesson's grade. 
    /// Both engines stream their answer back in the same SSE shape, so the client never needs to know which engine produced it.
    /// </summary>
    public class TutorStreamRequest
    {
        /// <summary>
        /// The conversation history. The last entry is the question the tutor must answer.
        /// </summary>
        public List<ChatMessageDto> Messages { get; set; } = new();

        /// <summary>
        /// The lesson the student is currently in. Grounds the answer and (for the
        /// generative engine) feeds the system prompt.
        /// </summary>
        public LessonContextDto? LessonContext { get; set; }

        /// <summary>
        /// The authenticated session. Engines that call downstream services (RAG orchestrator) use the user id / token.
        /// </summary>
        public FeignAPIs.Models.ValidationResponseModel Session { get; set; } = null!;

        /// <summary>
        /// Conversation id for the RAG engine. Passed to the orchestrator so it
        /// keeps the context
        /// </summary>
        public string? ConversationId { get; set; }

        /// <summary>
        /// Whether the caller acts as a student, resolved from the session role by the controller.
        /// Gates the safety classification: only a student's prompt is classified.
        /// </summary>
        public bool IsStudent { get; set; }

        /// <summary>
        /// The language the lesson is taught in, resolved from Athenas by the
        /// controller. The tutor answers in this language whatever language the student
        /// writes in, so an English lesson is not answered in Spanish. When it is
        /// "Unknown" the engines fall back to following the student.
        /// </summary>
        public LessonLanguage LessonLanguage { get; set; } = LessonLanguage.Unknown;
    }
}
