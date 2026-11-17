using TeachersAPI.Domain.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the response for teacher plan lessons with all details (includign the quiz details).
    /// </summary>
    public class TeacherPlanLessonFullDetailModel
    {
        /// <summary>
        /// Gets or sets the main teacher plan lesson model with all details.
        /// </summary>
        public TeacherPlanLessonModel TeacherPlanLessonModel { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan lesson quizzes model list.
        /// </summary>
        public List<TeacherPlanLessonQuizModel> TeacherPlanLessonQuizModels { get; set; }
    }
}