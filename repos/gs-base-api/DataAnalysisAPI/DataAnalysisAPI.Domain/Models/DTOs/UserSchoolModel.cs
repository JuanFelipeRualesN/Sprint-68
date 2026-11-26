using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs
{
    public class UserSchoolModel : ConnectedDateModel
    {
        public string SchoolName { get; set; }
        public string SchoolType { get; set; }
        public string City { get; set; }
        public string Ore { get; set; }
        public int? OreId { get; set; }
        public string StateIsoCode { get; set; }
        public string Country { get; set; }
        public string CountryIso { get; set; }
        public string SchoolIdentifier { get; set; }
        public string SubscriptionStatus { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime? CancellationDate { get; set; }
        public string PlanName { get; set; }
    }
}