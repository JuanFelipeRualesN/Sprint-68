namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To handle lesson request.
    /// </summary>
    public class LessonRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the lesson id.
        /// </summary>
        public string LessonId { get; set; }
    }
}