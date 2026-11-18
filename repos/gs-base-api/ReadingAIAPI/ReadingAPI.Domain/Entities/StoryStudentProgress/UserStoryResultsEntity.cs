namespace ReadingAPI.ReadingAPI.Domain.Entities.Reading
{
    public class UserStoryResultsEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public int RegionId { get; set; }
        public string Language { get; set; } = "en";
        public int StoryId { get; set; }
        public int SlideId { get; set; }
        public string EvaluatedText { get; set; } = "";
        public double AccuracyScore { get; set; }
        public double FluencyScore { get; set; }
        public double CompletenessScore { get; set; }
        public double PronunciationScore { get; set; }
        public int WordsPerMinute { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<UserUnmasteredWordEntity> UnmasteredWords { get; set; } = new List<UserUnmasteredWordEntity>();
    }
}
