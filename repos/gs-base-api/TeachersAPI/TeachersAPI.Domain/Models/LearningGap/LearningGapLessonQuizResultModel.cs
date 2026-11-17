namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapLessonQuizResultModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string LearningGapAssignedLessonId { get; set; }
        public string QuizId { get; set; }
        public string GswebUsquiResultsId { get; set; }
        public string Score { get; set; }
        public string Percentage { get; set; }
        public string ActualTry { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Properties extended
        public string QuizTitle { get; set; }

    }
}