namespace TeachersAPI.Domain.Models.Events
{
    public class EventRoleModel
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string RoleId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
