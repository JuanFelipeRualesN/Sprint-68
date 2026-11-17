using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.Reports
{
    public class ReportQuizModel
    {
        public string UsquiUserResultId { get; set; }
        public string QuizId { get; set; }
        public string DueDate { get; set; }
        public string DueDateTimeZone { get; set; }
        public string PlanId { get; set; }
        public string LessonId { get; set; }
        public string UserId { get; set; }
        public int TotalCorrect { get; set; }
        public int TotalValue { get; set; }
        public string Score { get; set; }
        public string PlanLessonId { get; set; }
        public string Premises { get; set; }
        public string TotalMaxScore { get; set; }
        public string TeacherPlanQuizId { get; set; }
    }
}