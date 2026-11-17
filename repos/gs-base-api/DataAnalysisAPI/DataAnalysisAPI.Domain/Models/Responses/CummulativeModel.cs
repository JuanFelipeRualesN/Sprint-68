using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class CummulativeModel
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string LevelCode { get; set; }
        public double Score { get; set; }
        public string Grade { get; set; }
    }
}