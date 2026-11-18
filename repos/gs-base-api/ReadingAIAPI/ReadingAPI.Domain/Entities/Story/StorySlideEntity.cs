namespace ReadingAPI.ReadingAPI.Domain.Entities.Story
{
    public class StorySlideEntity
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public int SlideOrder { get; set; }
        public string TextEn { get; set; } = "";
        public string TextEs { get; set; } = "";
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
