namespace TeachersAPI.Domain.Models.Events
{
    /// <summary>
    /// Canonical values for EventModel.Status (the computed functional status, independent from Available).
    /// </summary>
    public static class EventStatuses
    {
        public const string Upcoming = "upcoming";
        public const string Ongoing = "ongoing";
        public const string Finished = "finished";
        public const string Cancelled = "cancelled";
    }
}
