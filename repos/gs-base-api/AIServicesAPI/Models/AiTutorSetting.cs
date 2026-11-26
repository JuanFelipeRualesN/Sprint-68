using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GSAIServicesAPI.Models
{
    /// <summary>
    /// EF Core entity mapping the ai_tutor_settings table. Holds the single
    /// global configuration row that decides which lesson grades are answered by the restricted RAG engine.
    /// </summary>
    [Table("ai_tutor_settings")]
    public class AiTutorSetting
    {
        /// <summary>
        /// Primary key. There is only ever one configuration row for now, identified by <see cref="SingletonId"/>.
        /// Maps to the <c>id</c> column.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// List of grades that use the RAG engine,
        /// Any lesson grade not in this list uses the generative engine. 
        /// Stored as text so non-numeric grades are supported. 
        /// Maps to the <c>rag_grades</c> column.
        /// </summary>
        [Column("rag_grades")]
        [MaxLength(255)]
        public string RagGrades { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp.
        /// Maps to <c>created_at</c> column.
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last-update timestamp 
        /// Maps to <c>updated_at</c> column.
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The fixed PK value of the one configuration row.
        /// </summary>
        public const int SingletonId = 1;
    }
}
