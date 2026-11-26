namespace ReadingAPI.ReadingAPI.Domain.DTOs.Story
{
    public class StorySlideRequestDto
    {
        public int SlideOrder { get; set; }
        public string TextEn { get; set; } = "";
        public string TextEs { get; set; } = "";
        public string? ImageUrl { get; set; }
    }
}
