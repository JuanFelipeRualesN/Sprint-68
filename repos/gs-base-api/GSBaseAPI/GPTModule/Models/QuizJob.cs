namespace GSBaseAPI.GPTModule.Models
{
    public enum QuizJobStatus { Pending, Running, Succeeded, Failed }

    public sealed class QuizJob
    {
        public string JobId { get; set; } = Guid.NewGuid().ToString("N");
        public QuizJobStatus Status { get; set; } = QuizJobStatus.Pending;
        public string? QuizId { get; set; }
        public string? Error { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
