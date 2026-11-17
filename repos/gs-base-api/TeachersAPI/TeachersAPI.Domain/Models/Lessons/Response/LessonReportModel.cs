using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    public class LessonReportModel
    {
        public string LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }
        public string StandardCode { get; set; }
        public int TotalExamples { get; set; }
        public int TotalDefinitions { get; set; }
        public bool HasVideo { get; set; }
        public int TotalPractices { get; set; }
        public int TotalExams { get; set; }
    }
}
