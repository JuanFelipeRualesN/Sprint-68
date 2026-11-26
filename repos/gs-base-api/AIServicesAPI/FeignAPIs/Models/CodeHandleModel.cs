namespace GSAIServicesAPI.FeignAPIs.Models
{
    /// <summary>
    /// Represents a code entry with its associated metadata, subjects, and levels.
    /// </summary>
    public class CodeHandleModel : BaseModel
    {
        /// <summary>The unique identifier of this code.</summary>
        public string Id { get; set; }

        /// <summary>The short code string (e.g. "CS101").</summary>
        public string Code { get; set; }

        /// <summary>Human-readable description of the code.</summary>
        public string Description { get; set; }

        /// <summary>The display name of the code type this code belongs to.</summary>
        public string CodeTypeName { get; set; }

        /// <summary>Foreign key referencing the code's type.</summary>
        public string TypeId { get; set; }

        /// <summary>Foreign key to a related code, if any.</summary>
        public string RelationCodeId { get; set; }

        /// <summary>Subjects associated with this code.</summary>
        public List<CodeSubjectModel> CodeSubjectModels { get; set; }

        /// <summary>Levels associated with this code.</summary>
        public List<CodeLevelModel> CodeLevelModels { get; set; }

        /// <inheritdoc/>
        public string ModifiedBy { get; set; }

        /// <inheritdoc/>
        public string CreatedAt { get; set; }

        /// <inheritdoc/>
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the translations of the code.
        /// </summary>
        public List<CodeTranslationModel> CodeTranslationModels { get; set; }

    }

    /// <summary>
    /// Represents a translation for a code.
    /// </summary>
    public class CodeTranslationModel
    {
        /// <summary>
        /// Gets or sets the translation identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the associated code identifier.
        /// </summary>
        public int CodeId { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// Gets or sets the translated description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the user who last modified the translation.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last update date and time.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents a subject linked to a code entry.
    /// </summary>
    public class CodeSubjectModel : BaseModel
    {
        /// <summary>The unique identifier of this subject link.</summary>
        public string Id { get; set; }

        /// <summary>Foreign key referencing the parent code.</summary>
        public string CodeId { get; set; }

        /// <summary>The subject's short code (e.g. "MATH").</summary>
        public string SubjectCode { get; set; }

        /// <summary>The subject's display name. Populated manually for enriched queries.</summary>
        public string SubjectName { get; set; }

        /// <inheritdoc/>
        public string ModifiedBy { get; set; }

        /// <inheritdoc/>
        public string CreatedAt { get; set; }

        /// <inheritdoc/>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents a level linked to a code entry.
    /// </summary>
    public class CodeLevelModel : BaseModel
    {
        /// <summary>The unique identifier of this level link.</summary>
        public string Id { get; set; }

        /// <summary>Foreign key referencing the parent code.</summary>
        public string CodeId { get; set; }

        /// <summary>The level's short code (e.g. "ADV").</summary>
        public string LevelCode { get; set; }

        /// <summary>The level's display name. Populated manually for enriched queries.</summary>
        public string LevelName { get; set; }

        /// <inheritdoc/>
        public string ModifiedBy { get; set; }

        /// <inheritdoc/>
        public string CreatedAt { get; set; }

        /// <inheritdoc/>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Shared validation fields inherited by all code-related models.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>Indicates whether this model passed server-side validation.</summary>
        public bool Valid { get; set; }

        /// <summary>Validation or error messages returned from the server.</summary>
        public List<string> Messages { get; set; }
    }
}