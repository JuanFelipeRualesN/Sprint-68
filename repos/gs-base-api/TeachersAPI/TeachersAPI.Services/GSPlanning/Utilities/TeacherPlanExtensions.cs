using System;
using GSPlanning.Domain;

namespace GSPlanning.Services.Utilities
{
    public static class TeacherPlanExtensions
    {
        public static DateRange GetDateRange(this TeacherPlan teacherPlan)
        {
            if (teacherPlan.OpenDate != null && teacherPlan.CloseDate != null) 
                return new DateRange(teacherPlan.OpenDate.Value, teacherPlan.CloseDate.Value);
            return null;
        }
    }
}