using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Handle homework responses.
    /// </summary>
    public class HomeworkResponseModel
    {
        public HomeworkResponseModel()
        {
            PlanDetails = new List<PlanDetail>();
        }

        /// <summary>
        /// Gets or sets thte overall average.
        /// </summary>
        public string OverallAverage { get; set; }

        /// <summary>
        /// Gets or sets a list of homework details.
        /// </summary>
        public List<PlanDetail> PlanDetails { get; set; }
    }
}