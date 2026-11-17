using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Requests
{
    public class PlanRequestModel : PlanModel
    {
        public List<PlanLangModel> PlanDescriptionsByLang { get; set; }
        public List<PlanAvailableIsoModel> PlanAvailableIsos { get; set; }
        public List<string> PlanTaxesIds { get; set; }
        public int TrialDays { get; set; }

        /// <summary>
        /// Gets or sets the tax type.
        /// </summary>
        public string TaxType { get; set; } = "inclusive"; // inclusive or exclusive. If not sent, it will be inclusive by default.
    }
}
