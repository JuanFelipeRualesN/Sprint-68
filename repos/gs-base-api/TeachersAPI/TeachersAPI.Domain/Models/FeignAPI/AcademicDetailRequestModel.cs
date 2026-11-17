using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Model;
using TeachersAPI.Domain.Models.FeignAPI;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// To handle academic details.
    /// </summary>
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

        /* Properties from AcademicDetailsFullModel */
        public string EntityAcademicDetailId { get; set; }
        public string EntitySubscriptionId { get; set; }
        public string TermId { get; set; }
        public int PeriodQuantity { get; set; }
        public int DaysToClosePeriod { get; set; }
        public string FullTermStartDate { get; set; }
        public string FullTermEndDate { get; set; }
        public string EntityAcademicDetailCreatedDate { get; set; }
        public string PeriodId { get; set; }
        public int PeriodNumber { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string PeriodCreatedDate { get; set; }
        public string PeriodModifiedBy { get; set; }
        public string PeriodUpdatedDate { get; set; }
        /* Properties from AcademicDetailsFullModel */
    }
}