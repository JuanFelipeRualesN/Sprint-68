using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GSAIServicesAPI.Models.PromptMonitoring
{
    /// <summary>
    /// Append-only audit record of one prompt submitted by a user. Persisted to Azure Cosmos DB
    /// for MongoDB (Mongo API) for Puerto Rico Department of Education (DEPR) retention.
    /// </summary>
    /// <remarks>
    /// Read model: a user sees their own prompts, a teacher sees the prompts of the students they
    /// relate to, and an admin sees any user's prompts — all keyed by <see cref="UserId"/>, which is
    /// the shard key every query filters on.
    /// Primarily append-only: a prompt is captured once, so normally <see cref="UpdatedAtUtc"/>
    /// equals <see cref="CreatedAtUtc"/>; it is kept only for the rare case a record is corrected or
    /// re-classified afterwards. The shape stays backward-compatible via <see cref="SchemaVersion"/>
    /// and <see cref="ExtraElements"/>; add any new field as optional.
    /// </remarks>
    public class PromptLog
    {
        /// <summary>Schema version written by this build. Bump it when the shape changes.</summary>
        public const int CurrentSchemaVersion = 1;

        /// <summary>
        /// Primary key. Stored as an <c>ObjectId</c> in <c>_id</c> but exposed as a string so the
        /// domain stays driver-agnostic. Server-generated per write and embeds a timestamp.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>Shape version of this document. Maps to <c>schemaVersion</c>.</summary>
        [BsonElement("schemaVersion")]
        public int SchemaVersion { get; set; } = CurrentSchemaVersion;

        /// <summary>
        /// User who sent the prompt, resolved from the session token (never the client payload).
        /// <b>The shard key</b>: its high cardinality distributes writes across partitions and keeps a
        /// user's audit trail together, so every query pins it to reach one partition. Required and
        /// immutable. Maps to <c>userId</c>.
        /// </summary>
        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        /// <summary>Role the user acted as (<c>teacher</c>, <c>student</c>, <c>admin</c>). Maps to <c>role</c>.</summary>
        [BsonElement("role")]
        public string? Role { get; set; }

        /// <summary>The verbatim prompt text — the subject of the record. Maps to <c>prompt</c>.</summary>
        [BsonElement("prompt")]
        public string Prompt { get; set; } = null!;

        [BsonElement("response")]
        public string? Response { get; set; }

        /// <summary>
        /// Lesson subject shown in the teacher's history. Null outside the tutor flow. Maps to <c>subject</c>.
        /// </summary>
        [BsonElement("subject")]
        public string? Subject { get; set; }

        [BsonElement("isFlagged")]
        public bool IsFlagged { get; set; } = false;

        [BsonElement("alertCategories")]
        public List<string> AlertCategories { get; set; } = [];

        /// <summary>
        /// Why the prompt was flagged, shown to the teacher. Null when nothing was flagged.
        /// Maps to <c>alertReason</c>.
        /// </summary>
        [BsonElement("alertReason")]
        public string? AlertReason { get; set; }

        /// <summary>
        /// When the prompt was classified. Null means it never was, which is what keeps an
        /// unclassified prompt from looking like a clean one. Maps to <c>classifiedAtUtc</c>.
        /// </summary>
        [BsonElement("classifiedAtUtc")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ClassifiedAtUtc { get; set; }

        // --- Traceability ---

        /// <summary>
        /// UTC instant the prompt was received; set once and never changed. Time axis for exports
        /// and chronological retrieval. Maps to <c>createdAtUtc</c>.
        /// </summary>
        [BsonElement("createdAtUtc")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC instant the record was last modified. Defaults to creation time; since the log is
        /// append-only it usually matches <see cref="CreatedAtUtc"/>, and is kept only for the rare
        /// correction or re-classification. Maps to <c>updatedAtUtc</c>.
        /// </summary>
        [BsonElement("updatedAtUtc")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>IP address the request originated from. Maps to <c>ipAddress</c>.</summary>
        [BsonElement("ipAddress")]
        public string? IpAddress { get; set; }

        /// <summary>
        /// Unmapped BSON fields, kept so newer documents round-trip without data loss. This is what
        /// makes the collection safe to evolve. Null when there are none.
        /// </summary>
        [BsonExtraElements]
        public BsonDocument? ExtraElements { get; set; }
    }
}
