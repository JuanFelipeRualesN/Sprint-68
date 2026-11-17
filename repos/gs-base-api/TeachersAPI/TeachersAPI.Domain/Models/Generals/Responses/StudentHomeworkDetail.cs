using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class StudentHomeworkDetail : StudentLessonAssignmentDetail
    {
        public string TeacherPlanQuizId { get; set; }

        public string QuizId { get; set; }

        public string UserQuizResultId { get; set; }
        
        public string MaxScore { get; set; }
        
        public string Score { get; set; }

    }
}