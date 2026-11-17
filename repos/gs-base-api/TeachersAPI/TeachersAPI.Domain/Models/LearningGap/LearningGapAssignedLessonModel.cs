namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapAssignedLessonModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string AcademicYearId { get; set; }
        public string MaxTries { get; set; }
        public string TeacherId { get; set; }
        public string LimitDate { get; set; }
        public string LessonId { get; set; }
        public string SubjectCode { get; set; }
        public string StudentId { get; set; }
        public string Completed { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Custom properties
        public string SubjectName { get; set; }
        public string LevelName { get; set; }

        public string LessonTitle { get; set; }

    }
}