using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community
{
    // [EnumMember] holds the exact MySQL ENUM value. Declaration order is the column's ENUM order.
    // The member whose numeric value is 0 must be the column's DB default (EF does not send CLR defaults
    // for properties that have a DB default, so 0 and the DB default must mean the same thing).

    /// <summary>community_groups.visibility</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunityGroupVisibility
    {
        /// <summary>Anyone can join and becomes active right away. DB default.</summary>
        [EnumMember(Value = "public")] Public = 0,

        /// <summary>Joins stay pending until an owner or moderator approves them.</summary>
        [EnumMember(Value = "private")] Private = 1,
    }

    /// <summary>community_members.role</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunityMemberRole
    {
        /// <summary>Owner of the group (the creator becomes one). Only owners update or delete the group.</summary>
        [EnumMember(Value = "owner")] Owner = 2,

        /// <summary>Moderator: approves joins, manages forums, events and resources of the group.</summary>
        [EnumMember(Value = "moderator")] Moderator = 1,

        /// <summary>Regular member. DB default.</summary>
        [EnumMember(Value = "member")] Member = 0,
    }

    /// <summary>community_members.status</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunityMemberStatus
    {
        /// <summary>Waiting for approval to join a private group.</summary>
        [EnumMember(Value = "pending")] Pending = 1,

        /// <summary>Active member. DB default.</summary>
        [EnumMember(Value = "active")] Active = 0,

        /// <summary>Banned: cannot join again or leave (the row is kept).</summary>
        [EnumMember(Value = "banned")] Banned = 2,
    }

    /// <summary>community_events.event_availability</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunityEventAvailability
    {
        /// <summary>Accepts registrations. DB default.</summary>
        [EnumMember(Value = "open")] Open = 0,

        /// <summary>No longer accepts registrations.</summary>
        [EnumMember(Value = "closed")] Closed = 1,

        /// <summary>The event was cancelled.</summary>
        [EnumMember(Value = "cancelled")] Cancelled = 2,
    }

    /// <summary>community_events_interests.response</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunityEventResponse
    {
        /// <summary>Interested, not confirmed; does not count toward the capacity.</summary>
        [EnumMember(Value = "interested")] Interested = 1,

        /// <summary>Will attend; counts toward the event capacity. DB default.</summary>
        [EnumMember(Value = "going")] Going = 0,

        /// <summary>Will not attend (<c>not_going</c>).</summary>
        [EnumMember(Value = "not_going")] NotGoing = 2,
    }

    /// <summary>community_resources.resource_type</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunityResourceType
    {
        /// <summary>A web link. DB default.</summary>
        [EnumMember(Value = "link")] Link = 0,

        /// <summary>A document.</summary>
        [EnumMember(Value = "document")] Document = 1,

        /// <summary>A video.</summary>
        [EnumMember(Value = "video")] Video = 2,

        /// <summary>An audio file (e.g. mp3).</summary>
        [EnumMember(Value = "audio")] Audio = 3,
    }
}
