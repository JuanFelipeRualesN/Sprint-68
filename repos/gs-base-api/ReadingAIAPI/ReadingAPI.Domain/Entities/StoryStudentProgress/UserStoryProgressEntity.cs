namespace ReadingAPI.ReadingAPI.Domain.Entities.Reading
{
    public class UserStoryProgressEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public string Language { get; set; } = "en";
        public StoryProgressStatus StatusId { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
