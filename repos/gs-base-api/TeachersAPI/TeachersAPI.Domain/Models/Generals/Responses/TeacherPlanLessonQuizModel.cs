using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class TeacherPlanLessonQuizModel : Teachers.TeacherPlanLessonQuizModel
    {
        public string QuizTitle { get; set; }
    }
}