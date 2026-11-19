namespace ReadingAPI.ReadingAPI.Domain.DTOs.Story
{
    public class StorySlideDto
    {
        public int Id { get; set; }
        public int SlideOrder { get; set; }
        public string TextEn { get; set; } = "";
        public string TextEs { get; set; } = "";
        public string? ImageUrl { get; set; }
    }
}
