using TeachersAPI.Model;
using TeachersAPI.Domain.Models.Generals;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherPlanModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsInstructional { get; set; }
        public string PlanTypeId { get; set; }
        public string PlanNumber { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string UserId { get; set; }
        public string TeacherFullName { get; set; }
        public string Available { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public string PeriodId { get; set; }
        public string Opened { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string OpenDate { get; set; }
        public string CloseDate { get; set; }
        public string WeekNumber { get; set; }
        public string Timeframe { get; set; }
        // Extended property: 
        public List<TeacherPlanGroupModel> Groups { get; set; }

        // Extended property:
        public List<ExpiredDateResult> ExpiredDateResults { get; set; }

    }
}
