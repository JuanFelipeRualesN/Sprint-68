using SchoolsAPI.Dependencies.SchoolManagerLibV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request
{
    public class AcademicDetailRequestModel : IResponse
    {
        public AcademicDetailRequestModel()
        {
            AcademicDetail = new AcademicDetailModel();
            AcademicPeriods = new List<AcademicPeriodModel>();
            Messages = new List<string>();
        }

        public bool Valid { get; set; }
        public List<string> Messages { get; set; }

        public AcademicDetailModel AcademicDetail { get; set; }
        public List<AcademicPeriodModel> AcademicPeriods { get; set; }
    }
}