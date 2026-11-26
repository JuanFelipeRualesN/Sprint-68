using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// custom_plan_evidences: an evidence or project attached to a plan, optionally to one of its lessons. Soft-deleted
    /// (the blob is kept). Uploaded files live in Blob Storage (container AppSettings:azure_storage_container_name_gsweb);
    /// the row keeps the blob name and the response gets a fresh, short-lived read link. External links keep only the URL.
    /// </summary>
    public class CustomPlanEvidence : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_client (CASCADE).</summary>
        public ulong CustomPlanClientId { get; set; }

        /// <summary>FK to custom_plan_client_lessons (SET NULL). Null = evidence of the whole plan; also set to null if the lesson is removed from a draft plan.</summary>
        public ulong? CustomPlanClientLessonId { get; set; }

        /// <summary>Evidence or project (DB default <c>evidence</c>).</summary>
        public CustomPlanEvidenceType EvidenceType { get; set; }

        /// <summary>Title (max 150).</summary>
        public string Title { get; set; } = null!;

        /// <summary>Optional description (TEXT; max 5,000 chars, service rule).</summary>
        public string? Description { get; set; }

        /// <summary>The blob URL without SAS (uploaded files) or the external http/https link (max 1000).</summary>
        public string Url { get; set; } = null!;

        /// <summary>Blob name inside the gsweb container (max 500). Null for external links.</summary>
        public string? BlobName { get; set; }

        /// <summary>Original name of the uploaded file (max 255). Null for external links.</summary>
        public string? FileName { get; set; }

        /// <summary>MIME type of the uploaded file, taken from the extension (max 100). Null for external links.</summary>
        public string? ContentType { get; set; }

        /// <summary>Size of the uploaded file in bytes. Null for external links.</summary>
        public ulong? SizeBytes { get; set; }

        /// <summary>ul_user_accounts.id of the uploader (logical reference).</summary>
        public ulong UploadedByUserId { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Plan the evidence belongs to.</summary>
        public CustomPlanClient CustomPlanClient { get; set; } = null!;

        /// <summary>Lesson the evidence is attached to, if any.</summary>
        public CustomPlanClientLesson? Lesson { get; set; }
    }
}
