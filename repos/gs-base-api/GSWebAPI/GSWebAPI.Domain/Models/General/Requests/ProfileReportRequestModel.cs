namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To handle Profile results.
    /// </summary>
    public class ProfileReportRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the profile id.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the quiz id.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the user quiz id.
        /// </summary>
        public string UserQuizId { get; set; }

        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the lesson id.
        /// </summary>
        public string LessonId { get; set; }
    }
}