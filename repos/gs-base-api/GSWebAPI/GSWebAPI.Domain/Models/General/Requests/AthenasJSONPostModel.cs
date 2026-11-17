namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To handle Athenas POSTs.
    /// </summary>
    public class AthenasJSONPostModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether if it is for an outside source.
        /// </summary>
        public bool IsForOutsideSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if it is a multiple choice.
        /// </summary>
        public bool MultipleChoice { get; set; }

        /// <summary>
        /// Gets or sets the id of the quiz.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the id of the lesson.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if it is for a game or not.
        /// </summary>
        public bool IsForGame { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating  whether show the URL instead.
        /// </summary>
        public bool ShowUrlInstead { get; set; }
    }
}