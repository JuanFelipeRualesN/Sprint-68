namespace GSWebAPI.GSWebAPI.Domain.Models.LessonProgress
{
    /// <summary>
    /// A lesson section the student has completed.
    /// </summary>
    public class UserCompletedLessonSectionModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LessonId { get; set; }
        public string Section { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
