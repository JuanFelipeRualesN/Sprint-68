using TeachersAPI.Domain.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class TeacherPlanHomeworkResultModel : PlanAssignmentUserResultModel
    {
        public string Status { get; set; }

        public string TotalPercent { get; set; }
    }
}