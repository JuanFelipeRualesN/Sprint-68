namespace ReadingAPI.ReadingAPI.Domain.Entities.Reading
{
    public class UserUnmasteredWordEntity
    {
        public int Id { get; set; }
        public int PracticeId { get; set; }
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public string Word { get; set; } = "";
        public double AccuracyScore { get; set; }
        public string ErrorType { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
