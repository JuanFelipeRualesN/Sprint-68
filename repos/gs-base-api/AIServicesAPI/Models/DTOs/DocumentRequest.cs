namespace GSAIServicesAPI.Models.DTOs
{
    /// <summary>
    /// Represents the request payload for creating or partially updating a <c>TeacherDocument</c>.
    /// All properties are nullable — on a <b>POST</b> omitted fields fall back to their defaults,
    /// and on a <b>PATCH</b> omitted fields retain their current stored values.
    /// </summary>
    /// <remarks>
    /// <c>UserId</c> is intentionally ignored by the controller layer. Ownership is always resolved
    /// from the authenticated session token, never from this payload, to prevent user impersonation.
    /// </remarks>
    public class DocumentRequest
    {
        /// <summary>
        /// Optional client-supplied primary key for the document.
        /// When provided on a <b>POST</b>, this value is used as-is as the document's ID;
        /// when omitted, the controller generates a new <see cref="Guid"/>.
        /// Ignored on <b>PATCH</b> requests — the ID comes from the route parameter instead.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The display title of the document shown to the user in listings and headings.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The human-readable name of the AI tool that generated this document (e.g. "Lesson Planner").
        /// Used for display and filtering purposes on the client.
        /// </summary>
        public string? ToolTitle { get; set; }

        /// <summary>
        /// The category or subject area this document belongs to (e.g. "Mathematics", "Science").
        /// Used to group and filter documents in the UI.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// The document kind or output type produced by the AI tool (e.g. "Rubric", "Exam", "Summary").
        /// Allows the client to apply different rendering or export strategies per type.
        /// </summary>
        public string? Kind { get; set; }

        /// <summary>
        /// The AI model identifier used to generate the document's content (e.g. "claude-sonnet-4-6").
        /// Stored for auditing and reproducibility — lets the team know which model version produced a given output.
        /// </summary>
        public string? Model { get; set; }

        /// <summary>
        /// The full generated content of the document, typically HTML or plain text,
        /// as returned by the AI model and displayed to the teacher.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The prompt that was submitted to the AI model to produce this document.
        /// Stored alongside the content so the output can be audited, refined, or regenerated later.
        /// </summary>
        public string? Prompt { get; set; }
    }
}