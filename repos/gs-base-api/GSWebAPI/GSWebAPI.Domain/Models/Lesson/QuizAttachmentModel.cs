namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class QuizAttachmentModel
    {
        public string Id { get; set; }
        public string QuizId { get; set; }
        public string AttachmentUrl { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}