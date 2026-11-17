using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.FeignAPI;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class TeacherPlanModel
    {
        public string PlanId { get; set; }

        public string PlanName { get; set; }

        public string TeacherId { get; set; }

        public bool PlanAvailable { get; set; }

        public string SubjectCode { get; set; }

        public string LevelCode { get; set; }

        public string SubjectName { get; set; }

        public string PeriodId { get; set; }

        public bool IsPlanOpen { get; set; }

        public string CreatedAt { get; set; }
        
        public string OpenDate { get; set; }
        
        public string CloseDate { get; set; }
        
        public int WeekNumber { get; set; }

        public AcademicDetailRequestModel AcademicPeriodFullDetail { get; set; }

        public List<string> TeacherGroups { get; set; }

		public string Timeframe { get; set; }

    }
}