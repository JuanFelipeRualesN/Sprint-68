using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class PlanDetail
    {
        /// <summary>
        /// Gets or sets the plan Id.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the plan name.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Gets or sets level's code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the subject's name.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the overall average plan.
        /// </summary>
        public string OverallAveragePlan { get; set; }

        /// <summary>
        /// Gets or sets the period id.
        /// </summary>
        public string PeriodId { get; set; }

        /// <summary>
        /// Gets or sets the teacher id.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the teacher's name.
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// Gets or sets the total pages of homeworks.
        /// </summary>
        public int TotalPages { get; set; }
    }
}