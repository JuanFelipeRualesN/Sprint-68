using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class LessonAssignmentResponseModel
    {
        public LessonAssignmentResponseModel()
        {
            PlanDetails = new List<PlanDetailLessonModel>();
        }

        /// <summary>
        /// Gets or sets thte overall average.
        /// </summary>
        public string OverallAverage { get; set; }

        /// <summary>
        /// Gets or sets a list of plan details.
        /// </summary>
        public List<PlanDetailLessonModel> PlanDetails { get; set; }
    }
}