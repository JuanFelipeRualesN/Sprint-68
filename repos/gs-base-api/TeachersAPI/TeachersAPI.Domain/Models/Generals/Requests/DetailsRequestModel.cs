namespace TeachersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle the forum request for students.
    /// </summary>
    public class DetailsRequestModel
    {
        /// <summary>
        /// Gets or sets the from page.
        /// </summary>
        public int FromPage { get; set; }

        /// <summary>
        /// Gets or sets the limiter.
        /// </summary>
        public int Limiter { get; set; }

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level code. 
        /// </summary>
        public string LevelCode { get; set; }
    }
}