namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapStudentLearningSheetModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string AcademicYearId { get; set; }
        public string LearningSheetId { get; set; }
        public string QuizId { get; set; }
        public string Available { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}