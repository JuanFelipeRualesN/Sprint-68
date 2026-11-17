namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// Manages the student results.
    /// </summary>
    public class ProfileReportsResponseModel
    {
        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the quiz details.
        /// </summary>
        public QuizUserResultsCompletedInfoResponseModel UserQuizFullDetails { get; set; }

        /// <summary>
        /// Gets or sets the PDF location.
        /// </summary>
        public string PDFLocation { get; set; }
    }
}