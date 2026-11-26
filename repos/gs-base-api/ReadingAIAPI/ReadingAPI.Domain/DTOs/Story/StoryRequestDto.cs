namespace ReadingAPI.ReadingAPI.Domain.DTOs.Story
{
    public class StoryRequestDto
    {
        public int LevelId { get; set; }
        public int CategoryId { get; set; }
        public string TitleEn { get; set; } = "";
        public string TitleEs { get; set; } = "";
        public string? ImageUrl { get; set; }
        public int FluencyGoal { get; set; }
        public int AccuracyGoal { get; set; }
        public int CompletenessGoal { get; set; }
        public int PronunciationGoal { get; set; }
        public List<StorySlideRequestDto> Slides { get; set; } = new();
    }
}
