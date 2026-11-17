namespace TeachersAPI.Domain.Models.Lessons
{
    public class StatusModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string PublicAvailable { get; set; }
        public string IsFinal { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}