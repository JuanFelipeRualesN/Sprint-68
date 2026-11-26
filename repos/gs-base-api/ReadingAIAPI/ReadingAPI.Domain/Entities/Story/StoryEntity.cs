namespace ReadingAPI.ReadingAPI.Domain.Entities.Story
{
    public class StoryEntity
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public int CategoryId { get; set; }
        public string TitleEn { get; set; } = "";
        public string TitleEs { get; set; } = "";
        public string? ImageUrl { get; set; }
        public int FluencyGoal { get; set; }
        public int AccuracyGoal { get; set; }
        public int CompletenessGoal { get; set; }
        public int PronunciationGoal { get; set; }
        public int WordCountEn { get; set; }
        public int WordCountEs { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<StorySlideEntity> Slides { get; set; } = new List<StorySlideEntity>();
    }
}
