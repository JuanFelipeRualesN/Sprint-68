
namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    /// <summary>
    /// To handle the student exams details.
    /// </summary>
    public class StudentExamModel : TeacherPlanLessonQuizModel
    {
        /// <summary>
        /// Gets or sets whether the student completed the exam or not.
        /// </summary>
        public int Completed { get; set; }

        /// <summary>
        /// Gets or sets the quiz title.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the user result id.
        /// </summary>
        public string UsquiUserResultId { get; set; }
        
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }
        
    }
}