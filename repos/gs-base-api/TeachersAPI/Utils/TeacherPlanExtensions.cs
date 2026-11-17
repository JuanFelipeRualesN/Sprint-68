using System;
using GSPlanning.Services.Utilities;
using TeachersAPI.Domain.Models.Teachers;

namespace TeachersAPI.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class TeacherPlanExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherPlan"></param>
        /// <returns></returns>
        public static DateRange GetDateRange(this TeacherPlanModel teacherPlan)
        {
            if (teacherPlan.OpenDate != null && teacherPlan.CloseDate != null) 
                return new DateRange(DateTime.Parse(teacherPlan.OpenDate), DateTime.Parse(teacherPlan.CloseDate));
            return null;
        }
    }
}