namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the competition's rival.
    /// </summary>
    public class CompetitionRivalResponseModel
    {
        /// <summary>
        /// Gets or sets the user rival.
        /// </summary>
        public UserRivalModel UserRivalModel { get; set; }

        /// <summary>
        /// Gets or sets the quiz Id.
        /// </summary>
        public string QuizId { get; set; }
    }
}