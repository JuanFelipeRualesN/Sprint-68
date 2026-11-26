using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>community_resources: a resource (link, or an uploaded document, video or audio) shared in a group or platform-wide. Soft-deleted.</summary>
    public class CommunityResource : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to community_groups (RESTRICT). Null = shared with the whole platform.</summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>Author. ul_user_accounts.id (logical reference to the users DB).</summary>
        public ulong AuthorId { get; set; }

        /// <summary>Title (max 150).</summary>
        public string Title { get; set; } = null!;

        /// <summary>Optional description (TEXT).</summary>
        public string? Description { get; set; }

        /// <summary>Kind of resource (DB default <c>link</c>).</summary>
        public CommunityResourceType ResourceType { get; set; }

        /// <summary>External URL, or for uploaded files the blob URL without SAS (max 500). Uploaded files are read through a signed link.</summary>
        public string Link { get; set; } = null!;

        /// <summary>Blob name (path in the container) of an uploaded file; null for an external link (max 500).</summary>
        public string? BlobName { get; set; }

        /// <summary>Original file name of an uploaded file (max 255).</summary>
        public string? FileName { get; set; }

        /// <summary>Content type of an uploaded file, from its extension (max 100).</summary>
        public string? ContentType { get; set; }

        /// <summary>Size in bytes of an uploaded file.</summary>
        public ulong? SizeBytes { get; set; }

        /// <summary>UTC. Null = draft, visible only to the author (and group owners/moderators).</summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Group the resource belongs to; null for platform-wide resources.</summary>
        public CommunityGroup? Group { get; set; }
    }
}
