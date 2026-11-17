namespace GSWebAPI.GSWebAPI.Domain.Models.General
{
    public class GameVersionModel
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string EndDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}