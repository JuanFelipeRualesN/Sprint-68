namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the user lesson average full info.
    /// </summary>
    public class LessonsAverageCompleteInfoResponseModel
    {
        /// <summary>
        /// Gets or sets the lesson id.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the lesson no.
        /// </summary>
        public string LessonNo { get; set; }

        /// <summary>
        /// Gets or sets the lesson title.
        /// </summary>
        public string LessonTitle { get; set; }

        /// <summary>
        /// Gets or sets the average.
        /// </summary>
        public string Average { get; set; }
    }
}