using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class StudentAssignmentsResponseModel
    {
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        public List<StudentLessonAssignmentDetail> PendingLessonsModelList { get; set; }

        public List<StudentHomeworkDetail> HomeworkModelList { get; set; }

        public List<Dapper.TeacherPlanLessonQuizModel> LessonQuizModelList { get; set; }
    }
}