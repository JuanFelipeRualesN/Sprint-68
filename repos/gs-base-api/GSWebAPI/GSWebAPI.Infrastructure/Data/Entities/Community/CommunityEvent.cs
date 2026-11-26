using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Catalogs;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>community_events: an event of a community group. Soft-deleted.</summary>
    public class CommunityEvent : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to community_groups (RESTRICT).</summary>
        public ulong? CommunityGroupId { get; set; }

        /// <summary>Organizer. ul_user_accounts.id (logical reference to the users DB).</summary>
        public ulong OrganizerUserId { get; set; }

        /// <summary>Title (max 150).</summary>
        public string EventTitle { get; set; } = null!;

        /// <summary>Optional description (TEXT).</summary>
        public string? EventDescription { get; set; }

        /// <summary>FK to meeting_formats (RESTRICT): online or in person.</summary>
        public byte MeetingFormatId { get; set; }

        /// <summary>Address if in person, URL if online (max 255).</summary>
        public string? EventLocation { get; set; }

        /// <summary>Start, UTC.</summary>
        public DateTime EventDateTime { get; set; }

        /// <summary>End, UTC. Null = no end time; otherwise CHECK: after <see cref="EventDateTime"/>.</summary>
        public DateTime? EventEndDateTime { get; set; }

        /// <summary>IANA time zone name, for display (max 64).</summary>
        public string EventTimezone { get; set; } = null!;

        /// <summary>Whether the event accepts registrations (DB default <c>open</c>).</summary>
        public CommunityEventAvailability EventAvailability { get; set; }

        /// <summary>Capacity: maximum sum of persons of the <c>going</c> registrations. Null = no limit.</summary>
        public ushort? EventMaxAllowedPersons { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Group the event belongs to.</summary>
        public CommunityGroup? Group { get; set; }

        /// <summary>Meeting format of the event.</summary>
        public MeetingFormat MeetingFormat { get; set; } = null!;

        /// <summary>Registrations (responses) of users to the event.</summary>
        public List<CommunityEventInterest> Interests { get; set; } = new();
    }
}
