namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools
{
    public class RelationModel
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public string RelationEntityId { get; set; }
        public string TypeId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}