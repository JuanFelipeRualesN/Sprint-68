using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class PlanDetailLessonModel : PlanDetail
    {

        /// <summary>
        /// Gets or sets homework detail.
        /// </summary>
        public List<StudentLessonAssignmentDetail> AssignmentDetails { get; set; }
    }
}