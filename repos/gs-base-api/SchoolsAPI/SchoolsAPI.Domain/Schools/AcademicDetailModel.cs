using SchoolsAPI.Dependencies.SchoolManagerLibV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools
{
    public class AcademicDetailModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string TermId { get; set; }
        public string PeriodQuantity { get; set; }
        public string DaysToClosePeriod { get; set; }
        public string FullTermStartDate { get; set; }
        public string FullTermEndDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}