namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapDiagnosticTestResultModel 
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string QuizId { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public string TotalCorrect { get; set; }
        public string TotalQuestions { get; set; }
        public string TotalPercent { get; set; }
        public string TotalScore { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}