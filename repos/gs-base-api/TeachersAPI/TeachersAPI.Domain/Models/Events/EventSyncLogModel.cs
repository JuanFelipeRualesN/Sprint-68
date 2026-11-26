namespace TeachersAPI.Domain.Models.Events
{
    /// <summary>
    /// A single record of a Microsoft Graph sync attempt for an event (create/update/delete),
    /// regardless of outcome. Insert-only audit trail — never updated once written.
    /// </summary>
    public class EventSyncLogModel
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string LoggedAt { get; set; }
    }
}
