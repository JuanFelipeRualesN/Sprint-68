using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class ForumPostModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string ForumId { get; set; }
        public string Author { get; set; }
        public string Available { get; set; }
        public string Body { get; set; }
        public string ParentPostId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUsername { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int ChildPostCount { get; set; }
    }
}