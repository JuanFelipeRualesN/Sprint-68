namespace TeachersAPI.Domain.Models.Lessons
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserToId { get; set; }
        public string EntityId { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
        public string Completed { get; set; }
        public string Read { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}