namespace TeachersAPI.Domain.Models.Generals.Responses.Reports
{
    public class ReportPlanLessonQuizResponseModel
    {
        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; }
        
        public string LessonTitle { get; set; }
        
        public string Note { get; set; }
        
        public string PDFLocation { get; set; }
        
        public string AssignedDate { get; set; }
        
        public int ForGrading { get; set; }
        
        public string DueDate { get; set; }
        
        public string DueDateTimeZone { get; set; }
        
        public int Duration { get; set; }
        
        public string UserQuizResultId { get; set; }
        
        public string UserId { get; set; }
        
        public bool ManuallyCorrectedByTeacher { get; set; }
        
        public string Score { get; set; }
        
        public QuizUserResultsCompletedInfoResponseModel UserQuizFullDetails { get; set; }
        
    }
}