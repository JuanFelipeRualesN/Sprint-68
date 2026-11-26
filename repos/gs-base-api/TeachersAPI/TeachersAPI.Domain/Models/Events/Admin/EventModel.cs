using System.Collections.Generic;
using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Events
{
    public class EventModel : IResponse
    {
        public bool Valid { get; set; }
        public List<string> Messages { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TimeZoneCode { get; set; }
        public string TeamsEventId { get; set; }
        public string TeamsSyncStatus { get; set; }

        /// <summary>
        /// Gets or sets the timestamp (ISO string) of the last successful Graph sync. Null if the
        /// event has never synced successfully.
        /// </summary>
        public string TeamsSyncedAt { get; set; }
        public string Available { get; set; }

        /// <summary>
        /// Gets or sets the computed functional status ("upcoming", "ongoing", "finished", "cancelled").
        /// Not persisted — calculated from Available/StartDate/EndDate right before the response is returned.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the audience summary as language-neutral, comma-separated codes (e.g.
        /// "teacher,schools"), computed by EventAudienceSummaryCalculator. The frontend translates
        /// and joins these for display. Not persisted — output only, never used as a filter.
        /// </summary>
        public string AudienceSummary { get; set; }

        /// <summary>
        /// Gets or sets the RoleId to filter events by (Director/Teacher/Student, see EventAudienceRoles).
        /// Not persisted — filter input only, used by EventReaderManager.GetEvents.
        /// </summary>
        public string AudienceRoleId { get; set; }

        /// <summary>
        /// Gets or sets the user who originally created the event. Set once at creation and never
        /// overwritten by later edits — unlike ModifiedBy, which tracks the most recent editor.
        /// </summary>
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
