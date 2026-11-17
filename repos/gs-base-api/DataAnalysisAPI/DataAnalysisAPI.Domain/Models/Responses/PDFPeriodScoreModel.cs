using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class PDFPeriodScoreModel
    {
        public int PeriodNo { get; set; }
        public double Score { get; set; }

        public string Grade { get; set; }
    }
}