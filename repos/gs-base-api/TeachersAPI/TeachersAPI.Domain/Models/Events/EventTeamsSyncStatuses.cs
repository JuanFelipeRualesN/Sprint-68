namespace TeachersAPI.Domain.Models.Events
{
    /// <summary>
    /// Canonical values for EventModel.TeamsSyncStatus (the state of the Microsoft Graph sync).
    /// </summary>
    public static class EventTeamsSyncStatuses
    {
        public const string Pending = "pending";
        public const string Synced = "synced";
        public const string Error = "error";
    }
}
