using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    /// <summary>
    /// community_events_interests: a user's registration to an event (who plans to attend and how many people they bring).
    /// Unique per event and user. Soft-deleted.
    /// </summary>
    public class CommunityEventInterest : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to community_events (CASCADE).</summary>
        public ulong CommunityEventId { get; set; }

        /// <summary>ul_user_accounts.id (logical reference to the users DB).</summary>
        public ulong UserId { get; set; }

        /// <summary>The user's response (DB default <c>going</c>).</summary>
        public CommunityEventResponse Response { get; set; }

        /// <summary>Includes the user. Always 1 or more (CHECK, DB default 1).</summary>
        public byte QuantityOfPersonsAssisting { get; set; } = 1;

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Event of the registration.</summary>
        public CommunityEvent Event { get; set; } = null!;
    }
}
