using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class PeriodDetailsModel : CummulativeModel
    {
        public int AcademicYearId { get; set; }
        public int UserId { get; set; }
        public int PeriodNo { get; set; }
    }
}