using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;

namespace GSWebAPI.GSWebAPI.Domain.Models.Community
{
    /// <summary>Body to create or update a community group. Text values are trimmed; blank optional values are saved as null.</summary>
    public class CommunityGroupRequestModel
    {
        /// <summary>Group name. Required, up to 120 characters.</summary>
        public string CommunityName { get; set; } = null!;

        /// <summary>Free-text location (city, area). Optional, up to 255 characters; null or blank = none.</summary>
        public string? CommunityLocation { get; set; }

        /// <summary>Free-text description. Optional; null or blank = none.</summary>
        public string? CommunityDescription { get; set; }

        /// <summary>Public groups accept members right away; private groups need an owner or moderator to approve each join request.</summary>
        public CommunityGroupVisibility Visibility { get; set; } = CommunityGroupVisibility.Public;
    }

    /// <summary>Body to change the role of an active member (group owners only).</summary>
    public class ChangeMemberRoleRequestModel
    {
        /// <summary>New role: owner, moderator or member.</summary>
        public CommunityMemberRole Role { get; set; }
    }

    /// <summary>Body to create or update a community event. Text values are trimmed; blank optional values are saved as null.</summary>
    public class CommunityEventRequestModel
    {
        /// <summary>
        /// Optional group of the event (<c>POST events</c>); null = platform-wide, open to everyone. With a group, the same
        /// rules as in <c>POST groups/{groupId}/events</c> apply (the route's group wins there). Ignored on update: the group does not change.
        /// </summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>Event title. Required, up to 150 characters.</summary>
        public string EventTitle { get; set; } = null!;

        /// <summary>Free-text description. Optional; null or blank = none.</summary>
        public string? EventDescription { get; set; }

        /// <summary>meeting_formats.id (1 = online, 2 = in person). Must exist in the catalog.</summary>
        public byte MeetingFormatId { get; set; }

        /// <summary>Address if in person, URL if online. Optional, up to 255 characters.</summary>
        public string? EventLocation { get; set; }

        /// <summary>Start, UTC. Stored truncated to whole seconds.</summary>
        public DateTime EventDateTime { get; set; }

        /// <summary>End, UTC. Optional (null = no end time). Must be after the start.</summary>
        public DateTime? EventEndDateTime { get; set; }

        /// <summary>IANA time zone, for display. Required and must be a valid time zone id.</summary>
        public string EventTimezone { get; set; } = null!;

        /// <summary>
        /// Maximum number of people "going" (sum of their quantities). Null = no limit; 0 is not allowed. When updating,
        /// it cannot be lower than the people already going.
        /// </summary>
        public ushort? EventMaxAllowedPersons { get; set; }

        /// <summary>Only used when updating (null = unchanged); new events are always open.</summary>
        public CommunityEventAvailability? EventAvailability { get; set; }
    }

    /// <summary>Body to register (or change the registration of) the user in session to an event.</summary>
    public class EventRegistrationRequestModel
    {
        /// <summary>going, interested or not_going. Only going/interested need the event to be open.</summary>
        public CommunityEventResponse Response { get; set; } = CommunityEventResponse.Going;

        /// <summary>Includes the user. 1 or more. Ignored (saved as 1) when the response is not_going.</summary>
        public byte QuantityOfPersonsAssisting { get; set; } = 1;
    }

    /// <summary>Body to create or update a community resource. Text values are trimmed.</summary>
    public class CommunityResourceRequestModel
    {
        /// <summary>
        /// Group the resource belongs to (the author must be an active member). Null = shared with the whole platform
        /// (any user). Only used when creating; the group of a resource does not change.
        /// </summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>Resource title. Required, up to 150 characters.</summary>
        public string Title { get; set; } = null!;

        /// <summary>Free-text description. Optional; null or blank = none.</summary>
        public string? Description { get; set; }

        /// <summary>
        /// link, document, video or audio, for an external link. Ignored for an uploaded file: the type comes from the file
        /// (video for mp4, audio for mp3, document for the rest).
        /// </summary>
        public CommunityResourceType ResourceType { get; set; } = CommunityResourceType.Link;

        /// <summary>
        /// External http/https URL, up to 500 characters. Required when no file is uploaded; ignored when a file is uploaded,
        /// and on update of a resource that holds an uploaded file (the file is kept).
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// True to publish now (an already published resource keeps its original published date); false keeps it as a
        /// draft (visible to the author and group owners/moderators, or staff for platform-wide resources).
        /// </summary>
        public bool Publish { get; set; }
    }

    /// <summary>Body to create or update a group forum. Title and description pass the profanity filter.</summary>
    public class CommunityForumRequestModel
    {
        /// <summary>
        /// Optional group of the forum (<c>POST forums</c>); null = platform-wide, open to everyone. With a group, the same
        /// rules as in <c>POST groups/{groupId}/forums</c> apply (the route's group wins there). Ignored on update: the group does not change.
        /// </summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>Forum title. Required, up to 150 characters.</summary>
        public string Title { get; set; } = null!;

        /// <summary>Optional description, up to 10,000 characters; null or blank = none.</summary>
        public string? Description { get; set; }

        /// <summary>
        /// community_forum_categories.id. Null = general when creating, or the current category when updating.
        /// Categories marked moderators-only (announcements) need a group owner or moderator.
        /// </summary>
        public byte? CategoryId { get; set; }
    }

    /// <summary>Owner/moderator actions on a forum. Null fields are left unchanged.</summary>
    public class CommunityForumModerationRequestModel
    {
        /// <summary>True to pin the forum (listed first), false to unpin; null = unchanged.</summary>
        public bool? IsPinned { get; set; }

        /// <summary>True to lock the forum (no new posts or edits except from owners/moderators), false to unlock; null = unchanged.</summary>
        public bool? IsLocked { get; set; }
    }

    /// <summary>Body to create or edit a forum post. The text passes the profanity filter.</summary>
    public class CommunityForumPostRequestModel
    {
        /// <summary>Post text. Required, up to 10,000 characters; trimmed.</summary>
        public string PostText { get; set; } = null!;

        /// <summary>The post being replied to (same forum, not deleted). Null = top-level post. Only used when creating.</summary>
        public ulong? ParentPostId { get; set; }
    }
}
