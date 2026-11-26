using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GSAIServicesAPI.Models.PromptMonitoring
{
    /// <summary>
    /// Marks how far one viewer (teacher, director or administrator) has reviewed the alerts of one
    /// student. A single document per (viewer, student) pair: opening that student's history moves
    /// <see cref="LastReviewedAtUtc"/> forward, and anything flagged after it counts as unread.
    /// </summary>
    /// <remarks>
    /// "Read" is per viewer, not per prompt: several adults can share a student, and each one keeps
    /// their own watermark, so one reviewing a student never clears the indicator for another.
    /// <para>
    /// Shard key: <see cref="ViewerId"/>. All of a viewer's marks live in one partition, so reading
    /// the marks for a roster hits a single partition and an upsert routes straight to it. The pair
    /// (<c>viewerId</c>, <c>studentId</c>) is meant to be unique — back it with a unique compound
    /// index so a race cannot create two marks for the same pair.
    /// </para>
    /// </remarks>
    public class AlertReview
    {
        /// <summary>Schema version written by this build. Bump it when the shape changes.</summary>
        public const int CurrentSchemaVersion = 1;

        /// <summary>
        /// Primary key. Stored as an <c>ObjectId</c> in <c>_id</c> but exposed as a string so the
        /// domain stays driver-agnostic. Server-generated on the first upsert.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>Shape version of this document. Maps to <c>schemaVersion</c>.</summary>
        [BsonElement("schemaVersion")]
        public int SchemaVersion { get; set; } = CurrentSchemaVersion;

        /// <summary>
        /// Who reviewed the alerts — the teacher/director/admin from the validated session, never the
        /// client payload. <b>The shard key.</b> Maps to <c>viewerId</c>.
        /// </summary>
        [BsonElement("viewerId")]
        public string ViewerId { get; set; } = null!;

        /// <summary>The student whose alerts were reviewed. Maps to <c>studentId</c>.</summary>
        [BsonElement("studentId")]
        public string StudentId { get; set; } = null!;

        /// <summary>
        /// UTC instant this viewer last opened the student's history. Alerts classified after it are
        /// unread for this viewer. Maps to <c>lastReviewedAtUtc</c>.
        /// </summary>
        [BsonElement("lastReviewedAtUtc")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastReviewedAtUtc { get; set; }

        /// <summary>
        /// Unmapped BSON fields, kept so newer documents round-trip without data loss. Null when there
        /// are none.
        /// </summary>
        [BsonExtraElements]
        public BsonDocument? ExtraElements { get; set; }
    }
}
