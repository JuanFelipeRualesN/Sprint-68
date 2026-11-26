namespace TeachersAPI.Domain.Models.Events
{
    /// <summary>
    /// Minimal organizer info needed to sync an event with Microsoft Graph — deliberately narrower
    /// than ValidationResponseModel so the Teams sync flow (EventTeamsManager, EventAudienceResolver,
    /// GraphEventsAPI) doesn't need to depend on the full session model.
    /// </summary>
    public class EventOrganizerModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
    }
}
