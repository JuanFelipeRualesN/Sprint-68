using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class StudentScoreReportModel : SchoolModel
    {
        public List<PeriodDetailsModel> PeriodDetails { get; set; }

        public List<CummulativeModel> Cummulative { get; set; }

        public DateTime AcademicYearStart { get; set; }

        public DateTime AcademicYearEnd { get; set; }

        public string FullName { get; set; }

        public string Sie { get; set; }


    }
}