namespace GSAIServicesAPI.Models.DTOs
{
    /// <summary>
    /// Payload for reading/writing the AI tutor routing configuration
    /// RagGrades is the list of grades that should be answered by the RAG engine; every other grade uses the generative engine.
    /// </summary>
    public class TutorSettingsDto
    {
        /// <summary>
        /// Grades routed to the RAG engine
        /// </summary>
        public List<string> RagGrades { get; set; } = new();
    }
}
