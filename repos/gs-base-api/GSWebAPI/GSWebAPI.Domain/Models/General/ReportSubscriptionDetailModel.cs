using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General
{
    public class ReportSubscriptionDetailModel
    {
        public string SubscriptionId { get; set; }
        public string SubscriptionPlatformId { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Quantity { get; set; }
        public string Active { get; set; }
        public string Cancelled { get; set; }
        public string ActivationDate { get; set; }
        public string TerminationDate { get; set; }
        public string CancellationReason { get; set; }
        public string Plan { get; set; }
        public string PlanType { get; set; }

    }
}