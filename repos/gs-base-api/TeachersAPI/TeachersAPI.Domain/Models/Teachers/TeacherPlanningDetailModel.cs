using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherPlanningDetailModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the teacher plan general details.
        /// </summary>
        public TeacherPlanModel TeacherPlanModel { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan lesson. 
        /// </summary>
        public List<TeacherPlanLessonModel> TeacherPlanLessonModelList { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan quizzes.
        /// </summary>
        public List<TeacherPlanQuizModel> TeacherPlanQuizModelList { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan's groups.
        /// </summary>
        public List<TeacherPlanGroupModel> TeacherPlanGroupModelList { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan's students.
        /// </summary>
        public List<TeacherPlanStudentModel> TeacherPlanStudentModelList { get; set; }
    }
}
