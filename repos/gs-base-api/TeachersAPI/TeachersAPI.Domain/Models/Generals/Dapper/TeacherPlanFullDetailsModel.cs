using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class TeacherPlanFullDetailsModel
    {
        public TeacherPlanFullDetailsModel()
        {
            TeacherPlanModel = new TeacherPlanModel();
            LessonAssignmentModels = new List<LessonAssignmentModel>();
            HomeworkAssignmentModels = new List<HomeworkAssignmentModel>();
        }
        public TeacherPlanModel TeacherPlanModel { get; set; }

        public List<LessonAssignmentModel> LessonAssignmentModels { get; set; }

        public List<HomeworkAssignmentModel> HomeworkAssignmentModels { get; set; }

    }
}