using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the average of each subject.
    /// </summary>
    public class SubjectAverageResponseModel
    {
        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the average.
        /// </summary>
        public string Average { get; set; }

        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        public List<double> Scores { get; set; }
    }
}