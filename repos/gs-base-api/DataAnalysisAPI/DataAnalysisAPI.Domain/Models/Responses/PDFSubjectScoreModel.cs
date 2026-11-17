using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class PDFSubjectScoreModel
    {
        public string SubjectLevel { get; set; }
        public List<PDFPeriodScoreModel> PeriodScores { get; set; }
    }
}