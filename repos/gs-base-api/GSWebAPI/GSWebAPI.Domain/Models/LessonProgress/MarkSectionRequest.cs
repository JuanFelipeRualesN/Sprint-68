namespace GSWebAPI.GSWebAPI.Domain.Models.LessonProgress
{
    /// <summary>
    /// Request body to mark a lesson section as completed for the student.
    /// </summary>
    public class MarkSectionRequest
    {
        public string LessonId { get; set; }
        public string Section { get; set; }
    }
}
