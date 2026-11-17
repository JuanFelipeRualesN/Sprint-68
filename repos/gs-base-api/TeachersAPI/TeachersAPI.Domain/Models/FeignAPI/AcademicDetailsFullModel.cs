using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// To handle academic details.
    /// </summary>
    public class AcademicDetailsFullModel
    {
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

    }
}