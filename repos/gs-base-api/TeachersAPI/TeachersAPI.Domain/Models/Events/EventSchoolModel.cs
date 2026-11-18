namespace TeachersAPI.Domain.Models.Events
{
    public class EventSchoolModel
    {
        public string Id { get; set; }
        public string EventId { get; set; }

        /// <summary>
        /// The school's EntityId, as returned by SchoolsAPI/UsersAPI. Passed as-is to
        /// GetUserDetails.GetUserEmailsForEvents (SchoolIds) to resolve the school's members.
        /// </summary>
        public string SchoolIdentifier { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
