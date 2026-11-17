using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the overall average responses for student average call.
    /// </summary>
    public class OverallAverageResponseModel
    {
        /// <summary>
        /// Gets or sets the overall average.
        /// </summary>
        public string OverallAverage { get; set; }

        /// <summary>
        /// To get or set the level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the subject average response.
        /// </summary>
        public List<SubjectAverageResponseModel> SubjectAverageResponseModelList { get; set; }
    }
}