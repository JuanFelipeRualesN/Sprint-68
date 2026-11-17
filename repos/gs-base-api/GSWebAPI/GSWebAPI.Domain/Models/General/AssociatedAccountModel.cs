namespace GSWebAPI.GSWebAPI.Domain.Models.General
{
    public class AssociatedAccountModel
    {
        public string Id { get; set; }
        public string MainUserAccount { get; set; }
        public string ProfileUserAccount { get; set; }
        public string Available { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}