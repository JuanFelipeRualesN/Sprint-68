using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;

namespace GSWebAPI.GSWebAPI.Domain.Models.Community
{
    /// <summary>A community group, as seen by the user in session.</summary>
    public class CommunityGroupResponseModel
    {
        /// <summary>community_groups.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Group name.</summary>
        public string CommunityName { get; set; } = null!;

        /// <summary>Free-text location; null if not set.</summary>
        public string? CommunityLocation { get; set; }

        /// <summary>Free-text description; null if not set.</summary>
        public string? CommunityDescription { get; set; }

        /// <summary>User id (users DB) of the creator.</summary>
        public ulong CreatedByUserId { get; set; }

        /// <summary>Full name of the creator from the Users API; null if it could not be read.</summary>
        public string? CreatedByName { get; set; }

        /// <summary>Public (open joining) or private (joining needs approval).</summary>
        public CommunityGroupVisibility Visibility { get; set; }

        /// <summary>Number of active members (pending and banned are not counted).</summary>
        public int ActiveMembers { get; set; }

        /// <summary>Membership of the user in session, if any (also pending or banned); null if not a member.</summary>
        public CommunityMemberResponseModel? MyMembership { get; set; }

        /// <summary>Creation date, UTC.</summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>A membership (or join request) of a user in a group.</summary>
    public class CommunityMemberResponseModel
    {
        /// <summary>community_members.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Group of the membership.</summary>
        public ulong CommunityGroupId { get; set; }

        /// <summary>User id (users DB) of the member.</summary>
        public ulong UserId { get; set; }

        /// <summary>Full name from the Users API; null if not loaded or not available.</summary>
        public string? UserName { get; set; }

        /// <summary>owner, moderator or member.</summary>
        public CommunityMemberRole Role { get; set; }

        /// <summary>pending (waiting for approval), active or banned.</summary>
        public CommunityMemberStatus Status { get; set; }

        /// <summary>UTC date the membership became active; null while pending.</summary>
        public DateTime? JoinedAt { get; set; }
    }

    /// <summary>A community event, as seen by an active member of its group.</summary>
    public class CommunityEventResponseModel
    {
        /// <summary>community_events.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Group the event belongs to; null = platform-wide (no group).</summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>User id (users DB) of the organizer (the creator).</summary>
        public ulong OrganizerUserId { get; set; }

        /// <summary>Full name of the organizer from the Users API; null if it could not be read.</summary>
        public string? OrganizerName { get; set; }

        /// <summary>Event title.</summary>
        public string EventTitle { get; set; } = null!;

        /// <summary>Description; null if not set.</summary>
        public string? EventDescription { get; set; }

        /// <summary>meeting_formats.id (1 = online, 2 = in person).</summary>
        public byte MeetingFormatId { get; set; }

        /// <summary>meeting_formats.code of <see cref="MeetingFormatId"/>.</summary>
        public string? MeetingFormatCode { get; set; }

        /// <summary>Address if in person, URL if online; null if not set.</summary>
        public string? EventLocation { get; set; }

        /// <summary>Start, UTC.</summary>
        public DateTime EventDateTime { get; set; }

        /// <summary>End, UTC; null if not set.</summary>
        public DateTime? EventEndDateTime { get; set; }

        /// <summary>IANA time zone, for display.</summary>
        public string EventTimezone { get; set; } = null!;

        /// <summary>open (accepts registrations), closed or cancelled.</summary>
        public CommunityEventAvailability EventAvailability { get; set; }

        /// <summary>Maximum number of people going; null = no limit.</summary>
        public ushort? EventMaxAllowedPersons { get; set; }

        /// <summary>Sum of quantity_of_persons_assisting of the "going" responses.</summary>
        public int GoingPersons { get; set; }

        /// <summary>Registration of the user in session, if any.</summary>
        public CommunityEventInterestResponseModel? MyRegistration { get; set; }

        /// <summary>All registrations (any response). Filled only when reading one event; null in lists.</summary>
        public List<CommunityEventInterestResponseModel>? Attendees { get; set; }
    }

    /// <summary>A registration of a user to an event (community_events_interests).</summary>
    public class CommunityEventInterestResponseModel
    {
        /// <summary>community_events_interests.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Event of the registration.</summary>
        public ulong CommunityEventId { get; set; }

        /// <summary>User id (users DB) of the registered user.</summary>
        public ulong UserId { get; set; }

        /// <summary>Full name from the Users API; null if not loaded or not available.</summary>
        public string? UserName { get; set; }

        /// <summary>going, interested or not_going.</summary>
        public CommunityEventResponse Response { get; set; }

        /// <summary>People attending, including the user (1 for not_going).</summary>
        public byte QuantityOfPersonsAssisting { get; set; }
    }

    /// <summary>A community resource (link, document or video).</summary>
    public class CommunityResourceResponseModel
    {
        /// <summary>community_resources.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Group of the resource; null = platform-wide.</summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>User id (users DB) of the author.</summary>
        public ulong AuthorId { get; set; }

