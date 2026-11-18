using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Events.Admin
{
    /// <summary>
    /// Represents a single calendar event returned to the client.
    /// </summary>
    public class EventModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event is valid.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the messages associated with the event (for example, validation messages).
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the type of the event (homework, lesson, or lesson quiz).
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the event's identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the event's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the event's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the event's location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the event's start date, formatted as "yyyy-MM-dd HH:mm:ss".
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the event's end date, formatted as "yyyy-MM-dd HH:mm:ss".
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the event's time zone code.
        /// </summary>
        public string TimeZoneCode { get; set; }

        /// <summary>
        /// Gets or sets the associated Microsoft Teams event identifier.
        /// </summary>
        public string TeamsEventId { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft Teams synchronization status.
        /// </summary>
        public string TeamsSyncStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is available.
        /// </summary>
        public string Available { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who last modified the event.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the event's creation timestamp.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the event's last update timestamp.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}
