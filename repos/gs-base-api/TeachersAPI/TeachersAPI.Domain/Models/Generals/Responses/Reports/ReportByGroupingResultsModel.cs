using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.Reports
{
    public class ReportByGroupingResultsModel
    {
        public string QuizId { get; set; }
        public string QuizTitle { get; set; }
        public string LessonTitle { get; set; }
        public string DueDate { get; set; }
        public string DueDateTimeZone { get; set; }
        public string GroupAverage { get; set; }
        public int ForGrading { get; set; }
    }
}