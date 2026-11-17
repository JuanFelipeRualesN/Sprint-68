using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class PlanDetailHomeworkModel : PlanDetail
    {
        /// <summary>
        /// Gets or sets homework detail.
        /// </summary>
        public List<StudentHomeworkDetail> AssignmentDetails { get; set; }
    }
}