        /// <summary>Full name of the author from the Users API; null if it could not be read.</summary>
        public string? AuthorName { get; set; }

        /// <summary>Resource title.</summary>
        public string Title { get; set; } = null!;

        /// <summary>Description; null if not set.</summary>
        public string? Description { get; set; }

        /// <summary>link, document, video or audio.</summary>
        public CommunityResourceType ResourceType { get; set; }

        /// <summary>
        /// The external URL, or for an uploaded file a read-only signed link (SAS) valid for one hour from this response.
        /// Do not store it: read the resource again to get a new link.
        /// </summary>
        public string Link { get; set; } = null!;

        /// <summary>True when <see cref="Link"/> points to a file uploaded to Blob Storage (signed link).</summary>
        public bool IsUploadedFile { get; set; }

        /// <summary>Original file name of an uploaded file; null for an external link.</summary>
        public string? FileName { get; set; }

        /// <summary>Content type of an uploaded file; null for an external link.</summary>
        public string? ContentType { get; set; }

        /// <summary>Size in bytes of an uploaded file; null for an external link.</summary>
        public ulong? SizeBytes { get; set; }

        /// <summary>UTC publication date; null = draft.</summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>Creation date, UTC.</summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>A forum category (community_forum_categories catalog).</summary>
    public class CommunityForumCategoryResponseModel
    {
        /// <summary>community_forum_categories.id (1 = general, the default).</summary>
        public byte Id { get; set; }

        /// <summary>Stable code of the category (for example general, announcements).</summary>
        public string Code { get; set; } = null!;

        /// <summary>Name in the session's language (Accept-Language), e.g. "Preguntas" for es and "Questions" for en; English when the language has no translation.</summary>
        public string Name { get; set; } = null!;

        /// <summary>True if only group owners and moderators can create forums in (or move forums to) this category.</summary>
        public bool ModeratorsOnly { get; set; }
    }

    /// <summary>A group forum (thread), as seen by an active member of its group.</summary>
    public class CommunityForumResponseModel
    {
        /// <summary>community_forums.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Group the forum belongs to; null = platform-wide (no group).</summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>community_forum_categories.id.</summary>
        public byte CategoryId { get; set; }

        /// <summary>Code of the category.</summary>
        public string? CategoryCode { get; set; }

        /// <summary>Category name in the session's language (Accept-Language); English when the language has no translation.</summary>
        public string? CategoryName { get; set; }

        /// <summary>User id (users DB) of the author.</summary>
        public ulong AuthorId { get; set; }

        /// <summary>Full name of the author from the Users API; null if it could not be read.</summary>
        public string? AuthorName { get; set; }

        /// <summary>Forum title.</summary>
        public string Title { get; set; } = null!;

        /// <summary>Description; null if not set.</summary>
        public string? Description { get; set; }

        /// <summary>True if pinned (listed before the other forums).</summary>
        public bool IsPinned { get; set; }

        /// <summary>True if locked: only owners/moderators can post or edit.</summary>
        public bool IsLocked { get; set; }

        /// <summary>Posts that are not deleted.</summary>
        public int PostsCount { get; set; }

        /// <summary>UTC of the latest post that is not deleted; null if there are none.</summary>
        public DateTime? LastPostAt { get; set; }

        /// <summary>
        /// True if the user in session can edit and delete the forum (author, or group owner/moderator). It does not
        /// consider the lock: on a locked forum only owners/moderators can actually edit.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>Creation date, UTC.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Last update date, UTC.</summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>A post (or reply) in a group forum.</summary>
    public class CommunityForumPostResponseModel
    {
        /// <summary>community_forum_posts.id.</summary>
        public ulong Id { get; set; }

        /// <summary>Forum of the post.</summary>
        public ulong CommunityForumId { get; set; }

        /// <summary>The post this one replies to; null for a top-level post.</summary>
        public ulong? ParentPostId { get; set; }

        /// <summary>User id (users DB) of the author. Null when the post is deleted.</summary>
        public ulong? AuthorId { get; set; }

        /// <summary>Full name of the author from the Users API. Null when the post is deleted or the name could not be read.</summary>
        public string? AuthorName { get; set; }

        /// <summary>Null when the post is deleted.</summary>
        public string? PostText { get; set; }

        /// <summary>
        /// A deleted post is only returned (with no author or text) while it still has replies, so the thread
        /// keeps its shape.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>Direct replies that are not deleted.</summary>
        public int RepliesCount { get; set; }

        /// <summary>True if the user in session can edit the text (author, and the forum is not locked unless the author is an owner/moderator).</summary>
        public bool CanEdit { get; set; }

        /// <summary>True if the user in session can delete it (author, or group owner/moderator).</summary>
        public bool CanDelete { get; set; }

        /// <summary>Creation date, UTC.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC of the last text change; null if never edited.</summary>
        public DateTime? EditedAt { get; set; }
    }
}
