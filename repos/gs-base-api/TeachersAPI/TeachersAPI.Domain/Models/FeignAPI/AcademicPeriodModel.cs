using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    public class AcademicPeriodModel
    {
        public AcademicPeriodModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string EntityAcademicDetailId { get; set; }
        public string PeriodNumber { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}