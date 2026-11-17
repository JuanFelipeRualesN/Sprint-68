using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class TeacherPlanQuizModel : Teachers.TeacherPlanQuizModel
    {
        /// <summary>
        /// Gets or sets the custom assignment title.
        /// </summary>
        public string Title { get; set; }
        
        public decimal TotalMaxScore { get; set; }
    }
}